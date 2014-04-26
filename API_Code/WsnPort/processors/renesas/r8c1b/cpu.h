#ifndef _CPU_H_
#define _CPU_H_

/* Renesas is vonneuman */
#define READ_ROM_BYTE(x)		x
#define READ_ROM_WORD(x)		x
#define ROM_STR(x)				x
#define strcpy_from_rom			strcpy
#define strcmp_from_rom			strcmp
#define memcpy_from_rom			memcpy
#define rom_ptr
#define rom
#define USART_send_str_from_rom		USART_send_str


#define CPU_WAKEUP


// the OFS register is in flash, and by default disables watchdog after reset
#define stop_wdt()

#define reset_cpu()	\
	wdts = 1;		\
	for (;;) asm("nop");

#define ENABLE_GLOBAL_INTERRUPTS	asm("fset i")

/* Renesas doesnt provide ltoa() */
#define _ltoa	ltoa
int ltoa(long val, char *buffer);

void cpu_init(void);

typedef void main_return_t;
#define MAIN_RETURN_VALUE

void osc_internal_hispeed(void);

/* declare the clock output we want from RF transceiver */
#if defined SX1231
	#define RF_CLKOUT_DIV		RF_DIOMAPPING2_CLKOUT_16	/* we want 16MHz to r8c/1b */
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
