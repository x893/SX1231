#include "platform.h"

void
SPIInit()
{
#if defined SX1211
	NSS_CONFIG = 1;
	NSS_DATA = 1;
	DIR_NSS_DATA = OUTPUT;
	DIR_NSS_CONFIG = OUTPUT;
#elif defined SX1231
	NSS = 1;
	DIR_NSS = OUTPUT;
#endif

	/* fifo modes: MSSP will be connected to the register/fifo port of the transceiver */

	TRISCbits.TRISC3 = OUTPUT;	// SCK as output
	TRISCbits.TRISC5 = OUTPUT;	// SDO as output (MOSI)

	//SSP1CON1bits.SSPM1 = 1;		// master mode: fosc/64
	SSP1CON1bits.SSPM0 = 1;		// master mode: fosc/16 

	// clock phase/polarity settings for SX1211
	SSP1CON1bits.CKP = 0;
	SSP1STATbits.CKE = 1;
	SSP1STATbits.SMP = 0;	// input sample at middle of data output time

	SSP1CON1bits.SSPEN = 1;

}

uint8_t
SpiInOut(uint8_t tx_byte)
{
	// SSP1BUF is the buffer
	SSP1BUF = tx_byte;

	// reception: BF bit, SSP1STAT<0>
	while (SSP1STATbits.BF == 0)
		;

	return SSP1BUF;
}

