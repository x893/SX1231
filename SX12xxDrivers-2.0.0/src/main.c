/*
 * THE FOLLOWING FIRMWARE IS PROVIDED: (1) "AS IS" WITH NO WARRANTY; AND 
 * (2)TO ENABLE ACCESS TO CODING INFORMATION TO GUIDE AND FACILITATE CUSTOMER.
 * CONSEQUENTLY, SEMTECH SHALL NOT BE HELD LIABLE FOR ANY DIRECT, INDIRECT OR
 * CONSEQUENTIAL DAMAGES WITH RESPECT TO ANY CLAIMS ARISING FROM THE CONTENT
 * OF SUCH FIRMWARE AND/OR THE USE MADE BY CUSTOMERS OF THE CODING INFORMATION
 * CONTAINED HEREIN IN CONNECTION WITH THEIR PRODUCTS.
 * 
 * Copyright (C) SEMTECH S.A.
 */
/*! 
 * \file	   main.c
 * \brief	  Ping-Pong example application on how to use Semtech's Radio
 *			 drivers.
 *
 * \version	2.0
 * \date	   Nov 21 2012
 * \author	 Miguel Luis
 */
#include <string.h>
#include <stdint.h>
#include <stdbool.h>

#include "platform.h"
#include "led.h"

#if USE_UART
	#include "uart.h"
#endif

#include "radio.h"

#define BUFFER_SIZE	9								// Define the payload size here

static uint16_t BufferSize = BUFFER_SIZE;			// RF buffer size
static uint8_t Buffer[BUFFER_SIZE];					// RF buffer

static uint8_t EnableMaster = true; 				// Master/Slave selection

tRadioDriver *Radio = NULL;

const uint8_t PingMsg[] = "PING";
const uint8_t PongMsg[] = "PONG";

/*
 * Manages the master operation
 */
void OnMaster( void )
{
	uint8_t i;
	
	switch( Radio->Process( ) )
	{
	case RF_RX_TIMEOUT:
		// Send the next PING frame
		Buffer[0] = 'P';
		Buffer[1] = 'I';
		Buffer[2] = 'N';
		Buffer[3] = 'G';
		for( i = 4; i < BufferSize; i++ )
		{
			Buffer[i] = i - 4;
		}
		Radio->SetTxPacket( Buffer, BufferSize );
		break;
	case RF_RX_DONE:
		Radio->GetRxPacket( Buffer, ( uint16_t* )&BufferSize );
	
		if( BufferSize > 0 )
		{
			if( strncmp( ( const char* )Buffer, ( const char* )PongMsg, 4 ) == 0 )
			{
				// Indicates on a LED that the received frame is a PONG
				LedToggle( LED_GREEN );

				// Send the next PING frame			
				Buffer[0] = 'P';
				Buffer[1] = 'I';
				Buffer[2] = 'N';
				Buffer[3] = 'G';
				// We fill the buffer with numbers for the payload 
				for( i = 4; i < BufferSize; i++ )
				{
					Buffer[i] = i - 4;
				}
				Radio->SetTxPacket( Buffer, BufferSize );
			}
			else if( strncmp( ( const char* )Buffer, ( const char* )PingMsg, 4 ) == 0 )
			{ // A master already exists then become a slave
				EnableMaster = false;
				LedOff( LED_RED );
			}
		}			
		break;
	case RF_TX_DONE:
		// Indicates on a LED that we have sent a PING
		LedToggle( LED_RED );
		Radio->StartRx( );
		break;
	default:
		break;
	}
}

/*
 * Manages the slave operation
 */
void OnSlave( void )
{
	uint8_t i;

	switch( Radio->Process( ) )
	{
	case RF_RX_DONE:
		Radio->GetRxPacket( Buffer, ( uint16_t* )&BufferSize );
	
		if( BufferSize > 0 )
		{
			if( strncmp( ( const char* )Buffer, ( const char* )PingMsg, 4 ) == 0 )
			{
				// Indicates on a LED that the received frame is a PING
				LedToggle( LED_GREEN );

			   // Send the reply to the PONG string
				Buffer[0] = 'P';
				Buffer[1] = 'O';
				Buffer[2] = 'N';
				Buffer[3] = 'G';
				// We fill the buffer with numbers for the payload 
				for( i = 4; i < BufferSize; i++ )
				{
					Buffer[i] = i - 4;
				}

				Radio->SetTxPacket( Buffer, BufferSize );
			}
		}
		break;
	case RF_TX_DONE:
		// Indicates on a LED that we have sent a PONG
		LedToggle( LED_RED );
		Radio->StartRx( );
		break;
	default:
		break;
	}
}


/*
 * Main application entry point.
 */
int main( void )
{
	BoardInit( );

	Radio = RadioDriverInit( );
	
	Radio->Init( );

	Radio->StartRx( );
	
	while( 1 )
	{
		if( EnableMaster == true )
		{
			OnMaster( );
		}
		else
		{
			OnSlave( );
		}	
#if( PLATFORM == SX12xxEiger ) && ( USE_UART == 1 )

		UartProcess( );
		
		{
			uint8_t data = 0;
			if( UartGetChar( &data ) == UART_OK )
			{
				UartPutChar( data );
			}
		}
#endif		
	}
#ifdef __GNUC__
	return 0;
#endif
}

