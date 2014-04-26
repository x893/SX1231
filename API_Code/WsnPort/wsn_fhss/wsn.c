/**
 * \file wsn.c
 * implements startup and master and slave dialog functions
 *
 */
#include <string.h>
#include "transceiver.h"
#include "wsn.h"


/******************************************************************************
			 Global Variables declarations
			 ******************************************************************************/
typedef struct {	//Bit access
	unsigned char SyncNextCycle : 1;
	unsigned char END_CYCLE : 1;
} BITread;


// State machine -- exported to timers.c
#define BroadCast_Synchronization	0x00
#define Confirm_Sync_open_dialog	0x01
uint8_t Mode = BroadCast_Synchronization;

uint8_t Error = 0;				// For master, number of TX/RX errors
uint8_t Error_cycle = 0;		// Number of cycles without answer/request
volatile char Slave_Needs_Hop;	// flag
hw_address_e hw_address;
ltime_t MainClock;				// low-resolution time

/******************************************************************************
			 Constants
			 ******************************************************************************/
const rom ltime_t Each = LOWRES_TIMEOUT(101.5625);	// 100mS at 3.90625mS steps is 101.5625mS
const rom ltime_t Slave_CycleRX = LOWRES_TIMEOUT(380);			// mS
const rom ltime_t Slave_CycleRX_Timeout = LOWRES_TIMEOUT(350);	// mS -- if failed to receive dialog from master
const rom ltime_t Sleep_Sync = LOWRES_TIMEOUT(900);				// mS -- time to sleep during sync cycle, we're already sync'd
const rom ltime_t First = LOWRES_TIMEOUT(90);	// mS	time from sync end to dialog start
const rom ltime_t Second = LOWRES_TIMEOUT(195);	// mS	time from sync end to dialog start
const rom ltime_t Third = LOWRES_TIMEOUT(295);	// mS	time from sync end to dialog start
const rom ltime_t Fourth = LOWRES_TIMEOUT(395);	// mS	time from sync end to dialog start
#ifdef SLAVE_ANSWER_ALL
const rom ltime_t CycleRX_short = LOWRES_TIMEOUT(85);	// mS
#endif

/******************************************************************************
			 Function Prototype declarations
			 ******************************************************************************/
const rom uint16_t SLAVE_WAIT = HIRES_TIMEOUT(5000);			// uS used by MASTER at SYNC_END
const rom uint16_t RX_TIMEOUT_SLAVE = HIRES_TIMEOUT(65535);		// uS
const rom uint16_t RX_TIMEOUT_MASTER = HIRES_TIMEOUT(30000);	// uS: master RX timeout

typedef enum {
	MASTER_DIALOG_STATE__RX = 0,
	MASTER_DIALOG_STATE__TX,
	MASTER_DIALOG_STATE__WAIT
} master_dialog_state_e;

static char
master_increment_slave_ID(uint8_t *Slave_ID)
{
	if (++(*Slave_ID) > Slave3_ID) {	// onto the next slave
		// End of cycle
		*Slave_ID = Slave0_ID;	// Reset ID
		return TRUE;
	}
	else
		return FALSE;
}

