#include "platform.h"

uint8_t
SpiInOut(uint8_t tx_byte)
{
	// SSP1BUF is the buffer
	SPI1->DR = tx_byte;
	// reception: BF bit, SSP1STAT<0>
	while (SPI1->SR & 0x01)
		;
	return SPI1->DR;
}

void
SPIInit()
{
}
