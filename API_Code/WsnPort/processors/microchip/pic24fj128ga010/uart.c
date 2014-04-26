#include "platform.h"
#include <string.h>
#include <stdlib.h>	// for abs()

#define kUSART_RX_BUF_SIZE	32
#define kUSART_TX_BUF_SIZE	64

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
void __attribute__((interrupt, auto_psv)) _U2RXInterrupt(void)
{
	unsigned char usartchar;

#if 0
	// PIC hardware USART Rx interrupt?
	if (IFS1bits.U2RXIF)
	{
	} // ..rx interrupt
#endif

		// Reading Rx register clears interrupt
		usartchar = U2RXREG;
		// TODO: Do we need to do more for framing errors?
/*		if (RCSTAbits.FERR)
			printf((rom char *)"USART:Rx Framing Error, ");
		else */ if (U2STAbits.OERR)
		{
/*			printf((rom char *)"USART:Rx Overrun error");	*/
			U2STAbits.OERR = 0;
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
}

//void _ISRFAST __attribute__((interrupt, auto_psv)) _U2TXInterrupt(void)
void __attribute__((interrupt, auto_psv)) _U2TXInterrupt(void)
{
	while (U2STAbits.UTXBF == 0)
	{	// while transmit fifo isnt full

		// Do we have something to transmit?
		if (g_USART_TX_buf_in != g_USART_TX_buf_out)
		{
			U2TXREG = g_USART_TX_buf[g_USART_TX_buf_out++];
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
			IEC1bits.U2TXIE = 0;
			break;
		} 
	}

	IFS1bits.U2TXIF = 0;

}

/************ ...isr ********************************************/

void _usart_putc(char c)
{
	// Start of critical region - disable Tx interrupts
	IEC1bits.U2TXIE = 0;

	// Copy the character into the output buffer
	g_USART_TX_buf[g_USART_TX_buf_in++] = c;

	// Check for buffer wrap around
	g_USART_TX_buf_in &= (kUSART_TX_BUF_SIZE - 1);
	//  if (g_USART_TX_buf_in == kUSART_TX_BUF_SIZE)
	//    g_USART_TX_buf_in = 0;

	// End of critical region - re-enable Tx interrupts 
	IEC1bits.U2TXIE = 1;

	//  if (g_USART_TX_buf_in == g_USART_TX_buf_out)
	//    printf((rom char *)"USART: TX buffer overrun\n");

} /* _usart_putc */

void
USART_send(const uint8_t *buffer, uint8_t size)
{
	int i;

	// Start of critical region - disable Tx interrupts
	IEC1bits.U2TXIE = 0;

	for (i = 0; i < size; i++) {
		// Copy the character into the output buffer
		g_USART_TX_buf[g_USART_TX_buf_in++] = buffer[i];

		// Check for buffer wrap around
		g_USART_TX_buf_in &= (kUSART_TX_BUF_SIZE - 1);
	//	_usart_putc(buffer[i]);
	}

	// End of critical region - re-enable Tx interrupts 
	IEC1bits.U2TXIE = 1;
}

void
USART_send_str(const char *str)
{
	USART_send((uint8_t *)str, strlen(str));
}

void
uart_init()
{
//	TXSTA = 0;           // Reset USART registers to POR state
//	RCSTA = 0;
//	RCSTAbits.CREN = 1;
	U2MODEbits.BRGH = 0;
	// 25: 9600 at 4MHz clock
	U2BRG = 25;       // Write baudrate to SPBRG1
	U2MODEbits.UARTEN = 1;
	U2STAbits.UTXEN = 1;	// UTXEN set after UARTEN
	//RCSTAbits.SPEN = 1;  // Enable receiver
	// U2MODEbits.PDSEL left at default 00 for 8bits no parity
	// U2MODEbits.STSEL left at default 0 for one stop bit

	IFS1bits.U2TXIF = 0;

	// And the USART TX and RX buffer management
	g_USART_RX_buf_in = 0;
	g_USART_RX_buf_out = 0;
	g_USART_TX_buf_in = 0;
	g_USART_TX_buf_out = 0;

	// USART interrupts
//	IPR1bits.RCIP = 0;   // USART Rx on low priority interrupt
//	PIE1bits.RCIE = 1;   // Enable Rx interrupts
//	IPR1bits.TXIP = 0;   // USART Tx on low priority interrupt
//	PIE1bits.TXIE = 0;   // Disable Tx interrupts until we need to send

}

/************************** libc functions.. ***********************************************/

#define BUFLEN	20
int
_ltoa(long val, char *buffer)
{
    char           tempc[BUFLEN];
    register char *bufptr;
    register int   neg = val < 0;
    register long  uval = val;

    *(bufptr = &tempc[BUFLEN - 1]) = 0;

    do {*--bufptr = abs(uval % 10) + '0';}  while(uval /= 10);
    if (neg) *--bufptr = '-';

    memcpy(buffer,bufptr, uval = (tempc + BUFLEN) - bufptr);
    return uval - 1;    /* DON'T COUNT NULL TERMINATION */
}

