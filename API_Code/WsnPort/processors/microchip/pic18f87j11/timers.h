
// PWM duty cycle is set via CCPR4L and CCP4CON<5:4>
#define UPDATE_PWMDAC_COMPARE(x)	\
	if (x & 0x01)					\
		CCP4CONbits.DCCP4Y = 1;		\
	else							\
		CCP4CONbits.DCCP4Y = 0;		\
									\
	if (x & 0x02)					\
		CCP4CONbits.DCCP4X = 1;		\
	else							\
		CCP4CONbits.DCCP4X = 0;		\
									\
	CCPR4L = x >> 2


// Timer1 runs from 32KHz via T1OSCEN (T1CON<3)
// timer1 must be 16bit because 8 is max prescale, need one-second max timing
#define TIMER1_CLOCK_SOURCE_HZ	32768
#define TIMER1_CLOCK_DIV		8	
#define TIMER1_MILLISECONDS_PER_CLOCK		(1 / ( (TIMER1_CLOCK_SOURCE_HZ/1000.0) / TIMER1_CLOCK_DIV))

#define LOWRES_TIMEOUT(milliseconds)		(0xffff - (milliseconds / TIMER1_MILLISECONDS_PER_CLOCK))

#define SET_LOWRES_COUNTER(x)	\
			TMR1H = (x >> 8);	\
			TMR1L = (x & 0xff)

#define CLEAR_LOWRES_TIMER_FLAG				PIR1bits.TMR1IF = 0		// interrupt request flag
#define LOWRES_TIMER_FLAG					PIR1bits.TMR1IF			// interrupt request flag

/* interrupt priority for lowres timer1 assigned in cpu.c */
#define ENABLE_LOWRES_TIMER_INTERRUPT		PIE1bits.TMR1IE = 1		// for sleep wakeup
#define DISABLE_LOWRES_TIMER_INTERRUPT		PIE1bits.TMR1IE = 0	

typedef uint16_t ltime_t;

/* B used in:
 * 1) master sync hop rate 
 * 2) rx timeout
 * */
#define HIRES_COMPARE_B_FLAG	INTCONbits.TMR0IF

/* timer 0 has no compare registers, only an overflow interrupt */
#define TIMER0_CLOCK_SRC_MHZ				2.5		// in MHz
#define TIMER0_CLOCK_DIV					8		// 2.5 / 8 = 312.5KHz
#define TIMER0_MICROSECONDS_PER_CLOCK		(1 / (TIMER0_CLOCK_SRC_MHZ / TIMER0_CLOCK_DIV))
#define HIRES_TIMEOUT(microseconds)	\
	(0xffff - (microseconds / TIMER0_MICROSECONDS_PER_CLOCK))
#define HIRES_TIMER_MAX_VALUE		0

