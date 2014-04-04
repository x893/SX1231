/*******************************************************************
** File        : SX1223driver.h                                   **
********************************************************************
**                                                                **
** Version     : V 1.0                                            **
**                                                                **
** Written by   : Chaouki ROUAISSIA                               **
**                                                                **
** Date        : 29-05-2006                                       **
**                                                                **
** Project     : API-1223                                         **
**                                                                **
********************************************************************
**                                                                **
** Changes     : V 2.3 / CRo - 06-06-2006                         **
**               - No change                                      **
**                                                                **
** Changes     : V 2.4 / CRo - 09-01-2007                         **
**               - No change                                      **
**                                                                **
********************************************************************
** Description : SX1223 transmitter drivers implementation for the**
**               XE8000 family (BitBang)                          **
*******************************************************************/
#ifndef __SX1223DRIVER__
#define __SX1223DRIVER__

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
#define RF_TX_DONE           0x08
#define RF_ERROR             0x10


/*******************************************************************
** RF function return codes                                       **
*******************************************************************/
#define OK                   0x00
#define ERROR                0x01

/*******************************************************************
** I/O Ports Definitions                                          **
*******************************************************************/
#if defined(_XE88LC01A_) || defined(_XE88LC05A_)
	#define PORTO              RegPCOut
	#define PORTI              RegPCIn
	#define PORTDIR            RegPCDir
#endif
#if defined(_XE88LC02_) || defined(_XE88LC02_4KI_) || defined(_XE88LC02_8KI_)
	#define PORTO              RegPD1Out
	#define PORTI              RegPD1In
	#define PORTDIR            RegPD1Dir	
#endif
#if defined(_XE88LC06A_) || defined(_XE88LC07A_) 
	#define PORTO              RegPDOut
	#define PORTI              RegPDIn
	#define PORTDIR            RegPDDir
#endif
/*******************************************************************
** Port A pins definitions                                        **
********************************************************************
**                                  *  uC     * SX1223    * PAx   **
*******************************************************************/
#define DCLK            0x02      //*  In     *  Out      * PA1  
#define LD              0x04      //*  In     *  Out      * PA2

/*******************************************************************
** Port B pins definitions                                        **
********************************************************************
**                                  *  uC     * SX1223    * PBx   **
*******************************************************************/

/*******************************************************************
** Port C pins definitions                                        **
********************************************************************
**                                  *  uC     * SX1223    * PDx   **
*******************************************************************/
#define DATAIN          0x01      //*  Out     *  In      * PD0
#define EN              0x08      //*  Out     *  In      * PC3
#define SCK             0x10      //*  Out     *  In      * PC4
#define SI              0x20      //*  Out     *  In      * PC5
#define SO              0x40      //*  In      *  Out     * PC6


/*******************************************************************
** SX1223 3 wire serial interface macros definitions              **
*******************************************************************/
#define SrtInit()        (PORTDIR = SI | SCK | EN | DATAIN)
#define SrtSetSCK(level) (level) ? (set_bit(PORTO, SCK)) : (clear_bit(PORTO, SCK))
#define SrtSetSI(level)  (level) ? (set_bit(PORTO, SI)) : (clear_bit(PORTO, SI))
#define SrtCheckSO()     check_bit(PORTI, SO)

/*******************************************************************
** SX1223 definitions                                             **
*******************************************************************/

/*******************************************************************
** SX1223 Internal registers Address                              **
*******************************************************************/
#define REG_CONFIG0              0x00
#define REG_CONFIG1              0x01
#define REG_CONFIG2              0x02
#define REG_CONFIG3              0x03
#define REG_CONFIG4              0x04
#define REG_CONFIG5              0x05
#define REG_A0                   0x06
#define REG_N0MSB                0x07
#define REG_N0LSB                0x08
#define REG_M0MSB                0x09
#define REG_M0LSB                0x0A
#define REG_A1                   0x0B
#define REG_N1MSB                0x0C
#define REG_N1LSB                0x0D
#define REG_M1MSB                0x0E
#define REG_M1LSB                0x0F
#define REG_TESTOPT0             0x10
#define REG_TESTOPT1             0x11


