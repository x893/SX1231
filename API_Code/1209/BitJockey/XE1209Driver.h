/*******************************************************************
** File        : XE1209driver.c                                   **
********************************************************************
**                                                                **
** Version     : V 1.0                                            **
**                                                                **
** Written by   : Miguel Luis & Grégoire Guye                     **
**                                                                **
** Date        : 06-02-2003                                       **
**                                                                **
** Project     : API-1209                                         **
**                                                                **
********************************************************************
** Changes     : V 2.0 / MiL - 09-12-2003                         **
**                                                                **
** Changes     : V 2.1 / MiL - 24-04-2004                         **
**               - Add RfifMode variable                          **
**               - Change SetRFMode function in order to allways  **
**                 call SetModeRFIF                               **
**                                                                **
**                                                                **
** Changes     : V 2.3 / CRo - 06-06-2006                         **
**               - No Change                                      **
**                                                                **
** Changes     : V 2.4 / CRo - 09-01-2007                         **
**               - No change                                      **
**                                                                **
**                                                                **
********************************************************************
** Description : XE1209 transceiver drivers implementation for the**
**               XE8806A and XE8807A (BitJockey)                  **
*******************************************************************/
#ifndef __XE1209DRIVER__
#define __XE1209DRIVER__

/*******************************************************************
** Include files                                                  **
*******************************************************************/
#include "Globals.h"

/*******************************************************************
** Global definitions                                             **
*******************************************************************/

/*******************************************************************
** RF frame definition                                            **
*******************************************************************/
#define RF_BUFFER_SIZE_MAX   64
#define RF_BUFFER_SIZE       64
#define SYNC_BYTE_FREQ       4

#define PREAMBLE             0xAAAA

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
#define ANT_SWITCH         RegPDOut

/*******************************************************************
** Port A pins definitions                                        **
********************************************************************
**                                  *  uC     * XE1209    * PAx   **
*******************************************************************/

/*******************************************************************
** Port B pins definitions                                        **
********************************************************************
**                                  *  uC     * XE1209    * PBx   **
*******************************************************************/

/*******************************************************************
** Port C pins definitions                                        **
********************************************************************
**                                  *  uC     * XE1209    * PCx   **
*******************************************************************/
#define DATAOUT         0x01      //*  In     *  Out      * PD0 // DATA
#define CLKD            0x02      //*  In     *  Out      * PD1
#define DATAIN          0x04      //*  Out    *  In       * PD3 // DATA
#define SD              0x02      //*  Out    *  In       * PD4
#define SC              0x04      //*  Out    *  In       * PD5
#define DE              0x08      //*  Out    *  In       * PD6
#define RE              0x10      //*  Out    *  In       * PD7

/*******************************************************************
** XE1209 Serial Interface Macros definitions                     **
*******************************************************************/
#define SrtInit()         (PORTDIR = 0xFF)
#define SrtSetSCK(level)  (level) ? (set_bit(PORTO, SC)) : (clear_bit(PORTO, SC))
#define SrtSetSO(level)   (level) ? (set_bit(PORTO, SD)) : (clear_bit(PORTO, SD))

/*******************************************************************
** XE1209 definitions                                             **
*******************************************************************/
/*******************************************************************
** XE1209 Operating modes definition                              **
*******************************************************************/
#define RF_SLEEP              0x00
#define RF_CARRIER_DETECTOR   0x01
#define RF_TRANSMITTER        0x02
#define RF_RECEIVER           0x03

/*******************************************************************
** XE1209 Internal registers Address                              **
*******************************************************************/
#define REG_A                 0x00

/*******************************************************************
** XE1209 default register values definition                      **
*******************************************************************/
#define DEF_REG_A             0x00

