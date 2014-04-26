#include "types.h"
#include "timers.h"
#include "transceiver.h"
#include "io_port_mapping.h"
#include "wsn.h"
#include "cpu.h"

uint8_t pwmdac_dummy_compare;	// PWMDAC not implemented (could use timerX)

/*****************************************************/
#pragma vector=TIMERA1_VECTOR
/**
 * this ISR wakes up from sleep mode, which was entered from go_sleep()
 */
__interrupt void
TimerA1_isr(void)
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

	// necessary on MSP430 to clear timer interrupt flag
	CLEAR_LOWRES_TIMER_FLAG;

	_BIC_SR_IRQ(LPM3_bits); // Disable sleep mode, run main software
}


/* EnableClock_HiRes(): uses OCR1B compare for high-resolution (2.17uS) timing */
void
EnableClock_HiRes(uint16_t compare_value)
{
	uint16_t counter_snapshot;

	counter_snapshot = HIRES_COUNTER;
	CLEAR_HIRES_COMPARE_B_FLAG;
	HIRES_COMPARE_B = counter_snapshot + compare_value;
	/* ISR not used, will be polled via COMPARE_B_FLAG */
}

//Enable and set lowres-timer function
void
EnableClock(uint16_t lowres_count)
{
	SET_LOWRES_COUNTER(lowres_count);

	LOWRES_COUNTER_UPDATE_WAIT;

	CLEAR_LOWRES_TIMER_FLAG;

} // void EnableTimeOutSync(uint8_t enable,uint16 TimeOut)

void
go_sleep()
{
	/* dont sleep the radio if we are running from its clkout */
	SetRFMode(RF_MODE_STANDBY);

	ENABLE_LOWRES_TIMER_INTERRUPT;
	SLAVE_SLEEP_LED = 1;

	_BIS_SR(LPM3_bits + GIE);	// Enter LPM3 w/interrupt
	_NOP();
	_NOP();
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
	CLEAR_HIRES_COMPARE_A_FLAG;
	HIRES_COMPARE_A = HIRES_COUNTER	+ compare_value;

	while (!HIRES_COMPARE_A_FLAG)
			;
}

void
timers_init()
{
	/******** init low-res timer (timer-A) ************/
	/* source from ACLK, 32,768Hz/8 = 4096Hz -- continuous up mode
	 * div-by-16 from LOWRES_TIMEOUT() macro, gives 256Hz rate */
	TACTL = TASSEL_1 + ID_3 + MC_2;

	/******** init hi-res timer (timer-B) ************/
	/* SMCLK expected to be sourced from external 3.6864MHz */
	TBCTL = TBSSEL_2 + ID_3 + MC_2;	// source from SMCLK/8=460.8KHz, 16bit continuous mode
}

