#include <stdint.h>

#include "platform.h"
#include "spi.h"
#include "em_usart.h"
#include "em_gpio.h"
#include "em_cmu.h"

#if   ( PLATFORM == STK3200 )

	#define SPI_INTERFACE		USART1
	#define SPI_CLK				cmuClock_USART1
	#define SPI_LOCATION		USART_ROUTE_LOCATION_LOC3

	#define SPI_PIN_CS_PORT		gpioPortC
	#define SPI_PIN_CS			14

	#define SPI_PIN_SCK_PORT	gpioPortC
	#define SPI_PIN_SCK			15

	#define SPI_PIN_MISO_PORT	gpioPortD
	#define SPI_PIN_MISO		6
	
	#define SPI_PIN_MOSI_PORT	gpioPortD
	#define SPI_PIN_MOSI		7

#elif   ( PLATFORM == STK3700 )

	#define SPI_INTERFACE		USART1
	#define SPI_CLK				cmuClock_USART1
	#define SPI_LOCATION		USART_ROUTE_LOCATION_LOC1

	#define SPI_PIN_CS_PORT		gpioPortD
	#define SPI_PIN_CS			3

	#define SPI_PIN_SCK_PORT	gpioPortD
	#define SPI_PIN_SCK			2

	#define SPI_PIN_MISO_PORT	gpioPortD
	#define SPI_PIN_MISO		1

	#define SPI_PIN_MOSI_PORT	gpioPortD
	#define SPI_PIN_MOSI		0

#endif

void SpiInit( void )
{
	USART_InitSync_TypeDef init = USART_INITSYNC_DEFAULT;
	/* Keep CS high to not activate slave */
	// GPIO_PinModeSet(SPI_PIN_CS_PORT, SPI_PIN_CS, gpioModePushPull, 1);
	GPIO_PinModeSet(SPI_PIN_MOSI_PORT, SPI_PIN_MOSI, gpioModePushPull, 1);
	GPIO_PinModeSet(SPI_PIN_MISO_PORT, SPI_PIN_MISO, gpioModeInput, 0);
	GPIO_PinModeSet(SPI_PIN_SCK_PORT, SPI_PIN_SCK, gpioModePushPull, 0);
	
	/* Reset USART just in case */
	USART_Reset(SPI_INTERFACE);
	CMU_ClockEnable(SPI_CLK, true);

	/* Configure to use SPI master with manual CS */
	// init.refFreq	= 16000000;
	init.baudrate	= 2000000;
	init.msbf		= true;
	init.clockMode	= usartClockMode0;
	USART_InitSync(SPI_INTERFACE, &init);
	// SPI_INTERFACE->CTRL |= USART_CTRL_CLKPOL;
	// SPI_INTERFACE->CTRL |= USART_CTRL_CLKPHA;
  
	/* Module USART1 is configured to location 1 */
	SPI_INTERFACE->ROUTE = (SPI_INTERFACE->ROUTE & ~_USART_ROUTE_LOCATION_MASK) | SPI_LOCATION;

	/* Enable signals TX, RX, CLK */
	SPI_INTERFACE->ROUTE |= (USART_ROUTE_TXPEN | USART_ROUTE_RXPEN | USART_ROUTE_CLKPEN);
	// CS USART_ROUTE_CSPEN
}

uint8_t SpiInOut( uint8_t outData )
{
	USART_TypeDef *spi = SPI_INTERFACE;
	USART_Tx(spi, outData);

	/* Wait for transmition to finished */
	while (!(spi->STATUS & USART_STATUS_TXC)) ;

	return USART_Rx(spi);
}