/*******************************************************************
** XE1209 bit control definition                                  **
*******************************************************************/
// Register A
// Oscillator flag
#define RF_A_OSC_INT          0x80
#define RF_A_OSC_EXT          0x00
// Test flag
#define RF_A_TEST_TEST        0x40
#define RF_A_TEST_NORMAL      0x00
// Carrier detector threshold
#define RF_A_SENS_500         0x20
#define RF_A_SENS_200         0x00
// Power level
#define RF_A_PWR_110_0        0x14
#define RF_A_PWR_60_0         0x10
#define RF_A_PWR_30_0         0x0C
#define RF_A_PWR_7_5          0x08
#define RF_A_PWR_3_5          0x04
#define RF_A_PWR_1_8          0x00
// Transmission flag
#define RF_A_TR_1             0x02
#define RF_A_TR_0             0x00
// Carrier frequency
#define RF_A_FC_45_05         0x01
#define RF_A_FC_36_86         0x00

/*******************************************************************
** Timings section : all timing described here are done with the  **
**                   A&B counters cascaded                        **
*******************************************************************/
/*******************************************************************
**             -- XE1209 Recommended timings --                   **
********************************************************************
** These timings depends on the RC frequency                      **
** The following values corresponds to a 2.4576 MHz RC Freq       **
** Times calculation formula                                      **
**                                                                **
** CounterA&B value = RC * wanted time  = 2 457 600  * 2.5 ms = 6144
**                                                                **
*******************************************************************/
#define T_RAC                 6144 // 2.5 ms

/*******************************************************************
**                                                                **
**                     // RF_BUFFER_SIZE * 8 * (SYNC_BYTE_FREQ  + 1) \          \
** CounterA&B value = || -------------------------------------------- | * 128 Hz | + 1
**                     \\        SYNC_BYTE_FREQ * BitRate            /          /
**                                                                **
** The plus 1 at the end of formula is required for the highest   **
** baudrate as the resulting timeout is lower than the 1 / 128Hz  **
*******************************************************************/
#define RF_FRAME_TIMEOUT(BitRate) (_U16)(_F32)((((_F32)((_U32)RF_BUFFER_SIZE * (_U32)8 * ((_U32)SYNC_BYTE_FREQ  + (_U32)1)) / (_F32)((_U32)SYNC_BYTE_FREQ * (_U32)BitRate)) * (_F32)128) + (_F32)1)

/*******************************************************************
** BitJockey Standard parameters                                  **
*******************************************************************/
// All the values are calculated for microcontroller RC frequency = 2457600 Hz
// RegRfifCmd1
#define RFIF_BAUDRATE_1820        0x35

// RegRfifCmd2
#define RFIF_EN_START_INTERNAL    0xC0
#define RFIF_EN_START_EXTERNAL    0x80
#define RFIF_EN_START_PROTOCOL    0x40
#define RFIF_EN_DECODER           0x20
#define RFIF_RX_CLOCK             0x10
#define RFIF_TX_CLOCK             0x08
#define RFIF_PCM_NRZ_MARK         0x01
#define RFIF_PCM_NRZ_SPACE        0x02
#define RFIF_PCM_BPH_LEVEL        0x03
#define RFIF_PCM_BPH_MARK         0x04
#define RFIF_PCM_BPH_SPACE        0x05
#define RFIF_PCM_MILLER           0x06

// RegRfifCmd3
#define RFIF_RX_IRQ_EN_FULL       0x80
#define RFIF_RX_IRQ_EN_NEW        0x40
#define RFIF_RX_IRQ_EN_START      0x20
#define RFIF_RX_IRQ_FULL          0x10
#define RFIF_RX_IRQ_NEW           0x08
#define RFIF_RX_IRQ_START         0x04
#define RFIF_EN_RX                0x02
#define RFIF_EN_TX                0x01

// RFIF modes
#define RFIF_DISABLE              0
#define RFIF_TRANSMITTER          1
#define RFIF_RECEIVER             2
#define RFIF_OTHERS               3

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
** SetRFMode : Sets the XE1209 operating mode (Sleep, Receiver,   **
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
**                  on the XE1209                                 **
********************************************************************
** In  : address, value                                           **
** Out : -                                                        **
*******************************************************************/
void WriteRegister(_U8 address, _U16 value);

/*******************************************************************
** ReadRegister : Not possible with XE1209                        **
********************************************************************
** ReadRegister : Reads the pre initialized global variable that  **
**               is the image of RF chip registers value          **
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

#endif /* __XE1209DRIVER__ */
