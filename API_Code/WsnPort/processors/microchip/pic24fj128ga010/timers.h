
#define UPDATE_PWMDAC_COMPARE(x)	OC3RS = x

/*************************************************************************************/
/* Timer1 as low-res timer due to operation from 32768Hz crystal "Type A" timer" */
typedef uint16_t ltime_t;	/* low-res timer size */

#define TIMER1_CLOCK_SOURCE_HZ	32768
#define TIMER1_CLOCK_DIV		64
#define TIMER1_MILLISECONDS_PER_CLOCK		(1 / ( (TIMER1_CLOCK_SOURCE_HZ/1000.0) / TIMER1_CLOCK_DIV))
#define LOWRES_TIMEOUT(milliseconds)		(milliseconds / TIMER1_MILLISECONDS_PER_CLOCK)

/* set the period register: this timer counts up to the period register */
#define SET_LOWRES_COUNTER(x)	PR1 = x;	TMR1 = 0

#define LOWRES_TIMER_FLAG					IFS0bits.T1IF 		// interrupt request flag
#define CLEAR_LOWRES_TIMER_FLAG				LOWRES_TIMER_FLAG = 0

#define ENABLE_LOWRES_TIMER_INTERRUPT		IEC0bits.T1IE = 1	/* for sleep wakeup */
#define DISABLE_LOWRES_TIMER_INTERRUPT		IEC0bits.T1IE = 0
/*************************************************************************************/


/* Timer2 as hi-res timer "Type B" timer */
#define TIMER2_CLOCK_SRC_MHZ				4		// in MHz: Fosc/2 = 4MHz
#define TIMER2_CLOCK_DIV					8		// 4 / 8 = 500KHz
#define TIMER2_MICROSECONDS_PER_CLOCK		(1 / (TIMER2_CLOCK_SRC_MHZ / (float)TIMER2_CLOCK_DIV))
#define HIRES_TIMEOUT(microseconds)	(uint16_t)(microseconds / TIMER2_MICROSECONDS_PER_CLOCK)
#define HIRES_TIMER_MAX_VALUE		0xffff	// up counter

#define HIRES_COMPARE_B_FLAG		IFS0bits.T2IF

/*************************************************************************************/
