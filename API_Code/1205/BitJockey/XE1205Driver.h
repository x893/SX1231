/*******************************************************************
** File        : XE1205driver.h                                   **
********************************************************************
**                                                                **
** Version     : V 1.0                                            **
**                                                                **
** Written by   : Miguel Luis & Grégoire Guye                     **
**                                                                **
** Date        : 19-01-2004                                       **
**                                                                **
** Project     : API-1205                                         **
**                                                                **
********************************************************************
** Changes     : V 2.1 / MiL - 24-04-2004                         **
**                                                                **
** Changes     : V 2.3 / CRo - 06-06-2006                         **
**               - No change                                      **
**                                                                **
** Changes     : V 2.4 / CRo - 09-01-2007                         **
**               - No change                                      **
**                                                                **
**                                                                **
********************************************************************
** Description : XE1205 transceiver drivers implementation for the**
**               XE8806A and XE8807A (BitJockey)                  **
*******************************************************************/
#ifndef __XE1205DRIVER__
#define __XE1205DRIVER__

/*******************************************************************
** Include files                                                  **
*******************************************************************/
#include "Globals.h"

/*******************************************************************
** Global definitions                                             **
*******************************************************************/
/*******************************************************************
** RF packet definition                                           **
*******************************************************************/
#define RF_BUFFER_SIZE_MAX   64
#define RF_BUFFER_SIZE       64
#define SYNC_BYTE_FREQ       4

/*******************************************************************
** RF State machine                                               **
*******************************************************************/
#define RF_STOP              0x01
#define RF_BUSY              0x02
#define RF_RX_DONE           0x04
#define RF_TX_DONE           0x08
#define RF_ERROR             0x10
#define RF_TIMEOUT           0x20
#define RF_AFC_DONE          0x40

/*******************************************************************
** RF function return codes                                       **
*******************************************************************/
#define OK                   0x00
#define ERROR                0x01
#define RX_TIMEOUT           0x02
#define RX_RUNNING           0x03
#define TX_TIMEOUT           0x04
#define TX_RUNNING           0x05

/*******************************************************************
** I/O Ports Definitions                                          **
*******************************************************************/
#define PORTO              RegPDOut
#define PORTI              RegPDIn
#define PORTDIR            RegPDDir
#define PORTP              RegPDPullup
#define ANT_SWITCH         RegPBOut

/*******************************************************************
** Port A pins definitions                                        **
********************************************************************
**                                  *  uC     * XE1205    * PAx   **
*******************************************************************/

/*******************************************************************
** Port B pins definitions                                        **
********************************************************************
**                                  *  uC     * XE1205    * PBx   **
*******************************************************************/
#define SW1             0x10      //*  In     *  Out      * PB4 // TX
#define SW0             0x40      //*  In     *  Out      * PB5 // RX

/*******************************************************************
** Port C pins definitions                                        **
********************************************************************
**                                  *  uC     * XE1205    * PCx   **
*******************************************************************/
#define DATA            0x01      //*  In/Out  *  In/Out  * PD0 // DATA -> DataOut
#define IRQ_1           0x02      //*  In      *  Out     * PD1 // DCLK
#define IRQ_0           0x04      //*  In      *  Out     * PD2 // PATTERN
#define NSS_DATA        0x08      //*  In/Out  *  In/Out  * PD3 // NSS_DATA -> DataIn
#define NSS_CONFIG      0x10      //*  Out     *  In      * PD4
#define SCK             0x20      //*  Out     *  In      * PD5
#define MOSI            0x40      //*  Out     *  In      * PD6
#define MISO            0x80      //*  In      *  Out     * PD7


/*******************************************************************
** XE1205 SPI Macros definitions                                  **
*******************************************************************/
#define SPIInit()           (PORTDIR = (PORTDIR | SCK | NSS_CONFIG | MOSI) & (~(MISO | NSS_DATA | IRQ_0 | IRQ_1 | DATA)))
#define SPIClock(level)     ((level) ? (PORTO |= SCK) : (PORTO &= ~SCK))
#define SPIMosi(level)      ((level) ? (PORTO |= MOSI) : (PORTO &= ~MOSI))
#define SPINssData(level)   ((level) ? (RegPBOut |= NSS_DATA) : (RegPBOut &= ~NSS_DATA))
#define SPINssConfig(level) ((level) ? (PORTO |= NSS_CONFIG) : (PORTO &= ~NSS_CONFIG))
#define SPIMisoTest()       (PORTI & MISO)

