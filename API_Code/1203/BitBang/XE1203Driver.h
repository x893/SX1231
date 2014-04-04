/*******************************************************************
** File        : XE1203driver.h                                   **
********************************************************************
**                                                                **
** Version     : V 1.0                                            **
**                                                                **
** Written by   : Miguel Luis & Grégoire Guye                     **
**                                                                **
** Date        : 28-03-2003                                       **
**                                                                **
** Project     : API-1203                                         **
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
** Description : XE1203F transceiver drivers implementation for   **
**               XE8000 family (BitBang)                          **
*******************************************************************/
#ifndef __XE1203DRIVER__
#define __XE1203DRIVER__

/*******************************************************************
**                      W A R N I N G                             **
********************************************************************
** - Pin Data is always used as an input for the microcontroller  **
**                                                                **
*******************************************************************/

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
**                                  *  uC     * XE1203    * PAx   **
*******************************************************************/
#define DCLK         	0x02      //*  In     *  Out      * PA1
#define DATA            0x04      //*  In     *  Out      * PA2
#define PATTERN         0x80      //*  In     *  Out      * PA3

/*******************************************************************
** Port B pins definitions                                        **
********************************************************************
**                                  *  uC     * XE1203    * PBx   **
*******************************************************************/

/*******************************************************************
** Port C pins definitions                                        **
********************************************************************
**                                  *  uC     * XE1203    * PCx   **
*******************************************************************/
#define DATAIN          0x01      //*  Out     *  In      * PC0
#define TX              0x02      //*  Out     *  In      * PC1
#define RX              0x04      //*  Out     *  In      * PC2
#define EN              0x08      //*  Out     *  In      * PC3
#define SCK             0x10      //*  Out     *  In      * PC4
#define SI              0x20      //*  Out     *  In      * PC5
#define SO              0x40      //*  In      *  Out     * PC6

/*******************************************************************
** XE1203 3 wire serial interface macros definitions              **
*******************************************************************/
#define SrtInit()        (PORTDIR = ~SO)
#define SrtSetSCK(level) (level) ? (set_bit(PORTO, SCK)) : (clear_bit(PORTO, SCK))
#define SrtSetSI(level)  (level) ? (set_bit(PORTO, SI)) : (clear_bit(PORTO, SI))
#define SrtCheckSO()     check_bit(PORTI, SO)

/*******************************************************************
** XE1203 definitions                                             **
*******************************************************************/

/*******************************************************************
** XE1203 Internal registers Address                              **
*******************************************************************/
#define REG_CONFIG               0x00
#define REG_RTPARAM1             0x01
#define REG_RTPARAM2             0x02
#define REG_FSPARAM1             0x03
#define REG_FSPARAM2             0x04
#define REG_FSPARAM3             0x05
#define REG_SWPARAM1             0x06
#define REG_SWPARAM2             0x07
#define REG_SWPARAM3             0x08
#define REG_SWPARAM4             0x09
#define REG_SWPARAM5             0x0A
#define REG_SWPARAM6             0x0B
#define REG_DATAOUT1             0x0C
#define REG_DATAOUT2             0x0D
#define REG_ADPARAM1             0x0E
#define REG_ADPARAM2             0x0F
#define REG_ADPARAM3             0x10
#define REG_ADPARAM4             0x11
#define REG_ADPARAM5             0x12
#define REG_PATTERN1             0x13
#define REG_PATTERN2             0x14
#define REG_PATTERN3             0x15
#define REG_PATTERN4             0x16

/*******************************************************************
** XE1203 default register values definition                      **
*******************************************************************/
#define DEF_CONFIG               0x00

#define DEF_RTPARAM1             0x00
#define DEF_RTPARAM2             0x00

#define DEF_FSPARAM1             0x00
#define DEF_FSPARAM2             0x00
#define DEF_FSPARAM3             0x00

#define DEF_SWPARAM1             0x00
#define DEF_SWPARAM2             0x00
#define DEF_SWPARAM3             0x00
#define DEF_SWPARAM4             0x00
#define DEF_SWPARAM5             0x00
#define DEF_SWPARAM6             0x00

