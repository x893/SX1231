#include <types.h>
#include "sfr_r81B.h"
#include "io_port_mapping.h"
//#include "SX1211driver.h"

#include "platform.h"

/* INIT_SPI(MASTER, CLK_POLARITY, PHASE, MSB_FIRST) */
//INIT_SPI(TRUE, 0, 0, TRUE);
void
SPIInit()
{
	osc_internal_hispeed();

	re_sser = 0;
	te_sser = 0;

#if defined SX1211
	NSS_CONFIG = 1;
	NSS_DATA = 1;
	DIR_NSS_DATA = 1;
	DIR_NSS_CONFIG = 1;

	sscrh = 0x25;	// 0x20: master, 0x06: f/8 (460,800)

#elif defined SX1231
	NSS = 1;
	DIR_NSS = 1;

	//sscrh = 0x26;	// 0x20: master, 0x06: f/4 (4MHz)
	sscrh = 0x25;	// 0x20: master, 0x06: f/8 (2MHz)
#endif

	//sscrh = 0x26;	// 0x20: master, 0x06: f/4 (921,600)
	//sscrh = 0x25;	// 0x20: master, 0x06: f/8 (460,800)
	//sscrh = 0x24;	// 0x20: master, 0x06: f/16 (230,400)
	//sscrh = 0x22;	// 0x20: master, 0x06: f/64 (57,600)
	//sscrh = 0x20;	// 0x20: master, 0x06: f/256 (14,400)

	sscrl = 0x00;

	cpos_ssmr = 1;

	cphs_ssmr = 1;

	mls_ssmr = 0;// if sending MSb first //

	ssmr2 = 0x40;
	sser = 0x08;
	pmr = 0x08;

}

uint8_t
SpiInOut(uint8_t outputByte)
{
	uint8_t ret;


	re_sser = 1;
	te_sser = 1;

	// wait for transmit data register to be ready
	while (!tdre_sssr)
		;
	sstdr = outputByte;

	// wait for data to be received
	while (!rdrf_sssr)
		;

	ret = ssrdr;

	// make sure the transmission is complete
	while (!tend_sssr)
			;

	tend_sssr = 1;
	re_sser = 0;
	te_sser = 0;


	return ret;
}