/*******************************************************************
** XE1205 definitions                                             **
*******************************************************************/

/*******************************************************************
** XE1205 Operating modes definition                              **
*******************************************************************/
#define RF_SLEEP                         0x00
#define RF_RECEIVER                      0x40
#define RF_TRANSMITTER                   0x80
#define RF_STANDBY                       0xC0

/*******************************************************************
** XE1205 Internal registers Address                              **
*******************************************************************/
#define REG_MCPARAM1                     0x00
#define REG_MCPARAM2                     0x01
#define REG_MCPARAM3                     0x02
#define REG_MCPARAM4                     0x03
#define REG_MCPARAM5                     0x04

#define REG_IRQPARAM1                    0x05
#define REG_IRQPARAM2                    0x06

#define REG_TXPARAM1                     0x07

#define REG_RXPARAM1                     0x08
#define REG_RXPARAM2                     0x09
#define REG_RXPARAM3                     0x0A
#define REG_RXPARAM4                     0x0B // MSB FEI
#define REG_RXPARAM5                     0x0C // LSB FEI
#define REG_RXPARAM6                     0x0D // Pattern 1
#define REG_RXPARAM7                     0x0E // Pattern 2
#define REG_RXPARAM8                     0x0F // Pattern 3
#define REG_RXPARAM9                     0x10 // Pattern 4

#define REG_OSCPARAM1                    0x11
#define REG_OSCPARAM2                    0x12

/*******************************************************************
** XE1205 default register values definition                      **
*******************************************************************/
#define DEF_MCPARAM1                     0x00 //
#define DEF_MCPARAM2                     0x00 //
#define DEF_MCPARAM3                     0x00 //
#define DEF_MCPARAM4                     0x00 //
#define DEF_MCPARAM5                     0x00 //

#define DEF_IRQPARAM1                    0x00 //
#define DEF_IRQPARAM2                    0x00 //

#define DEF_TXPARAM1                     0x00 //

#define DEF_RXPARAM1                     0x00 //
#define DEF_RXPARAM2                     0x00 //
#define DEF_RXPARAM3                     0x00 //
#define DEF_RXPARAM4                     0x00 // MSB FEI
#define DEF_RXPARAM5                     0x00 // MSB FEI
#define DEF_RXPARAM6                     0x00 // Pattern 1
#define DEF_RXPARAM7                     0x00 // Pattern 2
#define DEF_RXPARAM8                     0x00 // Pattern 3
#define DEF_RXPARAM9                     0x00 // Pattern 4

#define DEF_OSCPARAM1                    0x00 //
#define DEF_OSCPARAM2                    0x00 //

/*******************************************************************
** XE1205 bit control definition                                  **
*******************************************************************/
// MC Param 1
// Chip operating mode
#define RF_MC1_STANDBY                   0xC0
#define RF_MC1_TRANSMITTER               0x80
#define RF_MC1_RECEIVER                  0x40
#define RF_MC1_SLEEP                     0x00
// Chip mode selection
#define RF_MC1_MODE_SW_PIN               0x20
#define RF_MC1_MODE_CHIP                 0x00
// Enables buffered mode
#define RF_MC1_BUFFERED_MODE_ON             0x10
#define RF_MC1_BUFFERED_MODE_OFF            0x00
// Configure data pin behavior
#define RF_MC1_DATA_UNIDIR_ON            0x08
#define RF_MC1_DATA_UNIDIR_OFF           0x00
// Frequency band
#define RF_MC1_BAND_915                  0x06
#define RF_MC1_BAND_868                  0x04
#define RF_MC1_BAND_433                  0x02
// Frequency deviation MSB
#define RF_MC1_FREQ_DEV_MSB_1            0x01
#define RF_MC1_FREQ_DEV_MSB_0            0x00

// MC Param 2
// Transceiver Frequency deviation
#define RF_MC2_FDEV_5                    0x0A
#define RF_MC2_FDEV_10                   0x14
#define RF_MC2_FDEV_20                   0x28
#define RF_MC2_FDEV_40                   0x50
#define RF_MC2_FDEV_55                   0x6E
#define RF_MC2_FDEV_80                   0xA0
#define RF_MC2_FDEV_100                  0xC8
#define RF_MC2_FDEV_160                  0x40 // RF_MC1_FREQ_DEV_MSB needs to be set
#define RF_MC2_FDEV_200                  0x90 // RF_MC1_FREQ_DEV_MSB needs to be set

