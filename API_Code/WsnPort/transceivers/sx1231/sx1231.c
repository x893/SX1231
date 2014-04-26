#if defined SX1231
#include <string.h>
#include "sx1231.h"
#include "transceiver.h"
#include "platform.h"
#include "FHSSapi.h"	// get NUM_CHANNELS

typedef enum
{
	RF_STATE_STOP = 0,
	RF_STATE_RX_BUSY,	// 1
	RF_STATE_TX_BUSY,	// 2
	RF_STATE_RX_DONE,
	RF_STATE_ERROR,
	RF_STATE_TIMEOUT
} rf_state_e;

/******************** ...private types ********************************/

typedef struct {
	uint8_t	msb;
	uint8_t	mid;
	uint8_t	lsb;
} freq_t;

#if defined AMATEUR_70CM
/*	{  msb,  mid,  lsb },	*/
const rom freq_t ch_0  = { 0x6c, 0x40, 0x00 }; /*  0 433.000000 */
const rom freq_t ch_1  = { 0x6c, 0x42, 0x9d }; /*  1 433.040816 */
const rom freq_t ch_2  = { 0x6c, 0x45, 0x39 }; /*  2 433.081633 */
const rom freq_t ch_3  = { 0x6c, 0x47, 0xd6 }; /*  3 433.122449 */
const rom freq_t ch_4  = { 0x6c, 0x4a, 0x73 }; /*  4 433.163265 */
const rom freq_t ch_5  = { 0x6c, 0x4d, 0x10 }; /*  5 433.204082 */
const rom freq_t ch_6  = { 0x6c, 0x4f, 0xac }; /*  6 433.244898 */
const rom freq_t ch_7  = { 0x6c, 0x52, 0x49 }; /*  7 433.285714 */
const rom freq_t ch_8  = { 0x6c, 0x54, 0xe6 }; /*  8 433.326531 */
const rom freq_t ch_9  = { 0x6c, 0x57, 0x83 }; /*  9 433.367347 */
const rom freq_t ch_10 = { 0x6c, 0x5a, 0x1f }; /* 10 433.408163 */
const rom freq_t ch_11 = { 0x6c, 0x5c, 0xbc }; /* 11 433.448980 */
const rom freq_t ch_12 = { 0x6c, 0x5f, 0x59 }; /* 12 433.489796 */
const rom freq_t ch_13 = { 0x6c, 0x61, 0xf6 }; /* 13 433.530612 */
const rom freq_t ch_14 = { 0x6c, 0x64, 0x92 }; /* 14 433.571429 */
const rom freq_t ch_15 = { 0x6c, 0x67, 0x2f }; /* 15 433.612245 */
const rom freq_t ch_16 = { 0x6c, 0x69, 0xcc }; /* 16 433.653061 */
const rom freq_t ch_17 = { 0x6c, 0x6c, 0x68 }; /* 17 433.693878 */
const rom freq_t ch_18 = { 0x6c, 0x6f, 0x05 }; /* 18 433.734694 */
const rom freq_t ch_19 = { 0x6c, 0x71, 0xa2 }; /* 19 433.775510 */
const rom freq_t ch_20 = { 0x6c, 0x74, 0x3f }; /* 20 433.816327 */
const rom freq_t ch_21 = { 0x6c, 0x76, 0xdb }; /* 21 433.857143 */
const rom freq_t ch_22 = { 0x6c, 0x79, 0x78 }; /* 22 433.897959 */
const rom freq_t ch_23 = { 0x6c, 0x7c, 0x15 }; /* 23 433.938776 */
const rom freq_t ch_24 = { 0x6c, 0x7e, 0xb2 }; /* 24 433.979592 */
const rom freq_t ch_25 = { 0x6c, 0x81, 0x4e }; /* 25 434.020408 */
const rom freq_t ch_26 = { 0x6c, 0x83, 0xeb }; /* 26 434.061224 */
const rom freq_t ch_27 = { 0x6c, 0x86, 0x88 }; /* 27 434.102041 */
const rom freq_t ch_28 = { 0x6c, 0x89, 0x25 }; /* 28 434.142857 */
const rom freq_t ch_29 = { 0x6c, 0x8b, 0xc1 }; /* 29 434.183673 */
const rom freq_t ch_30 = { 0x6c, 0x8e, 0x5e }; /* 30 434.224490 */
const rom freq_t ch_31 = { 0x6c, 0x90, 0xfb }; /* 31 434.265306 */
const rom freq_t ch_32 = { 0x6c, 0x93, 0x98 }; /* 32 434.306122 */
const rom freq_t ch_33 = { 0x6c, 0x96, 0x34 }; /* 33 434.346939 */
const rom freq_t ch_34 = { 0x6c, 0x98, 0xd1 }; /* 34 434.387755 */
const rom freq_t ch_35 = { 0x6c, 0x9b, 0x6e }; /* 35 434.428571 */
const rom freq_t ch_36 = { 0x6c, 0x9e, 0x0a }; /* 36 434.469388 */
const rom freq_t ch_37 = { 0x6c, 0xa0, 0xa7 }; /* 37 434.510204 */
const rom freq_t ch_38 = { 0x6c, 0xa3, 0x44 }; /* 38 434.551020 */
const rom freq_t ch_39 = { 0x6c, 0xa5, 0xe1 }; /* 39 434.591837 */
const rom freq_t ch_40 = { 0x6c, 0xa8, 0x7d }; /* 40 434.632653 */
const rom freq_t ch_41 = { 0x6c, 0xab, 0x1a }; /* 41 434.673469 */
const rom freq_t ch_42 = { 0x6c, 0xad, 0xb7 }; /* 42 434.714286 */
const rom freq_t ch_43 = { 0x6c, 0xb0, 0x54 }; /* 43 434.755102 */
const rom freq_t ch_44 = { 0x6c, 0xb2, 0xf0 }; /* 44 434.795918 */
const rom freq_t ch_45 = { 0x6c, 0xb5, 0x8d }; /* 45 434.836735 */
const rom freq_t ch_46 = { 0x6c, 0xb8, 0x2a }; /* 46 434.877551 */
const rom freq_t ch_47 = { 0x6c, 0xba, 0xc7 }; /* 47 434.918367 */
const rom freq_t ch_48 = { 0x6c, 0xbd, 0x63 }; /* 48 434.959184 */
const rom freq_t ch_49 = { 0x6c, 0xc0, 0x00 }; /* 49 435.000000 */
#endif	/* AMATEUR_70CM */