/*******************************************************************
** OnMaster : implements master                                   **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
static void
OnMaster(void)
{
	int idx;
	uint8_t ReturnCode;
	static master_dialog_state_e master_dialog_state;
	static uint8_t Slave_ID;
	static BITread master_flags;

	////// Synchronization mode	
	if (Mode == BroadCast_Synchronization)
	{
		ReturnCode = Sync_fhss();			// Synchronization function, ~400mS
		if (ReturnCode == SYNC_END)
		{	// End of synchronization catched
			Mode = Confirm_Sync_open_dialog;	// Enable next state
			Wait(READ_ROM_WORD(SLAVE_WAIT));	// Wait for the slaves
			SET_LOWRES_COUNTER(MainClock);		// Set Timer2	(100mS in this case)
			CLEAR_LOWRES_TIMER_FLAG;
			Slave_ID = Slave0_ID;				// Set first transmit ID
			Error = 0;							// Reset transmit errors number
			Error_cycle = 0;					// Reset cycle errors number
			master_flags.SyncNextCycle = FALSE;	// Reset next cycle synchronization state
			master_flags.END_CYCLE = FALSE;
			master_dialog_state = MASTER_DIALOG_STATE__WAIT;
			RX_OK_LED = 1;	// XXX debug
		}
	}
	////// Dialog session
	else if (Mode == Confirm_Sync_open_dialog)
	{
		// polling the timer interrupt:
		if (LOWRES_TIMER_FLAG)
		{
			/* the total time spent on single frequency is 406.25mS = 4x 101.5625mS */
			SET_LOWRES_COUNTER(READ_ROM_BYTE(Each));// Reload timer value (101.5625mS)
			CLEAR_LOWRES_TIMER_FLAG;
			ALARM_LED = 0;						// Clear all LEDs
			RX_OK_LED = 0;
			master_dialog_state = MASTER_DIALOG_STATE__TX;	  //Next state, TX
			if (master_flags.END_CYCLE)
			{	// End of the cycle
				master_flags.END_CYCLE = FALSE;

				if (master_flags.SyncNextCycle)
				{	// Synchronize the slaves lost
					Mode = BroadCast_Synchronization;
					USART_send_str_from_rom(ROM_STR(" resync"));
					return;
				}

				Fhss_Hop(&radio_channel_dialog);

				if (Error_cycle > 9)
				{	// Set flag to inform the other slaves that the next cycle is a sychronization cycle
					Error_cycle = 0;
					master_flags.SyncNextCycle = TRUE;
				}
			}
		}

		switch (master_dialog_state)
		{
			case MASTER_DIALOG_STATE__TX:
				if (master_flags.SyncNextCycle)
				{
					//LOAD "S" (Re-Sync message) to the TX RF buffer
					strcpy_from_rom((char *)RFbuffer, ROM_STR("S"));
				}
				else
				{
					// Load "?" into the TX RF buffer
					strcpy_from_rom((char *)RFbuffer, ROM_STR("?"));
				}

				DIALOG_LED = 1;	// indicate start of TX to slave
				//Send RF frame to slave
				ReturnCode = SendRfFrame(RFbuffer, strlen((char *)RFbuffer), Slave_ID, !master_flags.SyncNextCycle);

				/************* print.. *************/
				idx = 0;
				if (Slave_ID == Slave0_ID)
				{
					text[idx++] = '\r';
					text[idx++] = '\n';
					if (current_radio_channel < 10)
						text[idx++] = '0';
					ltoa(current_radio_channel, text + idx);
					idx = strlen(text);
					text[idx++] = ' ';

					LCDLine_2();
					text[idx] = 0;
					LCD_adds(text + 2);	// +2: dont print \r\n
				}
				ltoa(Slave_ID, text + idx);
				LCD_dwrite(text[idx]);
				idx = strlen(text);
				/************* ..print *************/
				if (master_flags.SyncNextCycle)
				{
					/************* print.. *************/
					LCD_dwrite('S');
					LCD_dwrite(' ');

					text[idx++] = 'S';
					text[idx++] = ' ';
					/************* ..print *************/
					// Goto a wait state (do nothing and wait for the next interrupt)
					master_dialog_state = MASTER_DIALOG_STATE__WAIT;
				}
				else
				{
					/************* print.. *************/
					text[idx++] = ':';
					/************* ..print *************/
					master_dialog_state = MASTER_DIALOG_STATE__RX;	// next state: RX
				}
				/************* print.. *************/
				text[idx++] = 0;
				USART_send_str(text);

				/************* ..print *************/
				if (master_dialog_state == MASTER_DIALOG_STATE__WAIT)
				{
					/* not receiving any reply to sync next cycle notification,
					 * so onto next slave now */
					master_flags.END_CYCLE = master_increment_slave_ID(&Slave_ID);
					DIALOG_LED = 0;
				}
				break;
			case MASTER_DIALOG_STATE__RX:
				ReturnCode = ReceiveRfFrame(RFbuffer, (uint8_t*)&RFbufferSize, READ_ROM_WORD(RX_TIMEOUT_MASTER));
				// Tests if there is a TimeOut or if we have received a frame
				if (ReturnCode == RADIO_OK)
				{
					Error = 0;
					if (RFbufferSize > 0)
					{
						// Sets the last byte of received buffer to 0. Needed by strcmp function
						master_dialog_state = MASTER_DIALOG_STATE__WAIT;
						RFbuffer[RFbufferSize] = '\0';
						// Test of the received buffer value 
						if (strcmp_from_rom((char *)RFbuffer, ROM_STR("K")) == 0)
						{
							RX_OK_LED = 1;
							// green background to indicate RX OK
							USART_send_str_from_rom(ROM_STR("[42m [0m "));

							LCD_dwrite(' ');
							LCD_dwrite(' ');
						}
						else if (strcmp_from_rom((char *)RFbuffer, ROM_STR("A")) == 0)
						{
							ALARM_LED = 1;
							// red background to indicate ALARM
							USART_send_str_from_rom(ROM_STR("[41mA[0m "));

							LCD_dwrite('A');
							LCD_dwrite(' ');
						}

						master_flags.END_CYCLE = master_increment_slave_ID(&Slave_ID);
					} //..if (RFbufferSize > 0)

					DIALOG_LED = 0;
				}
				else if (ReturnCode == RADIO_RX_TIMEOUT)
				{
					// No frame received
					USART_send_str_from_rom(ROM_STR("T "));	// uncolored space to indicate no reception

					LCD_dwrite('T');
					LCD_dwrite(' ');

					Error++;	// Increment error counter
					master_dialog_state = MASTER_DIALOG_STATE__TX;	//Return to Tx
					/* if retrying more than once,
					* then slave needs to adjust wakup time "CycleRX" to compensate delayed reception */
	
					/* if (Error > 4) {	// >1 or more would require retries to contain try count */
					if (Error > 0)
					{	// Slave not responding, retry on the next cycle
						Error = 0;
						Error_cycle++;
						master_dialog_state = MASTER_DIALOG_STATE__WAIT;
						master_flags.END_CYCLE = master_increment_slave_ID(&Slave_ID);
					}	//..if (error > 4)

					DIALOG_LED = 0;
				} // ..if (rx timeout)
				else if (ReturnCode != RADIO_RX_RUNNING)
				{
					USART_send_str_from_rom(ROM_STR("?"));	// indicate unhandled

					LCD_dwrite('?');
					LCD_dwrite(' ');
				}
				break;
			case MASTER_DIALOG_STATE__WAIT:
				break;
		} /* switch (master_dialog_state) */
	} //..if (Mode == Confirm_Sync_open_dialog)
}


