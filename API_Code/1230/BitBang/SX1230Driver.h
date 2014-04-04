/*******************************************************************
** File        : SX1230driver.h                                   **
********************************************************************
**                                                                **
** Version     : V 1.0                                            **
**                                                                **
** Written by  : Chaouki ROUAISSIA                                **
**                                                                **
** Date        : 03-08-2009                                       **
**                                                                **
** Project     : API-1230                                         **
**                                                                **
********************************************************************
**                                                                **
** Changes     :                                                  **
**                                                                **
********************************************************************
** Description : SX1230 transmitter drivers Implementation for the**
**               XE8000 family products (MCU mode)						**
*******************************************************************/
#ifndef __SX1230DRIVER__
#define __SX1230DRIVER__

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

/*******************************************************************
** RF State machine                                               **
*******************************************************************/
#define RF_STOP              0x01
#define RF_BUSY              0x02
#define RF_TX_DONE           0x08
#define RF_ERROR             0x10
#define RF_TIMEOUT           0x20

/*******************************************************************
** RF function return codes                                       **
*******************************************************************/
#define OK                   0x00
#define ERROR                0x01
#define TX_TIMEOUT           0x04
#define TX_RUNNING           0x05

/*******************************************************************
** I/O Ports Definitions                                          **
*******************************************************************/
#if defined(_XE88LC01A_) || defined(_XE88LC05A_)
	#define PORTO              RegPCOut
	#define PORTI              RegPCIn
	#define PORTDIR            RegPCDir
   #define PORTP              RegPCPullup

#endif
#if defined(_XE88LC02_) || defined(_XE88LC02_4KI_) || defined(_XE88LC02_8KI_)
	#define PORTO              RegPD1Out
	#define PORTI              RegPD1In
	#define PORTDIR            RegPD1Dir
   #define PORTP              RegPD1Pullup
	
#endif
#if defined(_XE88LC06A_) || defined(_XE88LC07A_) 
	#define PORTO              RegPDOut
	#define PORTI              RegPDIn
	#define PORTDIR            RegPDDir
   #define PORTP              RegPDPullup

#endif

/*******************************************************************
** Port A pins definitions                                        **
********************************************************************
**                                  *  uC     * SX1230    * PAx   **
*******************************************************************/
#define DCLK            0x02      //*  In     *  Out      * PA1 // DCLK
#define EOL             0x08      //*  In     *  Out      * PA3 // EOL (PB0)

/*******************************************************************
** Port B pins definitions                                        **
********************************************************************
**                                  *  uC     * SX1230    * PBx   **
*******************************************************************/

/*******************************************************************
** Port C pins definitions                                        **
********************************************************************
**                                  *  uC     * SX1230    * PCx   **
*******************************************************************/
#define DATA            0x01      //*  Out    *  In       * PC0 // DATA 
#define NSS             0x08      //*  Out    *  In       * PC3
#define SCK             0x10      //*  Out    *  In       * PC4
#define MOSI            0x20      //*  Out    *  In       * PC5
#define MISO            0x40      //*  In     *  Out      * PC6

/*******************************************************************
** SX1230 SPI Macros definitions                                  **
*******************************************************************/
#define SPIInit()           (PORTDIR = (PORTDIR | SCK | NSS | MOSI) & (~MISO))
#define SPIClock(level)     ((level) ? (PORTO |= SCK) : (PORTO &= ~SCK))
#define SPIMosi(level)      ((level) ? (PORTO |= MOSI) : (PORTO &= ~MOSI))
#define SPINssConfig(level) ((level) ? (PORTO |= NSS) : (PORTO &= ~NSS))
#define SPIMisoTest()       (PORTI & MISO)

/*******************************************************************
** SX1230 definitions                                             **
*******************************************************************/

