#ifdef SX1211
/**
 * \file sx1211driver.h
 * declaration of SX1211 registers and bitfields
 *
 */

#ifndef __SX1211DRIVER__
#define __SX1211DRIVER__



/*******************************************************************
** Global definitions                                             **
*******************************************************************/

/*******************************************************************
** RF State machine                                               **
*******************************************************************/
#define RF_STOP              0x01
#define RF_BUSY              0x02
#define RF_RX_DONE           0x04
#define RF_TX_DONE           0x08
#define RF_ERROR             0x10
#define RF_TIMEOUT           0x20
#define RX_TIMEOUT_STARTED   0x40



/*******************************************************************
** SX1211 definitions                                             **
*******************************************************************/

/*******************************************************************
** SX1211 Operating modes definition -- reg0 bits 7-5             **
*******************************************************************/
#define REG0_RF_SLEEP                         0x00
#define REG0_RF_STANDBY                       0x20
#define REG0_RF_SYNTHESIZER                   0x40
#define REG0_RF_RECEIVER                      0x60
#define REG0_RF_TRANSMITTER                   0x80


/*******************************************************************
** SX1211 registers Address                              **
*******************************************************************/
#define REG_MCPARAM1                     0
#define REG_MCPARAM2                     1
#define REG_FDEV                         2
#define REG_BITRATE                      3
#define REG_OOKFLOORTHRESH               4
#define REG_MCPARAM6                     5
#define REG_R1                           6
#define REG_P1                           7
#define REG_S1                           8
#define REG_R2                           9
#define REG_P2                           10
#define REG_S2                           11
#define REG_PARAMP                       12

#define REG_IRQPARAM1                    13
#define REG_IRQPARAM2                    14
#define REG_RSSIIRQTHRESH                15

#define REG_RXPARAM1                     16
#define REG_RXPARAM2                     17
#define REG_RXPARAM3                     18
#define REG_RES19                        19
#define REG_RSSIVALUE                    20
#define REG_RXPARAM6                     21

#define REG_SYNCBYTE1                    22
#define REG_SYNCBYTE2                    23
#define REG_SYNCBYTE3                    24
#define REG_SYNCBYTE4                    25

#define REG_TXPARAM                      26

#define REG_OSCPARAM                     27

#define REG_PKTPARAM1                    28
#define REG_NODEADRS                     29
#define REG_PKTPARAM3                    30
#define REG_PKTPARAM4                    31


/*******************************************************************
** SX1211 initialisation register values definition               **
*******************************************************************/
#define DEF_MCPARAM1                     0x00
#define DEF_MCPARAM2                     0x00
#define DEF_FDEV                         0x00
#define DEF_BITRATE                      0x00
#define DEF_OOKFLOORTHRESH               0x00
#define DEF_MCPARAM6                     0x00
#define DEF_R1                           0x00
#define DEF_P1                           0x00
#define DEF_S1                           0x00
#define DEF_R2                           0x00
#define DEF_P2                           0x00
#define DEF_S2                           0x00
#define DEF_PARAMP                       0x20

#define DEF_IRQPARAM1                    0x00
#define DEF_IRQPARAM2                    0x08
#define DEF_RSSIIRQTHRESH                0x00

#define DEF_RXPARAM1                     0x00
#define DEF_RXPARAM2                     0x08
#define DEF_RXPARAM3                     0x20
#define DEF_RES19                        0x07
#define DEF_RSSIVALUE                    0x00
#define DEF_RXPARAM6                     0x00

#define DEF_SYNCBYTE1                    0x00
#define DEF_SYNCBYTE2                    0x00
#define DEF_SYNCBYTE3                    0x00
#define DEF_SYNCBYTE4                    0x00

#define DEF_TXPARAM                      0x00

#define DEF_OSCPARAM                     0x00

#define DEF_PKTPARAM1                    0x00
#define DEF_NODEADRS                     0x00
#define DEF_PKTPARAM3                    0x00
#define DEF_PKTPARAM4                    0x00

