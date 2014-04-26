#ifndef _AVR_TIMERS_H_
#define _AVR_TIMERS_H_
#include <avr/interrupt.h>

#define UPDATE_PWMDAC_COMPARE(x)	OCR0A = x << 2

/*****************************************************************************************/

#define HIRES_COUNTER		TCNT1
#define HIRES_COMPARE_A		OCR1A
#define HIRES_COMPARE_B		OCR1B
#define CLEAR_HIRES_COMPARE_A_FLAG	TIFR1 |= (1<<OCF1A)	// Clear compare-A interrupt flag
#define HIRES_COMPARE_A_FLAG		TIFR1 & (1<<OCF1A)
#define CLEAR_HIRES_COMPARE_B_FLAG	TIFR1 |= (1<<OCF1B)	// Clear compare-B interrupt flag
#define HIRES_COMPARE_B_FLAG	TIFR1_OCF1B


#define SET_LOWRES_COUNTER(x)	TCNT2 = x
#define DISABLE_LOWRES_TIMER_INTERRUPT		TIMSK2 &= ~_BV(TOIE2)	//Disable timer2 overflow interrupt
#define ENABLE_LOWRES_TIMER_INTERRUPT			TIMSK2 |= _BV(TOIE2)	//Enable timer2 overflow interrupt

// timer2 is run asynchronously from the system clock
#define LOWRES_COUNTER_UPDATE_WAIT	\
	while (ASSR & (1<<TCN2UB))	\
			;//Wait the END of TCNT2 busy

/* TOV2: interrupt source, timer overflow flag
 * TIFR2: timer interrupt flag register
 */
#define CLEAR_LOWRES_TIMER_FLAG		TIFR2 |= (1<<TOV2)			// Clear overflow flag
#define LOWRES_TIMER_FLAG			TIFR2 & (1<<TOV2)		// Timer2 Overflow

/*****************************************************************************************/


/* timer1 is used for high-resolution timing, and it runs continuously.
 * The compares OCR1A and OCR1B are used */
#define AVR_IO_CLOCK_MHZ					3.6864		// in MHz
#define TIMER1_CLOCK_DIV					8
#define TIMER1_MICROSECONDS_PER_CLOCK		(1 / (AVR_IO_CLOCK_MHZ / TIMER1_CLOCK_DIV))
#define HIRES_TIMEOUT(microseconds)			(microseconds / TIMER1_MICROSECONDS_PER_CLOCK)
#define HIRES_TIMER_MAX_VALUE				0xffff

/*****************************************************************************************/

typedef uint8_t ltime_t;
#define TIMER2_CLOCK_SOURCE_HZ				32768
#define TIMER2_CLOCK_DIV					128
#define TIMER2_MILLISECONDS_PER_CLOCK		(1 / ( (TIMER2_CLOCK_SOURCE_HZ/1000.0) / TIMER2_CLOCK_DIV))
#define LOWRES_TIMEOUT(milliseconds)			(256 - (milliseconds / TIMER2_MILLISECONDS_PER_CLOCK) )

#define SLEEP_WAKEUP_ISR	ISR(TIMER2_OVF_vect)

/*****************************************************************************************/


#endif /* _AVR_TIMERS_H_ */

