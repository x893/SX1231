#include <stdlib.h>	// for ltoa

#define rom_ptr		rom	/* pointer to rom */
#define ROM_STR(x)	x	/* too keep a (string literal) constant out of ram, noop on C18 */

#define READ_ROM_BYTE(p)	p
#define READ_ROM_WORD(p)	p

/* either set compiler to large memory model, or recompile libc using small memory model */
#define strcpy_from_rom		strcpypgm2ram
#define strcmp_from_rom		strcmppgm2ram
#define memcpy_from_rom		memcpypgm2ram

void cpu_init(void);

#define ENABLE_GLOBAL_INTERRUPTS	\
	RCONbits.IPEN = 1;				\
	INTCONbits.GIEL = 1;			\
	INTCONbits.GIEH = 1

// watchdog stopped via "pragma config WDTEN = OFF"
#define stop_wdt()

/* exported from cpu.c: */
extern char debounce_count;
extern char RA5_pressed;



void uart_isr(void);

typedef void main_return_t;
#define MAIN_RETURN_VALUE

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

