#ifndef __TIMERS_H__
#define __TIMERS_H__

#include <stdint.h>
#include <stdbool.h>

#define UPDATE_PWMDAC_COMPARE(x)

/************************************************************************************/
/* Timer1 as low-res timer due to operation from 32768Hz crystal "Type A" timer"		*/
typedef uint16_t ltime_t;
extern volatile uint16_t LowResTimer;
extern volatile bool LowResDone;
extern volatile uint16_t HiResTimer;
extern volatile bool HiResDone;

/*
#define TIMER1_CLOCK_SOURCE_HZ			32768
#define TIMER1_CLOCK_DIV				64
#define TIMER1_MILLISECONDS_PER_CLOCK	(1 / ((TIMER1_CLOCK_SOURCE_HZ / 1000.0) / TIMER1_CLOCK_DIV))
#define LOWRES_TIMEOUT(ms)				(ltime_t)(ms / TIMER1_MILLISECONDS_PER_CLOCK)
*/
#define LOWRES_TIMEOUT(ms)				(ltime_t)(ms)

/* set the period register: this timer counts up to the period register */
#define SET_LOWRES_COUNTER(x)			LowResTimer = 0; LowResDone = 0; LowResTimer = x

#define LOWRES_TIMER_FLAG				(LowResDone != 0)
#define CLEAR_LOWRES_TIMER_FLAG
#define ENABLE_LOWRES_TIMER_INTERRUPT
#define DISABLE_LOWRES_TIMER_INTERRUPT

/*************************************************************************************/
/* Timer2 as hi-res timer "Type B" timer */
#define TIMER2_CLOCK_SRC_MHZ			4	// in MHz: Fosc/2 = 4MHz
#define TIMER2_CLOCK_DIV				8	// 4 / 8 = 500KHz
#define TIMER2_MICROSECONDS_PER_CLOCK	(1 / (TIMER2_CLOCK_SRC_MHZ / (float)TIMER2_CLOCK_DIV))
#define HIRES_TIMEOUT(us)				(uint16_t)(us / TIMER2_MICROSECONDS_PER_CLOCK)
#define HIRES_TIMER_MAX_VALUE			0xffff	// up counter

#define HIRES_COMPARE_B_FLAG			(HiResDone != 0)

#endif
