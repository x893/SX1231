#ifndef _CPU_H_
#define _CPU_H_
#include <stdlib.h>

#define rom PROGMEM	/* to declare variable to be in rom */
#define rom_ptr		/* pointer to rom, N/A on AVR */

/****** AVR needs these to read from program space (rom) *********/
#define READ_ROM_BYTE(p)  pgm_read_byte(&p)
#define READ_ROM_WORD(p)  pgm_read_word(&p)

#define ROM_STR		PSTR	/* to make string-literal stay only in rom/program-memory */

#define strcpy_from_rom		strcpy_P
#define strcmp_from_rom		strcmp_P
#define memcpy_from_rom		memcpy_P

#define CPU_WAKEUP

#define ltoa(i, s)	ltoa(i, s, 10)

#define ENABLE_GLOBAL_INTERRUPTS	sei()

#define cpu_init()	/* none necessary, fuse bits configure cpu */

void reset_cpu(void);
void stop_wdt(void);

typedef int main_return_t;
#define MAIN_RETURN_VALUE	0

/* declare the clock output we want from RF transceiver */
/* AVR has its own crystal */
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

#endif /* _CPU_H_ */

