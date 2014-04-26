/* 
 * File:   sx1243HAL.h
 * Author: gcristian
 *
 * Created on 8. février 2013, 08:49
 */

#ifndef SX1243HAL_H
#define	SX1243HAL_H

/*!
 *	Basic types definition
 */

typedef unsigned char   U8;

typedef unsigned short  U16;

typedef unsigned long   U32;

typedef signed char     S8;

typedef signed short    S16;

typedef signed long     S32;

typedef float          	F24;

typedef double          F32;

/*!
 *	boolean types definition
 */

#define TRUE 1

#define true 1

#define FALSE 0

#define false 0

/*!
 * MCU  dependent parameters
 */
 #define GPIO_ANALOG_RA4                            ANSELbits.ANS3
 #define GPIO_ANALOG_RC0                            ANSELbits.ANS4
 #define GPIO_ANALOG_RC1                            ANSELbits.ANS5
 #define GPIO_ANALOG_RC2                            ANSELbits.ANS6
 #define GPIO_ANALOG_RC3                            ANSELbits.ANS7
 #define GPIO_ANALOG_RB5                            ANSELHbits.ANS11
 #define GPIO_ANALOG_RC6                            ANSELHbits.ANS8
 #define GPIO_ANALOG_RB4                            ANSELHbits.ANS10
 #define GPIO_ANALOG_RC7                            ANSELHbits.ANS9

 #define VOLTAGE_REF                               ADCON1

/*!
 * MCU  Timer 0 parameters
 */
 #define TIMER0_PRESCALER_SLCT                      T0CONbits.T0CS
 #define TIMER0_PRESCALER_OFF                       T0CONbits.PSA
 #define FLAG_IRQ_CTRL_TIMER0                       INTCONbits.TMR0IF
 #define EN_IRQ_CTRL_TIMER0                         INTCONbits.TMR0IE
 #define TIMER0_ON                                  T0CONbits.TMR0ON
 #define TIMER0_CLK_SOURCE                          T0CONbits.T0CS
 #define TIMER0_PRESCALER                           T0CONbits.PSA
 #define TIMER0_CNT                                 TMR0L


/*!
 * MCU  Timer 1 parameters
 */
 #define TIMER1_IRQ                                 PIR1bits.TMR1IF
 #define ENABLE_IRQ_TIMER1                          PIE1bits.TMR1IE
 #define TIMER1_MODE_16                             T1CONbits.RD16
 #define TIMER1_CLK_STATUS                          T1CONbits.T1RUN
 #define TIMER1_PRESCALER                           T1CONbits.T1CKPS
 #define TIMER1_OSC_EN                              T1CONbits.T1OSCEN
 #define TIMER1_NOT_SYNC                            T1CONbits.NOT_T1SYNC
 #define TIMER1_CLOCK_SOURCE                        T1CONbits.TMR1CS
 #define TIMER1_ON                                  T1CONbits.TMR1ON


/*!
 * SX1243 pins
 */
#define RF_RESET_DIR                                TRISCbits.TRISC3
#define RF_RESET                                    PORTCbits.RC3

#define RF_DATA_DIR                                 TRISCbits.TRISC7
#define RF_DATA                                     PORTCbits.RC7

#define RF_DATA_I_DIR                               TRISBbits.TRISB4
#define RF_DATA_I                                   PORTBbits.RB4

#define RF_CTRL_DIR                                 TRISBbits.TRISB6
#define RF_CTRL                                     PORTBbits.RB6

#define RF_TX_RDY_DIR                               TRISCbits.TRISC0
#define RF_TX_RDY                                   PORTCbits.RC0

/*!
 * Timer and Interrupt handling functions
 */

#define TIMER1_CLK                                  12000000 // CPU clk at 48M but divided by 4 for internal operations
#define TIMER1_PRESCALER_VALUE                      1   // 1:1 prescaler

#define DisableInterrupts( )                       INTCONbits.GIEH = 0

#define EnableInterrupts( )                        INTCONbits.GIEH = 1

/*!
 * \brief Initializes the Timer 1 used to toggle the GPIO depending of the bitrate
 */

