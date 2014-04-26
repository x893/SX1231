#include "types.h"
#include <msp430x14x.h>

uint8_t uart_txbuf_out_idx;
uint8_t uart_txbuf_in_idx;
static char uart_tx_buf_c[64];
static char txbuf_empty;	// flag

/*************************************************************************/

#pragma vector=USART0TX_VECTOR
__interrupt void
usart0_tx (void)
{
	TXBUF0 = uart_tx_buf_c[uart_txbuf_out_idx++];
	if (uart_txbuf_out_idx == sizeof(uart_tx_buf_c))
		uart_txbuf_out_idx = 0;

	if (uart_txbuf_out_idx == uart_txbuf_in_idx) {
		IE1 &= ~UTXIE0;		// transmit interrupt disable
		txbuf_empty = TRUE;
	}
}

/*************************************************************************/

//char text[32];
char text[128];

void
USART_send(const uint8_t *buffer, uint8_t size)
{
	int i;
	uint8_t out_idx_saved;

	if (size == 0)
		return;

	IE1 &= ~UTXIE0;		// transmit interrupt disable -- start protected section

	for (i = 0; i < size; i++) {
		txbuf_empty = FALSE;
		uart_tx_buf_c[uart_txbuf_in_idx++] = buffer[i];
		if (uart_txbuf_in_idx == sizeof(uart_tx_buf_c))
			uart_txbuf_in_idx = 0;

		if (uart_txbuf_in_idx == uart_txbuf_out_idx) {
			// buffer is full, enable transmit and wait for buffer to empty
			out_idx_saved = uart_txbuf_out_idx;
			IE1 |= UTXIE0;		// transmit interrupt enable
/*			while (IE1 & UTXIE0)
				;	wait until all is empty */
			while (out_idx_saved == uart_txbuf_out_idx)
				;	// wait until a single position is available
			IE1 &= ~UTXIE0;
		}
	}

	if (txbuf_empty)
		return;

	//IFG1 |= UTXIFG0;
	IE1 |= UTXIE0;		// transmit interrupt enable
}

void
USART_send_str(const char *str)
{
	USART_send((uint8_t *)str, strlen(str));
}

void
uart_init()
{
	/* uart is being clocked by watch crystal */

	P3SEL |= 0x30;			// P3.4,5 = USART0 TXD/RXD
	ME1 |= UTXE0 + URXE0;	// Enable USART0 TXD/RXD
	UCTL0 |= CHAR;			// 8-bit character
	UTCTL0 |= SSEL0;		// UCLK = ACLK
	UBR00 = 0x03;			// 32k/9600 - 3.41
	UBR10 = 0x00;			//
	UMCTL0 = 0x4A;			// Modulation
	UCTL0 &= ~SWRST;		// Initialize USART state machine
//	IE1 |= URXIE0;			// Enable USART0 RX interrup

	///////////////
	uart_txbuf_in_idx = 0;
	uart_txbuf_out_idx = 0;
	txbuf_empty = TRUE;
}

