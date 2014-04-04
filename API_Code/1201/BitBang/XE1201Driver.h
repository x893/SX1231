/*******************************************************************
** File        : XE1201driver.h                                   **
********************************************************************
**                                                                **
** Version     : V 1.0                                            **
**                                                                **
** Written by  : Miguel Luis & Grégoire Guye                      **
**                                                                **
** Date        : 06-02-2003                                       **
**                                                                **
** Project     : API-1201                                         **
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
** Description : XE1201 transceiver drivers implementation for the**
**               XE8000 family products (BitBang)                 **
*******************************************************************/
#ifndef __XE1201DRIVER__
#define __XE1201DRIVER__

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
**                                  *  uC     * XE1201    * PAx   **
*******************************************************************/
#define CLKD             0x02      //*  In    *  Out      * PA1
#define RXD              0x04      //*  In    *  Out      * PA2

/*******************************************************************
** Port B pins definitions                                        **
********************************************************************
**                                  *  uC     * XE1201    * PBx   **
*******************************************************************/

/*******************************************************************
** Port C pins definitions                                        **
********************************************************************
**                                  *  uC     * XE1201    * PDx   **
*******************************************************************/
#define TXD             0x01      //*  Out    * In        * PC0
#define SD              0x02      //*  Out    * In        * PC1
#define SC              0x04      //*  Out    * In        * PC2
#define DE              0x08      //*  Out    * In        * PC3
#define EN              0x10      //*  Out    * In        * PC4
#define RXTX            0x20      //*  Out    * In        * PC5

/*******************************************************************
** XE1201 3 wire serial interface macros definitions              **
*******************************************************************/
#define SrtInit()        (PORTDIR = 0xFF)
#define SrtSetSCK(level) (level) ? (set_bit(PORTO, SC)) : (clear_bit(PORTO, SC))
#define SrtSetSO(level)  (level) ? (set_bit(PORTO, SD)) : (clear_bit(PORTO, SD))

/*******************************************************************
** XE1201 definitions                                             **
*******************************************************************/
/*******************************************************************
** XE1201 Operating modes definition                              **
*******************************************************************/
#define RF_SLEEP                  0x00
#define RF_RECEIVER               0x01
#define RF_TRANSMITTER            0x02

/*******************************************************************
** XE1201 Internal registers Address                              **
*******************************************************************/
#define REG_A                     0x00
#define REG_B                     0x01
#define REG_C                     0x02

/*******************************************************************
** XE1201 default register values definition                      **
*******************************************************************/
#define DEF_REG_A                 0x0000
#define DEF_REG_B                 0x0000
#define DEF_REG_C                 0x0000

/*******************************************************************
** XE1201 bit control definition                                  **
*******************************************************************/
// Register A
// Control mode bit
#define RF_A_CTRL_MODE_REG        0x2000
#define RF_A_CTRL_MODE_PIN        0x0000
// Clock control
#define RF_A_CLOCK_ALWAYS_ON     0x1000
#define RF_A_CLOCK_ENABLE_ON      0x0000
// Chip Enable
#define RF_A_CHIP_ENABLE_ON       0x0800
#define RF_A_CHIP_ENABLE_OFF      0x0000
// Tx/Rx mode
#define RF_A_RECEIVER_MODE        0x0400
#define RF_A_TRANSMITTER_MODE     0x0000
// Demodulator and bit synchronizer bypassing
#define RF_A_BIT_SYNC_ON          0x0000
#define RF_A_DEMODULATOR_BYPASSED 0x0100
#define RF_A_BIT_SYNC_OFF         0x0140
// Receiver data rate
#define RF_A_BAUDRATE_1200        0x002E
#define RF_A_BAUDRATE_2400        0x0026
#define RF_A_BAUDRATE_4800        0x001E
#define RF_A_BAUDRATE_9600        0x0016
#define RF_A_BAUDRATE_19200       0x000E
#define RF_A_BAUDRATE_38400       0x0006
#define RF_A_BAUDRATE_57600       0x0001

// Register B
// Test register
#define RF_B_TEST_BITS            0x0000

// Register C
// Transmitted output power
#define RF_C_POWER_M_15           0x0000
#define RF_C_POWER_M_5            0x1000
#define RF_C_POWER_P_2_5          0x2000
#define RF_C_POWER_P_5            0x3000
// Data inversion bit
#define RF_C_INVERT_ON            0x0800
#define RF_C_INVERT_OFF           0x0000
// Transmitted output amplifier enable
#define RF_C_TRANS_AMP_ON         0x0100
#define RF_C_TRANS_AMP_OFF        0x0000
// Transmitted data bit
#define RF_C_TRANS_DATA_BIT_1     0x0080
#define RF_C_TRANS_DATA_BIT_0     0x0000
// Modulator frequency deviation
#define RF_C_FDEV_125             0x0020

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
** CounterA&B value = ------------------ = -------------------- = 2048
**                      wanted baudrate            1200           **
**                                                                **
*******************************************************************/
#define TX_BAUDRATE_GEN_1200      2048
#define TX_BAUDRATE_GEN_2400      1024
#define TX_BAUDRATE_GEN_4800      512
#define TX_BAUDRATE_GEN_9600      256
#define TX_BAUDRATE_GEN_19200     128
#define TX_BAUDRATE_GEN_38400     64
#define TX_BAUDRATE_GEN_57600     43

/*******************************************************************
**              -- XE1201 Recommended timings --                  **
********************************************************************
** These timings depends on the RC frequency                      **
** The following values corresponds to a 2.4576 MHz RC Freq       **
** Times calculation formula                                      **
**                                                                **
** CounterA&B value = RC * wanted time  = 2 457 600  * 90 us = 221**
**                                                                **
*******************************************************************/
#define T_CLK                     8602 // 3.5 ms
#define T_TW                      221 // 90 us
#define T_TR                      221 // 90 us
#define T_RT                      73 // 30 us

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
** SetRFMode : Sets the XE1201 operating mode (Sleep, Receiver,   **
**           Transmitter)                                         **
********************************************************************
** In  : mode                                                     **
** Out : -                                                        **
*******************************************************************/
void SetRFMode(_U8 mode);

/*******************************************************************
** WriteRegister : Writes the register value at the given address **
**                  on the XE1201                                 **
********************************************************************
** In  : address, value                                           **
** Out : -                                                        **
*******************************************************************/
void WriteRegister(_U8 address, _U16 value);

/*******************************************************************
** ReadRegister : Not possible with XE1201                        **
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
** AutoFreqControl : This routine do an automatic frequency       **
**                   correction by according to a predefined      **
**                   pattern sent by the transmitter              **
********************************************************************
** ---------------------Not yet implemented-----------------------**
********************************************************************
** In  : -                                                        **
** Out : *pReturnCode                                             **
*******************************************************************/
void AutoFreqControl(_U8 *pReturnCode);

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

#endif /* __XE1201DRIVER__ */

