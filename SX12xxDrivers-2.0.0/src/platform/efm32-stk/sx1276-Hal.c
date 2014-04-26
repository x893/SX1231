#include <stdint.h>
#include <stdbool.h> 

#include "platform.h"
#include "em_gpio.h"

#if defined( USE_SX1276_RADIO )

#include "ioe.h"
#include "spi.h"
#include "../../radio/sx1276-Hal.h"


#if   ( PLATFORM == STK3200 )
	/*! SX1276 RESET I/O definitions */
	#define RESET_IOPORT				gpioPortA
	#define RESET_PIN					1

	/*! SX1276 SPI NSS I/O definitions */
	#define NSS_IOPORT					gpioPortC
	#define NSS_PIN						14

	/*! SX1276 DIO pins  I/O definitions */
	#define DIO0_IOPORT					gpioPortC
	#define DIO0_PIN					0

	#define DIO1_IOPORT					gpioPortC
	#define DIO1_PIN					1

	#define DIO2_IOPORT					gpioPortB
	#define DIO2_PIN					11

#elif ( PLATFORM == STK3700 )

	/*! SX1276 RESET I/O definitions */
	#define RESET_IOPORT				gpioPortB
	#define RESET_PIN					12

	/*! SX1276 SPI NSS I/O definitions */
	#define NSS_IOPORT					gpioPortD
	#define NSS_PIN						3

	/*! SX1276 DIO pins  I/O definitions */
	#define DIO0_IOPORT					gpioPortC
	#define DIO0_PIN					0

	#define DIO1_IOPORT					gpioPortC
	#define DIO1_PIN					3

	#define DIO2_IOPORT					gpioPortB
	#define DIO2_PIN					11

#endif

void SX1276InitIo( void )
{
	// Configure NSS as output
	GPIO_PinModeSet( NSS_IOPORT, NSS_PIN, gpioModePushPull, 1 );

	// Configure radio DIO as inputs
	GPIO_PinModeSet( DIO0_IOPORT, DIO0_PIN, gpioModeInput, 0 );
	GPIO_PinModeSet( DIO1_IOPORT, DIO1_PIN, gpioModeInput, 0 );
	GPIO_PinModeSet( DIO2_IOPORT, DIO2_PIN, gpioModeInput, 0 );
#ifdef DIO3_IOPORT
	GPIO_PinModeSet( DIO3_IOPORT, DIO3_PIN, gpioModeInput, 0 );
#endif
#ifdef DIO4_IOPORT
	GPIO_PinModeSet( DIO4_IOPORT, DIO4_PIN, gpioModeInput, 0 );
#endif
}

void SX1276SetReset( uint8_t state )
{
	if( state == RADIO_RESET_ON )
	{
		GPIO_PinModeSet( RESET_IOPORT, RESET_PIN, gpioModePushPull, 0 );
	}
	else
	{
#if FPGA == 0	
		// Configure RESET as input
		GPIO_PinModeSet( RESET_IOPORT, RESET_PIN, gpioModeInput, 0 );
#endif
	}
}

void SX1276Write( uint8_t addr, uint8_t data )
{
	SX1276WriteBuffer( addr, &data, 1 );
}

void SX1276Read( uint8_t addr, uint8_t *data )
{
	SX1276ReadBuffer( addr, data, 1 );
}

void SX1276WriteBuffer( uint8_t addr, uint8_t *buffer, uint8_t size )
{
	uint8_t i;

	//NSS = 0;
	GPIO_PinOutClear( NSS_IOPORT, NSS_PIN );

	SpiInOut( addr | 0x80 );
	for( i = 0; i < size; i++ )
	{
		SpiInOut( buffer[i] );
	}

	//NSS = 1;
	GPIO_PinOutSet( NSS_IOPORT, NSS_PIN );
}

void SX1276ReadBuffer( uint8_t addr, uint8_t *buffer, uint8_t size )
{
	uint8_t i;

	//NSS = 0;
	GPIO_PinOutClear( NSS_IOPORT, NSS_PIN );

	SpiInOut( addr & 0x7F );

	for( i = 0; i < size; i++ )
	{
		buffer[i] = SpiInOut( 0 );
	}

	//NSS = 1;
	GPIO_PinOutSet( NSS_IOPORT, NSS_PIN );
}

void SX1276WriteFifo( uint8_t *buffer, uint8_t size )
{
	SX1276WriteBuffer( 0, buffer, size );
}

void SX1276ReadFifo( uint8_t *buffer, uint8_t size )
{
	SX1276ReadBuffer( 0, buffer, size );
}

inline uint8_t SX1276ReadDio0( void )
{
	return GPIO_PinInGet( DIO0_IOPORT, DIO0_PIN );
}

inline uint8_t SX1276ReadDio1( void )
{
	return GPIO_PinInGet( DIO1_IOPORT, DIO1_PIN );
}

inline uint8_t SX1276ReadDio2( void )
{
	return GPIO_PinInGet( DIO2_IOPORT, DIO2_PIN );
}

inline uint8_t SX1276ReadDio3( void )
{
#ifdef DIO3_IOPORT
	return GPIO_PinInGet( DIO3_IOPORT, DIO3_PIN );
#else
	return 0;
#endif
}

inline uint8_t SX1276ReadDio4( void )
{
#ifdef DIO4_IOPORT
	return GPIO_PinInGet( DIO4_IOPORT, DIO4_PIN );
#else
	return 0;
#endif
}

inline void SX1276WriteRxTx( uint8_t txEnable )
{
/*
	if( txEnable != 0 )
	{
		IoePinOn( FEM_CTX_PIN );
		IoePinOff( FEM_CPS_PIN );
	}
	else
	{
		IoePinOff( FEM_CTX_PIN );
		IoePinOn( FEM_CPS_PIN );
	}
*/
}

#endif // USE_SX1276_RADIO