#define TMR1_INIT( )        T1CONbits.RD16 = 1; \
                            T1CONbits.T1RUN = 0; \
                            T1CONbits.T1CKPS = 0; \
                            T1CONbits.T1OSCEN = 0; \
                            T1CONbits.NOT_T1SYNC = 0; \
                            T1CONbits.TMR1CS = 0; \
                            T1CONbits.TMR1ON = 0; \

/*!
 * \brief Compute the bitrate for PIC MCU (incrementing timer)
 */
#define ComputeBitrate( bitrate ) ( U16 )( 65535 - ( TIMER1_CLK / (  TIMER1_PRESCALER_VALUE * bitrate ) ) )

/*!
 * \brief Initialize Timer 1 counter
 */
#define SET_TMR1( ) \
            TMR1H = ( U8 )( ( Bitrate >> 8 ) & 0xFF );\
            TMR1L = ( U8 )( Bitrate & 0xFF );\

/*!
 * \brief Timer 1 wait
 */
#define TMR1_WAIT( ) \
            while( TIMER1_IRQ == 0 );\
            TMR1H = ( U8 )( ( Bitrate >> 8 ) & 0xFF );\
            TMR1L = ( U8 )( Bitrate & 0xFF );\
            TIMER1_IRQ = 0;

/*!
 * \brief Start Timer 1
 */
#define TMR1_START( )	        PIE1bits.TMR1IE = 0; \
                                T1CONbits.TMR1ON = 1; \

/*!
 * \brief Stop Timer 1
 */
#define TMR1_STOP( )            PIE1bits.TMR1IE = 0; \
                                T1CONbits.TMR1ON = 0; \


/*!
 * SX1243 pins
 */
#define RESET_DIR                                   TRISCbits.TRISC3
#define RESET                                       PORTCbits.RC3

#define RF_DATA_DIR                                 TRISCbits.TRISC7
#define RF_DATA                                     PORTCbits.RC7

#define RF_DATA_I_DIR                               TRISBbits.TRISB4
#define RF_DATA_I                                   PORTBbits.RB4

#define RF_CTRL_DIR                                 TRISBbits.TRISB6
#define RF_CTRL                                     PORTBbits.RB6

#define RF_TX_RDY_DIR                               TRISCbits.TRISC0
#define RF_TX_RDY                                   PORTCbits.RC0


/*!
 * Led pins
 */
// LED logic is inverted
#define LED_ON                                      0
#define LED_OFF                                     1

#define LED1_DIR                                    TRISCbits.TRISC4
#define LED1                                        PORTCbits.RC4
#define LED1_L                                      LATCbits.LATC4

#define LED2_DIR                                    TRISCbits.TRISC5
#define LED2                                        PORTCbits.RC5
#define LED2_L                                      LATCbits.LATC5

/*!
 * Switches
 */
#define SW_ON                                       0
#define SW_OFF                                      1

#define SW1_DIR                                     TRISCbits.TRISC2
#define SW1                                         PORTCbits.RC2

#define SW2_DIR                                     TRISBbits.TRISB5
#define SW2                                         PORTBbits.RB5

#define SW3_DIR                                     TRISBbits.TRISB7
#define SW3                                         PORTBbits.RB7

#define SW4_DIR                                     TRISCbits.TRISC1
#define SW4                                         PORTCbits.RC1


/*!
 * Initialises the main application timer
 */
void TmrInit( void );



/*!
 * \brief Returns the tick counter value.
 * \brief One unit = 1 us.
 * \remark See the GetDeltaTickCounter( U32 oldTick ) function.
 * \retval Tick 32 bits counter value
 */
U32 TmrGetTickCounter( void );



/*!
 * Timer interrupt handler
 */
void TmrIrqHandler( void );



/*!
 * \brief Return the number of tick between the current value of the tick counter and
 *        an older value of the tick counter. The older value must be the return of GetDeltaCounter().
 *
 * \param[in] oldTick	Old tick value of the counter
 * \retval Delta between the two values.
 * \remark One unit = 1 ms
 */
U32 TmrGetDeltaTickCounter( U32 oldTick );


#endif	/* SX1243HAL_H */