/*******************************************************************
** SX1223 initialisation register values definition               **
*******************************************************************/
#define DEF_CONFIG0              0x00             // 0x3F
#define DEF_CONFIG1              0x00             // 0x7D
#define DEF_CONFIG2              0x00             // 0x02
#define DEF_CONFIG3              0x00             // 0x4B
#define DEF_CONFIG4              0x00             // 0x0C
#define DEF_CONFIG5              0x00             // 0x1A
#define DEF_A0                   0x00             // 0x02
#define DEF_N0MSB                0x00             // 0x00
#define DEF_N0LSB                0x00             // 0x76
#define DEF_M0MSB                0x00             // 0x00
#define DEF_M0LSB                0x00             // 0x20
#define DEF_A1                   0x00             // 0x02
#define DEF_N1MSB                0x00             // 0x00
#define DEF_N1LSB                0x00             // 0x76
#define DEF_M1MSB                0x00             // 0x00
#define DEF_M1LSB                0x00             // 0x20
#define DEF_TESTOPT0             0x00             // 0x20
#define DEF_TESTOPT1             0x00             // 0xDD

/*******************************************************************
** SX1223 bit control definition                                  **
*******************************************************************/
// Config0

// Operating Modes
#define RF_CFG0_MODE_SLEEP             0x00
#define RF_CFG0_MODE_STANDBY           0x20
#define RF_CFG0_MODE_SYNTHESIZER       0x40
#define RF_CFG0_MODE_TRANSMIT          0x60

// Output Power in dBm
#define RF_CFG0_POWER_MINUS11          0x00
#define RF_CFG0_POWER_MINUS8           0x04
#define RF_CFG0_POWER_MINUS5           0x08
#define RF_CFG0_POWER_MINUS2           0x0C
#define RF_CFG0_POWER_PLUS1            0x10
#define RF_CFG0_POWER_PLUS4            0x14
#define RF_CFG0_POWER_PLUS7            0x18
#define RF_CFG0_POWER_PLUS10           0x1C

// Clock Output
#define RF_CFG0_CLKOUT_OFF             0x00
#define RF_CFG0_CLKOUT_ON              0x02

// Data Clock Output
#define RF_CFG0_DCLK_OFF               0x00
#define RF_CFG0_DCLK_ON                0x01


// Config1

// Modulation Type
#define RF_CFG1_MODULATION_MW1         0x00
#define RF_CFG1_MODULATION_MW2         0x40
#define RF_CFG1_MODULATION_MW3         0x80

// Frequency Band
#define RF_CFG1_BAND_450               0x00
#define RF_CFG1_BAND_900               0x20

//Crystal oscillator internal capacitors
#define RF_CFG1_XCOINTCAPS_OFF         0x00
#define RF_CFG1_XCOINTCAPS_ON          0x10

//LDO regulator (VDD range selection)
#define RF_CFG1_LDO_OFF                0x00
#define RF_CFG1_LDO_ON                 0x08

// Open Loop Opamp
#define RF_CFG1_OLOPAMP_OFF            0x00
#define RF_CFG1_OLOPAMP_ON             0x04

// Lock Detect controlled PA
#define RF_CFG1_PALDC_OFF              0x00
#define RF_CFG1_PALDC_ON               0x02

// Lock Detector Output
#define RF_CFG1_LD_OFF                 0x00
#define RF_CFG1_LD_ON                  0x01


// Config2

// PA Start-up Control
#define RF_CFG2_PACAP_INT              0x00
#define RF_CFG2_PACAP_EXT              0x20

// Crystal Oscillator Quick Start
#define RF_CFG2_XCOQUICKSTART_OFF      0x00
#define RF_CFG2_XCOQUICKSTART_ON       0x10

// Crystal Oscillator Bias Current
#define RF_CFG2_XCOHIGHCURRENT_OFF     0x00
#define RF_CFG2_XCOHIGHCURRENT_ON      0x08