// MC Param 3
// Konnex mode enable
#define RF_MC3_KONNEX_ON                 0x80
#define RF_MC3_KONNEX_OFF                0x00
// Transceiver Standard baudrate values
#define RF_MC3_BAUDRATE_1200             0x7E
#define RF_MC3_BAUDRATE_2400             0x3E
#define RF_MC3_BAUDRATE_4800             0x1F
#define RF_MC3_BAUDRATE_9600             0x0F
#define RF_MC3_BAUDRATE_19200            0x07
#define RF_MC3_BAUDRATE_38400            0x03
#define RF_MC3_BAUDRATE_76800            0x01
#define RF_MC3_BAUDRATE_153600           0x00

// IRQ Param 1
// Select RX IRQ_0 sources (Buffered mode)
#define RF_IRQ0_RX_IRQ_PATTERN           0xC0
#define RF_IRQ0_RX_IRQ_FIFO_EMPTY        0x80
#define RF_IRQ0_RX_IRQ_WRITE_BYTE        0x40
#define RF_IRQ0_RX_IRQ_OFF               0x00
// Select RX IRQ_0 sources (normal mode)
#define RF_IRQ0_RX_IRQ_RSSI              0x40
// Select RX IRQ_1 sources (Buffered mode)
#define RF_IRQ1_RX_IRQ_FIFO_FULL         0x10
#define RF_IRQ1_RX_IRQ_RSSI              0x20
#define RF_IRQ1_RX_IRQ_OFF               0x00
// Select TX IRQ_1 sources (Buffered mode)
#define RF_IRQ1_TX_IRQ_TX_STOPPED        0x08
#define RF_IRQ1_TX_IRQ_TX_FIFO_FULL      0x00
// FIFO overrun error
#define RF_IRQ1_FIFO_CLEAR               0x01

// IRQ Param 2
// FIFO filling selection mode
#define RF_IRQ2_START_FILL_ALWAYS        0x80
#define RF_IRQ2_START_FILL_PATTERN_DET   0x00
// Start of FIFO filling
#define RF_IRQ2_START_DETECT_CLEAR       0x40
#define RF_IRQ2_START_FILL_FIFO_START    0x40
#define RF_IRQ2_START_FILL_FIFO_STOP     0x00
// Tx stopped
//#define RF_IRQ2_TX_STTOPED             0x20
// Transmission FIFO behavior
#define RF_IRQ2_START_TX_FIFO_FULL       0x00
#define RF_IRQ2_START_TX_FIFO_NOT_EMPTY  0x10
// Enable interrupt SIGNAL_DETECT when RSSI_threshold is reached
#define RF_IRQ2_RSSI_IRQ_ON              0x08
#define RF_IRQ2_RSSI_IRQ_OFF             0x00
// Detection of a signal above the RSSI_thres
#define RF_IRQ2_RSSI_SIGNAL_DETECT_CLEAR 0x04
// RSSI threshold for interrupt
#define RF_IRQ2_RSSI_THRES_VTHR4         0x03
#define RF_IRQ2_RSSI_THRES_VTHR3         0x02
#define RF_IRQ2_RSSI_THRES_VTHR2         0x01
#define RF_IRQ2_RSSI_THRES_VTHR1         0x00

// TX Param 1
// Transmitter output power
#define RF_TX1_POWER_15                  0xC0
#define RF_TX1_POWER_10                  0x80
#define RF_TX1_POWER_5                   0x40
#define RF_TX1_POWER_0                   0x00
// Inhibition of the modulation in transmitter mode
#define RF_TX1_MODUL_ON                  0x00
#define RF_TX1_MODUL_OFF                 0x20
// Pre-filtering of the bit stream in transmitter mode
#define RF_TX1_FILTER_ON                 0x10
#define RF_TX1_FILTER_OFF                0x00
//
#define RF_TX1_FIX_BSYNC_NOSY            0x02
#define RF_TX1_FIX_BSYNC_NORMAL          0x00
//
#define RF_TX1_SELECT_DEMOD_1            0x00
#define RF_TX1_SELECT_DEMOD_2            0x01

