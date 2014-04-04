/*******************************************************************
** File        : XE1202driver.h                                   **
********************************************************************
**                                                                **
** Version     : V 1.0                                            **
**                                                                **
** Written by   : Miguel Luis & Grégoire Guye                     **
**                                                                **
** Date        : 28-03-2003                                       **
**                                                                **
** Project     : API-1202                                         **
**                                                                **
********************************************************************
** Changes     : V 2.0 / MiL - 09-12-2003                         **
**                - RF frame format changed                       **
**                - Most functions modified to be more flexible   **
**                - Add BitJockey / Non BitJockey compatibility   **
**                                                                **
** Changes     : V 2.1 / MiL - 24-04-2004                         **
**               - No changes                                     **
**                                                                **
** Changes     : V 2.3 / CRo - 06-06-2006                         **
**               - I/O Ports Definitions section updated          **
**                                                                **
** Changes     : V 2.4 / CRo - 09-01-2007                         **
**               - No change                                      **
**                                                                **
**                                                                **
********************************************************************
** Description : XE1202 transceiver drivers implementation for the**
**               XE8000 family products (BitBang)                 **
*******************************************************************/
#ifndef __XE1202DRIVER__
#define __XE1202DRIVER__

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
#if defined(_XE88LC01A_) || defined(_XE88LC05A_)
	#define PORTO              RegPCOut
	#define PORTI              RegPCIn
	#define PORTDIR            RegPCDir
	#define ANT_SWITCH         RegPCOut
#endif
#if defined(_XE88LC02_) || defined(_XE88LC02_4KI_) || defined(_XE88LC02_8KI_)
	#define PORTO              RegPD1Out
	#define PORTI              RegPD1In
	#define PORTDIR            RegPD1Dir
	#define ANT_SWITCH         RegPD1Out	
#endif
#if defined(_XE88LC06A_) || defined(_XE88LC07A_) 
	#define PORTO              RegPDOut
	#define PORTI              RegPDIn
	#define PORTDIR            RegPDDir
	#define ANT_SWITCH         RegPDOut
#endif

/*******************************************************************
** Port A pins definitions                                        **
********************************************************************
**                                  *  uC     * XE1202    * PAx   **
*******************************************************************/
#define DCLK         	0x02      //*  In     *  Out      * PA1
#define DATAOUT         0x04      //*  In     *  Out      * PA2
#define PATTERN         0x08      //*  In     *  Out      * PA3

/*******************************************************************
** Port B pins definitions                                        **
********************************************************************
**                                  *  uC     * XE1202    * PBx   **
*******************************************************************/
#define MODE0           0x01      //*  Out    *  In       * PB0
#define MODE1           0x02      //*  Out    *  In       * PB1
#define MODE2           0x04      //*  Out    *  In       * PB2

/*******************************************************************
** Port C pins definitions                                        **
********************************************************************
**                                  *  uC     * XE1202    * PDx   **
*******************************************************************/
#define DATAIN          0x01      //*  Out     *  In      * PC0
#define TX              0x02      //*  Out     *  In      * PC1
#define RX              0x04      //*  Out     *  In      * PC2
#define EN              0x08      //*  Out     *  In      * PC3
#define SCK             0x10      //*  Out     *  In      * PC4
#define SI              0x20      //*  Out     *  In      * PC5
#define SO              0x40      //*  In      *  Out     * PC6

/*******************************************************************
** XE1202 3 wire serial interface macros definitions              **
*******************************************************************/
#define SrtInit()        (PORTDIR = ~SO)
#define SrtSetSCK(level) (level) ? (set_bit(PORTO, SCK)) : (clear_bit(PORTO, SCK))
#define SrtSetSI(level)  (level) ? (set_bit(PORTO, SI)) : (clear_bit(PORTO, SI))
#define SrtCheckSO()     check_bit(PORTI, SO)

/*******************************************************************
** XE1202 definitions                                             **
*******************************************************************/

/*******************************************************************
** XE1202 Operating modes definition                              **
*******************************************************************/
#define RF_SLEEP                 0
#define RF_STANDBY               1
#define RF_RECEIVER_1            2
#define RF_RECEIVER_2            3
#define RF_RECEIVER              4
#define RF_TRANSMITTER_1         6
#define RF_TRANSMITTER           7

