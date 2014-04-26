#include <string.h>
#include <stdlib.h>
#include "sfr_r81B.h"
//#include "types.h"
//#include "cpu.h"
#include "platform.h"
#include "WSN.h"

char dummy_io_pin;	// this cpu has only a few pins
char dummy_pll_lock;

void
poll_hardware_address()
{
	/* initialization: come up as slave0 by default */
	if (hw_address == HW_ADDRESS__MAX) {

//		hw_address = HW_ADDRESS__MASTER;
		hw_address = HW_ADDRESS__SLAVE0;
//		hw_address = HW_ADDRESS__SLAVE1;
//		hw_address = HW_ADDRESS__SLAVE2;
//		hw_address = HW_ADDRESS__SLAVE3;

		new_hw_address();
	}


}


#define NOP_COUNT_MS	325
void
delay_ms(uint16_t count)
{
    uint16_t i;

    while(count-- != 0)
    {
        for(i = 0; i < NOP_COUNT_MS; i++)
        {
            asm("nop");
            asm("nop");
            asm("nop");
            asm("nop");
            asm("nop");
            asm("nop");
            asm("nop");
            asm("nop");
            asm("nop");
            asm("nop");
        } // nop loop //
    } // delay loop //
} // delay_ms //

void
osc_internal_hispeed()
{
	int i = 0;
	// r8c-1b: hra00: 1=hi-speed on-chip osc ON.  prc0 must be first set
	// r8c-1b: hra01: 1=select hi-speed on-chip osc.  HRA00 must be set first
	// r8c-1b: hra1: Frequency.  0x00=highest freq, 0xff=lowest freq
	// r8c-1b: hra2: fRING-fast mode [0-2]
	/*-----------------------------------------------------------
	-	Set High-speed on-chip oscillator clock to System clock	-
	-----------------------------------------------------------*/
	prc0 = 1;				/* Protect off */
	cm14 = 0;				/* Low-speed on-chip oscillator on (R8C/1B same) */
	//fra2 = 0x00;			/* Selects Divide-by-2 mode */
	//fra00 = 1;				/* High-speed on-chip oscillator on */
	hra00 = 1;				/* High-speed on-chip oscillator on */
	while(i <= 255) i++;	/* This setting is an example of waiting time for the */
							/* oscillation stabilization. Please evaluate the time */
							/* for oscillation stabilization by a user. */
	//fra01 = 1;				/* Selects High-speed on-chip oscillator */
	hra01 = 1;				/* Selects High-speed on-chip oscillator */
	cm16 = 0;				/* No division mode in system clock division */
	cm17 = 0;
	cm06 = 0;				/* CM16 and CM17 enable */
	prc0 = 0;				/* Protect on */
}

void
cpu_init()
{
#ifdef OSC_HISPEED_ONCHIP
	if (hra01 == 0)
		osc_internal_hispeed();

#else	// external crystal or external clock source:
    prc0 = 1;                       // protect off
    cm13 = 1;                       // p4_6, 4_7 to XIN-XOUT
    cm05 = 0;                       // main clock oscillates

    // delay 1 ms to make sure the oscillator settles.  Since we are not
    // running at the speed used to calculate the constants for the delay, this
    // will actually take longer than 1ms running on the low speed internal
    // oscillator, but we don't want to make code size bigger, and this is only
    // done when the device starts up.
    delay_ms(1);
    asm("nop");                     // let it settle
    asm("nop");
    asm("nop");
    asm("nop");

    cm06 = 0;                       // enable cm16 & cm17 divide bits

    // divide system clock by 1
    cm16 = 0;
    cm17 = 0;

    ocd2 = 0;                       // select main clock as system clock
    prc0 = 0;                       // protect on
#endif


	DIR_LED1 = OUTPUT;
}

