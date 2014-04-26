#include "platform.h"
#include "wsn.h"

uint8_t dummy_led;

void
cpu_init(void)
{
}

void
poll_hardware_address()
{
	LCDProcessEvents();	/* continuously run the LCD state machine */

	/* initialization: come up as slave0 by default */
	if (hw_address == HW_ADDRESS__MAX)
	{
		hw_address = HW_ADDRESS__SLAVE0;
		new_hw_address();
	}

	if (++hw_address == HW_ADDRESS__MAX)
		hw_address = HW_ADDRESS__MASTER;

	new_hw_address();
}