/*******************************************************************
** SX1230 Internal registers Address                              **
*******************************************************************/
#define REG_MODE                         0x00
#define REG_BRMSB                        0x01
#define REG_BRLSB                        0x02
#define REG_FDEVMSB                      0x03
#define REG_FDEVLSB                      0x04
#define REG_RFFREQMSB                    0x05
#define REG_RFFREQMID                    0x06
#define REG_RFFREQLSB                    0x07
#define REG_PACTRL                       0x08
#define REG_PAFSKRAMP                    0x09
#define REG_PLLCTRL                      0x0A
#define REG_VCOCTRL1                     0x0B
#define REG_VCOCTRL2                     0x0C
#define REG_VCOCTRL3                     0x0D
#define REG_VCOCTRL4                     0x0E
#define REG_CLOCKCTRL                    0x0F
#define REG_EEPROM                       0x10  // Not used in MCU mode but kept for simplicity
#define REG_CLOCKSEL                     0x11
#define REG_EOLCTRL                      0x12
#define REG_PAOCPCTRL                    0x13  

/*******************************************************************
** SX1230 initialisation register values definition               **
*******************************************************************/
#define DEF_MODE                         0x00
#define DEF_BRMSB                        0x00
#define DEF_BRLSB                        0x00
#define DEF_FDEVMSB                      0x00
#define DEF_FDEVLSB                      0x00
#define DEF_RFFREQMSB                    0x00
#define DEF_RFFREQMID                    0x00
#define DEF_RFFREQLSB                    0x00
#define DEF_PACTRL                       0x00
#define DEF_PAFSKRAMP                    0x00
#define DEF_PLLCTRL                      0x00
//VCOCTRL1 have no default value                     
//VCOCTRL2 have no default value                     
//VCOCTRL3 have no default value                    
//VCOCTRL4 have no default value                    
#define DEF_CLOCKCTRL                    0x00
#define DEF_EEPROM                       0x10  // Not used in MCU mode but kept for simplicity
#define DEF_CLOCKSEL                     0x00
#define DEF_EOLCTRL                      0x00
#define DEF_PAOCPCTRL                    0x00  

/*******************************************************************
** SX1230 bit control definition                                  **
*******************************************************************/
// MC Param 1
// Chip operating mode
#define RF_SLEEP                         0x00
#define RF_STANDBY                       0x10  // Default
#define RF_SYNTHESIZER                   0x20
#define RF_TRANSMITTER                   0x30

// Modulation scheme
#define RF_MODUL_FSK                     0x00  // Default
#define RF_MODUL_OOK                     0x04

// Data shaping
#define RF_SHAPING_OFF                   0x00  // Default
#define RF_SHAPING_FSK_BT1               0x01
#define RF_SHAPING_FSK_BT05              0x02
#define RF_SHAPING_FSK_BT03              0x03
#define RF_SHAPING_OOK_BR                0x01
#define RF_SHAPING_OOK_2BR               0x02

// Bitrate (bit/sec)  
#define RF_BITRATE_MSB_1200              0x68
#define RF_BITRATE_LSB_1200              0x2B
#define RF_BITRATE_MSB_2400              0x34
#define RF_BITRATE_LSB_2400              0x15
#define RF_BITRATE_MSB_4800              0x1A  // Default
#define RF_BITRATE_LSB_4800              0x0B  // Default
#define RF_BITRATE_MSB_9600              0x0D
#define RF_BITRATE_LSB_9600              0x05
#define RF_BITRATE_MSB_19200             0x06
#define RF_BITRATE_LSB_19200             0x83
#define RF_BITRATE_MSB_38400             0x03
#define RF_BITRATE_LSB_38400             0x41
#define RF_BITRATE_MSB_76800             0x01
#define RF_BITRATE_LSB_76800             0xA1
#define RF_BITRATE_MSB_153600            0x00
#define RF_BITRATE_LSB_153600            0xD0
#define RF_BITRATE_MSB_57600             0x02
#define RF_BITRATE_LSB_57600             0x2C
#define RF_BITRATE_MSB_115200            0x01
#define RF_BITRATE_LSB_115200            0x16
#define RF_BITRATE_MSB_12500             0x0A
#define RF_BITRATE_LSB_12500             0x00
#define RF_BITRATE_MSB_25000             0x05
#define RF_BITRATE_LSB_25000             0x00
#define RF_BITRATE_MSB_50000             0x02
#define RF_BITRATE_LSB_50000             0x80
#define RF_BITRATE_MSB_100000            0x01
#define RF_BITRATE_LSB_100000            0x40
#define RF_BITRATE_MSB_150000            0x00
#define RF_BITRATE_LSB_150000            0xD5
#define RF_BITRATE_MSB_200000            0x00
#define RF_BITRATE_LSB_200000            0xA0
#define RF_BITRATE_MSB_250000            0x00
#define RF_BITRATE_LSB_250000            0x80
#define RF_BITRATE_MSB_300000            0x00
#define RF_BITRATE_LSB_300000            0x6B
#define RF_BITRATE_MSB_32768             0x03
#define RF_BITRATE_LSB_32768             0xD1

