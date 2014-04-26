#include "platform.h"
#include <string.h>

#define kUSART_RX_BUF_SIZE	32
#define kUSART_TX_BUF_SIZE	32

//#pragma udata access fast_vars
// PIC USART Global Variables
// In and out pointers to PIC USART input buffer
/* near */ unsigned char g_USART_RX_buf_in;
/* near */ unsigned char g_USART_RX_buf_out;

// In and out pointers to PIC USART output buffer
/* near */ unsigned char g_USART_TX_buf_in;
/* near */ unsigned char g_USART_TX_buf_out;

//#pragma udata usart_buf=0x280
// PIC hardware USART Rx buffer
unsigned char g_USART_RX_buf[kUSART_RX_BUF_SIZE];

// PIC hardware USART Tx buffer
unsigned char g_USART_TX_buf[kUSART_TX_BUF_SIZE];

char text[32];

/************ isr... ********************************************/

void
uart_isr()
{
	unsigned char usartchar;

	// PIC hardware USART Rx interrupt?
	if (PIR1bits.RCIF)
	{
		// Reading Rx register clears interrupt
		usartchar = RCREG;
		// TODO: Do we need to do more for framing errors?
/*		if (RCSTAbits.FERR)
			printf((rom char *)"USART:Rx Framing Error, ");
		else */ if (RCSTAbits.OERR)
		{
/*			printf((rom char *)"USART:Rx Overrun error");	*/
			RCSTAbits.CREN = 0;  // Clearing CREN clears any Overrun (OERR) errors
			RCSTAbits.CREN = 1;  // Re-enable continuous USART receive
		}
		else // We have a valid byte :-)
		{
			g_USART_RX_buf[g_USART_RX_buf_in++] = usartchar;
			// Check for wrap around
			g_USART_RX_buf_in &= (kUSART_RX_BUF_SIZE - 1);
			//if (g_USART_RX_buf_in == kUSART_RX_BUF_SIZE)
			//   g_USART_RX_buf_in = 0;
			// Check for buffer overrun
/*			if (g_USART_RX_buf_in == g_USART_RX_buf_out)
				printf((rom char *)"USART:RX buffer overrun!\n");	*/
		}
	} // ..rx interrupt

	// PIC hardware USART Tx interrupt?
	// NB: TXIF is almost always true... so need to look at TXIE
	//       to see if the Tx interrupt should be taken and then
	//       only take it if the Tx buffer is empty     (TRMT=1 : transmit shift register empty)
	//if ((PIE1bits.TXIE) && (TXSTAbits.TRMT))
	if (PIE1bits.TXIE && PIR1bits.TXIF)
	{
		// Do we have something to transmit?
		if (g_USART_TX_buf_in != g_USART_TX_buf_out)
		{
			TXREG = g_USART_TX_buf[g_USART_TX_buf_out++];
			// Do we need to wrap around to the start of the buffer?
			g_USART_TX_buf_out &= (kUSART_TX_BUF_SIZE - 1);
			//if (g_USART_TX_buf_out == kUSART_TX_BUF_SIZE)
			//   g_USART_TX_buf_out = 0;
		}
		else
		{
			// Disable Tx interrupts until we have something to send
			// NB: re-enable interrupts in buffer-fill routine to
			// restart Tx!
			PIE1bits.TXIE = 0;
		} 
	} // ..tx interrupt

}

/************ ...isr ********************************************/

void _usart_putc(char c)
{
	// Start of critical region - disable Tx interrupts
	PIE1bits.TXIE = 0;

	// Copy the character into the output buffer
	g_USART_TX_buf[g_USART_TX_buf_in++] = c;

	// Check for buffer wrap around
	g_USART_TX_buf_in &= (kUSART_TX_BUF_SIZE - 1);
	//  if (g_USART_TX_buf_in == kUSART_TX_BUF_SIZE)
	//    g_USART_TX_buf_in = 0;

	// End of critical region - re-enable Tx interrupts 
	PIE1bits.TXIE = 1;

	//  if (g_USART_TX_buf_in == g_USART_TX_buf_out)
	//    printf((rom char *)"USART: TX buffer overrun\n");

} /* _usart_putc */

static void
USART_send_from_rom(rom const uint8_t *buffer, uint8_t size)
{
	int i;

	// Start of critical region - disable Tx interrupts
	PIE1bits.TXIE = 0;

	for (i = 0; i < size; i++) {
		// Copy the character into the output buffer
		g_USART_TX_buf[g_USART_TX_buf_in++] = buffer[i];

		// Check for buffer wrap around
		g_USART_TX_buf_in &= (kUSART_TX_BUF_SIZE - 1);
	//	_usart_putc(buffer[i]);
	}

	// End of critical region - re-enable Tx interrupts 
	PIE1bits.TXIE = 1;
}

void
USART_send(const uint8_t *buffer, uint8_t size)
{
	int i;

	// Start of critical region - disable Tx interrupts
	PIE1bits.TXIE = 0;

	for (i = 0; i < size; i++) {
		// Copy the character into the output buffer
		g_USART_TX_buf[g_USART_TX_buf_in++] = buffer[i];

		// Check for buffer wrap around
		g_USART_TX_buf_in &= (kUSART_TX_BUF_SIZE - 1);
	//	_usart_putc(buffer[i]);
	}

	// End of critical region - re-enable Tx interrupts 
	PIE1bits.TXIE = 1;
}

void
USART_send_str_from_rom(const char rom *str)
{
	USART_send_from_rom((rom uint8_t *)str, strlenpgm(str));
}

void
USART_send_str(const char *str)
{
	USART_send((uint8_t *)str, strlen(str));
}

void
uart_init()
{
	TXSTA = 0;           // Reset USART registers to POR state
	RCSTA = 0;
	RCSTAbits.CREN = 1;
	TXSTAbits.BRGH = 1;
	// 66: 9600 at 10MHz clock
	SPBRG1 = 65;       // Write baudrate to SPBRG1
	SPBRGH1 = 1; // For 16-bit baud rate generation
	TXSTAbits.TXEN = 1;  // Enable transmitter
	RCSTAbits.SPEN = 1;  // Enable receiver

	// And the USART TX and RX buffer management
	g_USART_RX_buf_in = 0;
	g_USART_RX_buf_out = 0;
	g_USART_TX_buf_in = 0;
	g_USART_TX_buf_out = 0;

	// USART interrupts
	IPR1bits.RCIP = 0;   // USART Rx on low priority interrupt
	PIE1bits.RCIE = 1;   // Enable Rx interrupts
	IPR1bits.TXIP = 0;   // USART Tx on low priority interrupt
	PIE1bits.TXIE = 0;   // Disable Tx interrupts until we need to send

}

