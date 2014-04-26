#include <avr/io.h>
#include <avr/wdt.h>
//#include "cpu.h"
#include "platform.h"
#include "WSN.h"

uint8_t dummy_led;

void
poll_hardware_address()
{
	/* initialization: come up as slave0 by default */
	if (hw_address == HW_ADDRESS__MAX) {

		hw_address = HW_ADDRESS__MASTER;
//		hw_address = HW_ADDRESS__SLAVE0;
//		hw_address = HW_ADDRESS__SLAVE1;
//		hw_address = HW_ADDRESS__SLAVE2;
//		hw_address = HW_ADDRESS__SLAVE3;

		new_hw_address();
	}


}

void
stop_wdt()
{
	// Clear WDRF in MCUSR */
	MCUSR &= ~(1<<WDRF);
	// Write logical one to WDCE and WDE
	// Keep old prescaler setting to prevent unintentional time-out
	WDTCSR |= (1<<WDCE) | (1<<WDE);
	// Turn off WDT
	WDTCSR = 0x00;
}

void
reset_cpu()
{
	wdt_enable(WDTO_30MS);

	while (1)
		;
}

