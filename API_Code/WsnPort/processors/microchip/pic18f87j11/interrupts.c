//#include "platform.h"
#include "WSN.h"

extern char S2_pressed;	// from cpu.c

#pragma interruptlow LowISR
void
LowISR(void)
{
#if 0
	if (SX1211_IRQ1_INTERRUPT_FLAG == 1) {
		SX1211_IRQ1_INTERRUPT_FLAG == 0;
	} //..if (SX1211_IRQ1_INTERRUPT_FLAG == 1)

	if (SX1211_IRQ0_INTERRUPT_FLAG == 1) {
		SX1211_IRQ0_INTERRUPT_FLAG == 0;
	} //..if (SX1211_IRQ0_INTERRUPT_FLAG == 1)
#endif

	uart_isr();


	if (PIR1bits.TMR2IF) {

		//DEBOUNCE_LED ^= 1;

		if (PORTAbits.RA5 == 0) {
			if (debounce_count++ >= 10) {
				PIE1bits.TMR2IE = 0;	// disable timer2 interrupt
				T2CONbits.TMR2ON = 0;	// stop timer2
				S2_pressed = TRUE;
			}
		} else {
			/* this is bouncing: the pin went high */
			PIE1bits.TMR2IE = 0;	// disable timer2 interrupt
			T2CONbits.TMR2ON = 0;	// stop timer2
		}

		PIR1bits.TMR2IF = 0;
	}	// ..timer2
}

/*****************************************************************************************/

#pragma interrupt HighISR
void
HighISR(void)
{
	if (PIR1bits.TMR1IF) {
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

		PIR1bits.TMR1IF = 0;
	} // ..if timer1 overflow

}

/*****************************************************************************************/

#pragma code highVector=0x08
void HighVector (void)
{
    _asm goto HighISR _endasm
}
#pragma code /* return to default code section */

#pragma code lowhVector=0x18
void LowVector (void)
{
    _asm goto LowISR _endasm
}
#pragma code /* return to default code section */

