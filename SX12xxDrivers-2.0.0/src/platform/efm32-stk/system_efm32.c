#if   (PLATFORM == STK3200)
	#include "efm32libs\cmsis\system_efm32zg.c"
#elif (PLATFORM == STK3700)
	#include "efm32libs\cmsis\system_efm32gg.c"
#else
	#error Unknown board
#endif
