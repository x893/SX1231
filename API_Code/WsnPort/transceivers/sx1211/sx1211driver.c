#if defined SX1211
/**
 * \file sx1211driver.c
 * SX1211 transceiver driver.
 * using transceiver in packet mode.
 *
 */


/*******************************************************************
** Include files                                                 **
*******************************************************************/
#include <stdlib.h>
#include <string.h>
#include "transceiver.h"
#include "platform.h"
#include "FHSSapi.h"	// get NUM_CHANNELS
#include "SX1211driver.h"

/*******************************************************************
** private types                                                  **
*******************************************************************/

typedef struct {
	uint8_t	R;
	uint8_t	P;
	uint8_t	S;
	uint8_t	MCParam_Band;
} freq_t;

/*******************************************************************
** Constants                                                      **
*******************************************************************/

#if defined SX1211_CRYSTAL_12_8MHZ
const rom freq_t ch_0  = { 119,  99, 27, 0 };	//903.24
const rom freq_t ch_1  = { 119,  99, 31, 0 };	//903.72
const rom freq_t ch_2  = { 119,  99, 35, 0 };	//904.20
const rom freq_t ch_3  = { 119,  99, 39, 0 };	//904.68
const rom freq_t ch_4  = { 119,  99, 43, 0 };	//905.16
const rom freq_t ch_5  = { 119,  99, 47, 0 };	//905.64
const rom freq_t ch_6  = { 119,  99, 51, 0 };	//906.12
const rom freq_t ch_7  = { 119,  99, 55, 0 };	//906.60
const rom freq_t ch_8  = { 119,  99, 59, 0 };	//907.08
const rom freq_t ch_9  = { 119,  99, 63, 0 };	//907.56
const rom freq_t ch_10 = { 119,  99, 67, 0 };	//908.04
const rom freq_t ch_11 = { 119,  99, 71, 0 };	//908.52
const rom freq_t ch_12 = { 119, 100,  0, 0 };	//909.00
const rom freq_t ch_13 = { 119, 100,  4, 0 };	//909.48
const rom freq_t ch_14 = { 119, 100,  8, 0 };	//909.96
const rom freq_t ch_15 = { 119, 100, 12, 0 };	//910.44
const rom freq_t ch_16 = { 119, 100, 16, 0 };	//910.92
const rom freq_t ch_17 = { 119, 100, 20, 0 };	//911.40
const rom freq_t ch_18 = { 119, 100, 24, 0 };	//911.88
const rom freq_t ch_19 = { 119, 100, 28, 0 };	//912.36
const rom freq_t ch_20 = { 119, 100, 32, 0 };	//912.84
const rom freq_t ch_21 = { 119, 100, 36, 0 };	//913.32
const rom freq_t ch_22 = { 119, 100, 40, 0 };	//913.80
const rom freq_t ch_23 = { 119, 100, 44, 0 };	//914.28
const rom freq_t ch_24 = { 119, 100, 48, 0 };	//914.76
const rom freq_t ch_25 = { 119, 100, 52, 1 };	//915.24
const rom freq_t ch_26 = { 119, 100, 56, 1 };	//915.72
const rom freq_t ch_27 = { 119, 100, 60, 1 };	//916.20
const rom freq_t ch_28 = { 119, 100, 64, 1 };	//916.68
const rom freq_t ch_29 = { 119, 100, 68, 1 };	//917.16
const rom freq_t ch_30 = { 119, 100, 72, 1 };	//917.64
const rom freq_t ch_31 = { 119, 101,  1, 1 };	//918.12
const rom freq_t ch_32 = { 119, 101,  5, 1 };	//918.60
const rom freq_t ch_33 = { 119, 101,  9, 1 };	//919.08
const rom freq_t ch_34 = { 119, 101, 13, 1 };	//919.56
const rom freq_t ch_35 = { 119, 101, 17, 1 };	//920.04
const rom freq_t ch_36 = { 119, 101, 21, 1 };	//920.52
const rom freq_t ch_37 = { 119, 101, 25, 1 };	//921.00
const rom freq_t ch_38 = { 119, 101, 29, 1 };	//921.48
const rom freq_t ch_39 = { 119, 101, 33, 1 };	//921.96
const rom freq_t ch_40 = { 119, 101, 37, 1 };	//922.44
const rom freq_t ch_41 = { 119, 101, 41, 1 };	//922.92
const rom freq_t ch_42 = { 119, 101, 45, 1 };	//923.40
const rom freq_t ch_43 = { 119, 101, 49, 1 };	//923.88
const rom freq_t ch_44 = { 119, 101, 53, 1 };	//924.36
const rom freq_t ch_45 = { 119, 101, 57, 1 };	//924.84
const rom freq_t ch_46 = { 119, 101, 61, 1 };	//925.32
const rom freq_t ch_47 = { 119, 101, 65, 1 };	//925.80
const rom freq_t ch_48 = { 119, 101, 69, 1 };	//926.28
const rom freq_t ch_49 = { 119, 101, 73, 1 };	//926.76
#endif /* SX1211_CRYSTAL_12_8MHZ */