#if defined ISM_900
/*	{  msb,  mid,  lsb },	*/
const rom freq_t ch_0 = { 0xe1, 0xcf, 0x5c }; /* 903.240000 */
const rom freq_t ch_1 = { 0xe1, 0xee, 0x14 }; /* 903.720000 */
const rom freq_t ch_2 = { 0xe2, 0x0c, 0xcd }; /* 904.200000 */
const rom freq_t ch_3 = { 0xe2, 0x2b, 0x85 }; /* 904.680000 */
const rom freq_t ch_4 = { 0xe2, 0x4a, 0x3d }; /* 905.160000 */
const rom freq_t ch_5 = { 0xe2, 0x68, 0xf6 }; /* 905.640000 */
const rom freq_t ch_6 = { 0xe2, 0x87, 0xae }; /* 906.120000 */
const rom freq_t ch_7 = { 0xe2, 0xa6, 0x66 }; /* 906.600000 */
const rom freq_t ch_8 = { 0xe2, 0xc5, 0x1f }; /* 907.080000 */
const rom freq_t ch_9 = { 0xe2, 0xe3, 0xd7 }; /* 907.560000 */
const rom freq_t ch_10 = { 0xe3, 0x02, 0x8f }; /* 908.040000 */
const rom freq_t ch_11 = { 0xe3, 0x21, 0x48 }; /* 908.520000 */
const rom freq_t ch_12 = { 0xe3, 0x40, 0x00 }; /* 909.000000 */
const rom freq_t ch_13 = { 0xe3, 0x5e, 0xb8 }; /* 909.480000 */
const rom freq_t ch_14 = { 0xe3, 0x7d, 0x71 }; /* 909.960000 */
const rom freq_t ch_15 = { 0xe3, 0x9c, 0x29 }; /* 910.440000 */
const rom freq_t ch_16 = { 0xe3, 0xba, 0xe1 }; /* 910.920000 */
const rom freq_t ch_17 = { 0xe3, 0xd9, 0x9a }; /* 911.400000 */
const rom freq_t ch_18 = { 0xe3, 0xf8, 0x52 }; /* 911.880000 */
const rom freq_t ch_19 = { 0xe4, 0x17, 0x0a }; /* 912.360000 */
const rom freq_t ch_20 = { 0xe4, 0x35, 0xc3 }; /* 912.840000 */
const rom freq_t ch_21 = { 0xe4, 0x54, 0x7b }; /* 913.320000 */
const rom freq_t ch_22 = { 0xe4, 0x73, 0x33 }; /* 913.800000 */
const rom freq_t ch_23 = { 0xe4, 0x91, 0xec }; /* 914.280000 */
const rom freq_t ch_24 = { 0xe4, 0xb0, 0xa4 }; /* 914.760000 */
const rom freq_t ch_25 = { 0xe4, 0xcf, 0x5c }; /* 915.240000 */
const rom freq_t ch_26 = { 0xe4, 0xee, 0x14 }; /* 915.720000 */
const rom freq_t ch_27 = { 0xe5, 0x0c, 0xcd }; /* 916.200000 */
const rom freq_t ch_28 = { 0xe5, 0x2b, 0x85 }; /* 916.680000 */
const rom freq_t ch_29 = { 0xe5, 0x4a, 0x3d }; /* 917.160000 */
const rom freq_t ch_30 = { 0xe5, 0x68, 0xf6 }; /* 917.640000 */
const rom freq_t ch_31 = { 0xe5, 0x87, 0xae }; /* 918.120000 */
const rom freq_t ch_32 = { 0xe5, 0xa6, 0x66 }; /* 918.600000 */
const rom freq_t ch_33 = { 0xe5, 0xc5, 0x1f }; /* 919.080000 */
const rom freq_t ch_34 = { 0xe5, 0xe3, 0xd7 }; /* 919.560000 */
const rom freq_t ch_35 = { 0xe6, 0x02, 0x8f }; /* 920.040000 */
const rom freq_t ch_36 = { 0xe6, 0x21, 0x48 }; /* 920.520000 */
const rom freq_t ch_37 = { 0xe6, 0x40, 0x00 }; /* 921.000000 */
const rom freq_t ch_38 = { 0xe6, 0x5e, 0xb8 }; /* 921.480000 */
const rom freq_t ch_39 = { 0xe6, 0x7d, 0x71 }; /* 921.960000 */
const rom freq_t ch_40 = { 0xe6, 0x9c, 0x29 }; /* 922.440000 */
const rom freq_t ch_41 = { 0xe6, 0xba, 0xe1 }; /* 922.920000 */
const rom freq_t ch_42 = { 0xe6, 0xd9, 0x9a }; /* 923.400000 */
const rom freq_t ch_43 = { 0xe6, 0xf8, 0x52 }; /* 923.880000 */
const rom freq_t ch_44 = { 0xe7, 0x17, 0x0a }; /* 924.360000 */
const rom freq_t ch_45 = { 0xe7, 0x35, 0xc3 }; /* 924.840000 */
const rom freq_t ch_46 = { 0xe7, 0x54, 0x7b }; /* 925.320000 */
const rom freq_t ch_47 = { 0xe7, 0x73, 0x33 }; /* 925.800000 */
const rom freq_t ch_48 = { 0xe7, 0x91, 0xec }; /* 926.280000 */
const rom freq_t ch_49 = { 0xe7, 0xb0, 0xa4 }; /* 926.760000 */
#endif	/* ISM_900 */

