#ifndef _R8C_TIMERS_H_
#define _R8C_TIMERS_H_
#include "types.h"
#include "sfr_r81B.h"

extern uint8_t pwmdac_dummy_compare;
#define UPDATE_PWMDAC_COMPARE(x)	pwmdac_dummy_compare = x	// not implemented

/****************************************************************************/

// hires timerC: compares are tm0 and tm1
#define HIRES_COUNTER						tc	// clocked from f/8: 460.8KHz
#define HIRES_COMPARE_A						tm0
#define HIRES_COMPARE_B						tm1
#define CLEAR_HIRES_COMPARE_A_FLAG			ir_cmp0ic = 0
#define HIRES_COMPARE_A_FLAG				ir_cmp0ic
#define CLEAR_HIRES_COMPARE_B_FLAG			ir_cmp1ic = 0
#define HIRES_COMPARE_B_FLAG				ir_cmp1ic


/****************************************************************************/

#define SET_LOWRES_COUNTER(x)	tzpr = x	// Timer-Z fed from timer-X, which provides 256Hz rate

#define DISABLE_LOWRES_TIMER_INTERRUPT		tzic = 0
#define	ENABLE_LOWRES_TIMER_INTERRUPT		tzic = 1
#define LOWRES_COUNTER_UPDATE_WAIT	// this timer runs synchronous to cpu clock

#define CLEAR_LOWRES_TIMER_FLAG				ir_tzic = 0		// interrupt request flag
#define LOWRES_TIMER_FLAG					ir_tzic			// interrupt request flag

/* timerC is used for high-resolution timing, and it runs continuously. */
#if defined SX1231
	#define CPU_CLOCK_MHZ					16.0		// in MHz
	#define TIMERC_CLOCK_DIV				32			// (/32 = 2µS each, 131mS max)
#elif defined SX1211
	#if defined SX1211_CRYSTAL_12_8MHZ
		#define CPU_CLOCK_MHZ					6.4			// in MHz
		#define TIMERC_CLOCK_DIV				8			// (/8 = 1.25µS each, 82mS max)
	#elif defined SX1211_CRYSTAL_14_7456MHZ
		#define CPU_CLOCK_MHZ					3.6864		// in MHz
		#define TIMERC_CLOCK_DIV				8			// (/8 = 2.17µS each, 142mS max)
	#endif
#endif
#define TIMERC_MICROSECONDS_PER_CLOCK		(1 / (CPU_CLOCK_MHZ / TIMERC_CLOCK_DIV))
/* make sure this macro is only applied to const variables. cannot do math at runtime */
#define HIRES_TIMEOUT(microseconds)			((microseconds / TIMERC_MICROSECONDS_PER_CLOCK) - 1)
#define HIRES_TIMER_MAX_VALUE				0xffff

/******************************************************************************************/

typedef uint8_t ltime_t;
/* timer-X is used as a prescaler for timer-Z */
#if defined SX1231
	#define TIMERX_CLOCK_SOURCE_HZ				16e6
	#define TIMERX_CLOCK_DIV_HARDWARE			2	// gives 8000KHz
	#define TIMERX_CLOCK_DIV_PREX				250	// gives 32.0KHz
	#define TIMERX_CLOCK_DIV_TX					125	// gives 256Hz
#elif defined SX1211
	#if defined SX1211_CRYSTAL_12_8MHZ
		#define TIMERX_CLOCK_SOURCE_HZ				6.4e6
		#define TIMERX_CLOCK_DIV_HARDWARE			8	// gives 800KHz
		#define TIMERX_CLOCK_DIV_PREX				25	// gives 32KHz
		#define TIMERX_CLOCK_DIV_TX					125	// gives 256Hz
	#elif defined SX1211_CRYSTAL_14_7456MHZ
		#define TIMERX_CLOCK_SOURCE_HZ				3686400
		#define TIMERX_CLOCK_DIV_HARDWARE			8	// gives 460.8KHz
		#define TIMERX_CLOCK_DIV_PREX				225	// gives 2048Hz
		#define TIMERX_CLOCK_DIV_TX					8	// gives 256Hz
	#endif
#endif
#define TIMERX_CLOCK_DIV (unsigned int)(TIMERX_CLOCK_DIV_HARDWARE * TIMERX_CLOCK_DIV_PREX * TIMERX_CLOCK_DIV_TX)
#define TIMERZ_MILLISECONDS_PER_CLOCK		( TIMERX_CLOCK_DIV / (TIMERX_CLOCK_SOURCE_HZ/1000.0) )
#define LOWRES_TIMEOUT(milliseconds)		((milliseconds / TIMERZ_MILLISECONDS_PER_CLOCK) - 3)


/******************************************************************************************/

#endif /* _R8C_TIMERS_H_ */

