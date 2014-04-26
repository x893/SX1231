
/**
 * \file FHSSapi.c
 * implements frequency hopping and synchronization function
 *
 */

#include <string.h>
#include "transceiver.h"
#include "wsn.h"
#include "FHSSapi.h"

/******************************************************************/

uint8_t radio_channel_dialog;	// the running hop counter
uint8_t current_radio_channel;	// indicate where the radio is now

uint8_t RFbuffer[RF_BUFFER_SIZE];       // RF buffer
volatile uint8_t RFbufferSize;          // RF buffer size
uint8_t Node_adrs;
uint8_t SyncState;

#ifdef SLAVE_ANSWER_ALL
	volatile char hop_on_next_wakeup;
#endif

const rom uint16_t Master_Sync_Rate_HiRes = HIRES_TIMEOUT(8000);
const rom uint16_t Sync_Time_Slave = HIRES_TIMEOUT(65535);
const rom uint16_t Sync_Time_Master = HIRES_TIMEOUT(65535);

uint8_t Sync_fhss(void)
{
	int idx;
	static uint8_t radio_channel_sync;
	static char master_sync_count;

	if (hw_address == HW_ADDRESS__MASTER)
	{
		// master.. //
		if (SyncState == NOT_SYNC)
		{
			SYNC_MODE_LED = 1;
			radio_channel_sync = 0;
			master_sync_count = 0;
			Fhss_Hop(&radio_channel_sync);
#ifdef PLL_TEST
			do
			{
				radio_channel_sync--;
				Fhss_Hop(&radio_channel_sync);
				SetRFMode(RF_MODE_RECEIVER);
				for (idx = 0; idx < 0x7fff; idx++)
				{
					if (IN_PLL_LOCK)
						break;
				}
			} while (IN_PLL_LOCK == 0);
#endif
			EnableClock_HiRes(READ_ROM_WORD(Master_Sync_Rate_HiRes));
			SyncState = SYNC_TX_RUN;	// Next step

			return SyncState;
		}
		else if (SyncState == SYNC_TX_RUN)
		{
			if (!HIRES_COMPARE_B_FLAG)
				return SyncState;

			//p1_0 ^= 1;
			EnableClock_HiRes(READ_ROM_WORD(Master_Sync_Rate_HiRes));

			master_sync_count++;

			if (master_sync_count == NUM_CHANNELS)
			{	// 8ms * 50 = 400mS
				radio_channel_sync = SYNC_END;
			}
			SendRfFrame(&radio_channel_sync, 1, BroadCast_ID, FALSE);

			if (radio_channel_sync != SYNC_END)
			{
				Fhss_Hop(&radio_channel_sync);
				return SyncState;
			}
			else
			{
				// SYNC_END: prepare to send the dialog channel number
				EnableClock_HiRes(READ_ROM_WORD(Master_Sync_Rate_HiRes));
				SyncState = SYNC_TX_DIALOG_CH;
				return SyncState;
			}
		}
		else if (SyncState == SYNC_TX_DIALOG_CH)
		{
			// must be sure we dont transmit before the slave receiver is able
			if (!HIRES_COMPARE_B_FLAG)
				return SyncState;

			// send the channel number that were going to be on just after sending this message
			SYNCING_DIALOG_CH = LED_ON;
			SendRfFrame(&radio_channel_dialog, 1, BroadCast_ID, FALSE);
			SYNCING_DIALOG_CH = LED_OFF;

			Fhss_Hop(&radio_channel_dialog);
			SyncState = NOT_SYNC;				// we're done, reset our state
			SYNC_MODE_LED = 0;
			return SYNC_END;	// immediate return, indicate finished to caller
		}
		// ..master //
	}
	else
	{
		// slave.. //
		static uint8_t Slave_Sync_Step;
		uint8_t ReturnCode;	//for RX state machine

		if (SyncState == NOT_SYNC)
		{
			SYNC_MODE_LED = 1;
			LCD_puts_from_rom(2, "!sync");

			USART_send_str_from_rom(ROM_STR("[35m"));	// 35 = magenta
			radio_channel_sync = 0;
			Fhss_Hop(&radio_channel_sync);
			SyncState = SYNC_RX_RUN;		// Next step
			SetRFMode(RF_MODE_RECEIVER);	// RX mode	XXX check if this is necessary XXX
			Slave_Sync_Step = 0;

			RadioSetNodeAddress(BroadCast_ID);

			return SyncState;
		}

		if (SyncState != SYNC_RX_RUN)
			return SyncState;

		switch (Slave_Sync_Step)
		{
			case 0:
				// timeout ~65mS
				ReturnCode = ReceiveRfFrame(RFbuffer, (uint8_t*)&RFbufferSize, READ_ROM_WORD(Sync_Time_Slave));
				if (ReturnCode == RADIO_OK)
				{
					if (RFbufferSize > 0)
					{
						SYNC_RX_LED ^= 1;
						if (RFbuffer[0] >= NUM_CHANNELS)
						{
							USART_send_str_from_rom(ROM_STR(" 0:bad hop# "));
						}
						else
						{
							radio_channel_sync = RFbuffer[0];
							USART_send_str_from_rom(ROM_STR(" 0->1"));
							Slave_Sync_Step++;
							Fhss_Hop(&radio_channel_sync);
						}
					}
				}
				else if (ReturnCode == RADIO_RX_TIMEOUT)
				{
					USART_send_str_from_rom(ROM_STR(" 0t"));
					Fhss_Hop(&radio_channel_sync);
				}
				break;
			case 1:
				if (radio_channel_sync >= NUM_CHANNELS)
				{
					USART_send_str_from_rom(ROM_STR(" 1:bad hop# "));
					Slave_Sync_Step = 0;
					break;
				}
				// timeout ~65mS
				ReturnCode = ReceiveRfFrame(RFbuffer, (uint8_t*)&RFbufferSize, READ_ROM_WORD(Sync_Time_Master));
				// Tests if there is a TimeOut or if we have received a frame
				if (ReturnCode == RADIO_OK)
				{
					if (RFbufferSize > 0)
					{
						// Tests the received buffer value
						if (RFbuffer[0] == SYNC_END)
						{
							SYNC_RX_LED ^= 1;
							USART_send_str_from_rom(ROM_STR(" 1->2"));
							Slave_Sync_Step++;
						}
						else
						{
							USART_send_str_from_rom(ROM_STR(" 1h"));
							Fhss_Hop(&radio_channel_sync);
						}
					}
				}
				else if (ReturnCode == RADIO_RX_TIMEOUT)
				{
					/* had sync from step 0, but now lost it */
					USART_send_str_from_rom(ROM_STR(" 1t"));
					//Fhss_Hop(&radio_channel_sync);	//Fhss_Hopp();	// "broad"
					Slave_Sync_Step = 0;	// return to start
				}
				break;
			case 2:
				// timeout ~65mS
				SYNCING_DIALOG_CH = LED_ON;
				ReturnCode = ReceiveRfFrame(RFbuffer, (uint8_t*)&RFbufferSize, READ_ROM_WORD(Sync_Time_Master));
				// Tests if there is a TimeOut or if we have received a frame
				if (ReturnCode == RADIO_OK)
				{
					SYNC_RX_LED ^= 1;
					SYNCING_DIALOG_CH = LED_OFF;
					radio_channel_dialog = RFbuffer[0];

					////////// print... ///////////////
					idx = 0;
					strcpy_from_rom(text, ROM_STR(" [31m2ok"));	// red
					idx = strlen(text);
					text[idx++] = '[';
					ltoa(radio_channel_dialog, text+idx);
					idx = strlen(text);
					strcpy_from_rom(text+idx, ROM_STR("][0m"));
					idx = strlen(text);
					text[idx++] = 0;
					USART_send_str(text);
					////////// ...print ///////////////


					SyncState = NOT_SYNC;	// we are finished, reset state.
					SYNC_MODE_LED = 0;
					Slave_Sync_Step = 255;

#ifdef SLAVE_ANSWER_ALL
					// pretending to be four slaves, receive all
					WriteRegister(REG_PKTPARAM3, (READ_ROM_BYTE(RegistersCfg[REG_PKTPARAM3]) & 0xf9) | RF_PKT3_ADRSFILT_NONE);
#else
					RadioSetNodeAddress(Node_adrs);
#endif

					return SYNC_END; // finished, indicating we are now synchronized
				}
				else if (ReturnCode == RADIO_RX_TIMEOUT)
				{
					SYNCING_DIALOG_CH = LED_OFF;
					USART_send_str_from_rom(ROM_STR(" 2t"));
					Slave_Sync_Step = 0;
				}
				else if (ReturnCode != RADIO_RX_RUNNING)
				{
					SYNCING_DIALOG_CH = LED_OFF;
					USART_send_str_from_rom(ROM_STR(" 2?"));
					Slave_Sync_Step = 0;
				}
				break;
			default:
				USART_send_str_from_rom(ROM_STR(" sync step?"));
				break;
		} /* switch (Slave_Sync_Step) */
		// ..slave //
	}
	return SyncState;
}



