#include <avr/io.h>

#ifndef _IO_PORT_MAPPING_H_
#define _IO_PORT_MAPPING_H_

typedef struct {
    char  b0:1;
    char  b1:1;
    char  b2:1;
    char  b3:1;
    char  b4:1;
    char  b5:1;
    char  b6:1;
    char  b7:1;
} bit_def_t;


/////////////////////////////////////////////////////////

#define INPUT	0
#define OUTPUT	1

#define LED_ON	1
#define LED_OFF	0

/* Read DIP switch to know the MAC ID */
//#define MAC_PINS	(IN_ID&0x0E)>>1 //keep only bits 1 to 3
#define MAC_PINS	MAC_Master0	// force to be master
//#define MAC_PINS	MAC_Slave1	// force to be one of the slaves

#define IN_ALARM				((volatile bit_def_t *)&PINA)->b0

#define ALARM_LED			((volatile bit_def_t *)&PORTC)->b1
#define RX_OK_LED			((volatile bit_def_t *)&PORTC)->b0
#define DIR_ALARM_LED		((volatile bit_def_t *)&DDRC)->b1
#define DIR_RX_OK_LED		((volatile bit_def_t *)&DDRC)->b0

#if defined SX1211
	#define NSS_CONFIG		((volatile bit_def_t *)&PORTD)->b5
	#define DIR_NSS_CONFIG	((volatile bit_def_t *)&DDRD)->b5
	#define NSS_DATA		((volatile bit_def_t *)&PORTD)->b6
	#define DIR_NSS_DATA	((volatile bit_def_t *)&DDRD)->b6

	#define IN_IRQ0			((volatile bit_def_t *)&PIND)->b4
	#define IN_IRQ1			((volatile bit_def_t *)&PINB)->b0
#elif defined SX1231
	#define NSS				((volatile bit_def_t *)&PORTD)->b5
	#define DIR_NSS			((volatile bit_def_t *)&DDRD)->b5

	#define IN_RF_DIO0			((volatile bit_def_t *)&PIND)->b4
	#define IN_RF_DIO1			((volatile bit_def_t *)&PINB)->b0
#endif

#define DIR_MOSI	((volatile bit_def_t *)&DDRB)->b5
#define DIR_SCK		((volatile bit_def_t *)&DDRB)->b7
#define DIR_MISO	((volatile bit_def_t *)&DDRB)->b6
#define DIR_SSn		((volatile bit_def_t *)&DDRB)->b4

/************************** debug pins... *******************************/
#define RF_TRANSMIT_LED		((volatile bit_def_t *)&PORTA)->b6
#define RF_RECEIVING_LED	((volatile bit_def_t *)&PORTA)->b0
#define SYNC_RX_LED			((volatile bit_def_t *)&PORTA)->b7
#define SYNCING_DIALOG_CH	((volatile bit_def_t *)&PORTA)->b4//SYNC_MODE_LED		((volatile bit_def_t *)&PORTA)->b4
#define SLAVE_SLEEP_LED		((volatile bit_def_t *)&PORTA)->b5
#define DIALOG_LED			((volatile bit_def_t *)&PORTA)->b5	/* master transmitting to slave */
#define SYNC_MODE_LED		dummy_led

// Set P10 as output for LED1, initially pins low
#define INIT_DEBUG_PINS			\
	DDRA |= _BV(PINA0) | _BV(PINA4) | _BV(PINA5) | _BV(PINA6) | _BV(PINA7);	\
	PORTA &= ~_BV(PINA0) & ~_BV(PINA4) & ~_BV(PINA5) & ~_BV(PINA6) & ~_BV(PINA7)

/************************** ...debug pins *******************************/


/////////////////////////// register definitions... /////////////////////
#define TIFR1_OCF1A		((volatile bit_def_t *)&TIFR1)->b1
#define TIFR1_OCF1B		((volatile bit_def_t *)&TIFR1)->b2
#define TIMSK1_OCIE1A	((volatile bit_def_t *)&TIMSK1)->b1

#define UCSR0B_UDRIE0	((volatile bit_def_t *)&UCSR0B)->b5

/////////////////////////// ...register definitions /////////////////////

extern uint8_t dummy_led;

#endif /* _IO_PORT_MAPPING_H_ */