/*******************************************************************
** SX1211 bit control definition                                  **
*******************************************************************/
// MC Param 1 (reg0)
// Chip operating mode
#define RF_MC1_SLEEP                     0x00
#define RF_MC1_STANDBY                   0x20
#define RF_MC1_SYNTHESIZER               0x40
#define RF_MC1_RECEIVER                  0x60
#define RF_MC1_TRANSMITTER               0x80

// Frequency band (reg0)
#define RF_MC1_BAND_915L                 0x00
#define RF_MC1_BAND_915H                 0x08
#define RF_MC1_BAND_868                  0x10
#define RF_MC1_BAND_950                  0x10

// VCO trimming (reg0)
#define RF_MC1_VCO_TRIM_00               0x00
#define RF_MC1_VCO_TRIM_01               0x02
#define RF_MC1_VCO_TRIM_10               0x04
#define RF_MC1_VCO_TRIM_11               0x06

// RF frequency selection (reg0)
#define RF_MC1_RPS_SELECT_1              0x00
#define RF_MC1_RPS_SELECT_2              0x01

// MC Param 2
// Modulation scheme selection (reg1)
#define RF_MC2_MODULATION_OOK            0x40
#define RF_MC2_MODULATION_FSK            0x80

// Data operation mode (reg1)
#define RF_MC2_DATA_MODE_CONTINUOUS      0x00
#define RF_MC2_DATA_MODE_BUFFERED        0x20
#define RF_MC2_DATA_MODE_PACKET          0x04

// Rx OOK threshold mode selection (reg1)
#define RF_MC2_OOK_THRESH_TYPE_FIXED     0x00
#define RF_MC2_OOK_THRESH_TYPE_PEAK      0x08
#define RF_MC2_OOK_THRESH_TYPE_AVERAGE   0x10

// Gain on IF chain (reg1)
#define RF_MC2_GAIN_IF_00                0x00
#define RF_MC2_GAIN_IF_01                0x01
#define RF_MC2_GAIN_IF_10                0x02
#define RF_MC2_GAIN_IF_11                0x03
 

// Frequency deviation (kHz) (reg2)
#define RF_FDEV_33                       0x0B
#define RF_FDEV_40                       0x09
#define RF_FDEV_50                       0x07
#define RF_FDEV_80                       0x04	/* +/-92.16KHz with 14.7456MHz crystal  */
#define RF_FDEV_100                      0x03/* +/-115.2KHz with 14.7456MHz crystal  */
#define RF_FDEV_133                      0x02
#define RF_FDEV_200                      0x01


// Bitrate (bit/sec)   (reg3)
#define RF_BITRATE_1600                  0x7C
#define RF_BITRATE_2000                  0x63
#define RF_BITRATE_2500                  0x4F
#define RF_BITRATE_5000                  0x27
#define RF_BITRATE_8000                  0x18
#define RF_BITRATE_10000                 0x13
#define RF_BITRATE_20000                 0x09
#define RF_BITRATE_25000                 0x07
#define RF_BITRATE_40000                 0x04
#define RF_BITRATE_50000                 0x03
#define RF_BITRATE_100000                0x01


// OOK threshold (reg4)
#define RF_OOKFLOORTHRESH_VALUE          0x0C 


// MC Param 6
// FIFO size (reg5)
#define RF_MC6_FIFO_SIZE_16              0x00
#define RF_MC6_FIFO_SIZE_32              0x40
#define RF_MC6_FIFO_SIZE_48              0x80
#define RF_MC6_FIFO_SIZE_64              0xC0

// FIFO threshold (reg5)
#define RF_MC6_FIFO_THRESH_VALUE         0x0F


// PA ramp times in OOK
#define RF_PARAMP_00                     0x00
#define RF_PARAMP_01                     0x08
#define RF_PARAMP_10                     0x10
#define RF_PARAMP_11                     0x18


// IRQ Param 1 (reg13)
// Select RX&STDBY IRQ_0 sources (Packet mode) (reg13)
#define RF_IRQ0_RX_STDBY_PAYLOADREADY    0x00
#define RF_IRQ0_RX_STDBY_WRITEBYTE       0x40
#define RF_IRQ0_RX_STDBY_FIFOEMPTY       0x80
#define RF_IRQ0_RX_STDBY_SYNCADRS        0xC0