#if defined SX1211_CRYSTAL_14_7456MHZ
const rom freq_t ch_0  = { 126,  91, 15, 0 };	//00:903.240567
const rom freq_t ch_1  = { 156, 113,  3, 0 };	//01:903.719786
const rom freq_t ch_2  = { 149, 108,  1, 0 };	//02:904.200192
const rom freq_t ch_3  = { 182, 132,  5, 0 };	//03:904.678820
const rom freq_t ch_4  = { 123,  89, 16, 0 };	//04:905.159845
const rom freq_t ch_5  = { 122,  88, 40, 0 };	//05:905.640585
const rom freq_t ch_6  = { 142, 103, 11, 0 };	//06:906.119698
const rom freq_t ch_7  = { 151, 109, 57, 0 };	//07:906.599747
const rom freq_t ch_8  = { 146, 106, 13, 0 };	//08:907.080098
const rom freq_t ch_9  = { 195, 141, 73, 0 };	//09:907.559706
const rom freq_t ch_10 = { 125,  90, 72, 0 };	//10:908.039314
const rom freq_t ch_11 = { 175, 127, 39, 0 };	//11:908.519564
const rom freq_t ch_12 = { 151, 110,  4, 0 };	//12:909.000758
const rom freq_t ch_13 = { 119,  86, 54, 0 };	//13:909.480960
const rom freq_t ch_14 = { 129,  94,  6, 0 };	//14:909.959483
const rom freq_t ch_15 = { 127,  92, 50, 0 };	//15:910.440000
const rom freq_t ch_16 = { 135,  98, 43, 0 };	//16:910.920282
const rom freq_t ch_17 = { 117,  85, 33, 0 };	//17:911.399919
const rom freq_t ch_18 = { 131,  95, 56, 0 };	//18:911.881309
const rom freq_t ch_19 = { 119,  87,  0, 0 };	//19:912.384000
const rom freq_t ch_20 = { 181, 132, 40, 0 };	//20:912.839736
const rom freq_t ch_21 = { 194, 142, 11, 0 };	//21:913.319778
const rom freq_t ch_22 = {  81,  59, 17, 0 };	//22:913.800117
const rom freq_t ch_23 = { 104,  76, 12, 0 };	//23:914.279863
const rom freq_t ch_24 = { 194, 142, 28, 0 };	//24:914.765982
const rom freq_t ch_25 = { 150, 110,  6, 1 };	//25:915.240350
const rom freq_t ch_26 = { 183, 134, 32, 1 };	//26:915.719791
const rom freq_t ch_27 = {  99,  72, 48, 1 };	//27:916.199424
const rom freq_t ch_28 = { 138, 101, 31, 1 };	//28:916.680380
const rom freq_t ch_29 = { 131,  96, 23, 1 };	//29:917.159564
const rom freq_t ch_30 = { 100,  73, 37, 1 };	//30:917.639857
const rom freq_t ch_31 = { 106,  77, 72, 1 };	//31:918.120314
const rom freq_t ch_32 = { 119,  87, 45, 1 };	//32:918.604800
const rom freq_t ch_33 = { 108,  79, 39, 1 };	//33:919.080396
const rom freq_t ch_34 = { 177, 130, 42, 1 };	//34:919.560054
const rom freq_t ch_35 = { 116,  85, 39, 1 };	//35:920.040369
const rom freq_t ch_36 = { 156, 115, 12, 1 };	//36:920.519908
const rom freq_t ch_37 = { 128,  94, 37, 1 };	//37:920.999888
const rom freq_t ch_38 = { 123,  90, 63, 1 };	//38:921.481084
const rom freq_t ch_39 = { 122,  90, 11, 1 };	//39:921.959649
const rom freq_t ch_40 = { 192, 142,  7, 1 };	//40:922.440423
const rom freq_t ch_41 = { 147, 108, 59, 1 };	//41:922.920130
const rom freq_t ch_42 = { 127,  94,  0, 1 };	//42:923.400000
const rom freq_t ch_43 = { 113,  83, 49, 1 };	//43:923.879747
const rom freq_t ch_44 = { 186, 137, 70, 1 };	//44:924.359872
const rom freq_t ch_45 = { 192, 142, 35, 1 };	//45:924.847088
const rom freq_t ch_46 = { 108,  80,  5, 1 };	//46:925.320220
const rom freq_t ch_47 = { 182, 135, 13, 1 };	//47:925.800079
const rom freq_t ch_48 = { 153, 113, 49, 1 };	//48:926.279813
const rom freq_t ch_49 = { 119,  88, 29, 1 };	//49:926.760960
#endif /* SX1211_CRYSTAL_14_7456MHZ */

