#ifndef __IO_PORT_MAPPING_H__
#define __IO_PORT_MAPPING_H__

#include "stm32f4xx.h"

#define OUTPUT	0
#define INPUT	1

#define LED_ON	1
#define LED_OFF	0

#define PeriphSetBB(RegAddr, BitNumber)	\
	*(__IO uint32_t *) (PERIPH_BB_BASE | (((uint32_t)RegAddr - PERIPH_BASE) << 5) | ((BitNumber) << 2))

#define NSS			PeriphSetBB(GPIOA->ODR, 4)

#define IN_ALARM	(0)	/* GPIOA,GPIO_Pin_0 */

#define IN_RF_DIO1			PeriphSetBB(GPIOA->IDR, 0)
#define IN_RF_DIO0			PeriphSetBB(GPIOA->IDR, 1)

#define NSS_LOW()
#define NSS_HIGH()

#define DIR_ALARM_LED		dummy_led
#define DIR_RX_OK_LED		dummy_led

#define SYNC_MODE_LED		dummy_led
#define SYNC_RX_LED			dummy_led
#define RF_TRANSMIT_LED		dummy_led
#define DIALOG_LED			dummy_led
#define RF_RECEIVING_LED	dummy_led
#define SLAVE_SLEEP_LED		dummy_led
#define SYNCING_DIALOG_CH	dummy_led
#define RX_OK_LED			dummy_led
#define ALARM_LED			dummy_led


#define INIT_DEBUG_PINS

extern uint8_t dummy_led;

#endif