// Select RX&STDBY IRQ_1 sources (Packet mode) (reg13)
#define RF_IRQ1_RX_STDBY_CRCOK           0x00
#define RF_IRQ1_RX_STDBY_FIFOFULL        0x10
#define RF_IRQ1_RX_STDBY_RSSI            0x20
#define RF_IRQ1_RX_STDBY_FIFOTHRESH      0x30

// Select TX IRQ_1 sources (Packet mode) (reg13)
#define RF_IRQ1_TX_FIFOFULL              0x00
#define RF_IRQ1_TX_TXDONE                0x08

// FIFO overrun/clear  (reg13)
#define RF_IRQ1_FIFO_OVERRUN_CLEAR       0x01


// IRQ Param 2
// Select TX start condition and IRQ_0 source (Packet mode)
#define RF_IRQ0_TX_FIFOTHRESH_START_FIFOTHRESH     0x00
#define RF_IRQ0_TX_FIFOEMPTY_START_FIFONOTEMPTY    0x10

// RSSI IRQ flag
#define RF_IRQ2_RSSI_IRQ_CLEAR           0x04

// PLL_Locked flag
#define RF_IRQ2_PLL_LOCK_CLEAR           0x02

// PLL_Locked pin
#define RF_IRQ2_PLL_LOCK_PIN_OFF         0x00
#define RF_IRQ2_PLL_LOCK_PIN_ON          0x01

// RSSI threshold for interrupt
#define RF_RSSIIRQTHRESH_VALUE           0x00


// RX Param 1
// Passive filter (kHz)
#define RF_RX1_PASSIVEFILT_65            0x00
#define RF_RX1_PASSIVEFILT_82            0x10
#define RF_RX1_PASSIVEFILT_109           0x20
#define RF_RX1_PASSIVEFILT_137           0x30
#define RF_RX1_PASSIVEFILT_157           0x40
#define RF_RX1_PASSIVEFILT_184           0x50
#define RF_RX1_PASSIVEFILT_211           0x60
#define RF_RX1_PASSIVEFILT_234           0x70
#define RF_RX1_PASSIVEFILT_262           0x80
#define RF_RX1_PASSIVEFILT_321           0x90
#define RF_RX1_PASSIVEFILT_378           0xA0
#define RF_RX1_PASSIVEFILT_414           0xB0
#define RF_RX1_PASSIVEFILT_458           0xC0
#define RF_RX1_PASSIVEFILT_514           0xD0
#define RF_RX1_PASSIVEFILT_676           0xE0
#define RF_RX1_PASSIVEFILT_987           0xF0

// Butterworth filter (kHz)
#define RF_RX1_FC_VALUE                  0x03
// !!! Values defined below only apply if RFCLKREF = DEFAULT VALUE = 0x07 !!!
#define RF_RX1_FC_FOPLUS25               0x00
#define RF_RX1_FC_FOPLUS50               0x01
#define RF_RX1_FC_FOPLUS75               0x02
#define RF_RX1_FC_FOPLUS100              0x03
#define RF_RX1_FC_FOPLUS125              0x04
#define RF_RX1_FC_FOPLUS150              0x05
#define RF_RX1_FC_FOPLUS175              0x06
#define RF_RX1_FC_FOPLUS200              0x07
#define RF_RX1_FC_FOPLUS225              0x08
#define RF_RX1_FC_FOPLUS250              0x09



// RX Param 2
// Polyphase filter center value (kHz)
#define RF_RX2_FO_VALUE                  0x03
// !!! Values defined below only apply if RFCLKREF = DEFAULT VALUE = 0x07 !!!
#define RF_RX2_FO_50                     0x10
#define RF_RX2_FO_75                     0x20
#define RF_RX2_FO_100                    0x30
#define RF_RX2_FO_125                    0x40
#define RF_RX2_FO_150                    0x50
#define RF_RX2_FO_175                    0x60
#define RF_RX2_FO_200                    0x70
#define RF_RX2_FO_225                    0x80
#define RF_RX2_FO_250                    0x90
#define RF_RX2_FO_275                    0xA0
#define RF_RX2_FO_300                    0xB0
#define RF_RX2_FO_325                    0xC0
#define RF_RX2_FO_350                    0xD0
#define RF_RX2_FO_375                    0xE0
#define RF_RX2_FO_400                    0xF0