// Frequency deviation (kHz)
#define RF_FDEV_MSB_2000                 0x00
#define RF_FDEV_LSB_2000                 0x21
#define RF_FDEV_MSB_5000                 0x00  // Default
#define RF_FDEV_LSB_5000                 0x52  // Default
#define RF_FDEV_MSB_10000                0x00
#define RF_FDEV_LSB_10000                0xA4
#define RF_FDEV_MSB_15000                0x00
#define RF_FDEV_LSB_15000                0xF6
#define RF_FDEV_MSB_20000                0x01
#define RF_FDEV_LSB_20000                0x48
#define RF_FDEV_MSB_25000                0x01
#define RF_FDEV_LSB_25000                0x9A
#define RF_FDEV_MSB_30000                0x01
#define RF_FDEV_LSB_30000                0xEC
#define RF_FDEV_MSB_35000                0x02
#define RF_FDEV_LSB_35000                0x3D
#define RF_FDEV_MSB_40000                0x02
#define RF_FDEV_LSB_40000                0x8F
#define RF_FDEV_MSB_45000                0x02
#define RF_FDEV_LSB_45000                0xE1
#define RF_FDEV_MSB_50000                0x03
#define RF_FDEV_LSB_50000                0x33
#define RF_FDEV_MSB_55000                0x03
#define RF_FDEV_LSB_55000                0x85
#define RF_FDEV_MSB_60000                0x03
#define RF_FDEV_LSB_60000                0xD7
#define RF_FDEV_MSB_65000                0x04
#define RF_FDEV_LSB_65000                0x29
#define RF_FDEV_MSB_70000                0x04
#define RF_FDEV_LSB_70000                0x7B
#define RF_FDEV_MSB_75000                0x04
#define RF_FDEV_LSB_75000                0xCD
#define RF_FDEV_MSB_80000                0x05
#define RF_FDEV_LSB_80000                0x1F
#define RF_FDEV_MSB_85000                0x05
#define RF_FDEV_LSB_85000                0x71
#define RF_FDEV_MSB_90000                0x05
#define RF_FDEV_LSB_90000                0xC3
#define RF_FDEV_MSB_95000                0x06
#define RF_FDEV_LSB_95000                0x14
#define RF_FDEV_MSB_100000               0x06
#define RF_FDEV_LSB_100000               0x66
#define RF_FDEV_MSB_110000               0x07
#define RF_FDEV_LSB_110000               0x0A
#define RF_FDEV_MSB_120000               0x07
#define RF_FDEV_LSB_120000               0xAE
#define RF_FDEV_MSB_130000               0x08
#define RF_FDEV_LSB_130000               0x52
#define RF_FDEV_MSB_140000               0x08
#define RF_FDEV_LSB_140000               0xF6
#define RF_FDEV_MSB_150000               0x09
#define RF_FDEV_LSB_150000               0x9A
#define RF_FDEV_MSB_160000               0x0A
#define RF_FDEV_LSB_160000               0x3D
#define RF_FDEV_MSB_170000               0x0A
#define RF_FDEV_LSB_170000               0xE1
#define RF_FDEV_MSB_180000               0x0B
#define RF_FDEV_LSB_180000               0x85
#define RF_FDEV_MSB_190000               0x0C
#define RF_FDEV_LSB_190000               0x29
#define RF_FDEV_MSB_200000               0x0C
#define RF_FDEV_LSB_200000               0xCD
#define RF_FDEV_MSB_210000               0x0D
#define RF_FDEV_LSB_210000               0x71
#define RF_FDEV_MSB_220000               0x0E
#define RF_FDEV_LSB_220000               0x14
#define RF_FDEV_MSB_230000               0x0E
#define RF_FDEV_LSB_230000               0xB8
#define RF_FDEV_MSB_240000               0x0F
#define RF_FDEV_LSB_240000               0x5C
#define RF_FDEV_MSB_250000               0x10
#define RF_FDEV_LSB_250000               0x00
#define RF_FDEV_MSB_260000               0x10
#define RF_FDEV_LSB_260000               0xA4
#define RF_FDEV_MSB_270000               0x11
#define RF_FDEV_LSB_270000               0x48
#define RF_FDEV_MSB_280000               0x11
#define RF_FDEV_LSB_280000               0xEC
#define RF_FDEV_MSB_290000               0x12
#define RF_FDEV_LSB_290000               0x8F
#define RF_FDEV_MSB_300000               0x13
#define RF_FDEV_LSB_300000               0x33
                    
