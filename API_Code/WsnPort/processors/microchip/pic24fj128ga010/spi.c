#include "platform.h"

uint8_t
SpiInOut(uint8_t tx_byte)
{
	// SSP1BUF is the buffer
	SPI1BUF = tx_byte;

	// reception: BF bit, SSP1STAT<0>
	while (SPI1STATbits.SPIRBF == 0)
		;

	return SPI1BUF;
}

void
SPIInit()
{
#ifdef SX1211
	NSS_CONFIG = 1;
	NSS_DATA = 1;

	DIR_NSS_DATA = OUTPUT;
	DIR_NSS_CONFIG = OUTPUT;
#elif defined SX1231
	NSS = 1;
	DIR_NSS = OUTPUT;
#endif

	/* fifo modes: SPI will be connected to the register/fifo port of the transceiver */

	// pimary prescale: 4MHz / 4 = 1MHz
	SPI1CON1bits.PPRE1 = 1;
	SPI1CON1bits.PPRE0 = 0;

	// secondary prescale : no division
	SPI1CON1bits.SPRE2 = 1;
	SPI1CON1bits.SPRE1 = 1;
	SPI1CON1bits.SPRE0 = 1;

	// clock phase/polarity settings for SX1211
	SPI1CON1bits.CKP = 0;
	SPI1CON1bits.CKE = 1;
	SPI1CON1bits.SMP = 1;	// sample on middle of data output time

	SPI1CON1bits.MSTEN = 1;
	SPI1STATbits.SPIROV = 0;
	SPI1STATbits.SPIEN = 1;
}