/******* generate your random number sequence at http://random.org *******/
const rom freq_t * freqs_P[NUM_CHANNELS] = {
	&ch_22,		// 0
	&ch_20,		// 1
	&ch_0,		// 2
	&ch_6,		// 3
	&ch_46,		// 4
	&ch_23,		// 5
	&ch_47,		// 6
	&ch_49,		// 7
	&ch_37,		// 8
	&ch_39,		// 9
	&ch_41,		// 10
	&ch_24,		// 11
	&ch_4,		// 12
	&ch_34,		// 13
	&ch_36,		// 14
	&ch_16,		// 15
	&ch_27,		// 16
	&ch_48,		// 17
	&ch_40,		// 18
	&ch_11,		// 19
	&ch_10,		// 20
	&ch_32,		// 21
	&ch_21,		// 22
	&ch_28,		// 23
	&ch_18,		// 24
	&ch_5,		// 25
	&ch_29,		// 26
	&ch_44,		// 27
	&ch_19,		// 28
	&ch_12,		// 29
	&ch_1,		// 30
	&ch_33,		// 31
	&ch_35,		// 32
	&ch_8,		// 33
	&ch_43,		// 34
	&ch_30,		// 35
	&ch_13,		// 36
	&ch_42,		// 37
	&ch_9,		// 38
	&ch_3,		// 39
	&ch_31,		// 40
	&ch_14,		// 41
	&ch_38,		// 42
	&ch_2,		// 43
	&ch_25,		// 44
	&ch_7,		// 45
	&ch_26,		// 46
	&ch_15,		// 47
	&ch_17,		// 48
	&ch_45,		// 49
};

