//#include "cpu.h"
#include <msp430x14x.h>
#include "platform.h"
#include "WSN.h"

uint8_t dummy_led;

void
poll_hardware_address()
{
	/* initialization: come up as slave0 by default */
	if (hw_address == HW_ADDRESS__MAX) {

		hw_address = HW_ADDRESS__MASTER;
//		hw_address = HW_ADDRESS__SLAVE0;
//		hw_address = HW_ADDRESS__SLAVE1;
//		hw_address = HW_ADDRESS__SLAVE2;
//		hw_address = HW_ADDRESS__SLAVE3;

		new_hw_address();
	}


}

#if 0
#define BUFLEN	20
int _ltoa(long val, char *buffer, int radix)
{
    char           tempc[BUFLEN];
    register char *bufptr;
    register int   neg = val < 0;
    register long  uval = val;

    *(bufptr = &tempc[BUFLEN - 1]) = 0;

    do {*--bufptr = abs(uval % 10) + '0';}  while(uval /= 10);
    if (neg) *--bufptr = '-';

    memcpy(buffer,bufptr, uval = (tempc + BUFLEN) - bufptr);
    return uval - 1;    /* DON'T COUNT NULL TERMINATION */
}
#endif

void
reset_cpu()
{
	WDTCTL = WDTPW;	// wdt enable

	while (1) 
		;
}

void
cpu_init()
{
	int i;

	/***** SX1211 clkout is the clock source, must be enabled prior *****/
	//BCSCTL1 |= XTS;				// ACLK = LFXT1 HF XTAL
	do {
		IFG1 &= ~OFIFG;			// Clear OSC fault flag 

		for (i = 0; i < 10; i++)
			_NOP();				// delay to ensure startup 

	} while (IFG1 & OFIFG);

	BCSCTL2 |= SELM_2 | SELS;			// (CPU) MCLK = XT2CLK, SMCLK = XT2CLK
}