/*******************************************************************
** OnSlave : implements slave                                     **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
static void
OnSlave(void)
{
	uint8_t idx;
	uint8_t ReturnCode;
	static char rx_timeout;	// flag

	//////Synchronization mode
	if (Mode == BroadCast_Synchronization)
	{
		ReturnCode = Sync_fhss();	// Synchronization function, ~400ms
		if (ReturnCode == SYNC_END)
		{	// Synchronization end
			Mode = Confirm_Sync_open_dialog;
			USART_send_str_from_rom(ROM_STR("[32m"));	// green to indicate dialog mode
			rx_timeout = FALSE;
			CLEAR_LOWRES_TIMER_FLAG;
			Slave_Needs_Hop = FALSE;
#ifdef SLAVE_ANSWER_ALL
			hop_on_next_wakeup = TRUE;
#endif
			EnableClock(MainClock);	// Set wake up clock -- 100mS to 400mS depending on slave address

			// lcd clearing perhaps is time consuming, done after the delay is started
			LCD_puts_from_rom(2, "");	// clear line 2

			go_sleep();			   // Go to sleep
		}
	}

	//////Confirm synchronization / Dialog session
	else if (Mode == Confirm_Sync_open_dialog)
	{
		if (Slave_Needs_Hop)
		{
			Fhss_Hop(&radio_channel_dialog);
			Slave_Needs_Hop = FALSE;
			/************* print.. *************/
			LCDLine_2();

			idx = 0;
			text[idx++] = '\r';
			text[idx++] = '\n';
			if (current_radio_channel < 10)
				text[idx++] = '0';
			ltoa(current_radio_channel, text + idx);
			idx = strlen(text);
			text[idx++] = 0;
			USART_send_str(text);
			LCD_adds(text + 2);
			/************* ..print *************/
		}

		// RX timeout at ~65mS
		ReturnCode = ReceiveRfFrame(RFbuffer, (uint8_t*)&RFbufferSize, READ_ROM_WORD(RX_TIMEOUT_SLAVE));
		// Tests if there is a TimeOut or if we have received a frame
		if (ReturnCode == RADIO_OK)
		{
			LCD_dwrite(' ');
			if (RFbufferSize > 0)
			{
				if (rx_timeout)
				{
					USART_send_str_from_rom(ROM_STR("[32m"));		// RF link previously failed, but now good
					//USART_send_str("ab32m");		// RF link previously failed, but now good
					rx_timeout = FALSE;
				}

				// Sets the last byte of the received buffer to 0. Needed by strcmp function
				RFbuffer[RFbufferSize] = '\0';
				// Tests the received buffer value
				if (strcmp_from_rom((char *)RFbuffer, ROM_STR("?")) == 0) {
#ifdef SLAVE_ANSWER_ALL
					// wake up soon enough to hear request for another slave
					SET_LOWRES_COUNTER(READ_ROM_BYTE(CycleRX_short));
#else
					// Re-initialize wake-up time (370mS)
					SET_LOWRES_COUNTER(READ_ROM_BYTE(Slave_CycleRX));
#endif
					USART_send_str_from_rom(ROM_STR(" ?"));
					LCD_dwrite('?');

					if (IN_ALARM)
					{	// Test the status
						ALARM_LED = 1;
						strcpy_from_rom((char *)RFbuffer, ROM_STR("A"));
					}
					else
					{
						RX_OK_LED = 1;
						strcpy_from_rom((char *)RFbuffer, ROM_STR("K"));
					}
					ReturnCode = SendRfFrame(RFbuffer, strlen((char *)RFbuffer), Master_ID, FALSE);
				}
				else if (strcmp_from_rom((char *)RFbuffer, ROM_STR("S")) == 0)
				{
					/* master was unable to get reply from one of its four slaves,
					 * so all synchronoized slaves go to sleep during this period */
					// Next cycle is a synchronization, sleep longer
					SET_LOWRES_COUNTER(READ_ROM_BYTE(Sleep_Sync));
					USART_send_str_from_rom(ROM_STR(" S"));
					LCD_dwrite('S');
				}
				go_sleep();
			} //..if (RFbufferSize > 0)
			else
			{
				USART_send_str_from_rom(ROM_STR(" Z"));	// zero-lengthed message
				LCD_dwrite('Z');
			}
		} //.. if (ReturnCode == OK)
		//Slave has received nothing. Try to receive at next cycle
		else if (ReturnCode == RADIO_RX_TIMEOUT)
		{
			LCD_dwrite(' ');
			LCD_dwrite('T');

			USART_send_str_from_rom(ROM_STR("[0m T"));
			rx_timeout = TRUE;

			if (++Error_cycle > 4)
			{
				Error_cycle = 0;
				/* slave not responding during 5 cycles --> Go to slow hopping mode */
				Mode = BroadCast_Synchronization;
			}
			else
			{
				SET_LOWRES_COUNTER(READ_ROM_BYTE(Slave_CycleRX_Timeout));	/* XXX needs adjusting XXX */
				go_sleep();
			}
		}
	} //..  if(Mode == Confirm_Sync_open_dialog)

}