#define DEF_ADPARAM1             0x00 
#define DEF_ADPARAM2             0x00
#define DEF_ADPARAM3             0x00
#define DEF_ADPARAM4             0x00
#define DEF_ADPARAM5             0x00

#define DEF_PATTERN1             0x00
#define DEF_PATTERN2             0x00
#define DEF_PATTERN3             0x00
#define DEF_PATTERN4             0x00

/*******************************************************************
** XE1203 bit control definition                                  **
*******************************************************************/
// Config
#define RF_CONFIG_MODE1          0x00
#define RF_CONFIG_MODE2          0xFF
#define RF_CONFIG_MASK           0x80

// RT Param 1
// Bit synchronizer
#define RF_RT1_BIT_SYNC_ON       0x80
#define RF_RT1_BIT_SYNC_OFF      0x00
// Barker
#define RF_RT1_BARKER_ON         0x40
#define RF_RT1_BARKER_OFF        0x00
// RSSI enable
#define RF_RT1_RSSI_ON           0x20
#define RF_RT1_RSSI_OFF          0x00
// Range of the RSSI
#define RF_RT1_RSSIR_HIGH        0x10
#define RF_RT1_RSSIR_LOW         0x00
// FEI
#define RF_RT1_FEI_ON            0x08
#define RF_RT1_FEI_OFF           0x00
// Bandwidth of the Base Band Filter
#define RF_RT1_BW_600            0x04
#define RF_RT1_BW_200            0x00
// Source of reference frequency
#define RF_RT1_OSC_EXT           0x02
#define RF_RT1_OSC_INT           0x00
// ClkOut enable
#define RF_RT1_CLKOUT_ON         0x01
#define RF_RT1_CLKOUT_OFF        0x00

// RT Param 2
// Rising and falling times in case of pre-filtering in transmitter mode
#define RF_RT2_STAIR_20          0x80
#define RF_RT2_STAIR_10          0x00
// Pre-filtering of bit stream in transmitter mode
#define RF_RT2_FILTER_ON         0x40
#define RF_RT2_FILTER_OFF        0x00
// Inhibition of the modulation in transmitter mode
#define RF_RT2_MODUL_ON          0x00
#define RF_RT2_MODUL_OFF         0x20
// IQ amplifiers
#define RF_RT2_IQAMP_ON          0x10
#define RF_RT2_IQAMP_OFF         0x00
// Mode switch (functions implemented in the API only use ChipConfig)
#define RF_RT2_SWITCH_PAD        0x08
#define RF_RT2_SWITCH_REG        0x00
// Pattern recognition
#define RF_RT2_PATTERN_ON        0x04
#define RF_RT2_PATTERN_OFF       0x00
// Frequency band
#define RF_RT2_BAND_915          0x03
#define RF_RT2_BAND_868          0x02
#define RF_RT2_BAND_433          0x01

// FS Param 1
// Transceiver Frequency deviation
#define RF_FS1_FDEV_05           0x05
#define RF_FS1_FDEV_10           0x0A
#define RF_FS1_FDEV_20           0x14
#define RF_FS1_FDEV_40           0x28
#define RF_FS1_FDEV_55           0x37
#define RF_FS1_FDEV_80           0x50
#define RF_FS1_FDEV_100          0x64
#define RF_FS1_FDEV_160          0xA0
#define RF_FS1_FDEV_200          0xC8

// FS Param 2
// Change OSR
#define RF_FS2_CHANGE_OSR_ON     0x80
#define RF_FS2_CHANGE_OSR_OFF    0x00
// Transceiver Standard baudrate values
#define RF_FS2_BAUDRATE_1200     0x7E
#define RF_FS2_BAUDRATE_2400     0x3E
#define RF_FS2_BAUDRATE_4800     0x1F
#define RF_FS2_BAUDRATE_9600     0x0F
#define RF_FS2_BAUDRATE_19200    0x07
#define RF_FS2_BAUDRATE_38400    0x03
#define RF_FS2_BAUDRATE_76800    0x01
#define RF_FS2_BAUDRATE_153600   0x00