// Rx Param 3
// Polyphase filter enable
#define RF_RX3_POLYPFILT_ON              0x80
#define RF_RX3_POLYPFILT_OFF             0x00

// Bit synchronizer
// Automatically activated in Packet mode

// Sync word recognition
// Automatically activated in Packet mode

// Size of the reference sync word
#define RF_RX3_SYNC_SIZE_8               0x00
#define RF_RX3_SYNC_SIZE_16              0x08
#define RF_RX3_SYNC_SIZE_24              0x10
#define RF_RX3_SYNC_SIZE_32              0x18

// Number of tolerated errors for the sync word recognition
#define RF_RX3_SYNC_TOL_0                0x00
#define RF_RX3_SYNC_TOL_1                0x02
#define RF_RX3_SYNC_TOL_2                0x04
#define RF_RX3_SYNC_TOL_3                0x06


// RSSI Value (Read only)


// Rx Param 6
// Decrement step of RSSI threshold in OOK demodulator (peak mode)
#define RF_RX6_OOK_THRESH_DECSTEP_000    0x00
#define RF_RX6_OOK_THRESH_DECSTEP_001    0x20
#define RF_RX6_OOK_THRESH_DECSTEP_010    0x40
#define RF_RX6_OOK_THRESH_DECSTEP_011    0x60
#define RF_RX6_OOK_THRESH_DECSTEP_100    0x80
#define RF_RX6_OOK_THRESH_DECSTEP_101    0xA0
#define RF_RX6_OOK_THRESH_DECSTEP_110    0xC0
#define RF_RX6_OOK_THRESH_DECSTEP_111    0xE0

// Decrement period of RSSI threshold in OOK demodulator (peak mode)
#define RF_RX6_OOK_THRESH_DECPERIOD_000  0x00
#define RF_RX6_OOK_THRESH_DECPERIOD_001  0x04
#define RF_RX6_OOK_THRESH_DECPERIOD_010  0x08
#define RF_RX6_OOK_THRESH_DECPERIOD_011  0x0C
#define RF_RX6_OOK_THRESH_DECPERIOD_100  0x10
#define RF_RX6_OOK_THRESH_DECPERIOD_101  0x14
#define RF_RX6_OOK_THRESH_DECPERIOD_110  0x18
#define RF_RX6_OOK_THRESH_DECPERIOD_111  0x1C

// Cutoff freq of average function of RSSI threshold in OOK demodulator (average mode)
#define RF_RX6_OOK_THRESH_AVERAGING_00   0x00
#define RF_RX6_OOK_THRESH_AVERAGING_11   0x03


// TX Param 
// Interpolator filter Tx (kHz)
#define RF_TX_FC_VALUE                   0x70
// !!! Values defined below only apply if RFCLKREF = DEFAULT VALUE = 0x07 !!!
#define RF_TX_FC_25                      0x00
#define RF_TX_FC_50                      0x10
#define RF_TX_FC_75                      0x20
#define RF_TX_FC_100                     0x30
#define RF_TX_FC_125                     0x40
#define RF_TX_FC_150                     0x50
#define RF_TX_FC_175                     0x60
#define RF_TX_FC_200                     0x70
#define RF_TX_FC_225                     0x80
#define RF_TX_FC_250                     0x90
#define RF_TX_FC_275                     0xA0
#define RF_TX_FC_300                     0xB0
#define RF_TX_FC_325                     0xC0
#define RF_TX_FC_350                     0xD0
#define RF_TX_FC_375                     0xE0
#define RF_TX_FC_400                     0xF0