const uint8_t RegistersCfg[] =
{	// SX1231 configuration registers values
	/* 0x00 */ DEF_FIFO, // Left for convenience, not to be changed
	/* 0x01 */ DEF_OPMODE | RF_OPMODE_SEQUENCER_ON | RF_OPMODE_LISTEN_OFF | RF_OPMODE_STANDBY,
	/* 0x02 */ DEF_DATAMODUL | RF_DATAMODUL_DATAMODE_PACKET | RF_DATAMODUL_MODULATIONTYPE_FSK | RF_DATAMODUL_MODULATIONSHAPING_00,
	/* 0x03 */ DEF_BITRATEMSB | RF_BITRATEMSB_25000,
	/* 0x04 */ DEF_BITRATELSB | RF_BITRATELSB_25000,
	/* 0x05 */ DEF_FDEVMSB | RF_FDEVMSB_50000,
	/* 0x06 */ DEF_FDEVLSB | RF_FDEVLSB_50000,
	/* 0x07 */ DEF_FRFMSB | RF_FRFMSB_915,
	/* 0x08 */ DEF_FRFMID | RF_FRFMID_915,
	/* 0x09 */ DEF_FRFLSB | RF_FRFLSB_915,
	/* 0x0a */ DEF_OSC1,
	/* 0x0b */ DEF_OSC2,
	/* 0x0c */ DEF_LOWBAT | RF_LOWBAT_OFF | RF_LOWBAT_TRIM_1835,
	/* 0x0d */ DEF_LISTEN1 | RF_LISTEN1_RESOL_4100 | RF_LISTEN1_CRITERIA_RSSI | RF_LISTEN1_END_01,
	/* 0x0e */ DEF_LISTEN2 | RF_LISTEN2_COEFIDLE_VALUE,
	/* 0x0f */ DEF_LISTEN3 | RF_LISTEN3_COEFRX_VALUE,
	/* 0x10 */ DEF_VERSION,			// Read Only
	/* 0x11 */ DEF_PALEVEL | RF_PALEVEL_PA0_ON | RF_PALEVEL_PA1_OFF | RF_PALEVEL_PA2_OFF | RF_PALEVEL_OUTPUTPOWER_11111,
	/* 0x12 */ DEF_PARAMP | RF_PARAMP_40,
	/* 0x13 */ DEF_OCP | RF_OCP_ON | RF_OCP_TRIM_100,
	/* 0x14 */ DEF_AGCREF | RF_AGCREF_AUTO_ON | RF_AGCREF_LEVEL_MINUS80,
	/* 0x15 */ DEF_AGCTHRESH1 | RF_AGCTHRESH1_SNRMARGIN_101 | RF_AGCTHRESH1_STEP1_16,
	/* 0x16 */ DEF_AGCTHRESH2 | RF_AGCTHRESH2_STEP2_7 | RF_AGCTHRESH2_STEP3_11,
	/* 0x17 */ DEF_AGCTHRESH3 | RF_AGCTHRESH3_STEP4_9 | RF_AGCTHRESH3_STEP5_11,
	/* 0x18 */ DEF_LNA | RF_LNA_ZIN_200 | RF_LNA_LOWPOWER_OFF | RF_LNA_GAINSELECT_AUTO,
	/* 0x19 */ DEF_RXBW | RF_RXBW_DCCFREQ_010 | RF_RXBW_MANT_24 | RF_RXBW_EXP_2,	/* exp2=83KHz ssb bw */
	/* 0x1a */ DEF_AFCBW | RF_AFCBW_DCCFREQAFC_100 | RF_AFCBW_MANTAFC_20 | RF_AFCBW_EXPAFC_3,
	/* 0x1b */ DEF_OOKPEAK | RF_OOKPEAK_THRESHTYPE_PEAK | RF_OOKPEAK_PEAKTHRESHSTEP_000 | RF_OOKPEAK_PEAKTHRESHDEC_000,
	/* 0x1c */ DEF_OOKAVG | RF_OOKAVG_AVERAGETHRESHFILT_10,
	/* 0x1d */ DEF_OOKFIX | RF_OOKFIX_FIXEDTHRESH_VALUE,
	/* 0x1e */ DEF_AFCFEI | RF_AFCFEI_AFCAUTOCLEAR_OFF | RF_AFCFEI_AFCAUTO_OFF,
	/* 0x1f */ DEF_AFCMSB,			// Read Only
	/* 0x20 */ DEF_AFCLSB,			// Read Only
	/* 0x21 */ DEF_FEIMSB,			// Read Only
	/* 0x22 */ DEF_FEILSB,			// Read Only
	/* 0x23 */ DEF_RSSICONFIG | RF_RSSI_FASTRX_OFF,
	/* 0x24 */ DEF_RSSIVALUE,		// Read Only
	/* 0x25 */ DEF_DIOMAPPING1 | RF_DIOMAPPING1_DIO0_00 | RF_DIOMAPPING1_DIO1_11 | RF_DIOMAPPING1_DIO2_01 | RF_DIOMAPPING1_DIO3_01,
	/* 0x26 */ DEF_DIOMAPPING2 | RF_DIOMAPPING2_DIO4_00 | RF_DIOMAPPING2_DIO5_00 | RF_CLKOUT_DIV,	/* RF_DIOMAPPING2_CLKOUT_16, */
	/* 0x27 */ DEF_IRQFLAGS1,
	/* 0x28 */ DEF_IRQFLAGS2 | RF_IRQFLAGS2_FIFOOVERRUN,	// clear any previous fifo overrun
	/* 0x29 */ DEF_RSSITHRESH | 200,	// Must be set to dBm = (-Sensitivity / 2)
	/* 0x2a */ DEF_RXTIMEOUT1 | RF_RXTIMEOUT1_RXSTART_VALUE,
	/* 0x2b */ DEF_RXTIMEOUT2 | RF_RXTIMEOUT2_RSSITHRESH_VALUE,
	/* 0x2c */ DEF_PREAMBLEMSB | 0x00 /* RF_PREAMBLESIZE_MSB_VALUE */,
	/* 0x2d */ DEF_PREAMBLELSB | 0x04 /* RF_PREAMBLESIZE_LSB_VALUE */,
	/* 0x2e */ DEF_SYNCCONFIG | RF_SYNC_ON | RF_SYNC_FIFOFILL_AUTO | RF_SYNC_SIZE_4 | RF_SYNC_TOL_0,
	/* 0x2f */ DEF_SYNCVALUE1 | 0x69,
	/* 0x30 */ DEF_SYNCVALUE2 | 0x81,
	/* 0x31 */ DEF_SYNCVALUE3 | 0x7E,
	/* 0x32 */ DEF_SYNCVALUE4 | 0x96,
	/* 0x33 */ DEF_SYNCVALUE5 | RF_SYNC_BYTE5_VALUE,
	/* 0x34 */ DEF_SYNCVALUE6 | RF_SYNC_BYTE6_VALUE,
	/* 0x35 */ DEF_SYNCVALUE7 | RF_SYNC_BYTE7_VALUE,
	/* 0x36 */ DEF_SYNCVALUE8 | RF_SYNC_BYTE8_VALUE,
	/* 0x37 */ DEF_PACKETCONFIG1 | RF_PACKET1_FORMAT_VARIABLE | RF_PACKET1_DCFREE_OFF | RF_PACKET1_CRC_ON | RF_PACKET1_CRCAUTOCLEAR_ON | RF_PACKET1_ADRSFILTERING_OFF,
	/* 0x38 */ DEF_PAYLOADLENGTH | 255,
	/* 0x39 */ DEF_NODEADRS | RF_NODEADDRESS_VALUE,
	/* 0x3a */ DEF_BROADCASTADRS | RF_BROADCASTADDRESS_VALUE,
	/* 0x3b */ DEF_AUTOMODES | RF_AUTOMODES_ENTER_OFF | RF_AUTOMODES_EXIT_OFF | RF_AUTOMODES_INTERMEDIATE_SLEEP,
	/* 0x3c */ DEF_FIFOTHRESH | RF_FIFOTHRESH_TXSTART_FIFONOTEMPTY | RF_FIFOTHRESH_VALUE,
	/* 0x3d */ DEF_PACKETCONFIG2 | RF_PACKET2_RXRESTARTDELAY_2BITS | RF_PACKET2_AUTORXRESTART_ON | RF_PACKET2_AES_OFF,
	/* 0x3e */ DEF_AESKEY1 | RF_AESKEY1_VALUE,
	/* 0x3f */ DEF_AESKEY2 | RF_AESKEY2_VALUE,
	/* 0x40 */ DEF_AESKEY3 | RF_AESKEY3_VALUE,
	/* 0x41 */ DEF_AESKEY4 | RF_AESKEY4_VALUE,
	/* 0x42 */ DEF_AESKEY5 | RF_AESKEY5_VALUE,
	/* 0x43 */ DEF_AESKEY6 | RF_AESKEY6_VALUE,
	/* 0x44 */ DEF_AESKEY7 | RF_AESKEY7_VALUE,
	/* 0x45 */ DEF_AESKEY8 | RF_AESKEY8_VALUE,
	/* 0x46 */ DEF_AESKEY9 | RF_AESKEY9_VALUE,
	/* 0x47 */ DEF_AESKEY10 | RF_AESKEY10_VALUE,
	/* 0x48 */ DEF_AESKEY11 | RF_AESKEY11_VALUE,
	/* 0x49 */ DEF_AESKEY12 | RF_AESKEY12_VALUE,
	/* 0x4a */ DEF_AESKEY13 | RF_AESKEY13_VALUE,
	/* 0x4b */ DEF_AESKEY14 | RF_AESKEY14_VALUE,
	/* 0x4c */ DEF_AESKEY15 | RF_AESKEY15_VALUE,
	/* 0x4d */ DEF_AESKEY16 | RF_AESKEY16_VALUE,
	/* 0x4e */ DEF_TEMP1 | RF_TEMP1_ADCLOWPOWER_ON,
	/* 0x4f */ DEF_TEMP2
};

