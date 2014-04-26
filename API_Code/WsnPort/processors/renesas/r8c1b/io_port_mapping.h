#ifndef _IO_PORT_MAPPING_H_
#define _IO_PORT_MAPPING_H_
#include "sfr_r81B.h"

extern char dummy_io_pin;
extern char dummy_pll_lock;

/************************************************************************/

#define INPUT	0
#define OUTPUT	1

#define LED_ON		1
#define LED_OFF		0


#define ALARM_LED		dummy_io_pin	//p1_3
#define RX_OK_LED		dummy_io_pin	//p1_3
#define DIR_ALARM_LED	pd1_3
#define DIR_RX_OK_LED	pd1_3

#define DIR_LED1		pd1_3
#define LED1			p1_3


#define IN_ALARM				p1_2	// switch input


#define IN_PLL_LOCK		dummy_pll_lock

#if defined SX1211
	#define NSS_CONFIG		p3_4
	#define DIR_NSS_CONFIG	pd3_4
	#define NSS_DATA		p3_3
	#define DIR_NSS_DATA	pd3_3

	#define IN_IRQ1		p4_5
	#define IN_IRQ0		p1_7
#elif defined SX1231
	#define NSS				p3_4
	#define DIR_NSS			pd3_4

	#define IN_RF_DIO1		p4_5
	#define IN_RF_DIO0		p1_7
#endif

/********************* debug pins... *****************************/
#define RF_TRANSMIT_LED		p1_3	//p1_1
#define RF_RECEIVING_LED	dummy_io_pin	//p1_0
#define SYNC_RX_LED			dummy_io_pin
#define SYNC_MODE_LED		p1_1	//p1_3
#define SLAVE_SLEEP_LED		dummy_io_pin
#define DIALOG_LED			dummy_io_pin
#define SYNCING_DIALOG_CH	dummy_io_pin	//p1_3

//#define INIT_DEBUG_PINS
/* these pins are for bitbanging in continuous mode, must be hi-z in fifo modes */
#define INIT_DEBUG_PINS			\
		pd1_1 = 1; p1_1 = 0;	\
		pd1_0 = 1; p1_0 = 0

/********************* ...debug pins *****************************/

/////////////////////////// register definitions... /////////////////////
/////////////////////////// ...register definitions /////////////////////
#endif /* _IO_PORT_MAPPING_H_ */