// RF carrier frequency (MHz)
#define RF_RFFREQ_MSB_314                0x4E
#define RF_RFFREQ_MID_314                0x80
#define RF_RFFREQ_LSB_314                0x00
#define RF_RFFREQ_MSB_315                0x4E
#define RF_RFFREQ_MID_315                0xC0
#define RF_RFFREQ_LSB_315                0x00
#define RF_RFFREQ_MSB_316                0x4F
#define RF_RFFREQ_MID_316                0x00
#define RF_RFFREQ_LSB_316                0x00

#define RF_RFFREQ_MSB_433                0x6C
#define RF_RFFREQ_MID_433                0x40
#define RF_RFFREQ_LSB_433                0x00
#define RF_RFFREQ_MSB_434                0x6C
#define RF_RFFREQ_MID_434                0x80
#define RF_RFFREQ_LSB_434                0x00
#define RF_RFFREQ_MSB_435                0x6C
#define RF_RFFREQ_MID_435                0xC0
#define RF_RFFREQ_LSB_435                0x00

#define RF_RFFREQ_MSB_863                0xD7
#define RF_RFFREQ_MID_863                0xC0
#define RF_RFFREQ_LSB_863                0x00
#define RF_RFFREQ_MSB_864                0xD8
#define RF_RFFREQ_MID_864                0x00
#define RF_RFFREQ_LSB_864                0x00
#define RF_RFFREQ_MSB_865                0xD8
#define RF_RFFREQ_MID_865                0x40
#define RF_RFFREQ_LSB_865                0x00
#define RF_RFFREQ_MSB_866                0xD8
#define RF_RFFREQ_MID_866                0x80
#define RF_RFFREQ_LSB_866                0x00
#define RF_RFFREQ_MSB_867                0xD8
#define RF_RFFREQ_MID_867                0xC0
#define RF_RFFREQ_LSB_867                0x00
#define RF_RFFREQ_MSB_868                0xD9
#define RF_RFFREQ_MID_868                0x00
#define RF_RFFREQ_LSB_868                0x00
#define RF_RFFREQ_MSB_869                0xD9
#define RF_RFFREQ_MID_869                0x40
#define RF_RFFREQ_LSB_869                0x00
#define RF_RFFREQ_MSB_870                0xD9
#define RF_RFFREQ_MID_870                0x80
#define RF_RFFREQ_LSB_870                0x00