// FS Param 3
#define RF_FS3_NORMAL            0x00
#define RF_FS3_KONNEX            0x1D

// SW Param 1&4
// Chip mode configuration
#define RF_SW_SLEEP              0x00
#define RF_SW_STANDBY            0x40
#define RF_SW_RECEIVER           0x80
#define RF_SW_TRANSMITTER        0xC0
// Transmitter output power configuration
#define RF_SW_POWER_15           0x30
#define RF_SW_POWER_10           0x20
#define RF_SW_POWER_5            0x10
#define RF_SW_POWER_0            0x00
// Receiver mode configuration
#define RF_SW_RMODE_MODE_B       0x08
#define RF_SW_RMODE_MODE_A       0x00

// AD Param 1
// Size of the reference pattern
#define RF_AD1_P_SIZE_8          0x00
#define RF_AD1_P_SIZE_16         0x40
#define RF_AD1_P_SIZE_24         0x80
#define RF_AD1_P_SIZE_32         0xC0
// Number of tolerated errors for the pattern recognition
#define RF_AD1_P_TOL_0           0x00
#define RF_AD1_P_TOL_1           0x10
#define RF_AD1_P_TOL_2           0x20
#define RF_AD1_P_TOL_3           0x30
// Frequency of ClkOut
#define RF_AD1_CLK_FREQ_1_22_MHZ 0x00
#define RF_AD1_CLK_FREQ_2_44_MHZ 0x04
#define RF_AD1_CLK_FREQ_4_87_MHZ 0x08
#define RF_AD1_CLK_FREQ_9_75_MHZ 0x0C
// Inversion of the output data of the receiver
#define RF_AD1_INVERT_ON         0x02
#define RF_AD1_INVERT_OFF        0x00
// Regulation of the bandwidth of the base-band filter
#define RF_AD1_REG_BW_ON         0x01
#define RF_AD1_REG_BW_OFF        0x00

// AD Param 2
// Periodicity of regulation of the bandwidth of the base-band filter
#define RF_AD2_REG_FREQ_EACH_MIN 0x80
#define RF_AD2_REG_FREQ_START_UP 0x00
// Regulation process of the bandwidth of the base-band filter 
// according to selected bandwidth
#define RF_AD2_REG_COND_ON       0x00
#define RF_AD2_REG_COND_OFF      0x40
// Selection of the XOSC modes
#define RF_AD2_X_SEL_11_PF       0x20
#define RF_AD2_X_SEL_07_PF       0x00
// Select the resistor value put between TKA and TKB
#define RF_AD2_RES_X_OSC_3800    0x00
#define RF_AD2_RES_X_OSC_2_55    0x02
#define RF_AD2_RES_X_OSC_4_65    0x04
#define RF_AD2_RES_X_OSC_1_78    0x06
#define RF_AD2_RES_X_OSC_8_79    0x08
#define RF_AD2_RES_X_OSC_2_07    0x0A
#define RF_AD2_RES_X_OSC_3_22    0x0C
#define RF_AD2_RES_X_OSC_1_56    0x0E
#define RF_AD2_RES_X_OSC_16_55   0x10
#define RF_AD2_RES_X_OSC_2_26    0x12
#define RF_AD2_RES_X_OSC_3_79    0x14
#define RF_AD2_RES_X_OSC_1_66    0x16
#define RF_AD2_RES_X_OSC_6_04    0x18
#define RF_AD2_RES_X_OSC_1_91    0x1A
#define RF_AD2_RES_X_OSC_2_81    0x1C
#define RF_AD2_RES_X_OSC_1_48    0x1E
// Use the Konnex standard when using the FEI
#define RF_AD2_KONNEX_ON         0x01
#define RF_AD2_KONNEX_OFF        0x00

// AD Param 3
// Make the sync and acquisition threshold programmable, and allow
// the change of the Barker code
#define RF_AD3_CHG_THRES_ON      0x80
#define RF_AD3_CHG_THRES_OFF     0x00