uint8_t PreMode = 0xff;  // Previous chip operating mode
rf_state_e RFState = RF_STATE_STOP;

/************** ...global vars **************************************************/

/*******************************************************************
** WriteRegister : Writes the register value at the given address **
**				  on the SX1231								 **
********************************************************************
** In  : address, value										   **
** Out : -														**
*******************************************************************/
void
RadioWriteRegister(uint8_t address, uint8_t value)
{
	address |= 0x80;

	NSS = 0;
	SpiInOut(address);
	SpiInOut(value);
	NSS = 1;
}


/*******************************************************************
** ReadRegister : Reads the register value at the given address on**
**				the SX1231									  **
********************************************************************
** In  : address												  **
** Out : value													**
*******************************************************************/
uint8_t
RadioReadRegister(uint8_t address)
{
	uint8_t value;

	address &= 0x7F;

	NSS = 0;
	SpiInOut(address);
	value = SpiInOut(0);
	NSS = 1;

	return value;
}


/*******************************************************************
** SetRFMode : Sets the SX1231 operating mode					 **
********************************************************************
** In  : mode													 **
** Out : -														**
*******************************************************************/
void
SetRFMode(uint8_t mode)
{
	if (mode == PreMode)
		return;

	switch (mode) {
	case RF_MODE_TRANSMITTER:
		RadioWriteRegister(REG_OPMODE, (RegistersCfg[REG_OPMODE] & 0xE3) | RF_OPMODE_TRANSMITTER);
		break;
	case RF_MODE_RECEIVER:
		RadioWriteRegister(REG_OPMODE, (RegistersCfg[REG_OPMODE] & 0xE3) | RF_OPMODE_RECEIVER);
		break;
	case RF_MODE_SYNTHESIZER:
		RadioWriteRegister(REG_OPMODE, (RegistersCfg[REG_OPMODE] & 0xE3) | RF_OPMODE_SYNTHESIZER);
		break;
	case RF_MODE_STANDBY:
		RadioWriteRegister(REG_OPMODE, (RegistersCfg[REG_OPMODE] & 0xE3) | RF_OPMODE_STANDBY);
		break;
	case RF_MODE_SLEEP:
		RadioWriteRegister(REG_OPMODE, (RegistersCfg[REG_OPMODE] & 0xE3) | RF_OPMODE_SLEEP);
		break;
	default:
		/* handle bogus mode? */
		return;
	} // ..switch (mode)

#if 0
	/* we are using packet mode: waiting for mode ready is only necessary
	 * when going from to sleep because the FIFO may not be immediately available from previous mode */
	while ((RadioReadRegister(REG_IRQFLAGS1) & RF_IRQFLAGS1_MODEREADY) == 0x00)
		; // Wait for ModeReady
#endif

	PreMode = mode;

	if (mode == RF_MODE_TRANSMITTER)
		RF_TRANSMIT_LED = LED_ON;
	else
		RF_TRANSMIT_LED = LED_OFF;

	if (mode == RF_MODE_RECEIVER)
		RF_RECEIVING_LED = LED_ON;
	else
		RF_RECEIVING_LED = LED_OFF;

}