// RX Param 1
// Bit synchronizer
#define RF_RX1_BITSYNC_ON                0x00
#define RF_RX1_BITSYNC_OFF               0x80
// Bandwidth of the base band filter
#define RF_RX1_BW_200                    0x60
#define RF_RX1_BW_40                     0x40
#define RF_RX1_BW_20                     0x20
#define RF_RX1_BW_10                     0x00
// Forcing the bandwidth of the base band filter to
//it maximal value and disabling the regulation
#define RF_RX1_BW_MAX_ON                 0x10
#define RF_RX1_BW_MAX_OFF                0x00
// Regulation of the bandwidth of the base band filter
#define RF_RX1_REG_BW_NOW                0x0C
#define RF_RX1_REG_BW_BB_FILTER_CHANGE   0x08
#define RF_RX1_REG_BW_OFF                0x04
#define RF_RX1_REG_BW_START_UP           0x00
// Boosting of the base band filter
#define RF_RX1_BOSST_FILTER_NOW          0x03
#define RF_RX1_BOSST_FILTER_BW_CHANGE    0x02
#define RF_RX1_BOSST_FILTER_OFF          0x01
#define RF_RX1_BOSST_FILTER_START_UP     0x00

// RX Param 2
// Enable RSSI
#define RF_RX2_RSSI_ON                   0x80
#define RF_RX2_RSSI_OFF                  0x00
// Range of the RSSI
#define RF_RX2_HIGH_RANGE                0x40
#define RF_RX2_LOW_RANGE                 0x00
// Frequency Error Indicator ON/OFF
#define RF_RX2_FEI_ON                    0x08
#define RF_RX2_FEI_OFF                   0x00
// Start the Automatic Frequency Control process
#define RF_RX2_AFC_START                 0x04
#define RF_RX2_AFC_STOP                  0x00
// Disabling the AFC correction
#define RF_RX2_AFC_CORRECTION_ON         0x00
#define RF_RX2_AFC_CORRECTION_OFF        0x01

// RX Param 3
// AFC overflow indicator
#define RF_RX3_AFC_OVERFLOW_CLEAR        0x80
// IQ amplifiers
#define RF_RX3_IQAMP_ON                  0x40
#define RF_RX3_IQAMP_OFF                 0x00
// Linearity/Sensitivity mode
#define RF_RX3_RMODE_MODE_B              0x20
#define RF_RX3_RMODE_MODE_A              0x00
// Pattern recognition
#define RF_RX3_PATTERN_ON                0x10
#define RF_RX3_PATTERN_OFF               0x00
// Size of the reference pattern
#define RF_RX3_P_SIZE_32                 0x0C
#define RF_RX3_P_SIZE_24                 0x08
#define RF_RX3_P_SIZE_16                 0x04
#define RF_RX3_P_SIZE_8                  0x00
// Number of tolerated errors for the pattern recognition
#define RF_RX3_P_TOL_0                   0x00
#define RF_RX3_P_TOL_1                   0x01
#define RF_RX3_P_TOL_2                   0x02
#define RF_RX3_P_TOL_3                   0x03

// OSC Param 1
// Source of reference frequency
#define RF_OSC1_OSC_EXT                  0x80
#define RF_OSC1_OSC_INT                  0x00
// ClkOut enable
#define RF_OSC1_CLKOUT_ON                0x40
#define RF_OSC1_CLKOUT_OFF               0x00
// Frequency of ClkOut
#define RF_OSC1_CLK_FREQ_1_22_MHZ        0x00
#define RF_OSC1_CLK_FREQ_2_44_MHZ        0x08
#define RF_OSC1_CLK_FREQ_4_87_MHZ        0x10
#define RF_OSC1_CLK_FREQ_9_75_MHZ        0x18
#define RF_OSC1_CLK_FREQ_20_00_MHZ       0x20

// OSC Param 2
// Select the resistor value put between TKA and TKB
#define RF_OSC2_RES_X_OSC_3800           0x00
#define RF_OSC2_RES_X_OSC_1_48           0x10
#define RF_OSC2_RES_X_OSC_1_56           0x20
#define RF_OSC2_RES_X_OSC_1_66           0x30
#define RF_OSC2_RES_X_OSC_1_78           0x40
#define RF_OSC2_RES_X_OSC_1_91           0x50
#define RF_OSC2_RES_X_OSC_2_07           0x60
#define RF_OSC2_RES_X_OSC_2_26           0x70
#define RF_OSC2_RES_X_OSC_2_55           0x80
#define RF_OSC2_RES_X_OSC_2_81           0x90
#define RF_OSC2_RES_X_OSC_3_22           0xA0
#define RF_OSC2_RES_X_OSC_3_79           0xB0
#define RF_OSC2_RES_X_OSC_4_65           0xC0
#define RF_OSC2_RES_X_OSC_6_04           0xD0
#define RF_OSC2_RES_X_OSC_8_79           0xE0
#define RF_OSC2_RES_X_OSC_16_55          0xF0

