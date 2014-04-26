#include "sfr_r81B.h"
#include <string.h>
#include <stdlib.h>	// abs for ltoa

#include "platform.h"



#define UART0_RXBUFLEN	16
#define UART0_TXBUFLEN	64
#define UART0_PRIO	0x03

#define TX_BUFFEROVERFLOW 0x01
#define RX_BUFFEROVERFLOW 0x02


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

/* #define _BAUD(bps)	((R8C_CLK/(float)bps)/16)	*/
#if defined SX1231
	/* #define R8C_CLK	16e6 */
    BAUD_115200 = 7,	//!< Baud rate of 115200
    BAUD_38400 = 26,	//!< Baud rate of 38400
    BAUD_19200 = 52,	//!< Baud rate of 19200
    BAUD_9600 = 104,	//!< Baud rate of 9600
#elif defined SX1211
	#if defined SX1211_CRYSTAL_12_8MHZ
		/* #define R8C_CLK	6e4 */
		BAUD_115200 = 3,	//!< Baud rate of 115200
		BAUD_38400 = 10,		//!< Baud rate of 38400
		BAUD_19200 = 21,	//!< Baud rate of 19200
		BAUD_9600 = 42,		//!< Baud rate of 9600
	#elif defined SX1211_CRYSTAL_14_7456MHZ
		/* #define R8C_CLK	3686400 */
		BAUD_115200 = 2,	//!< Baud rate of 115200
		BAUD_38400 = 6,		//!< Baud rate of 38400
		BAUD_19200 = 12,	//!< Baud rate of 19200
		BAUD_9600 = 24,		//!< Baud rate of 9600
	#endif
#endif
} baud_rate_t;


char text[32];

char uart0_txbuf[UART0_TXBUFLEN];
char *uart0_txin;
char *uart0_txout;
char uart0_txempty;
char uart0_txfull;

#ifdef ENABLE_UART_RX
char uart0_rxbuf[UART0_RXBUFLEN];
char *uart0_rxin;
char *uart0_rxout;
char uart0_rxfull;
char uart0_rxempty;
#endif /* ENABLE_UART_RX */



char uart0_health;


/************************** interrupt.. ****************************************************/
#pragma interrupt uart_tx_isr
// interrupt: send one char out of tx-buffer to serial interface
void uart_tx_isr(void)
{
#ifdef UART_DEBUG
  uart0_txcnt++;
#endif

  //if ( !uart0_txempty && !u0irs )
  if ( !uart0_txempty )
  {
    if (ti_u0c1) {  // Platz fuer neues Zeichen?
      u0tb = *uart0_txout;
      if (++uart0_txout >= uart0_txbuf + UART0_TXBUFLEN)
        uart0_txout = uart0_txbuf;
      uart0_txfull = FALSE;
      if (uart0_txin == uart0_txout)
        uart0_txempty = TRUE;
    }
  }
  return;
}

#ifdef ENABLE_UART_RX
#pragma interrupt uart_rx_isr
// interrupt: receiving one char from interface an send it to rx-buffer
void uart_rx_isr(void)
{
#ifdef UART_DEBUG
  uart0_rxcnt++;
#endif

  if ( ri_u0c1 ) // ist es ein neues Zeichen ?
  {
    *uart0_rxin = u0rb;
    if (uart0_rxfull)
      uart0_health |= RX_BUFFEROVERFLOW;
    else {
      if ( ++uart0_rxin >= uart0_rxbuf + UART0_RXBUFLEN )
	uart0_rxin = uart0_rxbuf;
      if ( uart0_rxin == uart0_rxout )
	uart0_rxfull = TRUE;
      uart0_rxempty = FALSE;
    }
  }

  return;
}
#endif /* ENABLE_UART_RX */


/************************** ..interrupt ****************************************************/

// putting one char into the tx-buffer
void
_usart_putc(uint8_t c)
{
  char was_empty;

  if (uart0_txfull)
  {
    asm( "\tINT #17");	/* interrupt 17 = UART0 Tx */
    uart0_health |= TX_BUFFEROVERFLOW;
    return;// EOF;
  }

  *uart0_txin = c;

  if (++uart0_txin >= uart0_txbuf + UART0_TXBUFLEN)
    uart0_txin = uart0_txbuf;

  if (uart0_txin == uart0_txout)
    uart0_txfull = TRUE;

  was_empty = uart0_txempty;
  uart0_txempty = FALSE;

  if (was_empty && ti_u0c1 /*&& txept_u0c0*/)	  // Interrupt ausloesen, da es ja sonst keiner tut...
  {
    asm( "\tINT #17");	/* interrupt 17 = UART0 Tx */
  }
  //return 0;
}


#ifdef ENABLE_UART_RX
// getting one char out of rx-buffer
int uart0_getc ( void )
{
  int c;

  if ( uart0_rxempty )
	return EOF;

  c = *uart0_rxout;
  uart0_rxfull = FALSE;

  if ( ++uart0_rxout >= uart0_rxbuf + UART0_RXBUFLEN )
	uart0_rxout = uart0_rxbuf;

  if ( uart0_rxin == uart0_rxout )
	uart0_rxempty = TRUE;

  return c & 0xff;
}
#endif /* ENABLE_UART_RX */


void
USART_send_str(const char *str)
{

	while (*str != 0)
		_usart_putc(*str++);

}

void
uart_init()
{
	uart0_txin = uart0_txbuf;
	uart0_txout = uart0_txbuf;
	uart0_txempty = TRUE;
	uart0_txfull = FALSE;

#ifdef ENABLE_UART_RX
	uart0_rxin = uart0_rxbuf;
	uart0_rxout = uart0_rxbuf;
	uart0_rxempty = TRUE;
	uart0_rxfull = FALSE;
#endif /* ENABLE_UART_RX */

	uart0_health = 0;

  /////////////////////////////////////////////////////////////////

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
    u0c1 = 0x02;	// bit1: tx buffer empty=1 (TI bit is read only)

	// this indicates to send function that it needs to kickstart first byte
	u0irs = 1;	// for transmit interrupt on u0tb empty (TI = 1)
	// beware of cntrsel bit : ucon = 0x00;

	u0brg = BAUD_38400 - 1;

    // set rx/tx interrupt priority levels
	//s0ric = 0x03;
	//enable when transmitting -- s0tic = 0x03;

    //re_u0c1 = 1;	// enable reception
    te_u0c1 = 1;	// enable transmission -- will enable when sending

	/////////////////////////////////////////////////////////////////


#ifdef UART_DEBUG
  uart0_txcnt = 0;
  uart0_rxcnt = 0;
#endif

  s0tic = UART0_PRIO;	// Tx-Interrupt enable
  //s0ric = UART0_PRIO;	// Rx-Interrupt enable
  //u0c1 = 0x07;	// Rx enabled, Tx enabled

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

int
hltoa(long val, char *buffer)
{
    char           tempc[BUFLEN];
    register char *bufptr;
    register long  uval = val;

    *(bufptr = &tempc[BUFLEN - 1]) = 0;

//	printf("val: %lx\n", val);
    do {
		char nib = uval & 0xf;
		if (nib > 9)
			*--bufptr = nib + 87;	// ten becomes ascii 'a'
		else
			*--bufptr = nib + '0';	// 0 to 9

	} while (uval >>= 4);

	*--bufptr = 'x';
	*--bufptr = '0';

    memcpy(buffer,bufptr, uval = (tempc + BUFLEN) - bufptr);
    return uval - 1;    /* DON'T COUNT NULL TERMINATION */
}

