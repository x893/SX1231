#include <stdint.h>
#include "platform.h"
#include "led.h"
#include "bsp.h"

#include "em_chip.h"
#include "em_cmu.h"

// System tick (1ms)
volatile uint32_t TickCounter = 0;

void BoardInit( void )
{
	CHIP_Init();

	BSP_LedsInit();
	BSP_LedsSet(0xF);	// On all LEDs

	CMU_OscillatorEnable(cmuOsc_HFXO, true, true);
	CMU_ClockSelectSet(cmuClock_HF,cmuSelect_HFXO);

	/* Enable clock for HF peripherals */
	CMU_ClockEnable(cmuClock_HFPER, true);
	CMU_ClockEnable(cmuClock_GPIO, true);

	/* Setup SysTick Timer for 1 us interrupts ( not too often to save power ) */
	if( SysTick_Config( SystemCoreClock / 1000 ) )
	{ 	/* Capture error */ 
		while (1);
	}
	BSP_LedsInit();
}

void Delay ( uint16_t delay )
{
	// Wait delay ms
	uint32_t startTick = TickCounter;
	while( ( TickCounter - startTick ) < delay );
}

void LongDelay ( uint8_t delay )
{
	uint32_t longDelay;
	uint32_t startTick;

	longDelay = delay * 1000;

	// Wait delay s
	startTick = TickCounter;
	while( ( TickCounter - startTick ) < longDelay );   
}
