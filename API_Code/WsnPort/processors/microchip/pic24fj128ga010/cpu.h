
#define rom_ptr		/* string constants are access via PSV window */
#define rom __attribute__((space(psv)))
#define ROM_STR(x)	x	/* too keep a (string literal) constant out of ram, noop on C30 */

#define READ_ROM_BYTE(p)  	p
#define READ_ROM_WORD(p)  	p

/* PSV window allows access as if it were in ram */
#define strcpy_from_rom		strcpy
#define memcpy_from_rom		memcpy
#define memcmp_from_rom		memcmp
#define strcmp_from_rom		strcmp

#define USART_send_str_from_rom		USART_send_str

#define ENABLE_GLOBAL_INTERRUPTS

/* watchdog stopped via FWDTEN_OFF in _CONFIG1 */
#define stop_wdt()

/* ltoa not in pic24 libc */
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