// Tx Output power (dBm)
#define RF_TX_POWER_MAX                  0x00
#define RF_TX_POWER_PLUS10               0x02
#define RF_TX_POWER_PLUS7                0x04
#define RF_TX_POWER_PLUS4                0x06
#define RF_TX_POWER_PLUS1                0x08
#define RF_TX_POWER_MINUS2               0x0a
#define RF_TX_POWER_MINIMUM              0x0e


// OSC Param
// CLKOUT enable
#define RF_OSC_CLKOUT_ON                 0x80
#define RF_OSC_CLKOUT_OFF                0x00

// CLKOUT frequency (kHz)
#define CLKOUT_DIVBY_1		(0 << 2)	// ie: 12.8MHz -> 12.8MHz
#define CLKOUT_DIVBY_2		(1 << 2)	// ie: 12.8MHz -> 6.4MHz
#define CLKOUT_DIVBY_4		(2 << 2)	// ie: 12.8MHz -> 3.2MHz
#define CLKOUT_DIVBY_6		(3 << 2)	// ie: 12.8MHz -> 2.13333MHz
#define CLKOUT_DIVBY_8		(4 << 2)	//
#define CLKOUT_DIVBY_10		(5 << 2)	//
#define CLKOUT_DIVBY_12		(6 << 2)	//
#define CLKOUT_DIVBY_14		(7 << 2)	//
#define CLKOUT_DIVBY_16		(8 << 2)	//
#define CLKOUT_DIVBY_18		(9 << 2)	//
#define CLKOUT_DIVBY_20		(10 << 2)	//
#define CLKOUT_DIVBY_22		(11 << 2)	//
#define CLKOUT_DIVBY_24		(12 << 2)	//
#define CLKOUT_DIVBY_26		(13 << 2)	//
#define CLKOUT_DIVBY_28		(14 << 2)	//
#define CLKOUT_DIVBY_30		(15 << 2)	//
#define CLKOUT_DIVBY_32		(16 << 2)	//
#define CLKOUT_DIVBY_34		(17 << 2)	//
#define CLKOUT_DIVBY_36		(18 << 2)	//
#define CLKOUT_DIVBY_38		(19 << 2)	//

#define CLKOUT_DISABLE		0xff	// impossible value to signify no clkout desired
#if RF_CLKOUT_DIV == CLKOUT_DISABLE
	#define RF_OSC_CLKOUT_ENABLE	RF_OSC_CLKOUT_OFF
#else
	#define RF_OSC_CLKOUT_ENABLE	RF_OSC_CLKOUT_ON
#endif


// PKT Param 1
// Manchester enable
#define RF_PKT1_MANCHESTER_ON            0x80
#define RF_PKT1_MANCHESTER_OFF           0x00

// Payload length
#define RF_PKT1_LENGTH_VALUE             0x00


// Node Address
#define RF_NODEADRS_VALUE                0x00


// PKT Param 3
//Packet format
#define RF_PKT3_FORMAT_FIXED             0x00
#define RF_PKT3_FORMAT_VARIABLE          0x80

// Preamble size
#define RF_PKT3_PREAMBLE_SIZE_8          0x00
#define RF_PKT3_PREAMBLE_SIZE_16         0x20
#define RF_PKT3_PREAMBLE_SIZE_24         0x40
#define RF_PKT3_PREAMBLE_SIZE_32         0x60

// Whitening enable
#define RF_PKT3_WHITENING_ON             0x10
#define RF_PKT3_WHITENING_OFF            0x00

// CRC enable
#define RF_PKT3_CRC_ON                   0x08
#define RF_PKT3_CRC_OFF                  0x00

// Address filtering
#define RF_PKT3_ADRSFILT_NONE				0x00	/* no filtering */
#define RF_PKT3_ADRSFILT_ME_ONLY			0x02	/* only Node_adrs accepted (register 29) */
#define RF_PKT3_ADRSFILT_ME_AND_00			0x04	/* Node_adrs and 0x00 accepted */
#define RF_PKT3_ADRSFILT_ME_AND_00_AND_FF	0x06	/* Node_adrs and 0x00 and 0xff accepted */

//CRC status (Read only)
#define RF_PKT3_CRC_STATUS               0x01