// AD Param 4
#define RF_AD4_DATA_BIDIR_ON     0x00
#define RF_AD4_DATA_BIDIR_OFF    0x80

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
#define TX_BAUDRATE_GEN_1200     2048
#define TX_BAUDRATE_GEN_2400     1024
#define TX_BAUDRATE_GEN_4800     512
#define TX_BAUDRATE_GEN_9600     256
#define TX_BAUDRATE_GEN_19200    128
#define TX_BAUDRATE_GEN_38400    64
#define TX_BAUDRATE_GEN_76800    32
#define TX_BAUDRATE_GEN_153600   16

/*******************************************************************
**             -- XE1203 Recommended timings --                   **
********************************************************************
** These timings depend on the uC RC frequency                    **
** The following values corresponds to a 2.4576 MHz RC Freq       **
** Times calculation formula                                      **
**                                                                **
** CounterA&B value = RC * wanted time  = 2 457 600  * 2 ms = 4915**
**                                                                **
*******************************************************************/
#define TS_OS                    4915 // Quartz Osc Wake up time, 2 ms
//#define TS_TR                    614  // Transmitter wake-up time, 250 us
#define TS_TR                    2458  // Transmitter wake-up time, 1 ms
#define TS_RE                    4424 // Receiver Baseband wake-up time, 1.8 ms
#define TS_RSSI                  2458 // RSSI wake-up time, 1 ms
#define TS_SYNC_AQ               1229 // Receiver RF front end wake-up time, 500 us

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
#define TS_FEI_PREDEF(RegBr, RegOsr) (_U16)(((_F32)((RegBr & 0x7F) + 1) * (_F32)4915200) / (_F32)152340)   // FEI wake-up time

/*******************************************************************
**                                                                **
**                          4875000                               **
** BitRate = ------------------------------------                 **
**            ((RegBr & 0x7F) + 1) * (RegOsr + 1)                 **
**                        2               2 * ((RegBr & 0x7F) + 1) * (RegOsr + 1)
** CounterA&B value = --------- * RC = ------------------------------------------- * RC
**                     BitRate                           4875000  **
**                                                                **
*******************************************************************/
#define TS_FEI_USER(RegBr, RegOsr)   (_U16)((((_F32)((RegBr & 0x7F) + 1) * (_F32)(RegOsr + 1)) * (_F32)4915200) / (_F32)4875000) // FEI wake-up time

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
** InitRFChip : This routine initializes the RFChip registers     **
**              Using Pre Initialized variable                    **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/ 
void InitRFChip (void);

/*******************************************************************
** SetRFMode : Sets the XE1203 operating mode (Sleep, Receiver,   **
**           Transmitter)                                         **
********************************************************************
** In  : mode                                                     **
** Out : -                                                        **
*******************************************************************/
void SetRFMode(_U8 mode);

/*******************************************************************
** WriteRegister : Writes the register value at the given address **
**                  on the XE1203                                 **
********************************************************************
** In  : address, value                                           **
** Out : -                                                        **
*******************************************************************/
void WriteRegister(_U8 address, _U16 value);
 
/*******************************************************************
** ReadRegister : Reads the register value at the given address on**
**                the XE1203                                      **
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
** ReadLO : Reads the LO frequency value from  XE1203             **
********************************************************************
** In  : -                                                        **
** Out : value                                                    **
*******************************************************************/
_U16 ReadLO(void);

/*******************************************************************
** WriteLO : Writes the LO frequency value on the XE1203          **
********************************************************************
** In  : value                                                    **
** Out : -                                                        **
*******************************************************************/
void WriteLO(_U16 value);

/*******************************************************************
** InitFei : Initializes the XE1203 to enable the FEI reading     **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitFei(void);

/*******************************************************************
** ReadFei : Reads the FEI value from  XE1203                     **
********************************************************************
** In  : -                                                        **
** Out : value                                                    **
*******************************************************************/
_S16 ReadFei(void);

/*******************************************************************
** InitRssi : Initializes the XE1203 to enable the RSSI reading   **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitRssi(void);

/*******************************************************************
** ReadRssi : Reads the Rssi value from  XE1203                   **
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

#endif /* __XE1203DRIVER__ */