/*******************************************************************
** XE1202 Internal registers Address                              **
*******************************************************************/
#define REG_RTPARAM1             0x00
#define REG_RTPARAM2             0x01

#define REG_FSPARAM1             0x02
#define REG_FSPARAM2             0x03
#define REG_FSPARAM3             0x04

#define REG_DATAOUT              0x05

#define REG_ADPARAM1             0x06
#define REG_ADPARAM2             0x07

#define REG_PATTERN1             0x08
#define REG_PATTERN2             0x09
#define REG_PATTERN3             0x0A
#define REG_PATTERN4             0x0B

/*******************************************************************
** XE1202 default register values definition                      **
*******************************************************************/
#define DEF_RTPARAM1             0x00
#define DEF_RTPARAM2             0x00
 
#define DEF_FSPARAM1             0x00
#define DEF_FSPARAM2             0x00
#define DEF_FSPARAM3             0x00

#define DEF_ADPARAM1             0x00
#define DEF_ADPARAM2             0x00

#define DEF_PATTERN1             0x00
#define DEF_PATTERN2             0x00
#define DEF_PATTERN3             0x00
#define DEF_PATTERN4             0x00

/*******************************************************************
** XE1202 bit control definition                                  **
*******************************************************************/
// RT Param 1
// Receiver mode configuration
#define RF_RT1_RMODE_MODE_B      0x80
#define RF_RT1_RMODE_MODE_A      0x00
// Bit synchronizer
#define RF_RT1_BIT_SYNC_ON       0x40
#define RF_RT1_BIT_SYNC_OFF      0x00
// RSSI enable
#define RF_RT1_RSSI_ON           0x20
#define RF_RT1_RSSI_OFF          0x00
// FEI
#define RF_RT1_FEI_ON            0x10
#define RF_RT1_FEI_OFF           0x00
// Bandwidth of the baseband filter
#define RF_RT1_BW_10             0x00
#define RF_RT1_BW_20             0x04
#define RF_RT1_BW_40             0x08
#define RF_RT1_BW_200            0x0C
// Transmit Output power 
#define RF_RT1_POWER_0			 0x00
#define RF_RT1_POWER_5			 0x01
#define RF_RT1_POWER_10			 0x02
#define RF_RT1_POWER_15			 0x03

// RT Param 2
// Source for the reference frequency
#define RF_RT2_OSC_EXT           0x80
#define RF_RT2_OSC_INT           0x00
// Receiver wake-up type selection
#define RF_RT2_WBB_STD           0x40
#define RF_RT2_WBB_BOOST         0x00
// Pre-filtering of bit stream in transmitter mode
#define RF_RT2_FILTER_ON         0x20
#define RF_RT2_FILTER_OFF        0x00
// Selection of FEI block
#define RF_RT2_FSEL_CORRELATOR   0x10
#define RF_RT2_FSEL_DEMODULATOR  0x00
// Rising and falling times in case of pre-filtering in transmitter mode
#define RF_RT2_STAIR_20          0x08
#define RF_RT2_STAIR_10          0x00
// Inhibition of the modulation in transmitter mode
#define RF_RT2_MODUL_ON          0x00
#define RF_RT2_MODUL_OFF         0x04
// Range of the RSSI
#define RF_RT2_RSSR_HIGH         0x02
#define RF_RT2_RSSR_LOW          0x00
// ClkOut enable
#define RF_RT2_CLKOUT_ON         0x01
#define RF_RT2_CLKOUT_OFF        0x00

// FS Param 1
// Transceiver Frequency band
#define RF_FS1_BAND_433          0x40
#define RF_FS1_BAND_868          0x80
#define RF_FS1_BAND_915          0xC0
// Transceiver Frequency deviation
#define RF_FS1_FDEV_5            0x00
#define RF_FS1_FDEV_10           0x08
#define RF_FS1_FDEV_20           0x10
#define RF_FS1_FDEV_40           0x18
#define RF_FS1_FDEV_100          0x20
// Transceiver Standard baudrate values                         
#define RF_FS1_BAUDRATE_4800     0x00
#define RF_FS1_BAUDRATE_9600     0x01	
#define RF_FS1_BAUDRATE_19200    0x02
#define RF_FS1_BAUDRATE_38400    0x03
#define RF_FS1_BAUDRATE_76800    0x04