#ifdef READBACK_VERIFY
uint8_t regs[0x50];
#endif

void
InitRFChip(void)
{
	int i;

	/* possibly writing to registers too early, checkit & write again as necessary */
	do
	{
		RadioWriteRegister(REG_SYNCVALUE1, 0xAA);
	} while (RadioReadRegister(REG_SYNCVALUE1) != 0xAA);

	do
	{
		RadioWriteRegister(REG_SYNCVALUE1, 0x55);
	} while (RadioReadRegister(REG_SYNCVALUE1) != 0x55);


	/////// RC CALIBRATION (Once at POR) ///////
	SetRFMode(RF_MODE_STANDBY);
	while ((RadioReadRegister(REG_IRQFLAGS1) & RF_IRQFLAGS1_MODEREADY) == 0x00)
		; // Wait for ModeReady

	RadioWriteRegister(0x57, 0x80);
	RadioWriteRegister(REG_OSC1, RadioReadRegister(REG_OSC1) | RF_OSC1_RCCAL_START);
	while ((RadioReadRegister(REG_OSC1) & RF_OSC1_RCCAL_DONE) == 0x00)
		;
	RadioWriteRegister(REG_OSC1, RadioReadRegister(REG_OSC1) | RF_OSC1_RCCAL_START);
	while ((RadioReadRegister(REG_OSC1) & RF_OSC1_RCCAL_DONE) == 0x00)
		;
	RadioWriteRegister(0x57, 0x00);

	NSS = 0;
	SpiInOut(REG_OPMODE | 0x80);	// send address + r/w bit
	for (i = 1; i <= REG_TEMP2; i++)
	{
		SpiInOut(RegistersCfg[i]);
	}
	NSS = 1;

#ifdef READBACK_VERIFY
	for (i = 1; i <= REG_TEMP2; i++)
	{
		do
		{
			NSS = 0;
			SpiInOut(i & 0x7F);	// send address + r/w bit
			regs[i] = SpiInOut(0);
			NSS = 1;

			switch (i)
			{
				case REG_OSC1:
					regs[i] &= ~RF_OSC1_RCCAL_DONE;	// read-only bit
					break;
				case REG_VERSION:
					regs[i] = DEF_VERSION;	// ignore hw-version
					break;
				case REG_LNA:
					regs[i] &= ~RF_LNA_CURRENTGAIN;	// read-only bits
					break;
				case REG_AFCFEI:
					regs[i] &= ~RF_AFCFEI_AFC_DONE;	// read-only bit
					break;
				case REG_RSSICONFIG:
					regs[i] &= ~RF_RSSI_DONE;	// read-only bit
					break;
				case REG_RSSIVALUE:
					regs[i] = DEF_RSSIVALUE;		// ignore rssi r/o
					break;
				case REG_IRQFLAGS1:
					regs[i] &= ~RF_IRQFLAGS1_MODEREADY;	// read-only bit
					break;
				case REG_IRQFLAGS2:
					regs[i] |= RF_IRQFLAGS2_FIFOOVERRUN;	// write-1->reset bit
					break;
				case REG_OSC2:
					regs[i] = DEF_OSC2;
					break;
				}
			} while (regs[i] != READ_ROM_BYTE(RegistersCfg[i]));
	}
#endif /* ..READBACK_VERIFY */

	SetRFMode(RF_MODE_STANDBY);
}

