#include "io_port_mapping.h"
#include "platform.h"

#if defined(atmega644p)
static	uint8_t  SPI_MODE = U1_SPI;		// USART SPI Mode -- U1_SPI for ATmega644P
#endif


/*******************************************************************
** SPIInit : This routine initializes the SPI in normal SPI   or  **
**           SPI Hardware drived by USART		                  **
********************************************************************
** In  : Mode (normal SPI or USART Spi)                           **
** Out : -                                                        **
*******************************************************************/
void
SPIInit()
{
#if defined(atmega644)
#if defined SX1211
	NSS_CONFIG = 1;
	NSS_DATA = 1;

	DIR_NSS_DATA = 1;
	DIR_NSS_CONFIG = 1;

	// max 1MHz SCLK for sx1211
	SPCR = 0x51;	// 01010001b 0x51	SPR=1 for f/16
#elif defined SX1231
	NSS = 1;
	DIR_NSS = 1;

	// max 10MHz SCLK for sx1231
	SPCR = 0x50;	// 01010001b 0x51	SPR=1 for f/4
#endif

	DIR_MOSI = 1;	// master out, slave in
	DIR_SCK = 1;	// serial clock
	DIR_MISO = 0;	// master in, slave out
	DIR_SSn = 1;	// slave select as output

	//REG_P_SPI |= (_BV(DD_MOSI) | _BV(DD_SCK) | _BV(PINB4)) & ~(1<<DD_MISO); //Set MOSI and SCK outputs, MISO input
	// PINB4: because its a slave select pin. If its an input in master mode then a low means another master is using the SPI bus.
	//REG_P_NSS |=_BV(DD_NSS_C)|_BV(DD_NSS_D); // Set nss_d & nss_c outputs
#elif defined(atmega644p)
if(SPI_MODE==U0_SPI){
	REG_UBRR1=0;
	//Setting the XCKn port pin as an output, enables master mode
	P_USART0_A |= _BV(DD_SCK_USART0); //SCK output
	REG_P_USART0_A |= _BV(DD_MOSI_USART0)&~(1<<DD_MISO_USART0);//Set MOSI output, MISO input
	//Set MSPI mode of operation and SPI data mode 0
	REG_UCSR0C = USART0_CONFIG_MODE0;
	//Enable receiver and transmitter
	REG_UCSR0B = USART0_CONFIG_MODE1;
	//Set baud rate
	//IMPORTANT: The Baud Rate must be set after the transmitter is enabled
	REG_UBRR0 = USART_SPI_BAUD;
	REG_P_NSS |=_BV(DD_NSS_C)|_BV(DD_NSS_D); // Set nss_d & nss_c outputs
	}

else{//U1_SPI
	//Setting the XCKn port pin as an output, enables master mode
	REG_UBRR1=0;
	REG_P_USART1|=(_BV(DD_SCK_USART1)|_BV(DD_MOSI_USART1))&~(1<<DD_MISO_USART1); //Set MOSI and SCK outputs, MISO input
	//Set MSPI mode of operation and SPI data mode 0
	REG_UCSR1C = USART1_CONFIG_MODE0;
	//Enable receiver and transmitter
	REG_UCSR1B = USART1_CONFIG_MODE1;
	//Set baud rate
	//IMPORTANT: The Baud Rate must be set after the transmitter is enabled
	REG_UBRR1 = USART_SPI_BAUD;
	REG_P_NSS |=_BV(DD_NSS_C)|_BV(DD_NSS_D); // Set nss_d & nss_c output
	}
#endif
}

/*******************************************************************
** SpiInOut : Sends a byte from the SPI bus			          **
********************************************************************
** In  : outputByte                                               **
** Out : -		                                                  **
*******************************************************************/
uint8_t
SpiInOut(uint8_t outputByte)
{
#if defined(atmega644)

	SPDR = outputByte;

	while (!(SPSR & (1<<SPIF)))
			; //Wait for trasmission to complete

	return SPDR; // Get and return received data from buffer

#elif defined(atmega644p)
if(SPI_MODE==U0_SPI){

	//Wait for empty transmit buffer
	while(!(REG_UCSR0A&(1<<REG_UDRE0)) )
			;

	//Put data into buffer, sends the data
	REG_UDR0 = outputByte;

	//Wait for data to be transmitted
	while (!(REG_UCSR0A&(1<<REG_TXC0)))
			;

	REG_UCSR0A|=(1<<REG_TXC0);//Clear TX complete flag
	//Get and return received data from buffer
	return(REG_UDR0);
	}
else{//U1_SPI
	//Wait for empty transmit buffer
	while(!(REG_UCSR1A&(1<<REG_UDRE1)) );
	//Put data into buffer, sends the data
	REG_UDR1 = outputByte;
	//Wait for data to be transmitted
	while (!(REG_UCSR1A&(1<<REG_TXC1)));
	REG_UCSR1A|=(1<<REG_TXC1);//Clear TX complete flag
	//Get and return received data from buffer
	return(REG_UDR1);
	}
#endif
} // uint8_t SpiInOut (uint8_t outputByte)