// AD Param 1
// Pattern recognition
#define RF_AD1_PATTERN_ON        0x80
#define RF_AD1_PATTERN_OFF       0x00
// Size of the reference pattern
#define RF_AD1_P_SIZE_8          0x00
#define RF_AD1_P_SIZE_16         0x20
#define RF_AD1_P_SIZE_24         0x40
#define RF_AD1_P_SIZE_32         0x60
// Number of tolerated errors for the pattern recognition
#define RF_AD1_P_TOL_0           0x00
#define RF_AD1_P_TOL_1           0x08
#define RF_AD1_P_TOL_2           0x10
#define RF_AD1_P_TOL_3           0x18
// Frequency of ClkOut
#define RF_AD1_CLK_FREQ_1_22_MHZ 0x00
#define RF_AD1_CLK_FREQ_2_44_MHZ 0x02
#define RF_AD1_CLK_FREQ_4_87_MHZ 0x04
#define RF_AD1_CLK_FREQ_9_75_MHZ 0x06
// IQ amplifiers
#define RF_AD1_IQA_ON            0x01
#define RF_AD1_IQA_OFF           0x00

// AD Param 2
// Inversion of the output data of the receiver
#define RF_AD2_INVERT_ON         0x40
#define RF_AD2_INVERT_OFF        0x00
// Regulation of the bandwidth of the base-band filter
#define RF_AD2_REG_BW_ON         0x00
#define RF_AD2_REG_BW_OFF        0x20
// Periodicity of regulation of the bandwidth of the base-band filter
#define RF_AD2_REG_FREQ_EACH_MIN 0x10
#define RF_AD2_REG_FREQ_START_UP 0x00
// Regulation process of the bandwidth of the base-band filter 
// according to selected bandwidth
#define RF_AD2_REG_COND_ON       0x00
#define RF_AD2_REG_COND_OFF      0x08
// Boosting process of the base-band filter according to the selected
// bandwidth
#define RF_AD2_WBB_COND_ON       0x00
#define RF_AD2_WBB_COND_OFF      0x40
// Selection of the XOSC modes
#define RF_AD2_X_SEL_11_PF       0x20
#define RF_AD2_X_SEL_15_PF       0x00

/*******************************************************************
** Timings section : all timing described here are done with the  **
**                   A&B counters cascaded                        **
*******************************************************************/
/*******************************************************************
**                     -- TX Bitrate --                           **
********************************************************************
** The bitrate values depend on the RC frequency                  **
** The following values corresponds to a 2.4576 MHz RC Freq       **
** Bitrate calculation formula                                    **
**                                                                **
**                     RC osc frequency         2 457 600         **
** CounterA&B value = ------------------ = -------------------- = 512
**                      wanted baudrate            4800           **
**                                                                **
*******************************************************************/
#define TX_BAUDRATE_GEN_4800     512
#define TX_BAUDRATE_GEN_9600     256
#define TX_BAUDRATE_GEN_19200    128
#define TX_BAUDRATE_GEN_38400    64
#define TX_BAUDRATE_GEN_76800    32

/*******************************************************************
**             -- XE1202 Recommended timings --                   **
********************************************************************
** These timings depends on the RC frequency                      **
** The following values corresponds to a 2.4576 MHz RC Freq       **
** Times calculation formula                                      **
**                                                                **
** CounterA&B value = RC * wanted time  = 2 457 600  * 2 ms = 4915**
**                                                                **
*******************************************************************/
#define TS_OS                    4915 // Quartz Osc Wake up time, 2 ms
#define TS_BBR                   492  // Receiver BB processing wake-up time, 200 us
#define TS_FS                    492  // Frequency synthesizer wake-up time, 200 us
#define TS_BB0                   8602 // Receiver RF front end wake-up time, 3.5 ms (STANDARD)
#define TS_BB2                   1229 // Receiver RF front end wake-up time, 500 us (BOOST)
#define TS_TR                    245  // Transmitter wake-up time, 100 us
#define TS_RS                    2458 // RSSI wake-up time, 1 ms

