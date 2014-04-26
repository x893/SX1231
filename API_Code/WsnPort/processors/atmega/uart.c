#include <avr/interrupt.h>
#include <avr/pgmspace.h>
#include <string.h>
#include "io_port_mapping.h"
#include "types.h"

static char uart_tx_buf_c[64];
static uint8_t uart_txbuf_in_idx;
static uint8_t uart_txbuf_out_idx;

#if defined(atmega644)
static const uint8_t baud = 23;			// Usart speed -- 3.6864MHz crystal installed
#elif defined(atmega644p)
static const uint8_t baud = BAUD250;	// Usart speed -- 4MHz crystal installed
#endif

char text[32];

/********************/

ISR(USART0_UDRE_vect)
{
	UDR0 = uart_tx_buf_c[uart_txbuf_out_idx++];
	if (uart_txbuf_out_idx == sizeof(uart_tx_buf_c))
		uart_txbuf_out_idx = 0;

	if (uart_txbuf_out_idx == uart_txbuf_in_idx) {
		UCSR0B_UDRIE0 = 0;	//	UCSR0B &= ~(1<<UDRIE0);	// transmit interrupt disable
	}
}

//Function used to send data to the CPU (via USB bridge)
static void
USART_send(const uint8_t *buffer, uint8_t size, char from_rom)
{
	int i;

	if (size == 0)
		return;

	UCSR0B_UDRIE0 = 0;	//UCSR0B &= ~(1<<UDRIE0);	// transmit interrupt disable -- start protected section

	for (i = 0; i < size; i++) {
		if (from_rom)
			uart_tx_buf_c[uart_txbuf_in_idx++] = pgm_read_byte(buffer + i);
		else
			uart_tx_buf_c[uart_txbuf_in_idx++] = buffer[i];

		if (uart_txbuf_in_idx == sizeof(uart_tx_buf_c))
			uart_txbuf_in_idx = 0;

		if (uart_txbuf_in_idx == uart_txbuf_out_idx) {
			// buffer is full, enable transmit and wait for buffer to empty
			UCSR0B_UDRIE0 = 1;	//UCSR0B |= (1<<UDRIE0);	// transmit interrupt enable
			while (UCSR0B & (1<<UDRIE0))
				;
		}
	}

	UCSR0B |= (1<<UDRIE0);	// transmit interrupt enable
}


void
USART_send_str(const char *str)
{
	USART_send((uint8_t *)str, strlen(str), FALSE);
}

void
USART_send_str_from_rom(const char *str)
{
	USART_send((uint8_t *)str, strlen_P(str), TRUE);
}

void
uart_init()
{
	// Set baud rate
	UBRR0H = (baud>>8);
	UBRR0L = baud;
	//Enable transmitter, receiver
	UCSR0B = (1<<TXEN0) | (1<<RXEN0);
	//Set frame format: 8data, 1stop bit, parity Even
	UCSR0C = (1<<USBS0) | (1<<UCSZ00) | (1<<UCSZ01);

	uart_txbuf_in_idx = 0;
	uart_txbuf_out_idx = 0;
	//uart_tx_buf_c_full = FALSE;

}

// Function used to receive data from serial port
uint8_t
USART_Receive(void)
{
	// unused variables: unsigned char status, resh, resl;
	// Wait for data to be received
	if (UCSR0A & (1<<RXC0))
		return UDR0;
	else
		return -1;
}