#define RF_RFFREQ_MSB_902                0xE1
#define RF_RFFREQ_MID_902                0x80
#define RF_RFFREQ_LSB_902                0x00
#define RF_RFFREQ_MSB_903                0xE1
#define RF_RFFREQ_MID_903                0xC0
#define RF_RFFREQ_LSB_903                0x00
#define RF_RFFREQ_MSB_904                0xE2
#define RF_RFFREQ_MID_904                0x00
#define RF_RFFREQ_LSB_904                0x00
#define RF_RFFREQ_MSB_905                0xE2
#define RF_RFFREQ_MID_905                0x40
#define RF_RFFREQ_LSB_905                0x00
#define RF_RFFREQ_MSB_906                0xE2
#define RF_RFFREQ_MID_906                0x80
#define RF_RFFREQ_LSB_906                0x00
#define RF_RFFREQ_MSB_907                0xE2
#define RF_RFFREQ_MID_907                0xC0
#define RF_RFFREQ_LSB_907                0x00
#define RF_RFFREQ_MSB_908                0xE3
#define RF_RFFREQ_MID_908                0x00
#define RF_RFFREQ_LSB_908                0x00
#define RF_RFFREQ_MSB_909                0xE3
#define RF_RFFREQ_MID_909                0x40
#define RF_RFFREQ_LSB_909                0x00
#define RF_RFFREQ_MSB_910                0xE3
#define RF_RFFREQ_MID_910                0x80
#define RF_RFFREQ_LSB_910                0x00
#define RF_RFFREQ_MSB_911                0xE3
#define RF_RFFREQ_MID_911                0xC0
#define RF_RFFREQ_LSB_911                0x00
#define RF_RFFREQ_MSB_912                0xE4
#define RF_RFFREQ_MID_912                0x00
#define RF_RFFREQ_LSB_912                0x00
#define RF_RFFREQ_MSB_913                0xE4
#define RF_RFFREQ_MID_913                0x40
#define RF_RFFREQ_LSB_913                0x00
#define RF_RFFREQ_MSB_914                0xE4
#define RF_RFFREQ_MID_914                0x80
#define RF_RFFREQ_LSB_914                0x00
#define RF_RFFREQ_MSB_915                0xE4  // Default
#define RF_RFFREQ_MID_915                0xC0  // Default
#define RF_RFFREQ_LSB_915                0x00  // Default
#define RF_RFFREQ_MSB_916                0xE5
#define RF_RFFREQ_MID_916                0x00
#define RF_RFFREQ_LSB_916                0x00
#define RF_RFFREQ_MSB_917                0xE5
#define RF_RFFREQ_MID_917                0x40
#define RF_RFFREQ_LSB_917                0x00
#define RF_RFFREQ_MSB_918                0xE5
#define RF_RFFREQ_MID_918                0x80
#define RF_RFFREQ_LSB_918                0x00
#define RF_RFFREQ_MSB_919                0xE5
#define RF_RFFREQ_MID_919                0xC0
#define RF_RFFREQ_LSB_919                0x00
#define RF_RFFREQ_MSB_920                0xE6
#define RF_RFFREQ_MID_920                0x00
#define RF_RFFREQ_LSB_920                0x00
#define RF_RFFREQ_MSB_921                0xE6
#define RF_RFFREQ_MID_921                0x40
#define RF_RFFREQ_LSB_921                0x00
#define RF_RFFREQ_MSB_922                0xE6
#define RF_RFFREQ_MID_922                0x80
#define RF_RFFREQ_LSB_922                0x00
#define RF_RFFREQ_MSB_923                0xE6
#define RF_RFFREQ_MID_923                0xC0
#define RF_RFFREQ_LSB_923                0x00
#define RF_RFFREQ_MSB_924                0xE7
#define RF_RFFREQ_MID_924                0x00
#define RF_RFFREQ_LSB_924                0x00
#define RF_RFFREQ_MSB_925                0xE7
#define RF_RFFREQ_MID_925                0x40
#define RF_RFFREQ_LSB_925                0x00
#define RF_RFFREQ_MSB_926                0xE7
#define RF_RFFREQ_MID_926                0x80
#define RF_RFFREQ_LSB_926                0x00
#define RF_RFFREQ_MSB_927                0xE7
#define RF_RFFREQ_MID_927                0xC0
#define RF_RFFREQ_LSB_927                0x00
#define RF_RFFREQ_MSB_928                0xE8
#define RF_RFFREQ_MID_928                0x00
#define RF_RFFREQ_LSB_928                0x00

// PA selection
#define RF_PA_SELECT_PA1                 0x20  // Default
#define RF_PA_SELECT_PA2                 0x40
#define RF_PA_SELECT_PA12                0x60