volatile uint8_t msb;
volatile uint8_t mid;
volatile uint8_t lsb;

void
Fhss_Hop(uint8_t *hop_count)
{
	freq_t freq;


#if defined(__AVR__)
	/* AVR needs casting for copying from program memory */
	memcpy_P(&freq, (freq_t *)pgm_read_word(&freqs_P[*hop_count]), sizeof(freq_t));
#else
	memcpy_from_rom(&freq, freqs_P[*hop_count], sizeof(freq_t));
#endif

#if 0
	// debug: forcing to single freq
	freq.msb = RF_FRFMSB_915;
	freq.mid = RF_FRFMID_915;
	freq.lsb = RF_FRFLSB_915;
#endif

	NSS = 0;
	/* all three Frf registers are adjacent: Msb @7, Mid @8, Lsb @9 */
	SpiInOut(REG_FRFMSB | 0x80);	// send start address with write bit
	SpiInOut(freq.msb);	// send Frf MSB
	msb = freq.msb;
	SpiInOut(freq.mid);	// send Frf MID
	mid = freq.mid;
	SpiInOut(freq.lsb);	// send Frf LSB
	lsb = freq.lsb;
	NSS = 1;

	current_radio_channel = *hop_count;	// to indicate where the radio actually is
	UPDATE_PWMDAC_COMPARE(current_radio_channel);	// update DAC output

	if (++(*hop_count) == NUM_CHANNELS)
		(*hop_count) = 0;

}

void
RadioSetNodeAddress(uint8_t address)
{	/* receiver filtering address */
	// address filtering on node address
	RadioWriteRegister(REG_PACKETCONFIG1, (RegistersCfg[REG_PACKETCONFIG1] & 0xf8) | RF_PACKET1_ADRSFILTERING_NODE);
	RadioWriteRegister(REG_NODEADRS, address);
}

static void
start_rf_rx(uint16_t rx_timeout)
{
	// set DIO0 to "PAYLOADREADY" in receive mode
	RadioWriteRegister(REG_DIOMAPPING1, (RegistersCfg[REG_DIOMAPPING1] & 0x3F) | RF_DIOMAPPING1_DIO0_01);
	// in reg 0x2e: fifo fill auto was set on initialization

	SetRFMode(RF_MODE_RECEIVER);
#ifdef LNA_TEST
	flags.read_lna = TRUE;
#endif /* LNA_TEST */

	if (rx_timeout != 0)
	{
		EnableClock_HiRes(rx_timeout);
		RFState = RF_STATE_RX_BUSY;
	}
	else
	{
		EnableClock_HiRes(HIRES_TIMER_MAX_VALUE);	// prevent premature tripping
		/*	state not assigned to RX_BUSY here because the timeout needs to be started
			for real when ReceiveRfFrame() is initially called.
			assuming this function was called elsewhere besides ReceiveRfFrame()
			in order to quickly start reception
		*/
		RFState = RF_STATE_STOP;
	}
}

