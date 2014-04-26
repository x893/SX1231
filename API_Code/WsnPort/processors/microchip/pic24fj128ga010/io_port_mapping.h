#include <p24FJ128GA010.h>

#define OUTPUT	0	// for TRIS registers (0 = output)
#define INPUT	1	// for TRIS registers (1 = input)

#define LED_ON		1	// all leds are ON with high logic level
#define LED_OFF		0

#define IN_ALARM	PORTDbits.RD6	// S3

#ifdef SX1211
	#define IN_IRQ1			PORTEbits.RE9	// RE9/INT2
	#define IN_IRQ0			PORTEbits.RE8	// RE8/INT1

	#define DIR_NSS_CONFIG	TRISFbits.TRISF0
	#define NSS_CONFIG		LATFbits.LATF0

	#define DIR_NSS_DATA	TRISGbits.TRISG6
	#define NSS_DATA		LATGbits.LATG6
#elif defined SX1231
	#define IN_RF_DIO1			PORTEbits.RE9	// RE9/INT2
	#define IN_RF_DIO0			PORTEbits.RE8	// RE8/INT1

	#define DIR_NSS		TRISFbits.TRISF0
	#define NSS			LATFbits.LATF0
#endif


#define DIR_LED3	TRISAbits.TRISA0
#define DIR_LED4	TRISAbits.TRISA1
#define DIR_LED5	TRISAbits.TRISA2
#define DIR_LED6	TRISAbits.TRISA3
#define DIR_LED7	TRISAbits.TRISA4
#define DIR_LED8	TRISAbits.TRISA5
#define DIR_LED9	TRISAbits.TRISA6
#define DIR_LED10	TRISAbits.TRISA7

#define LED3		LATAbits.LATA0
#define LED4		LATAbits.LATA1
#define LED5		LATAbits.LATA2
#define LED6		LATAbits.LATA3
#define LED7		LATAbits.LATA4
#define LED8		LATAbits.LATA5
#define LED9		LATAbits.LATA6
#define LED10		LATAbits.LATA7

#define DIR_SYNC_MODE_LED		DIR_LED3
#define DIR_SYNC_RX_LED			DIR_LED4
#define DIR_RF_TRANSMIT_LED		DIR_LED5
#define DIR_DIALOG_LED			DIR_LED6
#define DIR_RF_RECEIVING_LED	DIR_LED7
#define DIR_SLAVE_SLEEP_LED		DIR_LED8
#define DIR_ALARM_LED			DIR_LED9
#define DIR_RX_OK_LED			DIR_LED10

#define SYNC_MODE_LED			LED3
#define SYNC_RX_LED				LED4
#define RF_TRANSMIT_LED			LED5
#define DIALOG_LED				LED6
#define RF_RECEIVING_LED		LED7
#define SLAVE_SLEEP_LED			LED8
#define SYNCING_DIALOG_CH		LED9 //ALARM_LED				LED9
#define RX_OK_LED				LED10

#define ALARM_LED			dummy_led


#define INIT_DEBUG_PINS					\
	TRISGbits.TRISG1 = OUTPUT;	\
	DIR_SYNC_MODE_LED = OUTPUT;	\
	DIR_SYNC_RX_LED = OUTPUT;		\
	DIR_RF_TRANSMIT_LED = OUTPUT;	\
	DIR_DIALOG_LED = OUTPUT;		\
	DIR_SLAVE_SLEEP_LED = OUTPUT;	\
	DIR_RF_RECEIVING_LED = OUTPUT

extern uint8_t dummy_led;
