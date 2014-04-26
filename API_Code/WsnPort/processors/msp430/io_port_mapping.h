#include "f149.h"

#define INPUT		0
#define OUTPUT		1

#define LED_ON		1
#define LED_OFF		0

#if defined SX1231
	#define NSS			po4_6
	#define DIR_NSS		pd4_6

	#define IN_RF_DIO1		pi1_2
	#define IN_RF_DIO0		pi1_3
#elif defined SX1211
	#define IN_IRQ1			pi1_2
	#define IN_IRQ0			pi1_3

	#define NSS_CONFIG		po4_6
	#define DIR_NSS_CONFIG	pd4_6
	#define NSS_DATA		po4_5
	#define DIR_NSS_DATA	pd4_5
#endif

#define IN_ALARM		pi1_7

#define ALARM_LED		po2_0
#define RX_OK_LED		po2_1
#define DIR_ALARM_LED	pd2_0
#define DIR_RX_OK_LED	pd2_1


/************************** debug pins... *******************************/
#define DIALOG_LED			po2_2
#define SLAVE_SLEEP_LED		po2_3
#define SYNC_MODE_LED		po2_4
#define SYNCING_DIALOG_CH	po2_5	//SYNC_RX_LED			po2_5
#define RF_TRANSMIT_LED		po2_6
#define RF_RECEIVING_LED	po2_7

#define SYNC_RX_LED			dummy_led

#define INIT_DEBUG_PINS			\
		pd2_2 = 1;	\
		pd2_3 = 1;	\
		pd2_4 = 1;	\
		pd2_5 = 1;	\
		pd2_6 = 1;	\
		pd2_7 = 1

extern uint8_t dummy_led;

/************************** ...debug pins *******************************/
