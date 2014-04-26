#include "timers.h"
#include "cpu.h"
#include "wsn.h"
#include "transceiver.h"
#include "io_port_mapping.h"
#include <avr/sleep.h>

/**************************************************/

#if !defined(_CPU_SLEEP_)
volatile char wakeup_isr_ran;	// flag
#endif

/**************************************************/

/**
 * this ISR wakes up from sleep mode, which was entered from go_sleep()
 */
ISR(TIMER2_OVF_vect)
{
	ALARM_LED = 0;	// Clear LED
	RX_OK_LED = 0;	// Clear LED
	CPU_WAKEUP;	//	sleep_disable();	//Disable sleep mode
	SLAVE_SLEEP_LED = 0;
	DISABLE_LOWRES_TIMER_INTERRUPT;

#ifdef SLAVE_ANSWER_ALL
	if (hop_on_next_wakeup) {
		Slave_Needs_Hop = TRUE;
		hop_on_next_wakeup = FALSE;
	}
#else
	Slave_Needs_Hop = TRUE;	// dont hop in ISR (too time-consuming). do it from main loop
#endif

#if !defined(_CPU_SLEEP_)
	wakeup_isr_ran = TRUE;
#endif

}

/* EnableClock_HiRes(): uses OCR1B compare for high-resolution (2.17uS) timing */
void
EnableClock_HiRes(uint16_t compare_value)
{
	uint16_t counter_snapshot;

	counter_snapshot = HIRES_COUNTER;
	CLEAR_HIRES_COMPARE_B_FLAG;
	HIRES_COMPARE_B = counter_snapshot + compare_value;
	/* ISR not used, will be polled via (REG_TIFR1 & (1<<OCF1B)) */

}

//Enable and set lowres-timer function
void
EnableClock(uint16_t lowres_count)
{
	SET_LOWRES_COUNTER(lowres_count);

	LOWRES_COUNTER_UPDATE_WAIT;

	CLEAR_LOWRES_TIMER_FLAG;

} // void EnableTimeOutSync(uint8_t enable,uint16 TimeOut)

/*******************************************************************
** Wait : Wait until an overflow interrupt drived by TIMER1       **
********************************************************************
** In  : cntVal		                                              **
** Out : -		                                                  **
*******************************************************************/
void
Wait(uint16_t compare_value)
{
	CLEAR_HIRES_COMPARE_A_FLAG;
	HIRES_COMPARE_A = HIRES_COUNTER	+ compare_value;

	while (!HIRES_COMPARE_A_FLAG)
			;
}

// timers_init(): called from HardWareSetup(), in wsn.c
void
timers_init()
{
#if (TIMER2_CLOCK_SOURCE_HZ == 32768)
	/* timer2 is the low-resolution (8bit) timer :
	 * 32768 / 128 = 256Hz or 3906.25uS each */
	ASSR |= (1<<AS2);	// Source timer2 from 32.768 kHz crystal
#else
	#error unhandled_timer2_clock
#endif

#if (TIMER2_CLOCK_DIV == 128)
	TCCR2B |= ((1<<CS22) | (1<<CS20)) & ~(1<<CS21); // starts timer2: Clk div by 128
#else
	#error unhandled_timer2_div
#endif


#if defined(atmega644)
	/************** Timer0 PWM-DAC output... *************************/
	TCCR0A = 0x83;	/* COM0A set to 2: non interting PWM output, WGM mode is fast PWM */
	TCCR0B = 0x01;	/* timer0 clock source: 0x01 = ClkI/O no prescaling */
	OCR0A = 0x80;	// set to 50% duty
	DDRB |= _BV(PINB3);		// set the OCR0A pin as an output

	/************** ...Timer0 PWM-DAC output *************************/
#endif /* atmega644 */


	/********* TIMER1 is the high-resolution timer, always running *********/

	/* clock source is selected by CS1[2:0] in TCCR1B */
	TCCR1A = 0;		// Mode normal
	/* TCCR1B clock selection starts Timer1 */
#if TIMER1_CLOCK_DIV == 1
	TCCR1B = 0x01;	// Div by 1	: 3.6864MHz 271.3nS each (overflow at 17.77mS)
#elif TIMER1_CLOCK_DIV == 8
	TCCR1B = 0x02;	// Div by 8	: 3.6864MHz / 8 = 460.8KHz = 2.17uS each
#elif TIMER1_CLOCK_DIV == 64
	TCCR1B = 0x03;	// Div by 64 : 3.6864MHz / 64 = 57.6KHz = 17.36uS each
#else
	#error TIMER1_CLOCK_DIV
#endif

}

// Go to sleep function
void
go_sleep()
{
	SetRFMode(RF_MODE_SLEEP);	// main clock not from radio

#if defined(_CPU_SLEEP_)
	/* wait for UART TX to finish */
	while (UCSR0B & (1<<UDRIE0))
		;
#endif

	ENABLE_LOWRES_TIMER_INTERRUPT;

#if defined(_CPU_SLEEP_)
	SLAVE_SLEEP_LED = 1;
	//set_sleep_mode(SLEEP_MODE_PWR_SAVE);	// Macro to enable power save mode
	set_sleep_mode(SLEEP_MODE_IDLE);	// Macro to enable power save mode
	sleep_enable();	// Set the SE (sleep enable) bit
// already enabled	sei();//Enable interrupts (to wake-up with timer/counter2)
	sleep_cpu();	// Put the device in sleep mode
#else
	/* just loop waiting for timer to kick off */

	wakeup_isr_ran = FALSE;

	while (wakeup_isr_ran == FALSE)
		;
#error spinning
#endif

}

#if 0
/* this ISR kicks off to indicate timeout waiting for RF reception */
ISR(TIMER1_COMPA_vect)
{
	/* hit OCR1A */
	RFState |= RF_TIMEOUT;
    RFState &= ~RF_BUSY;
}
#endif

#if 0
ISR(TIMER1_COMPB_vect)
{
	/* hit OCR1B */
}
#endif

/*******************************************************************
** Handle_Irq_CntA : Handles the interruption from the Counter A  **
********************************************************************
** In              : -                                            **
** Out             : -                                            **
*******************************************************************/
#if 0
ISR(TIMER1_OVF_vect){
	RFState |= RF_TIMEOUT;
    RFState &= ~RF_BUSY;
} //End Handle_Irq_CntA
#endif


