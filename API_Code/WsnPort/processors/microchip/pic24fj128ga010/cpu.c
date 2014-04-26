//#include <p24FJ128GA010.h>
#include "platform.h"
#include "WSN.h"
#ifdef ENABLE_LCD
#include "lcd.h"
#endif

/* Explorer 16 board: pic24fj128ga010 */

/* flash configuration words */
_CONFIG2(FNOSC_PRI & POSCMOD_XT)		// Primary XT OSC (XXX ? with 4x PLL ? XXX)
/* jtag OFF or else RA0 - RA5 non-operational */
_CONFIG1(JTAGEN_OFF & FWDTEN_OFF & WDTPS_PS2)		// JTAG off, watchdog timer off

/***********************************************************************************/
uint8_t dummy_led;

static char prev_RD13;
static char debounce_count;
static char S4_pressed;

void __attribute__((interrupt, auto_psv)) _T4Interrupt(void)
{
	/* interrupt flag must be cleared in software */
	IFS1bits.T4IF = 0;

	LATGbits.LATG1 ^= 1;

	if (PORTDbits.RD13 == 0) {
		if (debounce_count++ >= 10) {
			IEC1bits.T4IE = 0;	// disable timer3 interrupt
			T4CONbits.TON = 0;	// stop timer3
			S4_pressed = TRUE;
		}
	} else {
		/* this is bouncing: the pin went high */
		IEC1bits.T4IE = 0;	// disable timer3 interrupt
		T4CONbits.TON = 0;	// stop timer3
	}

}


void
poll_hardware_address()
{

	/* this is board-specific to the PIC24 explorer board: RD13 (S6) */
	
#ifdef ENABLE_LCD
	LCDProcessEvents();	/* continuously run the LCD state machine */
#endif

	/* initialization: come up as slave0 by default */
	if (hw_address == HW_ADDRESS__MAX) {
		hw_address = HW_ADDRESS__SLAVE0;
		new_hw_address();
	}

	if (prev_RD13 != PORTDbits.RD13) {
		if (PORTDbits.RD13 == 1) {
			/* low to high transition */
		} else {
			/* high to low transition */
			if (T4CONbits.TON == 0) {
				debounce_count = 0;
				IEC1bits.T4IE = 1;	// enable timer3 interrupt
				T4CONbits.TON = 1;	// start timer3
			}
		}
		prev_RD13 = PORTDbits.RD13;
	}

	if (PORTDbits.RD13 == 0) {
		/* dont run what follows while the button is held down */
		return;
	}

	if (S4_pressed) {
		/* RD13/S6 has been pressed and released */
		S4_pressed = FALSE;

		if (++hw_address == HW_ADDRESS__MAX)
			hw_address = HW_ADDRESS__MASTER;

		new_hw_address();
	}

}

void
cpu_init(void)
{
}

