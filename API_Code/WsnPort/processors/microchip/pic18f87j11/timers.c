#include "platform.h"
#include "transceiver.h"
#include "io_port_mapping.h"

/*******************************************************************
** EnableClock_HiRes: non-blocking function, sets the timer to    **
** at some future time.                                           **
********************************************************************
** In  : run (0 = cancel the timeout)			                  **
** Out : -		                                                  **
*******************************************************************/
void
EnableClock_HiRes(uint16_t compare_value)
{
	TMR0H = compare_value >> 8;
	TMR0L = compare_value & 0xff;

	/* timer0 is always left running */
	/* the interrupt flag is polled, no isr */

	INTCONbits.TMR0IF = 0;	// clear overflow flag

}

//Enable and set lowres-timer function
void
EnableClock(uint16_t lowres_count)
{
	SET_LOWRES_COUNTER(lowres_count);

	CLEAR_LOWRES_TIMER_FLAG;
}

/*******************************************************************
** Wait : block execution until overflow of timer0.               **
** timer0 has no compare registers, can only generate overflow.   **
********************************************************************
** In  : cntVal		                                              **
** Out : -		                                                  **
*******************************************************************/
void
Wait(uint16_t start_value)
{
	INTCONbits.TMR0IE = 0;

	/* timer0 is always left running */

	TMR0H = start_value >> 8;
	TMR0L = start_value & 0xff;

	INTCONbits.TMR0IF = 0;	// clear overflow flag
	while (INTCONbits.TMR0IF == 0)
			;

	INTCONbits.TMR0IF = 0;	// clear overflow flag
}

void
go_sleep()
{
	SetRFMode(RF_MODE_SLEEP);	// use RF_STANDBY if using clkout from radio

	SLAVE_SLEEP_LED = 1;
	CLEAR_LOWRES_TIMER_FLAG;
	ENABLE_LOWRES_TIMER_INTERRUPT;

#ifdef __DEBUG

	while (PIE1bits.TMR1IE == 1)
			;

#else

	do {
		/* do while lowres timer has not yet expired.
		 * timer isr disables its interrupt */
		Sleep();

	    _asm
		nop
		nop
		_endasm

	} while (PIE1bits.TMR1IE == 1);

#endif
}

void
timers_init()
{
	/********************* Timer0 for "high" resolution *****************/

	// internal instruction clock CLKO (== Fosc/4 ?)
	T0CONbits.T0CS = 0;
	/* 10MHz/4 = 2.5MHz */

	T0CONbits.T08BIT = 0;	// 16bit timer

#if TIMER0_CLOCK_DIV == 8
	T0CONbits.PSA = 0;	// use prescaler
	T0CONbits.T0PS2 = 0;
	T0CONbits.T0PS1 = 1;
	T0CONbits.T0PS0 = 0;
#else
	#error TIMER0_CLOCK_DIV
#endif

	T0CONbits.TMR0ON = 1;	// start timer0


	/********************* Timer1 for "low" resolution *****************/
	T1CONbits.T1OSCEN = 1;	// enable the 32768Hz crystal
	T1CONbits.TMR1CS = 1;	// run from 32768Hz crystal on RC0/T1OSO

	T1CONbits.RD16 = 1;	// low & high accessed atomically

#if TIMER1_CLOCK_DIV == 8
	// divide by 8 for a 4096Hz run rate
	T1CONbits.T1CKPS1 = 1;
	T1CONbits.T1CKPS0 = 1;
#else
	#error TIMER1_CLOCK_DIV
#endif

	T1CONbits.TMR1ON = 1;	// start timer1

	/************************** button debouncer... *********************/
	// 625KHz / 78 = 8012Hz	(1:4 prescale, period=78)
	// 8012Hz / 16 = 500.75Hz (1:16 postscale)
	T2CON = 0x79;	// dont turn on until needed
	PR2 = 78;
	IPR1bits.TMR2IP = 0;	// put into the low-priority ISR

	/***************************** pwm dac... ***************************/
	// use RG3/CCP4 as pwm output.  feeds from timer4
	TRISGbits.TRISG3 = OUTPUT;
	//PR4 = 49;	// number of steps we want.  the pwm period
	PR4 = 12;	// number of steps we want.  the pwm period

	// set clock source for CCP modules: Timer4
	T3CONbits.T3CCP2 = 1;
	T3CONbits.T3CCP1 = 1;

	// leave T4CON prescale at 0 for no clock division

	// configure CCP4 for PWM operation
	CCP4CONbits.CCP4M3 = 1;
	CCP4CONbits.CCP4M2 = 1;

	T4CONbits.TMR4ON = 1;	// start timer4

	// test duty..
	CCPR4L = 0x00;	// bits 9:2
	CCP4CONbits.DCCP4Y = 1;		// lsbit (bit0)
	CCP4CONbits.DCCP4X = 0;		// bit1
}