/*******************************************************************
** Timings section : all timing described here are done with the  **
**                   A&B counters cascaded                        **
*******************************************************************/
/*******************************************************************
**             -- XE1205 Recommended timings --                  **
********************************************************************
** These timings depends on the RC frequency                      **
** The following values corresponds to a 2.4576 MHz RC Freq       **
** Times calculation formula                                      **
**                                                                **
** CounterA&B value = RC * wanted time  = 2 457 600  * 2 ms = 4915**
**                                                                **
*******************************************************************/
#define TS_OS                4915 // Quartz Osc Wake up time, 2 ms
#define TS_STR               860  // Transmitter wake-up time from OSC run, 350 us
#define TS_TR                369  // Transmitter wake-up time, 150 us
#define TS_SRE               2089  // Receiver wake-up time from OSC run, 850 us
#define TS_RE                1474 // Receiver wake-up time, 600 us
#define TS_RSSI              3686 // RSSI wake-up time, 1.5 ms

/*******************************************************************
**                                                                **
**                  152340                                        **
** BitRate = -------------------                                  **
**           (RegBr & 0x7F) + 1                                   **
**                        2               2 * ((RegBr & 0x7F) + 1)**
** CounterA&B value = --------- * RC = ------------------------------ * RC
**                     BitRate                      152340        **
**                                                                **
*******************************************************************/
#define TS_FEI(RegBr) (_U16)(((_F32)((RegBr & 0x7F) + 1) * (_F32)4915200) / (_F32)152340)   // FEI wake-up time

/*******************************************************************
**                                                                **
**                     // RF_BUFFER_SIZE * 8 * (SYNC_BYTE_FREQ  + 1) \          \
** CounterA&B value = || -------------------------------------------- | * 128 Hz | + 1
**                     \\        SYNC_BYTE_FREQ * BitRate            /          /
**                                                                **
** The plus 1 at the end of formula is required for the highest   **
** baudrate as the resulting timeout is lower than the 1 / 128Hz  **
*******************************************************************/
#define RF_FRAME_TIMEOUT(BitRate) (_U16)(_F32)((((_F32)((_U32)RF_BUFFER_SIZE * (_U32)8 *((_U32)SYNC_BYTE_FREQ  + (_U32)1)) / (_F32)((_U32)SYNC_BYTE_FREQ * (_U32)BitRate)) * (_F32)128) + (_F32)1)

/*******************************************************************
** BitJockey Standard parameters                                  **
*******************************************************************/
// All the values are calculated for microcontroller RC frequency = 2457600 Hz
// RegRfifCmd1
#define RFIF_BAUDRATE_1200      0x37
#define RFIF_BAUDRATE_2400      0x33
#define RFIF_BAUDRATE_4800      0x31
#define RFIF_BAUDRATE_9600      0x21
#define RFIF_BAUDRATE_19200     0x11
#define RFIF_BAUDRATE_38400     0x10
#define RFIF_BAUDRATE_76800     0x01
#define RFIF_BAUDRATE_153600    0x00

// RegRfifCmd2
#define RFIF_EN_START_INTERNAL  0xC0
#define RFIF_EN_START_EXTERNAL  0x80
#define RFIF_EN_START_PROTOCOL  0x40
#define RFIF_EN_DECODER         0x20
#define RFIF_RX_CLOCK           0x10
#define RFIF_TX_CLOCK           0x08
#define RFIF_PCM_NRZ_MARK       0x01
#define RFIF_PCM_NRZ_SPACE      0x02
#define RFIF_PCM_BPH_LEVEL      0x03
#define RFIF_PCM_BPH_MARK       0x04
#define RFIF_PCM_BPH_SPACE      0x05
#define RFIF_PCM_MILLER         0x06

// RegRfifCmd3
#define RFIF_RX_IRQ_EN_FULL     0x80
#define RFIF_RX_IRQ_EN_NEW      0x40
#define RFIF_RX_IRQ_EN_START    0x20
#define RFIF_RX_IRQ_FULL        0x10
#define RFIF_RX_IRQ_NEW         0x08
#define RFIF_RX_IRQ_START       0x04
#define RFIF_EN_RX              0x02
#define RFIF_EN_TX              0x01

// RFIF modes
#define RFIF_DISABLE            0
#define RFIF_TRANSMITTER        1
#define RFIF_RECEIVER           2
#define RFIF_OTHERS             3

/*******************************************************************
** Functions prototypes                                           **
*******************************************************************/