/******************************************************************************
Name       : Hardware setup file
Parameters : none
Returns    : nothing
Description: Hardware setup file
*******************************************************************************/
static void
HardwareSetup(void)
{
	SPIInit();

	/* initializing transceiver first, because on some platforms,
	 * it could be providing our cpu clock */
	InitRFChip();
	cpu_init();	/* use the external clock, if applicable for this platform */

	DIR_ALARM_LED = OUTPUT;
	DIR_RX_OK_LED = OUTPUT;

	ALARM_LED = 0;
	RX_OK_LED = 0;

	INIT_DEBUG_PINS;

	uart_init();
	ENABLE_GLOBAL_INTERRUPTS;

	//#ifdef _DEBUG
	USART_send_str_from_rom(ROM_STR("[0m\r\nreset\r\n"));
	//#endif

	timers_init();

	lcd_init();

	/* setting hw_address to invalid value will cause initialization */
	hw_address = HW_ADDRESS__MAX;
}

/*
 * new_hw_address(): callback from change of hardware address.
 */
void
new_hw_address(void)
{
	const char rom_ptr *mode_str;

	switch (hw_address)
	{
	case HW_ADDRESS__MASTER:
		MainClock = READ_ROM_BYTE(Each);	// 100ms
		Node_adrs = Master_ID;
		mode_str = "master";
		break;
	case HW_ADDRESS__SLAVE0:
		MainClock = READ_ROM_BYTE(First);	// 100ms
		Node_adrs = Slave0_ID;
		mode_str = "slave0";
		break;
	case HW_ADDRESS__SLAVE1:
		MainClock = READ_ROM_BYTE(Second);
		Node_adrs = Slave1_ID;
		mode_str = "slave1";
		break;
	case HW_ADDRESS__SLAVE2:
		MainClock = READ_ROM_BYTE(Third);
		Node_adrs = Slave2_ID;
		mode_str = "slave2";
		break;
	case HW_ADDRESS__SLAVE3:
		MainClock = READ_ROM_BYTE(Fourth);
		Node_adrs = Slave3_ID;
		mode_str = "slave3";
		break;
	default:
		mode_str = "";
		break;
	} /* ..switch (hw_address) */

	LCD_puts_from_rom(1, mode_str);
	LCD_puts_from_rom(2, "");	// clear line 2

	USART_send_str_from_rom(ROM_STR("[0m"));
	USART_send_str_from_rom(mode_str);

	/* assert the new configuration */
	RadioSetNodeAddress(Node_adrs);

	/* reset state of Sync_Fhss() */
	SyncState = NOT_SYNC;

	Mode = BroadCast_Synchronization;	// restart
}

main_return_t
main(void)
{
	stop_wdt();

	HardwareSetup();

	while (1)
	{
		/* node address (am I master or slave?) can be configured at run-time */
		poll_hardware_address();

		if (hw_address == HW_ADDRESS__MASTER)
		{
			OnMaster();
		}
		else
		{
			OnSlave();
		}
	}
}
