#include "types.h"
#include "sfr_r81B.h"
#include <string.h>

#include "platform.h"

#define UART_TX_INT_LEVEL	0x03

typedef enum
{
    STOP_BITS_1 = 0x00,             //!< 1 stop bit
    STOP_BITS_2 = 0x10              //!< 2 stop bit
} stop_bits_t;


typedef enum
{
    DATA_BITS_7 = 0x04,             //!< 7 data bits
    DATA_BITS_8 = 0x05,             //!< 8 data bits
    DATA_BITS_9 = 0x06,             //!< 9 data bits
} data_bits_t;


typedef enum
{
    PARITY_NONE = 0x00,             //!< No parity
    PARITY_EVEN = 0x60,             //!< Even parity
    PARITY_ODD = 0x40               //!< Odd parity
} parity_t;

//! The baud rate numbers are assuming a 20Mhz system clock divided by 1
//! Note that the -1 is done when the uart brg register is set
typedef enum
{
/*
#define R81B_CLK	3686400
#define _BAUD(bps)	((R81B_CLK/(float)bps)/16)	*/
    BAUD_115200 = 2,	//!< Baud rate of 115200
    BAUD_38400 = 6,		//!< Baud rate of 38400
    BAUD_19200 = 12,	//!< Baud rate of 19200
    BAUD_9600 = 24,		//!< Baud rate of 9600
} baud_rate_t;


/*************************************************************************/

static char uart_tx_buf_c[64];
static uint8_t uart_txbuf_out_idx;
static uint8_t uart_txbuf_in_idx;
static char txbuf_empty;	// flag

/*************************************************************************/

#define TRANSMIT_OLDEST_BYTE	\
	u0tbl = uart_tx_buf_c[uart_txbuf_out_idx++];	\
	if (uart_txbuf_out_idx == sizeof(uart_tx_buf_c))	\
		uart_txbuf_out_idx = 0


#pragma interrupt uart_tx_isr
void
uart_tx_isr(void)
{

	if (uart_txbuf_out_idx >= sizeof(uart_tx_buf_c))
		asm("brk");

	TRANSMIT_OLDEST_BYTE;

	if (uart_txbuf_out_idx == uart_txbuf_in_idx) {
		s0tic = 0;	// transmit interrupt disable
		txbuf_empty = TRUE;
	}

}


/*************************************************************************/

void
uart_init()
{

	// Using internal clock
    u0mr = PARITY_NONE | STOP_BITS_1 | DATA_BITS_8;

    /*
        Setting UART0 transmit/receive control register 0
        b7 : Transfer LSb first
        b6 : Transmit on falling edge, receive on rising edge
        b5 : TxDi pin is CMOS output
        b4 : Reserved, write 0
        b3 : Transmit register empty flag (Read only value)
        b2 : Reserved, write 0
        b1-b0 : f8 is the count source
    */
    u0c0 = 0x08;

    // disable uart, and clear buffers.
    u0c1 = 0x02;	// bit1: tx buffer empty=1

	ucon = 0x00;
    
	u0brg = BAUD_9600 - 1;

    // set rx/tx interrupt priority levels
	//s0ric = 0x03;
	//enable when transmitting -- s0tic = 0x03;

    //re_u0c1 = 1;	// enable reception
    te_u0c1 = 1;	// enable transmission

	///////////////
	uart_txbuf_in_idx = 0;
	uart_txbuf_out_idx = 0;
	txbuf_empty = TRUE;
}


void
USART_send(const uint8_t *buffer, uint8_t size)
{
	int i;
	uint8_t out_idx_saved;

/*	if (size == 0)
		return;	*/

	s0tic = 0;	// transmit interrupt disable -- start protected section
	// this also clears ir_s0tic, which fires off the transmitter empty interrupt
	asm("nop");
	asm("nop");

	for (i = 0; i < size; i++) {
		txbuf_empty = FALSE;
		uart_tx_buf_c[uart_txbuf_in_idx++] = buffer[i];
		if (uart_txbuf_in_idx == sizeof(uart_tx_buf_c))
			uart_txbuf_in_idx = 0;

		if (uart_txbuf_in_idx == uart_txbuf_out_idx) {
			// buffer is full, enable transmit and wait for buffer to empty
			if (txept_u0c0 == 1) {	// do not use ti_u0c1
				TRANSMIT_OLDEST_BYTE;
			}
			out_idx_saved = uart_txbuf_out_idx;
			asm("fclr i");
			s0tic |= UART_TX_INT_LEVEL;	// transmit interrupt enable
			asm("nop");
			asm("nop");
			asm("fset i");
/*			while (s0tic)
				;	*/
			while (out_idx_saved == uart_txbuf_out_idx)
				;
			s0tic = 0;
			// space is now available
		}
	}

	if (txbuf_empty)
		return;

	// do not use ti_u0c1
	if (txept_u0c0 == 1) {	// if no data in u0tb, and something to send
		TRANSMIT_OLDEST_BYTE;
	} else {
		asm("nop");
	}

	asm("fclr i");
	s0tic |= UART_TX_INT_LEVEL;	// transmit interrupt enable
	asm("nop");
	asm("nop");
	asm("fset i");

}

void
USART_send_str(const char *str)
{
	USART_send((uint8_t *)str, strlen(str));
}