// Output power (dBm)
#define RF_PA_OUT_MINUS18                0x00
#define RF_PA_OUT_MINUS17                0x01
#define RF_PA_OUT_MINUS16                0x02
#define RF_PA_OUT_MINUS15                0x03
#define RF_PA_OUT_MINUS14                0x04
#define RF_PA_OUT_MINUS13                0x05
#define RF_PA_OUT_MINUS12                0x06
#define RF_PA_OUT_MINUS11                0x07
#define RF_PA_OUT_MINUS10                0x08
#define RF_PA_OUT_MINUS9                 0x09
#define RF_PA_OUT_MINUS8                 0x0A
#define RF_PA_OUT_MINUS7                 0x0B
#define RF_PA_OUT_MINUS6                 0x0C
#define RF_PA_OUT_MINUS5                 0x0D
#define RF_PA_OUT_MINUS4                 0x0E
#define RF_PA_OUT_MINUS3                 0x0F
#define RF_PA_OUT_MINUS2                 0x10
#define RF_PA_OUT_MINUS1                 0x11
#define RF_PA_OUT_0                      0x12
#define RF_PA_OUT_PLUS1                  0x13
#define RF_PA_OUT_PLUS2                  0x14
#define RF_PA_OUT_PLUS3                  0x15
#define RF_PA_OUT_PLUS4                  0x16
#define RF_PA_OUT_PLUS5                  0x17
#define RF_PA_OUT_PLUS6                  0x18
#define RF_PA_OUT_PLUS7                  0x19
#define RF_PA_OUT_PLUS8                  0x1A
#define RF_PA_OUT_PLUS9                  0x1B
#define RF_PA_OUT_PLUS10                 0x1C
#define RF_PA_OUT_PLUS11                 0x1D
#define RF_PA_OUT_PLUS12                 0x1E
#define RF_PA_OUT_PLUS13                 0x1F  // Default

// PA Rise/fall time in FSK (us) 
#define RF_PA_RAMPFSK_2000               0x00
#define RF_PA_RAMPFSK_1000               0x01
#define RF_PA_RAMPFSK_500                0x02
#define RF_PA_RAMPFSK_250                0x03
#define RF_PA_RAMPFSK_125                0x04
#define RF_PA_RAMPFSK_100                0x05
#define RF_PA_RAMPFSK_62                 0x06
#define RF_PA_RAMPFSK_50                 0x07
#define RF_PA_RAMPFSK_40                 0x08  // Default
#define RF_PA_RAMPFSK_31                 0x09
#define RF_PA_RAMPFSK_25                 0x0A
#define RF_PA_RAMPFSK_20                 0x0B
#define RF_PA_RAMPFSK_15                 0x0C
#define RF_PA_RAMPFSK_12                 0x0D
#define RF_PA_RAMPFSK_10                 0x0E
#define RF_PA_RAMPFSK_8                  0x0F
 
// PLL lock status
#define RF_PLL_LOCK_FLAG                 0x20  // Read Only

// PLL calibration status
#define RF_PLL_CALDONE_FLAG              0x10  // Read Only

// PLL calibration result 
#define RF_PLL_CALOK_FLAG                0x08  // Read Only 

// PLL calibration manual start
#define RF_PLL_CALSTART                  0x04  // Write Only

// PLL division ratio
#define RF_PLL_DIVRATIO_AUTO             0x00  // Default
#define RF_PLL_DIVRATIO_1                0x01
#define RF_PLL_DIVRATIO_2                0x02  
#define RF_PLL_DIVRATIO_3                0x03

// RC enable 
#define RF_CLK_RC_OFF                    0x00  // Default   
#define RF_CLK_RC_ON                     0x08

// CLKOUT frequency (MHz)
#define RF_CLK_OUT_32                    0x00
#define RF_CLK_OUT_16                    0x01
#define RF_CLK_OUT_8                     0x02
#define RF_CLK_OUT_4                     0x03
#define RF_CLK_OUT_2                     0x04
#define RF_CLK_OUT_1                     0x05  // Default
#define RF_CLK_OUT_RC                    0x06
#define RF_CLK_OUT_OFF                   0x07

