//#include "platform.h"
#include "transceiver.h"
#include "io_port_mapping.h"
#include "WSN.h"

/******************************* isr.. ********************************************/
void __attribute__((interrupt, auto_psv)) _T1Interrupt(void)
{
	/* lowres timer interrupt: wakeup from sleep */

	ALARM_LED = 0;
	RX_OK_LED = 0;
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

}
/**********************************************************************************/

 
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
	TMR2 = 0;
	PR2 = compare_value;

	/* Timer2 "Type B" */
	/* the interrupt flag is polled, no isr */

	IFS0bits.T2IF = 0;	// clear overflow flag

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
Wait(uint16_t compare_value)
{
	IEC0bits.T2IE = 0;

	TMR2 = 0;
	PR2 = compare_value;

	IFS0bits.T2IF = 0;	// clear flag
	while (IFS0bits.T2IF == 0)
			;

	IFS0bits.T2IF = 0;	// clear flag
}

void
go_sleep()
{
	SetRFMode(RF_MODE_SLEEP);	// use RF_STANDBY if using clkout from radio

	SLAVE_SLEEP_LED = 1;
	CLEAR_LOWRES_TIMER_FLAG;
	ENABLE_LOWRES_TIMER_INTERRUPT;

#ifdef __DEBUG

	while (IEC0bits.T1IE == 1)
			;

#else

	do {
		Idle();
	} while (IEC0bits.T1IE == 1);

#endif
}

void
timers_init()
{
	/********************* Timer1 for "low" resolution *****************/
	T1CONbits.TCKPS1 = 1;	// prescaler 64
	T1CONbits.TCKPS0 = 0;	// prescaler 64
	T1CONbits.TCS = 1;	// clock from 32768Hz crystal on T1CK/SOSCO
	T1CONbits.TSYNC = 0;	// sleep mode operation: do not synchronize this crystal
	//timer period into PR1
	T1CONbits.TON = 1;

// Unlock Registers
asm volatile (  "MOV #OSCCON, w1  \n" \
              "MOV #0x46, w2    \n" \
              "MOV #0x57, w3    \n" \
              "MOV.b w2, [w1]   \n" \
              "MOV.b w3, [w1]   \n" \
              "BSET OSCCON,     #1" );	// SOSCEN: enable the 32768Hz oscillator


/*	OSCCON = 0x46;	// unlock
	OSCCON = 0x57;	// unlock
	OSCCONbits.SOSCEN = 1;	// enable the 32768Hz oscillator
	*/

	/********************* Timer2 for "high" resolution *****************/
	T2CONbits.T32 = 0;	// 16bit operation for both timer2 and timer3
	T2CONbits.TCKPS1 = 0;	// prescaler 8 (500KHz for 2 microseconds each)
	T2CONbits.TCKPS0 = 1;	// prescaler 8
	T2CONbits.TCS = 0;	// clock from internal Fosc/2  (8MHz/2 = 4MHz)
	T2CONbits.TON = 1;


	/************************** Timer3 button debouncer... *********************/
	// 4MHz / 256 = 15.625KHz = 64 microseconds each
	// 2mS / 0.064 = 31.25
	T4CONbits.TCKPS1 = 1;	// prescaler 256
	T4CONbits.TCKPS0 = 1;	// prescaler 256
	T4CONbits.TCS = 0;	// clock from internal Fosc/2  (8MHz/2 = 4MHz)
	PR4 = 31;	// for 2mS period
	// turn timer on when button pressed: T2CONbits.TON = 1;


	/************************** pwm dac.. *******************************/
/*	* OC3 for PWMDAC
	* 1) PWM period set via PRy
	* 2) write duty cycle to OCxRS
	* 3) write OC1R with the initial duty cycle
	* 4) no interrupts
	* 5) OCM<2:0> in OCxCON<2:0> to set to PWM mode
	* 6) set TMRy prescale value. enable via TON in TxCON<15>	*/

	T3CONbits.TCKPS1 = 0;	// prescaler 1
	T3CONbits.TCKPS0 = 0;	// prescaler 1
	T3CONbits.TCS = 0;	// clock from internal Fosc/2  (8MHz/2 = 4MHz)
	PR3 = 49;	// number of steps we want (divides the clock source Fcy)

	// initial duty cycle
	OC3RS = 25;
	OC3R = 25;

	// set output to PWM mode
	OC3CONbits.OCM2 = 1;
	OC3CONbits.OCM1 = 1;
	OC3CONbits.OCM0 = 0;

	OC3CONbits.OCTSEL = 1;	// source from Timer3

	T3CONbits.TON = 1;
}