/*******************************************************************
** Configuration functions                                        **
*******************************************************************/

/*******************************************************************
** InitRFChip : This routine initializes the RFChip registers     **
**              Using Pre Initialized variable                    **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitRFChip (void);

/*******************************************************************
** SetRFMode : Sets the XE1205 operating mode (Sleep, Receiver,   **
**           Transmitter)                                         **
********************************************************************
** In  : mode                                                     **
** Out : -                                                        **
*******************************************************************/
void SetRFMode(_U8 mode);

/*******************************************************************
** SetModeRFIF : Sets the BitJockey in the given mode             **
********************************************************************
** In  : mode                                                     **
** Out :                                                          **
*******************************************************************/
void SetModeRFIF(_U8 mode);

/*******************************************************************
** WriteRegister : Writes the register value at the given address **
**                  on the XE1205                                 **
********************************************************************
** In  : address, value                                           **
** Out : -                                                        **
*******************************************************************/
void WriteRegister(_U8 address, _U16 value);

/*******************************************************************
** ReadRegister : Reads the register value at the given address on**
**                the XE1205                                      **
********************************************************************
** In  : address                                                  **
** Out : value                                                    **
*******************************************************************/
_U16 ReadRegister(_U8 address);

/*******************************************************************
** Communication functions                                        **
*******************************************************************/

/*******************************************************************
** SendRfFrame : Sends a RF frame                                 **
********************************************************************
** In  : *buffer, size                                            **
** Out : *pReturnCode                                             **
*******************************************************************/
void SendRfFrame(_U8 *buffer, _U8 size, _U8 *pReturnCode);

/*******************************************************************
** ReceiveRfFrame : Receives a RF frame                           **
********************************************************************
** In  : -                                                        **
** Out : *buffer, size, *pReturnCode                              **
*******************************************************************/
void ReceiveRfFrame(_U8 *buffer, _U8 *size, _U8 *pReturnCode);

/*******************************************************************
** Transceiver specific functions                                 **
*******************************************************************/

/*******************************************************************
** AutoFreqControl : Calibrates the receiver LO frequency to the  **
**               transmitter LO frequency                         **
********************************************************************
** In  : -                                                        **
** Out : *pReturnCode                                             **
*******************************************************************/
void AutoFreqControl(_U8 *pReturnCode);

/*******************************************************************
** ReadLO : Reads the LO frequency value from  XE1205             **
********************************************************************
** In  : -                                                        **
** Out : value                                                    **
*******************************************************************/
_U16 ReadLO(void);

/*******************************************************************
** WriteLO : Writes the LO frequency value on the XE1205          **
********************************************************************
** In  : value                                                    **
** Out : -                                                        **
*******************************************************************/
void WriteLO(_U16 value);

/*******************************************************************
** InitFei : Initializes the XE1205 to enable the FEI reading     **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitFei(void);

/*******************************************************************
** ReadFei : Reads the FEI value from  XE1205                     **
********************************************************************
** In  : -                                                        **
** Out : value                                                    **
*******************************************************************/
_S16 ReadFei(void);

/*******************************************************************
** InitRssi : Initializes the XE1205 to enable the RSSI reading   **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitRssi(void);

/*******************************************************************
** ReadRssi : Reads the Rssi value from  XE1205                   **
********************************************************************
** In  : -                                                        **
** Out : value                                                    **
*******************************************************************/
_U16 ReadRssi(void);

/*******************************************************************
** Utility functions                                              **
*******************************************************************/

/*******************************************************************
** Wait : This routine uses the counter A&B to create a delay     **
**        using the RC ck source                                  **
********************************************************************
** In   : cntVal                                                  **
** Out  : -                                                       **
*******************************************************************/
void Wait(_U16 cntVal);

/*******************************************************************
** EnableTimeOut : Enables/Disables the RF frame timeout          **
********************************************************************
** In  : enable                                                   **
** Out : -                                                        **
*******************************************************************/
void EnableTimeOut(_U8 enable);

/*******************************************************************
** InvertByte : Inverts a byte. MSB -> LSB, LSB -> MSB            **
********************************************************************
** In  : b                                                        **
** Out : b                                                        **
*******************************************************************/
_U8 InvertByte(_U8 b);

/*******************************************************************
** SpiInOut : Sends and receives a byte from the SPI bus          **
********************************************************************
** In  : outputByte                                               **
** Out : inputByte                                                **
*******************************************************************/
_U8 SpiInOut (_U8 outputByte);

#endif /* __XE1205DRIVER__ */

