#include "platform.h"
#include "WSN.h"

#pragma config	FOSC = HS
#pragma config	STVREN = OFF, XINST = ON, WDTEN = OFF, CP0 = OFF, FCMEN = OFF, IESO = OFF
#pragma config	MODE = MM	// microcontroller mode, external bus disabled

uint8_t dummy_led;

char S2_pressed;
char prev_RA5;
char debounce_count;

#if defined ENABLE_LCD
void lcd_task(void);	// from lcd.c
#endif

void
poll_hardware_address()
{

	/* this function is board-specific to the PIC18 explorer board: RA5 (S2) */

#if defined ENABLE_LCD
	lcd_task();	/* run the LCD state machine */
#endif

	/* initialization: come up as slave0 by default */
	if (hw_address == HW_ADDRESS__MAX) {
		hw_address = HW_ADDRESS__SLAVE0;
		new_hw_address();
	}

	if (prev_RA5 != PORTAbits.RA5) {
		if (PORTAbits.RA5 == 1) {
			/* low to high transition */
		} else {
			/* high to low transition */
			if (T2CONbits.TMR2ON == 0) {
				debounce_count = 0;
				PIE1bits.TMR2IE = 1;	// enable timer2 interrupt
				T2CONbits.TMR2ON = 1;	// start timer2
			}
		}
		prev_RA5 = PORTAbits.RA5;
	}

	if (PORTAbits.RA5 == 0) {
		/* dont run what follows while the button is held down */
		return;
	}

	if (S2_pressed) {
		/* RA5/S2 has been pressed and released */
		S2_pressed = FALSE;

		if (++hw_address == HW_ADDRESS__MAX)
			hw_address = HW_ADDRESS__MASTER;

		new_hw_address();
	}

}

void
cpu_init(void)
{
	// enable interrupt priorities
	RCONbits.IPEN = 1;

	// enable all low-priority interrupts
	INTCONbits.GIEL = 1;
	INTCONbits.GIEH = 1;

	// SX1211-IRQ1 is on RB3/INT3
	TRISBbits.TRISB3 = INPUT;	//
	INTCON2bits.INTEDG3 = 1;	// rising edge
	INTCON2bits.INT3IP = 0;		// low priority

#if 0
	// SX1211-IRQ0 is on RB0/INT0
	TRISBbits.TRISB0 = INPUT;
	INTCON2bits.INTEDG0 = 1;	// rising edge
	// leave INT0 at high-priority
#endif

	// ANx pins come up as analog port from reset
	WDTCONbits.ADSHR = 1;	// select alternate registers to get REFOCON
	ANCON0bits.PCFG4 = 1;	// enable RA4 as digital i/o
	ANCON0bits.PCFG2 = 1;	// enable RA2 as digital i/o
	ANCON0bits.PCFG0 = 0;	// analog input for pot
	WDTCONbits.ADSHR = 0;	// restore to default SFR

	TRISAbits.TRISA5 = INPUT;	// S2

	// sleep wakeup: timer1 (lowres timer) interrupt high priority
	IPR1bits.TMR1IP = 1;

	// keep peripherials on during sleep
	OSCCONbits.IDLEN = 1;
}

