#ifndef _CPU_H_
#define _CPU_H_
#include <stdlib.h>	// to get ltoa

/* msp430 is not harvard arch */
#define rom
#define rom_ptr
#define ROM_STR(x)			x
#define READ_ROM_BYTE(x)	x
#define READ_ROM_WORD(x)	x
#define strcpy_from_rom			strcpy
#define strcmp_from_rom			strcmp
#define memcpy_from_rom			memcpy
#define USART_send_str_from_rom		USART_send_str


#define stop_wdt()		WDTCTL = WDTPW + WDTHOLD

#define ENABLE_GLOBAL_INTERRUPTS		_EINT()

typedef void main_return_t;
#define MAIN_RETURN_VALUE

/* declare the clock output we want from RF transceiver */
#if defined SX1231
	/* we want 4MHz to msp430 -- lower speed limit on this device */
	#define RF_CLKOUT_DIV		RF_DIOMAPPING2_CLKOUT_4
#elif defined SX1211
	#if defined SX1211_CRYSTAL_12_8MHZ
		#define RF_CLKOUT_DIV		CLKOUT_DIVBY_2	// output 6.4MHz
	#elif defined SX1211_CRYSTAL_14_7456MHZ
		#define RF_CLKOUT_DIV		CLKOUT_DIVBY_4	// output 3.6864MHz
	#else
		#error SX1211_CRYSTAL
	#endif
#endif


#endif /* _CPU_H_ */