/*******************************************************************
**                                                                **
**                             20                    20           **
** Counter A&B value = RC * --------- = 2457600 * --------- = 10240*
**                           BitRate                4800          **
**                                                                **
*******************************************************************/
#define TS_FE_04_8_KB_CORR       10240
#define TS_FE_09_6_KB_CORR       5120
#define TS_FE_19_2_KB_CORR       2560
#define TS_FE_38_4_KB_CORR       1280
#define TS_FE_76_8_KB_CORR       640

/*******************************************************************
**                                                                **
**                              5                     5           **
** Counter A&B value = RC * --------- = 2457600 * --------- = 2560**
**                           BitRate                4800          **
**                                                                **
*******************************************************************/
#define TS_FE_04_8_KB_DEMOD      2560
#define TS_FE_09_6_KB_DEMOD      1280
#define TS_FE_19_2_KB_DEMOD      640
#define TS_FE_38_4_KB_DEMOD      320
#define TS_FE_76_8_KB_DEMOD      160

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
** Functions prototypes                                           **
*******************************************************************/

/*******************************************************************
** Configuration functions                                        **
*******************************************************************/

/*******************************************************************
** InitRFChip : Initializes the RF Chip registers using           **
**            pre initialized global variable                     **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitRFChip(void);

/*******************************************************************
** SetRFMode : Sets the XE1202 operating mode (Sleep, Receiver,   **
**           Transmitter)                                         **
********************************************************************
** In  : mode                                                     **
** Out : -                                                        **
*******************************************************************/
void SetRFMode(_U8 mode);

/*******************************************************************
** WriteRegister : Writes the register value at the given address **
**                  on the XE1202                                 **
********************************************************************
** In  : address, value                                           **
** Out : -                                                        **
*******************************************************************/
void WriteRegister(_U8 address, _U16 value);
 
/*******************************************************************
** ReadRegister : Reads the register value at the given address on**
**                the XE1202                                      **
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
** SendByte : Send a data of 8 bits to the transceiver LSB first  **
********************************************************************
** In  : b                                                        **
** Out : -                                                        **
*******************************************************************/
void SendByte(_U8 b);

/*******************************************************************
** ReceiveByte : Receives a data of 8 bits from the transceiver   **
**              LSB first                                         **
********************************************************************
** In  : -                                                        **
** Out : b                                                        **
*******************************************************************/
_U8 ReceiveByte(void);

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
** ReadLO : Reads the LO frequency value from  XE1202             **
********************************************************************
** In  : -                                                        **
** Out : value                                                    **
*******************************************************************/
_U16 ReadLO(void);

/*******************************************************************
** WriteLO : Writes the LO frequency value on the XE1202          **
********************************************************************
** In  : value                                                    **
** Out : -                                                        **
*******************************************************************/
void WriteLO(_U16 value);

/*******************************************************************
** InitFei : Initializes the XE1202 to enable the FEI reading     **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitFei(void);

/*******************************************************************
** ReadFei : Reads the FEI value from  XE1202                     **
********************************************************************
** In  : -                                                        **
** Out : value                                                    **
*******************************************************************/
_S16 ReadFei(void);

/*******************************************************************
** InitRssi : Initializes the XE1202 to enable the RSSI reading   **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitRssi(void);

/*******************************************************************
** ReadRssi : Reads the Rssi value from  XE1202                   **
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
** TxEventsOn : Initializes the timers and the events related to  **
**             the TX routines PA1 CntA&B                         **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void TxEventsOn(void);

/*******************************************************************
** TxEventsOff : Initializes the timers and the events related to **
**             the TX routines PA1 CntA&B                         **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void TxEventsOff(void);

/*******************************************************************
** RxEventsOn : Initializes the timers and the events related to  **
**             the RX routines PA1 CntA&B                         **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void RxEventsOn(void);

/*******************************************************************
** RxEventsOff : Initializes the timers and the events related to **
**             the RX routines PA1 CntA&B                         **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void RxEventsOff(void);

/*******************************************************************
** InvertByte : Inverts a byte. MSB -> LSB, LSB -> MSB            **
********************************************************************
** In  : b                                                        **
** Out : b                                                        **
*******************************************************************/
_U8 InvertByte(_U8 b);

#endif /* __XE1202DRIVER__ */
