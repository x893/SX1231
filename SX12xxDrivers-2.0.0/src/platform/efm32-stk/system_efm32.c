#if   (PLATFORM == STK3200)

	#define EFM32_HFXO_FREQ	(24000000UL)
	#include "efm32libs\cmsis\system_efm32zg.c"

#elif (PLATFORM == STK3700)

	#define EFM32_HFXO_FREQ	(48000000UL)
	#include "efm32libs\cmsis\system_efm32gg.c"

#else

	#error Unknown board

#endif