uint8_t
SendRfFrame(const uint8_t *buffer, uint8_t size, uint8_t Node_adrs, char immediate_rx)
{
	uint8_t ByteCounter;

	/* turn off receiver to prevent reception while filling fifo */
	SetRFMode(RF_MODE_STANDBY);
	while ((RadioReadRegister(REG_IRQFLAGS1) & RF_IRQFLAGS1_MODEREADY) == 0x00)
		; // Wait for ModeReady

	RadioWriteRegister(REG_DIOMAPPING1, (RegistersCfg[REG_DIOMAPPING1] & 0x3F) | RF_DIOMAPPING1_DIO0_00); // DIO0 is "Packet Sent"
	RadioWriteRegister(REG_FIFOTHRESH, (RegistersCfg[REG_FIFOTHRESH] & 0x7F) | RF_FIFOTHRESH_TXSTART_FIFONOTEMPTY);

	RFState = RF_STATE_TX_BUSY;

	/* SX1231 FIFO access (write) */
	NSS = 0;
	SpiInOut(REG_FIFO | 0x80);
	SpiInOut(size + 1);
	SpiInOut(Node_adrs);
	for (ByteCounter = 0; ByteCounter < size; ByteCounter++)
	{
		SpiInOut(buffer[ByteCounter]);
	}
	NSS = 1;

	/* no need to wait for transmit mode to be ready since its handled by the radio */
	SetRFMode(RF_MODE_TRANSMITTER);

	/* blocking here until finished transmitting */
	while (IN_RF_DIO0 == 0)
		;

	if (immediate_rx)
	{
		start_rf_rx(0);
	}
	else
	{
		RFState = RF_STATE_STOP;
		//RF_TRANSMIT_LED	= LED_OFF;
		SetRFMode(RF_MODE_STANDBY);
	}

	return RADIO_OK;
} // ...SendRfFrame()


uint8_t
ReceiveRfFrame(uint8_t *buffer, uint8_t *size, uint16_t rx_timeout)
{
	uint8_t RFFrameSize;
	uint8_t Node_Adrs;
	uint8_t i;

	switch (RFState)
	{
		case RF_STATE_STOP:
			start_rf_rx(rx_timeout);
			return RADIO_RX_RUNNING;
		case RF_STATE_RX_BUSY:
			if (IN_RF_DIO0)
			{
				RFState = RF_STATE_RX_DONE;
			}
			else if (HIRES_COMPARE_B_FLAG)
			{
				RFState = RF_STATE_TIMEOUT;
#ifdef LNA_TEST
			}
			else if (flags.read_lna && IN_RF_DIO4)
			{
				flags.read_lna = FALSE;
				for (i = 0; i < 2300; i++)
				{
					asm("nop");	// about 2mS delay at 20MHz
					asm("nop");
					asm("nop");
					asm("nop");
				}
				i = RadioReadRegister(REG_LNA);
				ltoa((i & 0x38) >> 3, ucStr1, 0);
				UART_send_str(ucStr1, TRUE);
#endif /* LNA_TEST */
			}
			return RADIO_RX_RUNNING;
		case RF_STATE_RX_DONE:
			SetRFMode(RF_MODE_STANDBY);

			/* SX1231 FIFO access (read) */
			NSS = 0;
			SpiInOut(REG_FIFO & 0x7F);
			RFFrameSize = SpiInOut(0);
			Node_Adrs = SpiInOut(0);
			i = Node_Adrs;	// suppress compiler warning
			RFFrameSize--;
			for (i = 0; i < RFFrameSize; i++)
				buffer[i] = SpiInOut(0);
			NSS = 1;

			*size = RFFrameSize;
			RFState = RF_STATE_STOP;
			return RADIO_OK;
		case RF_STATE_ERROR:
			SetRFMode(RF_MODE_STANDBY);

			RFState = RF_STATE_STOP;
			return RADIO_ERROR;
		case RF_STATE_TIMEOUT:
			SetRFMode(RF_MODE_STANDBY);

			RFState = RF_STATE_STOP;
			return RADIO_RX_TIMEOUT;
		default:
			SetRFMode(RF_MODE_STANDBY);
			RFState = RF_STATE_STOP;
			return RADIO_ERROR;
	}
}

#endif /* SX1231 */