// PKT Param 4
// FIFO autoclear if CRC failed for current packet
#define RF_PKT4_AUTOCLEAR_ON             0x00
#define RF_PKT4_AUTOCLEAR_OFF            0x80

// Select FIFO access in standby mode (read or write)
#define RF_PKT4_FIFO_STANDBY_WRITE       0x00
#define RF_PKT4_FIFO_STANDBY_READ        0x40

/**
 * one-time power-up initialization of the registers in the SX1211
 * Note that the clkout pin will be assigned a divider value (register 27),
 * and that this function should be call before using the clkout pin.
 */
void InitRFChip(void);

/**
 * configures the Chip_mode of the SX1211 transceiver (bits 5,6,7 in register 0)
 * \param mode sleep, standby, frequency-synthesizer, receive, or transmit.
 */
void SetRFMode(uint8_t mode);

/**
 * Writes to an SX1211 register.
 * The NSS_CONFIG pin is handled by the caller.
 * This function is provided for reducing overhead of multiple consecutive register writes.
 * The SX1211 does not require NSS_CONFIG be re-asserted after every register access.
 * \param address the address to write to, 0 to 31
 * \param value the	octet to be written
 */
void _WriteRegister(uint8_t address, uint8_t value);	// NSS_CONFIG handled by caller

/**
 * writes to an SX1211 register using NSS_CONFIG pin
 * this function is a wrapper around _Write_Register()
 * \param address the address to write to, 0 to 31
 * \param value the	octet to be written
 */
void WriteRegister(uint8_t address, uint8_t value);
 
/**
 * reads the value of an SX1211 register using NSS_CONFIG pin
 * \param address the address to read from, 0 to 31
 * \return the contents of the register
 */
uint16_t ReadRegister(uint8_t address);

/*******************************************************************
** Communication functions                                        **
*******************************************************************/

/**
 * transmits a packet over the radio transceiver.
 * \param buffer pointer to the bytes to send (packet payload)
 * \param size the count of bytes to send
 * \param Node_adrs the node_address to send with this packet.
 */
uint8_t SendRfFrame(const uint8_t *buffer, uint8_t size, uint8_t Node_adrs, char immediate_rx);

/**
 * receives an RF packet from the transceiver.
 * Note this function must be called in "cooperative multitasking" fashion, where it is called repeatedly until somthing other than RX_RUNNING is returned.
 * \param buffer the received bytes will be placed here
 * \param size the count of bytes placed into buffer
 * \param timer compare the amount of time to receive before declaring a timeout. use the macro HIRES_TIMEOUT() to convert the needed value from microseconds.
 */
uint8_t ReceiveRfFrame(uint8_t *buffer, uint8_t *size, uint16_t timer_compare);

/**
 * Writes to the transceiver's fifo using NSS_DATA.
 * Note that for packet-mode operation, the bit
 * RF_PKT4_FIFO_STANDBY_WRITE (bit 6) in register 31 must have
 * been previously cleared to 0.
 *
 * The SX1211 requires that NSS_DATA be re-asserted for every byte accessed via the fifo.
 *
 * \param b the byte to send into fifo
 */
//void SendByte(uint8_t b);

/**
 * read a byte from the transceiver's fifo, using NSS_DATA.
 * Note that for packet-mode operation, the bit
 * RF_PKT4_FIFO_STANDBY_READ (bit 6) in register 31 must have
 * been previously set to 1.
 *
 * The SX1211 requires that NSS_DATA be re-asserted for every byte accessed via the fifo.
 *
 * \return the byte received from the fifo
 * \ingroup wsn_fhss
 */
uint8_t ReceiveByte(void);

/**
 * read the current signal strength of received radio signal.
 * \ingroup wsn_fhss
 */
uint16_t ReadRssi(void);


/*******************************************************************
** exported variables                                             **
*******************************************************************/
extern const rom uint8_t RegistersCfg[];
//pic24f: extern const uint8_t RegistersCfg[];

extern uint8_t PreMode;

#endif /* __SX1211DRIVER__ */

#endif /* SX1211 */
