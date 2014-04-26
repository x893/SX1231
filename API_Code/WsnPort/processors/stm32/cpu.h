#ifndef __CPU_H__
#define __CPU_H__

#include "stdint.h"

#define rom_ptr
#define rom
#define ROM_STR(x)	x

#define READ_ROM_BYTE(p)	p
#define READ_ROM_WORD(p)	p

#define strcpy_from_rom		strcpy
#define memcpy_from_rom		memcpy
#define memcmp_from_rom		memcmp
#define strcmp_from_rom		strcmp

#define ENABLE_GLOBAL_INTERRUPTS
#define stop_wdt()

#define _ltoa	ltoa
int ltoa(long val, char *buffer);

void cpu_init(void);

typedef int main_return_t;

#define MAIN_RETURN_VALUE	0

/* declare the clock output we want from RF transceiver */
#if defined SX1231
	#define RF_CLKOUT_DIV		RF_DIOMAPPING2_CLKOUT_OFF		/* we dont need any clkout */
#elif defined SX1211
	#if defined SX1211_CRYSTAL_12_8MHZ
		#define RF_CLKOUT_DIV		CLKOUT_DISABLE	/* we dont need any clkout */
	#elif defined SX1211_CRYSTAL_14_7456MHZ
		#define RF_CLKOUT_DIV		CLKOUT_DISABLE	/* we dont need any clkout */
	#else
		#error SX1211_CRYSTAL
	#endif
#endif

#endif
