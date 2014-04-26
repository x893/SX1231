#include "transceiver.h"
#include "io_port_mapping.h"
#include "wsn.h"

volatile uint16_t LowResTimer;
volatile bool LowResDone;

volatile uint16_t HiResTimer;
volatile bool HiResDone;

void
EnableClock_HiRes(uint16_t timeout)
{
	HiResTimer = 0;
	HiResDone = 0;
	HiResTimer = timeout;
}

void
EnableClock(uint16_t lowres_count)
{
	SET_LOWRES_COUNTER(lowres_count);
	CLEAR_LOWRES_TIMER_FLAG;
}

void
Wait(uint16_t timeout)
{
	EnableClock_HiRes(timeout);
	while (HiResDone == 0)
		;
}

void
timers_init()
{

}

void
go_sleep()
{
	SetRFMode(RF_MODE_SLEEP);	// use RF_STANDBY if using clkout from radio

	SLAVE_SLEEP_LED = 1;
	CLEAR_LOWRES_TIMER_FLAG;
	ENABLE_LOWRES_TIMER_INTERRUPT;
	do
	{
		// sleep now
	} while (!LOWRES_TIMER_FLAG);
}
