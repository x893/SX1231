#include <p18f87j11.h>

#define OUTPUT	0	// for TRIS registers (0 = output)
#define INPUT	1	// for TRIS registers (1 = input)

#define LED_ON	1	// leds are all tied to ground with cpu driving (+) side
#define LED_OFF	0

//#define MAC_PINS

#define IN_ALARM	PORTGbits.RG0

#define IN_PLL_LOCK		PORTBbits.RB5	// RB5/KBI1

#if defined SX1211
	#define DIR_NSS_CONFIG	TRISEbits.TRISE2	// RE2 direction
	#define NSS_CONFIG		LATEbits.LATE2		// RE2 out
	#define DIR_NSS_DATA	TRISEbits.TRISE3	// RE3 direction
	#define NSS_DATA		LATEbits.LATE3		// RE3 out

	#define IN_IRQ1			PORTBbits.RB3	// RB3/INT3
	#define IN_IRQ0			PORTBbits.RB0	// RB0/INT0
#elif defined SX1231
	#define DIR_NSS			TRISEbits.TRISE2	// RE2 direction
	#define NSS				LATEbits.LATE2		// RE2 out

	#define IN_RF_DIO1			PORTBbits.RB3	// RB3/INT3
	#define IN_RF_DIO0			PORTBbits.RB0	// RB0/INT0
#endif


#define DIR_LED1	TRISDbits.TRISD0
#define DIR_LED2	TRISDbits.TRISD1
#define DIR_LED3	TRISDbits.TRISD2
#define DIR_LED4	TRISDbits.TRISD3
#define DIR_LED5	TRISDbits.TRISD4
#define DIR_LED6	TRISDbits.TRISD5
#define DIR_LED7	TRISDbits.TRISD6
#define DIR_LED8	TRISDbits.TRISD7

#define LED1		LATDbits.LATD0
#define LED2		LATDbits.LATD1
#define LED3		LATDbits.LATD2
#define LED4		LATDbits.LATD3
#define LED5		LATDbits.LATD4
#define LED6		LATDbits.LATD5
#define LED7		LATDbits.LATD6
#define LED8		LATDbits.LATD7

#define DIR_SYNC_MODE_LED		DIR_LED1
#define DIR_SYNC_RX_LED			DIR_LED2
#define DIR_RF_TRANSMIT_LED		DIR_LED3
#define DIR_DIALOG_LED			DIR_LED4
#define DIR_RF_RECEIVING_LED	DIR_LED5
#define DIR_SLAVE_SLEEP_LED		DIR_LED6
#define DIR_ALARM_LED			DIR_LED7
#define DIR_RX_OK_LED			DIR_LED8

#define SYNC_MODE_LED			LED1
#define SYNC_RX_LED				LED2
#define RF_TRANSMIT_LED			LED3
#define DIALOG_LED				LED4
#define RF_RECEIVING_LED		LED5
#define SLAVE_SLEEP_LED			LED6
#define SYNCING_DIALOG_CH		LED7 //ALARM_LED				LED7
#define RX_OK_LED				LED8

#define ALARM_LED			dummy_led


#define INIT_DEBUG_PINS					\
	TRISGbits.TRISG1 = OUTPUT;	\
	LATGbits.LATG1 = 0;		\
	DIR_SYNC_MODE_LED = OUTPUT;	\
	DIR_SYNC_RX_LED = OUTPUT;		\
	DIR_RF_TRANSMIT_LED = OUTPUT;	\
	DIR_DIALOG_LED = OUTPUT;		\
	DIR_SLAVE_SLEEP_LED = OUTPUT;	\
	DIR_RF_RECEIVING_LED = OUTPUT

#if 0
#define SX1211_IRQ1_INTERRUPT_FLAG		INTCON3bits.INT3IF
#define SX1211_IRQ0_INTERRUPT_FLAG		INTCONbits.INT0IF

#define ENABLE_IRQ1_INTERRUPT		\
	SX1211_IRQ1_INTERRUPT_FLAG	= 0;		\
	INTCON3bits.INT3IE = 1

#define ENABLE_IRQ0_INTERRUPT		\
	SX1211_IRQ0_INTERRUPT_FLAG	= 0;		\
	INTCONbits.INT0IE = 1
#endif

#define LCD_CS		LATAbits.LATA2	// MISO_bitbang pin
#define LCD_CS_DIR	TRISAbits.TRISA2

#define LCD_RST		LATFbits.LATF6
#define LCD_RST_DIR	TRISFbits.TRISF6

extern uint8_t dummy_led;