// PLL Charge Pump Current
#define RF_CFG2_CPHIGHCURRENT_OFF      0x00
#define RF_CFG2_CPHIGHCURRENT_ON       0x04

// VCO Center Frequency
#define RF_CFG2_VCOFREQ_850            0x00
#define RF_CFG2_VCOFREQ_868            0x01
#define RF_CFG2_VCOFREQ_915            0x02
#define RF_CFG2_VCOFREQ_950            0x03


// Config3

// Modulator current setting for frequency deviation
#define RF_CFG3_MODI_0                 0x00
#define RF_CFG3_MODI_1                 0x08
#define RF_CFG3_MODI_2                 0x10
#define RF_CFG3_MODI_3                 0x18
#define RF_CFG3_MODI_4                 0x20
#define RF_CFG3_MODI_5                 0x28
#define RF_CFG3_MODI_6                 0x30
#define RF_CFG3_MODI_7                 0x38
#define RF_CFG3_MODI_8                 0x40
#define RF_CFG3_MODI_9                 0x48
#define RF_CFG3_MODI_10                0x50
#define RF_CFG3_MODI_11                0x58
#define RF_CFG3_MODI_12                0x60
#define RF_CFG3_MODI_13                0x68
#define RF_CFG3_MODI_14                0x70
#define RF_CFG3_MODI_15                0x78
#define RF_CFG3_MODI_16                0x80
#define RF_CFG3_MODI_17                0x88
#define RF_CFG3_MODI_18                0x90
#define RF_CFG3_MODI_19                0x98
#define RF_CFG3_MODI_20                0xA0
#define RF_CFG3_MODI_21                0xA8
#define RF_CFG3_MODI_22                0xB0
#define RF_CFG3_MODI_23                0xB8
#define RF_CFG3_MODI_24                0xC0
#define RF_CFG3_MODI_25                0xC8
#define RF_CFG3_MODI_26                0xD0
#define RF_CFG3_MODI_27                0xD8
#define RF_CFG3_MODI_28                0xE0
#define RF_CFG3_MODI_29                0xE8
#define RF_CFG3_MODI_30                0xF0
#define RF_CFG3_MODI_31                0xF8

// Modulator attenuator setting for frequency deviation 
#define RF_CFG3_MODA_0                 0x00
#define RF_CFG3_MODA_1                 0x01
#define RF_CFG3_MODA_2                 0x02
#define RF_CFG3_MODA_3                 0x03
#define RF_CFG3_MODA_4                 0x04
#define RF_CFG3_MODA_5                 0x05
#define RF_CFG3_MODA_6                 0x06
#define RF_CFG3_MODA_7                 0x07


// Config4

// Bitrate setting   
#define RF_CFG4_BRN_0                  0x00
#define RF_CFG4_BRN_1                  0x08
#define RF_CFG4_BRN_2                  0x10
#define RF_CFG4_BRN_3                  0x18
#define RF_CFG4_BRN_4                  0x20
#define RF_CFG4_BRN_5                  0x28
#define RF_CFG4_BRN_6                  0x30
#define RF_CFG4_BRN_7                  0x38

// Modulator filter setting
#define RF_CFG4_MODF_0                 0x00
#define RF_CFG4_MODF_1                 0x01
#define RF_CFG4_MODF_2                 0x02
#define RF_CFG4_MODF_3                 0x03
#define RF_CFG4_MODF_4                 0x04
#define RF_CFG4_MODF_5                 0x05
#define RF_CFG4_MODF_6                 0x06
#define RF_CFG4_MODF_7                 0x07

// Config5

// Modulator and bitrate clock setting
#define RF_CFG5_REFCLKK_VALUE          0x04


// A0
#define RF_A0_VALUE                    0x02

// N0 MSB
#define RF_N0MSB_VALUE                 0x00
// NO LSB
#define RF_N0LSB_VALUE                 0x76

// M0 MSB
#define RF_M0MSB_VALUE                 0x00
// M0 LSB
#define RF_M0LSB_VALUE                 0x20


// A1
#define RF_A1_VALUE                    0x02

// N1 MSB
#define RF_N1MSB_VALUE                 0x00
// N1 LSB
#define RF_N1LSB_VALUE                 0x76

