#include "types.h"
#include "cpu.h"
#include "wsn.h"
#include "transceiver.h"
#include "io_port_mapping.h"

#include "platform.h"


uint8_t pwmdac_dummy_compare;	// PWMDAC not implemented (could use timerX)

/******************************************************************************/

static char timerZ_expired;

#pragma interrupt TimerZ_isr
/**
 * this ISR wakes up from wait mode, which was entered from go_sleep()
 */
void
TimerZ_isr(void)
{
	ALARM_LED = 0;
	RX_OK_LED = 0;
	SLAVE_SLEEP_LED = 0;
	DISABLE_LOWRES_TIMER_INTERRUPT;

#ifdef SLAVE_ANSWER_ALL
	if (hop_on_next_wakeup) {
		Slave_Needs_Hop = TRUE;
		hop_on_next_wakeup = FALSE;
	}
#else
	Slave_Needs_Hop = TRUE;	// dont hop in ISR (too time-consuming). do it from main loop
#endif

	/* unnecessary to disable wait (sleep) mode on Rensas */
	timerZ_expired = TRUE;
}

/* EnableClock_HiRes(): uses OCR1B compare for high-resolution (2.17uS) timing */
void
EnableClock_HiRes(uint16_t compare_value)
{
	uint16_t new_compare;
	int idx;

	p1_0 = 1;
	/* the renesas compare registers TM0 and TM1 dont update
	 * when the counter is running, only upon TC overflow do they update */
	tcc00 = 0;	// stop counter, also resets counter
	CLEAR_HIRES_COMPARE_B_FLAG;
	HIRES_COMPARE_B = compare_value;	 // only updates immediately when couter is stopped
	tcc00 = 1;	// resume counting (from zero)

	p1_0 = 0;
}

//Enable and set lowres-timer function
void
EnableClock(uint16_t lowres_count)
{
	SET_LOWRES_COUNTER(lowres_count);

	// accomodate the lowres counter running async to cpu clock
	LOWRES_COUNTER_UPDATE_WAIT;

	CLEAR_LOWRES_TIMER_FLAG;

}

void
go_sleep()
{
	SetRFMode(RF_MODE_STANDBY);	//cannot RF_SLEEP, due to using its clkout as ours

	SLAVE_SLEEP_LED = 1;
	CLEAR_LOWRES_TIMER_FLAG;
	ENABLE_LOWRES_TIMER_INTERRUPT;
	timerZ_expired = FALSE;

	prc0 = 1;	// Enable write to CM registers
	cm02 = 0;	// keep peripherals running
	prc0 = 1;	// Disable write to CM registers
	do {
		asm("wait");
		asm("nop");
		asm("nop");
	} while (!timerZ_expired);	// some other non-timerZ ISR
}


/*******************************************************************
** Wait : Wait until an overflow interrupt drived by TIMER1       **
********************************************************************
** In  : cntVal		                                              **
** Out : -		                                                  **
*******************************************************************/
void
Wait(uint16_t compare_value)
{
	int idx;

	tcc00 = 0;	// stop counter, also resets counter
	CLEAR_HIRES_COMPARE_A_FLAG;
	HIRES_COMPARE_A = compare_value;
	tcc00 = 1;	// resume counting (from zero)

	while (!HIRES_COMPARE_A_FLAG)
			;
}

void
timers_init()
{
	/************** init low-res timer-X and Z *******/

#if TIMERX_CLOCK_DIV_HARDWARE == 8
	/* 460.8KHz -> timer-X (256Hz) -> Timer-Z */
	// timer-X: select source f8: 460.8KHz from 3.6864MHz
	txck0 = 1;
	txck1 = 0;
#elif TIMERX_CLOCK_DIV_HARDWARE == 2
	// timer-X: select source f8: 8000KHz from 16MHz
	txck0 = 1;
	txck1 = 1;
#else
	#error TIMERX_CLOCK_DIV_HARDWARE
#endif
	// timer-Z: select source timer-X underflow
	tzck0 = 0;
	tzck1 = 1;

	prex = TIMERX_CLOCK_DIV_PREX - 1;	// prescale to 2048Hz
	tx = TIMERX_CLOCK_DIV_TX - 1;		// divide to 256Hz, fed to timer Z

	txmr = 0x84;	// timer-X: timer mode, start counting
	//txic = 5;		// interrupt priority level 5
	//ir_txic = 0;	// interrupt request flag clear
	txs = 1;		// start Timer-X

	/* txund in txmr, bit 7*/

	prez = 0;		// no prescaling
	tzmr &= 0x04;	// timer-X: timer mode
	//tzic = 5;		// interrupt priority level 5
	//ir_tzic = 0;	// interrupt request flag clear
	tzs = 1;		// start Timer-Z


	/************** init hi-res timer C *******/
#if TIMERC_CLOCK_DIV == 8
	tcc01 = 1;	// source from 3.6864MHz, f8 = 460.8KHz
	tcc02 = 0;
#elif TIMERC_CLOCK_DIV == 32
	tcc01 = 0;	// source from 16MHz, f32 = 500KHz
	tcc02 = 1;
#else
	#error TIMERC_CLOCK_DIV
#endif

	tcc13 = 1;	// enable writing to tm0 for "compare A"

	tcc00 = 1;	// start count
}