/* freq_t: { R, P, S, MCParam_Band } */
/******* generate your random number sequence at http://random.org *******/
static const rom freq_t * freqs_P[NUM_CHANNELS] = {
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

// TX_done interrupt comes just before last bit is transmitted
const rom uint16_t TS_TX_LASTBIT = HIRES_TIMEOUT(20);

const rom uint16_t TS_OSC = HIRES_TIMEOUT(3000);

/*******************************************************************
** Global variables                                               **
*******************************************************************/

static volatile uint8_t RFState = RF_STOP;     // RF state machine
uint8_t PreMode = RF_MODE_STANDBY;  // Previous chip operating mode


rom const uint8_t RegistersCfg[] = { // SX1211 configuration registers values
		/*  0 */ DEF_MCPARAM1 | RF_MC1_STANDBY | RF_MC1_BAND_915L | RF_MC1_VCO_TRIM_00 | RF_MC1_RPS_SELECT_1,
		/*  1 */ DEF_MCPARAM2 | RF_MC2_MODULATION_FSK | RF_MC2_DATA_MODE_PACKET | RF_MC2_OOK_THRESH_TYPE_PEAK | RF_MC2_GAIN_IF_00,
		/*  2 */ DEF_FDEV | RF_FDEV_50,
		/*  3 */ DEF_BITRATE | RF_BITRATE_25000,
		/*  4 */ DEF_OOKFLOORTHRESH | RF_OOKFLOORTHRESH_VALUE,
		/*  5 */ DEF_MCPARAM6 | RF_MC6_FIFO_SIZE_64 | 1/*RF_MC6_FIFO_THRESH_VALUE*/,
		/*  6 */ 0, // R1
		/*  7 */ 0, // P1
		/*  8 */ 0, // S1
		/*  9 */ 0, // R2
		/* 10 */ 0, // P2
		/* 11 */ 0, // S2
		/* 12 */ DEF_PARAMP | RF_PARAMP_11,
		/* 13 */ DEF_IRQPARAM1 | RF_IRQ0_RX_STDBY_FIFOEMPTY | RF_IRQ1_RX_STDBY_CRCOK | RF_IRQ1_TX_TXDONE,
		/* 14 */ DEF_IRQPARAM2 | RF_IRQ0_TX_FIFOEMPTY_START_FIFONOTEMPTY/*RF_IRQ0_TX_FIFOTHRESH_START_FIFOTHRESH*/ | RF_IRQ2_PLL_LOCK_PIN_ON,
		/* 15 */ DEF_RSSIIRQTHRESH | RF_RSSIIRQTHRESH_VALUE,
		/* 16 */ DEF_RXPARAM1 | RF_RX1_PASSIVEFILT_378 | RF_RX1_FC_FOPLUS100,
		/* 17 */ DEF_RXPARAM2 | RF_RX2_FO_100,
		/* 18 */ DEF_RXPARAM3 | RF_RX3_POLYPFILT_OFF | RF_RX3_SYNC_SIZE_32 | RF_RX3_SYNC_TOL_0,
		/* 19 */ DEF_RES19,
		/* 20 */ 0, //RSSI Value (Read only)
		/* 21 */ DEF_RXPARAM6 | RF_RX6_OOK_THRESH_DECSTEP_000 | RF_RX6_OOK_THRESH_DECPERIOD_000 | RF_RX6_OOK_THRESH_AVERAGING_00,
		/* 22 */ DEF_SYNCBYTE1 | 0x69, // 1st byte of Sync word,
		/* 23 */ DEF_SYNCBYTE2 | 0x81, // 2nd byte of Sync word,
		/* 24 */ DEF_SYNCBYTE3 | 0x7E, // 3rd byte of Sync word,
		/* 25 */ DEF_SYNCBYTE4 | 0x96, // 4th byte of Sync word,
		/* 26 */ DEF_TXPARAM | RF_TX_FC_200 | /* rf test: RF_TX_POWER_MINIMUM	*/ RF_TX_POWER_PLUS10,
		/* 27 */ DEF_OSCPARAM | RF_OSC_CLKOUT_ENABLE | RF_CLKOUT_DIV,
		/* 28 */ DEF_PKTPARAM1 | RF_PKT1_MANCHESTER_OFF | 64,
		/* 29 */ DEF_NODEADRS  | RF_NODEADRS_VALUE,
#ifdef SLAVE_ANSWER_ALL
		/* 30 */ DEF_PKTPARAM3 | RF_PKT3_FORMAT_VARIABLE | RF_PKT3_PREAMBLE_SIZE_32 | RF_PKT3_WHITENING_OFF | RF_PKT3_CRC_ON | RF_PKT3_ADRSFILT_NONE,
#else
		/* 30 */ DEF_PKTPARAM3 | RF_PKT3_FORMAT_VARIABLE | RF_PKT3_PREAMBLE_SIZE_32 | RF_PKT3_WHITENING_OFF | RF_PKT3_CRC_ON | RF_PKT3_ADRSFILT_ME_ONLY,//RF_PKT3_ADRSFILT_ME_AND_00,
#endif
		/* 31 */ DEF_PKTPARAM4 | RF_PKT4_AUTOCLEAR_ON | RF_PKT4_FIFO_STANDBY_READ /*RF_PKT4_FIFO_STANDBY_WRITE */

};


#ifdef READBACK_VERIFY
uint8_t regs[32];
uint8_t readback_idx;

static void
do_readback(char skip_rps)
{
	char fail;

	readback_idx = 31;
	do {
		regs[readback_idx] = ReadRegister(readback_idx);
	} while (regs[readback_idx] != READ_ROM_BYTE(RegistersCfg[readback_idx]));

	for (readback_idx = 0; readback_idx < 31; readback_idx++) {

		do {
			regs[readback_idx] = ReadRegister(readback_idx);

			switch (readback_idx) {
				case REG_IRQPARAM1:	// 13
					/* 0xf9 mask because of read-only Fifofull and /Fifoempty */
					regs[readback_idx] &= 0xf9;
					break;
				case REG_IRQPARAM2: // 14
					/* 0xfd mask because of bit1 is read-only PLL_LOCK */
					regs[readback_idx] &= 0xfd;
					break;
				case REG_PKTPARAM3:	// 30
					/* bit0 of register 30 is read-only CRC_status */
					regs[readback_idx] &= 0xfe;
					break;
				default:
					break;
			} /* switch (readback_idx) */

			if (skip_rps && ((readback_idx > 5 && readback_idx < 12) || readback_idx == REG_NODEADRS) ) {
				// skip checking the RPS values
				fail = FALSE;
			} else {
				if (regs[readback_idx] != READ_ROM_BYTE(RegistersCfg[readback_idx]))
					fail = TRUE;
				else
					fail = FALSE;
			}

		} while (fail);

	} // ..for()

}
#endif	/* ..READBACK_VERIFY */

void
InitRFChip()
{
	uint16_t i;

	/* possibly writing to first register too early, checkit & write again as necessary */
	do {
		WriteRegister(0, READ_ROM_BYTE(RegistersCfg[0]));
		i = ReadRegister(0);
	} while (i != READ_ROM_BYTE(RegistersCfg[0]));

    for (i = 0; i < 32; i++) {
		WriteRegister(i, READ_ROM_BYTE(RegistersCfg[i]));
    }

#ifdef READBACK_VERIFY
	do_readback(FALSE);
#endif	/* ..READBACK_VERIFY */


    SetRFMode(RF_MODE_STANDBY);	/* sleep or no? some playforms may use clkout */

}


/*******************************************************************
** SetRFMode : Sets the SX1211 operating mode                     **
********************************************************************
** In  : mode                                                     **
** Out : -                                                        **
*******************************************************************/
void
SetRFMode(uint8_t mode)
{
	char was_sleeping;

	if (mode == PreMode)
		return;	/* no action necessary */

	/* Note that we are using the transceiver in packet mode, meaning that the transceiver
	 * handles any required delays by itself.
	 * However, if continuous or buffered modes are used, the required delays must be provided by software.
	 */

	if (PreMode == RF_MODE_SLEEP)
		was_sleeping = TRUE;
	else
		was_sleeping = FALSE;


	switch (mode) {
		case RF_MODE_TRANSMITTER:
			WriteRegister(REG_MCPARAM1, (READ_ROM_BYTE(RegistersCfg[REG_MCPARAM1]) & 0x1F) | REG0_RF_TRANSMITTER);
			PreMode = RF_MODE_TRANSMITTER;
			break;
		case RF_MODE_RECEIVER:
			WriteRegister(REG_MCPARAM1, (READ_ROM_BYTE(RegistersCfg[REG_MCPARAM1]) & 0x1F) | REG0_RF_RECEIVER);
			PreMode = RF_MODE_RECEIVER;
			break;
		case RF_MODE_SYNTHESIZER:
			WriteRegister(REG_MCPARAM1, (READ_ROM_BYTE(RegistersCfg[REG_MCPARAM1]) & 0x1F) | REG0_RF_SYNTHESIZER);
			PreMode = RF_MODE_SYNTHESIZER;
			break;
		case RF_MODE_STANDBY:
			WriteRegister(REG_MCPARAM1, (READ_ROM_BYTE(RegistersCfg[REG_MCPARAM1]) & 0x1F) | REG0_RF_STANDBY);
			PreMode = RF_MODE_STANDBY;
			break;
		case RF_MODE_SLEEP:
			WriteRegister(REG_MCPARAM1, (READ_ROM_BYTE(RegistersCfg[REG_MCPARAM1]) & 0x1F) | REG0_RF_SLEEP);
			PreMode = RF_MODE_SLEEP;
			break;
		default:
			/* fail: indicate failure here in some fashion */
			break;
	} /* switch (mode) */

	if (was_sleeping)
	{
		/* some parts of the radio need time to wake up */
		Wait(READ_ROM_WORD(TS_OSC));
	}

	if (mode == RF_MODE_TRANSMITTER)
		RF_TRANSMIT_LED = LED_ON;
	else
		RF_TRANSMIT_LED = LED_OFF;

	if (mode == RF_MODE_RECEIVER)
		RF_RECEIVING_LED = LED_ON;
	else
		RF_RECEIVING_LED = LED_OFF;
}


void
_WriteRegister(uint8_t address, uint8_t value)
{
	SpiInOut((address << 1) & 0x3E);
	SpiInOut(value);
}

/*******************************************************************
** WriteRegister : Writes the register value at the given address **
**                  on the SX1211                                 **
********************************************************************
** In  : address, value                                           **
** Out : -                                                        **
*******************************************************************/
void
WriteRegister(uint8_t address, uint8_t value)
{
    NSS_CONFIG = 0;
	_WriteRegister(address, value);
    NSS_CONFIG = 1;
}

/*******************************************************************
** ReadRegister : Reads the register value at the given address on**
**                the SX1211                                      **
********************************************************************
** In  : address                                                  **
** Out : value                                                    **
*******************************************************************/
uint16_t ReadRegister(uint8_t address)
{
    uint8_t value = 0;

	address = ((address << 1) & 0x7E) | 0x40;
    NSS_CONFIG = 0;
    SpiInOut(address);
    value = SpiInOut(0);
    NSS_CONFIG = 1;

    return value;
}

/*******************************************************************
** SendByte : Sends a data to the transceiver trough the SPI      **
**            interface                                           **
********************************************************************
** In  : b                                                        **
** Out : -                                                        **
*******************************************************************/
/*******************************************************************
**  Information                                                   **
********************************************************************
**	This function send a data like a regester by a  SPI hardware  **
*******************************************************************/
static void
SendByte(uint8_t data)
{
    NSS_DATA = 0;
    SpiInOut(data);		// Send DATA
    NSS_DATA = 1;
} // void SendByte(uint8_t b)

/*******************************************************************
** Communication functions                                        **
*******************************************************************/

static void
start_rf_rx(uint16_t timeout)
{
	SetRFMode(RF_MODE_RECEIVER);

	if (timeout != 0) {
		EnableClock_HiRes(timeout);
		RFState |= RX_TIMEOUT_STARTED;
	} else {
		EnableClock_HiRes(HIRES_TIMER_MAX_VALUE);	// prevent premature tripping
		RFState &= ~RX_TIMEOUT_STARTED;
	}

	RFState |= RF_BUSY;
	RFState &= ~RF_STOP;
	RFState &= ~RF_TIMEOUT;

	RF_RECEIVING_LED = 1;
}

/*******************************************************************
** SendRfFrame : Sends a RF frame                                 **
********************************************************************
** In  : *buffer, size                                            **
** Out : *pReturnCode                                             **
*******************************************************************/
uint8_t
SendRfFrame(const uint8_t *buffer, uint8_t size, uint8_t Node_adrs, char immediate_rx)
{
#ifdef READBACK_VERIFY
	uint16_t count = 0;
#endif

	uint8_t ByteCounter;

	if (size > ((READ_ROM_BYTE(RegistersCfg[REG_MCPARAM6])>>6)+1)*16) {  // If size > FIFO size
        RFState |= RF_STOP;
        return RADIO_ERROR;
    }


	SetRFMode(RF_MODE_STANDBY);
	/*	no need to go to write direction because we goto tx mode before filling fifo.
	 *  actual transmission doesnt start until first byte is put into fifo. */
	//WriteRegister(REG_PKTPARAM4, (READ_ROM_BYTE(RegistersCfg[REG_PKTPARAM4]) & 0xBF) | RF_PKT4_FIFO_STANDBY_WRITE);
	if (IN_IRQ0 == 1) {
		WriteRegister(REG_IRQPARAM1, READ_ROM_BYTE(RegistersCfg[REG_IRQPARAM1]) | RF_IRQ1_FIFO_OVERRUN_CLEAR);
	}

	/* clear PLL_LOCK flag so we can see it restore on the new frequency */
	WriteRegister(REG_IRQPARAM2, READ_ROM_BYTE(RegistersCfg[REG_IRQPARAM2]) | 0x02);

	// going to transmit now because Tx_start_irq_0=1 for nFifoempty
	// first byte written to fifo causes tx to start
    SetRFMode(RF_MODE_TRANSMITTER);
    RFState |= RF_BUSY;
    RFState &= ~RF_STOP;

	SendByte(size + 1);
	SendByte(Node_adrs);
	for (ByteCounter = 0; ByteCounter < size; ) {
		SendByte(buffer[ByteCounter++]);
	}

#if 0
	if (IN_IRQ0 == 0) {
		/* IRQ0 in standby means nFifoempty */
		/* if the fifo is empty, transmit will hang */
/*		for (;;) {
			//asm("nop");
			_asm nop _endasm
		}	*/
		return RADIO_ERROR;
	}
#endif


	while (!IN_IRQ1) {
#ifdef READBACK_VERIFY
		if (++count == 0xffff) {
			for (;;)
				do_readback(TRUE);
		}
#endif
		; // Wait for TX done
	}

	// delay here needed only for slower bitrates
	/* do not use wait function here due to wait-timer being used by caller for other purpose
	Wait(READ_ROM_WORD(TS_TX_LASTBIT));	*/
	for (ByteCounter = 0; ByteCounter < 255; ByteCounter++)
		;

    RFState &= ~RF_TX_DONE;

	if (immediate_rx) {
		start_rf_rx(0);
	} else {
		SetRFMode(RF_MODE_STANDBY);
		RFState |= RF_STOP;
	}

	return RADIO_OK;
} // ..SendRfFrame()


/*******************************************************************
** ReceiveRfFrame : Receives a RF frame                           **
********************************************************************
** In  : -                                                        **
** Out : *buffer, size                                            **
** Return : ReturnCode                                            **
*******************************************************************/
uint8_t
ReceiveRfFrame(uint8_t *buffer, uint8_t *size, uint16_t Timeout)
{
	uint8_t ret, Node_Adrs;
	uint8_t ByteCounter;
	uint8_t RFFrameSize;


    if (RFState & RF_BUSY) {
		/* we are currently waiting to receive something */
		if (HIRES_COMPARE_B_FLAG) {
			/* but we've been waiting too long */
			RFState |= RF_TIMEOUT;
			RFState &= ~RF_BUSY;
		}

		if (IN_IRQ1) {
			/* we have received something */
			RFState |= RF_RX_DONE;
			RFState &= ~RF_BUSY;
		}
	}

	if ((RFState & RX_TIMEOUT_STARTED) == 0) {
		/* reception was previously started without a timeout */
		EnableClock_HiRes(Timeout);
		RFState |= RX_TIMEOUT_STARTED;
	}

    if (RFState & RF_STOP) {
		/* starting RF receiver */
		start_rf_rx(Timeout);

        return RADIO_RX_RUNNING;
    }
    else if (RFState & RF_RX_DONE) {

		/* Fifo_stby direction already set on init, and not touched when transmitting
		WriteRegister(REG_PKTPARAM4, (READ_ROM_BYTE(RegistersCfg[REG_PKTPARAM4]) & 0xBF) | RF_PKT4_FIFO_STANDBY_READ);	*/

        SetRFMode(RF_MODE_STANDBY);
		RF_RECEIVING_LED = 0;

        RFFrameSize = ReceiveByte();
		if (RFFrameSize  > 0) {
			RFFrameSize--;	// length byte not counted as part of the length
			Node_Adrs = ReceiveByte();
#ifdef SLAVE_ANSWER_ALL
			if (Node_Adrs == Slave3_ID)
				hop_on_next_wakeup = TRUE;
#else
			ret = Node_Adrs;	// suppress compiler warning
#endif
			for (ByteCounter = 0; ByteCounter < RFFrameSize; ) {
				buffer[ByteCounter++] = ReceiveByte();
			}
			ret = RADIO_OK;
		} else {
			while (IN_IRQ0) {
				RFFrameSize = ReceiveByte();
			}
			ret = RADIO_ERROR;
		}
		*size = RFFrameSize;
        RFState |= RF_STOP;
        RFState &= ~RF_RX_DONE;
		RFState &= ~RX_TIMEOUT_STARTED;
        return ret;
    }
    else if (RFState & RF_ERROR) {
        SetRFMode(RF_MODE_STANDBY);
		RF_RECEIVING_LED = 0;

        RFState |= RF_STOP;
        RFState &= ~RF_ERROR;
		RFState &= ~RX_TIMEOUT_STARTED;
        return RADIO_ERROR;
    }
    else if (RFState & RF_TIMEOUT) {
        SetRFMode(RF_MODE_STANDBY);
		RF_RECEIVING_LED = 0;

		RFState |= RF_STOP;
        RFState &= ~RF_TIMEOUT;
		RFState &= ~RX_TIMEOUT_STARTED;
        return RADIO_RX_TIMEOUT;
    }

	return RADIO_RX_RUNNING;
} // void ReceiveRfFrame(uint8_t *buffer, uint8_t size, uint8_t *pReturnCode)


/*******************************************************************
** ReceiveByte : Receives a data from the transceiver trough the  **
**               SPI interface                                    **
********************************************************************
** In  : -                                                        **
** Out : b                                                        **
*******************************************************************/
/*******************************************************************
**  Information                                                   **
********************************************************************
**This function received a data like a regester by a SPI hardware **
*******************************************************************/
uint8_t ReceiveByte(void)
{
    uint8_t inputByte;

    NSS_DATA = 0;
	inputByte = SpiInOut(0);	// Receive DATA
    NSS_DATA = 1;

	return inputByte;
}
/*******************************************************************
** Transceiver specific functions                                 **
*******************************************************************/

void
RadioSetNodeAddress(uint8_t node_adrs)
{
	// this is the address we want to hear:
	WriteRegister(REG_NODEADRS, node_adrs);
	// only receive the broadcast messages, which are fhss-sync packets
	// upon initialization: register 30, bits 1-2 Adrs_filt set to Node_adrs accepted only
}

void
Fhss_Hop(uint8_t *hop_count)
{
	freq_t freq;

	NSS_CONFIG = 0;

#if defined(__AVR__)
	/* AVR needs casting for copying from program memory */
	memcpy_P(&freq, (freq_t *)pgm_read_word(&freqs_P[*hop_count]), sizeof(freq_t));
#else
	memcpy_from_rom(&freq, freqs_P[*hop_count], sizeof(freq_t));
#endif

	/* SetRFMode(RF_SYNTHESIZER); */
	_WriteRegister(REG_MCPARAM1, (READ_ROM_BYTE(RegistersCfg[REG_MCPARAM1]) & 0x1F) | REG0_RF_SYNTHESIZER);
	PreMode = RF_MODE_SYNTHESIZER;

	/* clear PLL_LOCK flag so we can see it restore on the new frequency */
	_WriteRegister(REG_IRQPARAM2, READ_ROM_BYTE(RegistersCfg[REG_IRQPARAM2]) | 0x02);

	if (freq.MCParam_Band == 0) {
		_WriteRegister(REG_MCPARAM1, (READ_ROM_BYTE(RegistersCfg[REG_MCPARAM1]) & 0xE7) | RF_MC1_BAND_915L);
	} else if (freq.MCParam_Band == 1) {
		_WriteRegister(REG_MCPARAM1, (READ_ROM_BYTE(RegistersCfg[REG_MCPARAM1]) & 0xE7) | RF_MC1_BAND_915H);
	}

#if 0
	// debug with 12.8MHz crystal:
	freq.R = 119;
	freq.P = 100;
	freq.S = 50;
#endif

	_WriteRegister(REG_R1, freq.R);
	_WriteRegister(REG_P1, freq.P);
	_WriteRegister(REG_S1, freq.S);

	current_radio_channel = *hop_count;	// to indicate where the radio actually is
	UPDATE_PWMDAC_COMPARE(current_radio_channel);	// update DAC output


	if (++(*hop_count) == NUM_CHANNELS)
		(*hop_count) = 0;

	NSS_CONFIG = 1;
}

#endif /* SX1211 */
