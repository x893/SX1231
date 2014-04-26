#include "platform.h"
#include <string.h>
#include <stdlib.h>

#define kUSART_RX_BUF_SIZE	32
#define kUSART_TX_BUF_SIZE	64

unsigned char g_USART_RX_buf_in;
unsigned char g_USART_RX_buf_out;
unsigned char g_USART_TX_buf_in;
unsigned char g_USART_TX_buf_out;

unsigned char g_USART_RX_buf[kUSART_RX_BUF_SIZE];
unsigned char g_USART_TX_buf[kUSART_TX_BUF_SIZE];
char text[32];

void USART1_IRQHandler(void)
{
#warning "Not implemented"
/*
	unsigned char usartchar;
	// Reading Rx register clears interrupt
	usartchar = USART1->DR;
	if (USART1->SR & OERR)
	{
		usartchar = USART1->DR;
	}
	else
	{
		g_USART_RX_buf[g_USART_RX_buf_in++] = usartchar;
		// Check for wrap around
		g_USART_RX_buf_in &= (kUSART_RX_BUF_SIZE - 1);
	}

	if (USART1-SR & TXE == 0)
	{	// Do we have something to transmit?
		if (g_USART_TX_buf_in != g_USART_TX_buf_out)
		{
			USART1->DR = g_USART_TX_buf[g_USART_TX_buf_out++];
			g_USART_TX_buf_out &= (kUSART_TX_BUF_SIZE - 1);
		}
		else
		{
			// disable IRQ
		} 
	}
*/
}

void
_usart_putc(char c)
{
	g_USART_TX_buf[g_USART_TX_buf_in++] = c;
	g_USART_TX_buf_in &= (kUSART_TX_BUF_SIZE - 1);
}

void
USART_send(const uint8_t *buffer, uint8_t size)
{
	int i;
	for (i = 0; i < size; i++)
	{
		g_USART_TX_buf[g_USART_TX_buf_in++] = buffer[i];
		g_USART_TX_buf_in &= (kUSART_TX_BUF_SIZE - 1);
	}
}

void
USART_send_str_from_rom(const char rom_ptr *str)
{
	USART_send((uint8_t *)str, strlen(str));
}

void
USART_send_str(const char *str)
{
	USART_send((uint8_t *)str, strlen(str));
}

void
uart_init()
{
	// And the USART TX and RX buffer management
	g_USART_RX_buf_in  = 0;
	g_USART_RX_buf_out = 0;
	g_USART_TX_buf_in  = 0;
	g_USART_TX_buf_out = 0;
}

#define BUFLEN	20
int
_ltoa(long val, char *buffer)
{
    char           tempc[BUFLEN];
    register char *bufptr;
    register int   neg = val < 0;
    register long  uval = val;

    *(bufptr = &tempc[BUFLEN - 1]) = 0;

    do
	{
		*--bufptr = abs(uval % 10) + '0';
	} while(uval /= 10);
    if (neg) *--bufptr = '-';

    memcpy(buffer,bufptr, uval = (tempc + BUFLEN) - bufptr);
    return uval - 1;    /* DON'T COUNT NULL TERMINATION */
}
