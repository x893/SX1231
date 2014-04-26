#include <stdint.h>

#include "platform.h"
#include "spi.h"

#if   ( PLATFORM == STK3200 )

	#define SPI_INTERFACE		SPI1
	#define SPI_CLK				RCC_APB2Periph_SPI1

	#define SPI_PIN_SCK_PORT	GPIOB
	#define SPI_PIN_SCK			GPIO_Pin_3

	#define SPI_PIN_MISO_PORT	GPIOB
	#define SPI_PIN_MISO		GPIO_Pin_4
	
	#define SPI_PIN_MOSI_PORT	GPIOA
	#define SPI_PIN_MOSI		GPIO_Pin_7

#elif   ( PLATFORM == STK3700 )

	#define SPI_INTERFACE		SPI3
	#define SPI_CLK				RCC_APB1Periph_SPI3

	#define SPI_PIN_SCK_PORT	GPIOB
	#define SPI_PIN_SCK			GPIO_Pin_3

	#define SPI_PIN_MISO_PORT	GPIOB
	#define SPI_PIN_MISO		GPIO_Pin_4

	#define SPI_PIN_MOSI_PORT	GPIOB
	#define SPI_PIN_MOSI		GPIO_Pin_5

#endif

void SpiInit( void )
{
	#error Not implemented
}

uint8_t SpiInOut( uint8_t outData )
{
	#error Not implemented
	return 0;
}
