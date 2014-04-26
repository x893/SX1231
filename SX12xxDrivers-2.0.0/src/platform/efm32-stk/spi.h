#ifndef __SPI_H__
#define __SPI_H__

#include <stdint.h>

void SpiInit( void );
uint8_t SpiInOut( uint8_t outData );

#endif //__SPI_H__
