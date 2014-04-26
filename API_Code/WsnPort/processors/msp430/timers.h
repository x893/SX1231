#ifndef _MSP_TIMERS_H_
#define _MSP_TIMERS_H_
#include <msp430x14x.h>

extern uint8_t pwmdac_dummy_compare;
#define UPDATE_PWMDAC_COMPARE(x)	pwmdac_dummy_compare = x	// not implemented

/*****************************************************************************************/


/* timer-B is used for high-resolution timing, and it runs continuously. */
#define CLEAR_HIRES_COMPARE_A_FLAG	TBCCTL1 &= ~CCIFG
#define CLEAR_HIRES_COMPARE_B_FLAG	TBCCTL2 &= ~CCIFG
#define HIRES_COMPARE_A_FLAG		(TBCCTL1 & CCIFG)
#define HIRES_COMPARE_B_FLAG		(TBCCTL2 & CCIFG)
#define HIRES_COMPARE_A				TBCCR1
#define HIRES_COMPARE_B				TBCCR2
#define HIRES_COUNTER				TBR	// used for reading, this is not async

#if defined SX1231
	#define SMCLK_CLOCK_MHZ					4.0		// in MHz	2.0µS each, 132mS max
#elif defined SX1211
	#if defined SX1211_CRYSTAL_12_8MHZ
		#define SMCLK_CLOCK_MHZ					6.4			// in MHz	1.25µS each, 82mS max
	#elif defined SX1211_CRYSTAL_14_7456MHZ
		#define SMCLK_CLOCK_MHZ					3.6864		// in MHz	2.17µS each, 142mS max
	#endif
#endif
#define TIMERB_CLOCK_DIV					8
#define TIMERB_MICROSECONDS_PER_CLOCK		(1 / (SMCLK_CLOCK_MHZ / TIMERB_CLOCK_DIV))
/* make sure this macro is only applied to const variables. cannot do math at runtime */
#define HIRES_TIMEOUT(microseconds)			(microseconds / TIMERB_MICROSECONDS_PER_CLOCK)
#define	HIRES_TIMER_MAX_VALUE			0xffff

/*****************************************************************************************/

typedef uint16_t ltime_t;	// must fit a 16bit value

/* timer-A is low-resolution timer, async to cpu clock */
#define SET_LOWRES_COUNTER(x)			TAR = x
#define CLEAR_LOWRES_TIMER_FLAG			TACTL &= ~TAIFG			// Clear overflow flag
#define LOWRES_TIMER_FLAG				TACTL & TAIFG
#define	ENABLE_LOWRES_TIMER_INTERRUPT	TACTL |= TAIE
#define DISABLE_LOWRES_TIMER_INTERRUPT	TACTL &= ~TAIE

#define TIMERA_CLOCK_SOURCE_HZ			32768
#define TIMERA_CLOCK_DIV				8
#define TIMERA_MILLISECONDS_PER_CLOCK	(1 / ( (TIMERA_CLOCK_SOURCE_HZ/1000.0) / TIMERA_CLOCK_DIV))
#define LOWRES_TIMEOUT(milliseconds)	(65536 - (milliseconds / TIMERA_MILLISECONDS_PER_CLOCK) )


#define SLEEP_WAKEUP_ISR	ISR(TIMER2_OVF_vect)

#define LOWRES_COUNTER_UPDATE_WAIT		/* (async timer) any write to TAR takes effect immediately */

/*****************************************************************************************/

#endif /*  _MSP_TIMERS_H_ */