// M1 MSB
#define RF_M1MSB_VALUE                 0x00
// M1 LSB
#define RF_M1LSB_VALUE                 0x20


// TESTOPT0

// VCO Bias
#define RF_TESTOPT0_VCOBIAS_850        0xE0
#define RF_TESTOPT0_VCOBIAS_868        0xA0
#define RF_TESTOPT0_VCOBIAS_915        0x60
#define RF_TESTOPT0_VCOBIAS_950        0x00

// Prescaler type
#define RF_TESTOPT0_PRE_PHASESEL       0x00
#define RF_TESTOPT0_PRE_PULSESWAL      0x10

// VCO bypass
#define RF_TESTOPT0_VCOBYPASS_OFF      0x00
#define RF_TESTOPT0_VCOBYPASS_ON       0x08


// TESTOPT1

// PA bias current source
#define RF_TESTOPT1_PABIASSOURCE_EXT1  0x00
#define RF_TESTOPT1_PABIASSOURCE_EXT2  0x40
#define RF_TESTOPT1_PABIASSOURCE_INT1  0x80
#define RF_TESTOPT1_PABIASSOURCE_INT2  0xC0

// PA bias current level
#define RF_TESTOPT1_PABIASLEVEL_0      0x00
#define RF_TESTOPT1_PABIASLEVEL_1      0x10
#define RF_TESTOPT1_PABIASLEVEL_2      0x20
#define RF_TESTOPT1_PABIASLEVEL_3      0x30

// PA Buffer bias current source
#define RF_TESTOPT1_PABBIASSOURCE_EXT1 0x00
#define RF_TESTOPT1_PABBIASSOURCE_EXT2 0x04
#define RF_TESTOPT1_PABBIASSOURCE_INT1 0x08
#define RF_TESTOPT1_PABBIASSOURCE_INT2 0x0C

// PA Buffer bias current level
#define RF_TESTOPT1_PABBIASLEVEL_0     0x00
#define RF_TESTOPT1_PABBIASLEVEL_1     0x01
#define RF_TESTOPT1_PABBIASLEVEL_2     0x02
#define RF_TESTOPT1_PABBIASLEVEL_3     0x03


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
**             -- SX1223 Recommended timings --                   **
********************************************************************
** These timings depend on the uC RC frequency                    **
** The following values corresponds to a 2.4576 MHz RC Freq       **
** Times calculation formula                                      **
**                                                                **
** CounterA&B value = RC * wanted time  = 2 457 600  * 2 ms = 4915**
**                                                                **
*******************************************************************/
#define TS_OS                    4915  // Max Quartz Osc Wake up time, 2 ms
#define TS_FS                    4915  // Max Freq synthesizer wake-up time, 2 ms
#define TS_TR                    1229  // Max Transmitter wake-up time, 500 us


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
** SetRFMode : Sets the SX1223 operating mode (Sleep, Standby,    **
**           Synthesizer, Transmitter)                            **
********************************************************************
** In  : mode                                                     **
** Out : -                                                        **
*******************************************************************/
void SetRFMode(_U8 mode);

/*******************************************************************
** WriteRegister : Writes the register value at the given address **
**                  on the SX1223                                 **
********************************************************************
** In  : address, value                                           **
** Out : -                                                        **
*******************************************************************/
void WriteRegister(_U8 address, _U16 value);
 
/*******************************************************************
** ReadRegister : Reads the register value at the given address on**
**                the SX1223                                      **
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
** SendByte : Send a data of 8 bits to the transceiver LSB first  **
********************************************************************
** In  : b                                                        **
** Out : -                                                        **
*******************************************************************/
void SendByte(_U8 b);


/*******************************************************************
** Transceiver specific functions                                 **
*******************************************************************/

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
** InvertByte : Inverts a byte. MSB -> LSB, LSB -> MSB            **
********************************************************************
** In  : b                                                        **
** Out : b                                                        **
*******************************************************************/
_U8 InvertByte(_U8 b);

#endif /* __SX1223DRIVER__ */

