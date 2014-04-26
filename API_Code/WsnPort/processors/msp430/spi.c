#include "types.h"
#include "io_port_mapping.h"
#include <msp430x14x.h>
#include "spi.h"

/*******************************************************************
** SpiInOut : Sends a byte from the SPI bus				          **
********************************************************************
** In  : outputByte                                               **
** Out : -		                                                  **
*******************************************************************/
uint8_t SpiInOut(uint8_t outputByte)
{
	while (!(U1TCTL & TXEPT))
			;	// wait here if was already transmitting

	TXBUF1 = outputByte;

	while (!(IFG2 & URXIFG1))
			;	// wait here until received a byte

	return RXBUF1;
}

SPIInit()
{
#if defined SX1231
	NSS = 1;
	DIR_NSS = 1;
#elif defined SX1211
	NSS_CONFIG = 1;
	NSS_DATA = 1;

	DIR_NSS_DATA = 1;
	DIR_NSS_CONFIG = 1;
#endif


	psel5_1 = 1;	// SIMO1
	psel5_2 = 1;	// SOMI1
	psel5_3 = 1;	// UCLK1
	U1CTL = CHAR + SYNC + MM + SWRST;	// 8-bit, SPI, Master
	// for sx1211: CKPH=1	CKPL=0
	U1TCTL = CKPH + SSEL1 + STC;		// Polarity, SMCLK, 3-wire
	U1BR0 = 0x09;						// SPICLK = SMCLK/x
	U1BR1 = 0x00;
	U1MCTL = 0x00;
	/* transmit (and receive) enable : USPIE1 */
	ME2 |= USPIE1;						// USART1 SPI-mode Module enable
	U1CTL &= ~SWRST;					// SPI enable
}