// Clock source selection
#define RF_CLK_SOURCE_XTAL               0x00  // Default 
#define RF_CLK_SOURCE_EXTERNAL           0x10

// EOL flag (active low)
#define RF_EOL_FLAG                      0x10

// EOL enable
#define RF_EOL_OFF                       0x00  // Default
#define RF_EOL_ON                        0x08

// EOL threshold trimming (mV)
#define RF_EOL_TRIM_1695                 0x00
#define RF_EOL_TRIM_1764                 0x01
#define RF_EOL_TRIM_1835                 0x02  // Default
#define RF_EOL_TRIM_1905                 0x03
#define RF_EOL_TRIM_1976                 0x04
#define RF_EOL_TRIM_2045                 0x05
#define RF_EOL_TRIM_2116                 0x06
#define RF_EOL_TRIM_2185                 0x07

// OCP enable
#define RF_PA_OCP_OFF                    0x00
#define RF_PA_OCP_ON                     0x10  // Default

// PA OverCurrentProtection threshold trimming (mA)
#define RF_PA_OCP_TRIM_45                0x00
#define RF_PA_OCP_TRIM_50                0x01  // Default 
#define RF_PA_OCP_TRIM_55                0x02 
#define RF_PA_OCP_TRIM_60                0x03 
#define RF_PA_OCP_TRIM_65                0x04 
#define RF_PA_OCP_TRIM_70                0x05 
#define RF_PA_OCP_TRIM_75                0x06 
#define RF_PA_OCP_TRIM_80                0x07  
#define RF_PA_OCP_TRIM_85                0x08
#define RF_PA_OCP_TRIM_90                0x09 
#define RF_PA_OCP_TRIM_95                0x0A 
#define RF_PA_OCP_TRIM_100               0x0B 
#define RF_PA_OCP_TRIM_105               0x0C 
#define RF_PA_OCP_TRIM_110               0x0D 
#define RF_PA_OCP_TRIM_115               0x0E 
#define RF_PA_OCP_TRIM_120               0x0F  

/*******************************************************************
** Timings section : all timing described here are done with the  **
**                   A&B counters cascaded                        **
*******************************************************************/

/*******************************************************************
**             -- SX1230 Recommended timings --                   **
********************************************************************
** These timings depends on the RC frequency                      **
** The following values corresponds to a 2.4576 MHz RC Freq       **
** Times calculation formula                                      **
**                                                                **
** CounterA&B value = RC * wanted time  = 2 457 600  * 2 ms = 4915**
**                                                                **
*******************************************************************/
#define TS_OS           1229 // Quartz Osc wake up time, max 500 us
#define TS_TR           8602 // !!! WORST CASE !!! Transmitter wake-up time from FS, with worst case PA ramping (2ms) and worst case bitrate (1200bps). Can be greatly reduced depending on actual settings used.

// No RX frame timeout, SX1230 is a transmitter only

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
** SetRFMode : Sets the SX1230 operating mode (Sleep, Receiver,   **
**           Transmitter)                                         **
********************************************************************
** In  : mode                                                     **
** Out : -                                                        **
*******************************************************************/
void SetRFMode(_U8 mode);

/*******************************************************************
** WriteRegister : Writes the register value at the given address **
**                  on the SX1230                                 **
********************************************************************
** In  : address, value                                           **
** Out : -                                                        **
*******************************************************************/
void WriteRegister(_U8 address, _U16 value);
 
/*******************************************************************
** ReadRegister : Reads the register value at the given address on**
**                the SX1230                                      **
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
** SendByte : Sends a data to the transceiver LSB first           **
**                                                                **
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
**             the TX routines                                    **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void TxEventsOn(void);

/*******************************************************************
** TxEventsOff : Initializes the timers and the events related to **
**             the TX routines                                    **
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
 
/*******************************************************************
** SpiInOut : Sends and receives a byte from the SPI bus          **
********************************************************************
** In  : outputByte                                               **
** Out : inputByte                                                **
*******************************************************************/
_U8 SpiInOut (_U8 outputByte);

#endif /* __SX1230DRIVER__ */
