/*******************************************************************
** File        : XE1201driver.c                                   **
********************************************************************
**                                                                **
** Version     : V 1.0                                            **
**                                                                **
** Whiten by   : Miguel Luis & Grégoire Guye                      **
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
**               - No changes                                     **
**                                                                **
** Changes     : V 2.4 / CRo - 09-01-2007                         **
**               - No change                                      **
**                                                                **
**                                                                **
********************************************************************
** Description : XE1201A transceiver drivers implementation for   **
**               XE8806A and XE8807A (BitJockey)                  **
*******************************************************************/

/*******************************************************************
** Include files                                                  **
*******************************************************************/
#include "XE1201Driver.h"

/*******************************************************************
** Global variables                                               **
*******************************************************************/
static   _U8 RFState = RF_STOP;      // RF state machine
static   _U8 *pRFFrame;              // Pointer to the RF frame
static   _U8 RFFramePos;             // RF frame current position
static   _U8 RFFrameSize;            // RF frame size
static  _U16 ByteCounter = 0;        // RF frame byte counter
static   _U8 PreMode = RF_SLEEP;     // Previous chip operating mode
volatile _U8 EnableSyncByte = true;  // Enables/disables the synchronization byte reception/transmission
static   _U8 SyncByte;               // RF synchronization byte counter
static   _U8 PatternSize = 4;        // Size of pattern detection
static   _U8 StartByte[4] = {0x69, 0x81, 0x7E, 0x96}; // Pattern detection values
static  _U32 RFFrameTimeOut = RF_FRAME_TIMEOUT(1200); // Reception counter value (full frame timeout generation)
static  _U32 RfifBaudrate = RFIF_BAUDRATE_1200;       // BitJockeyTM baudrate setting
static   _U8 RfifMode = RFIF_DISABLE;// BitJockeyTM mode

static   _U8 StartDetect = false;    // Indicates when a start detection has been made
static   _U8 StartByteStatus = 0;    // Indicates which start byte has been detected
static   _U8 StartByteCnt = 0;       // Start byte counter

_U16 RegistersCfg[] = { // XE1201 configuration registers values
    DEF_REG_A | RF_A_CTRL_MODE_PIN | RF_A_CLOCK_ENABLE_ON | RF_A_CHIP_ENABLE_OFF | RF_A_TRANSMITTER_MODE | RF_A_BIT_SYNC_ON | RF_A_BAUDRATE_4800,
    DEF_REG_B,
    DEF_REG_C | RF_C_POWER_P_5 | RF_C_INVERT_OFF | RF_C_TRANS_AMP_ON | RF_C_TRANS_DATA_BIT_0 | RF_C_FDEV_125
};

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
void InitRFChip(void){
    _U16 i;

    // Initializes XE1201
    SrtInit();

    for(i = 0; i < REG_C + 1; i++){
        WriteRegister(i, RegistersCfg[i]);
    }

    if((RegistersCfg[REG_A] & 0x003F) == RF_A_BAUDRATE_1200){
        RfifBaudrate = RFIF_BAUDRATE_1200;
        RFFrameTimeOut = RF_FRAME_TIMEOUT(1200);
    }
    else if((RegistersCfg[REG_A] & 0x003F) == RF_A_BAUDRATE_2400){
        RfifBaudrate = RFIF_BAUDRATE_2400;
        RFFrameTimeOut = RF_FRAME_TIMEOUT(2400);
    }
    else if((RegistersCfg[REG_A] & 0x003F) == RF_A_BAUDRATE_4800){
        RfifBaudrate = RFIF_BAUDRATE_4800;
        RFFrameTimeOut = RF_FRAME_TIMEOUT(4800);
    }
    else if((RegistersCfg[REG_A] & 0x003F) == RF_A_BAUDRATE_9600){
        RfifBaudrate = RFIF_BAUDRATE_9600;
        RFFrameTimeOut = RF_FRAME_TIMEOUT(9600);
    }
    else if((RegistersCfg[REG_A] & 0x003F) == RF_A_BAUDRATE_19200){
        RfifBaudrate = RFIF_BAUDRATE_19200;
        RFFrameTimeOut = RF_FRAME_TIMEOUT(19200);
    }
    else if((RegistersCfg[REG_A] & 0x003F) == RF_A_BAUDRATE_38400){
        RfifBaudrate = RFIF_BAUDRATE_38400;
        RFFrameTimeOut = RF_FRAME_TIMEOUT(38400);
    }
    else if((RegistersCfg[REG_A] & 0x003F) == RF_A_BAUDRATE_57600){
        RfifBaudrate = RFIF_BAUDRATE_57600;
        RFFrameTimeOut = RF_FRAME_TIMEOUT(57600);
    }
    else {
        RfifBaudrate = RFIF_BAUDRATE_1200;
        RFFrameTimeOut = RF_FRAME_TIMEOUT(1200);
    }
    SetRFMode(RF_SLEEP);
} // void InitRFChip(void)

/*******************************************************************
** SetRFMode : Sets the XE1201 operating mode (Sleep, Receiver,   **
**           Transmitter)                                         **
********************************************************************
** In  : mode                                                     **
** Out : -                                                        **
*******************************************************************/
void SetRFMode(_U8 mode){
    if(mode != PreMode){
        if((mode == RF_TRANSMITTER) && (PreMode == RF_SLEEP)){
            PreMode = RF_TRANSMITTER;
            set_bit(PORTO, EN);
            clear_bit(ANT_SWITCH, RXTX);
            // Waits T_CLK (3.5 ms)
            Wait(T_CLK);
            // Waits T_TW (90 us)
            Wait(T_TW);
            RfifMode = RFIF_TRANSMITTER;
        }
        else if((mode == RF_TRANSMITTER) && (PreMode == RF_RECEIVER)){
            PreMode = RF_TRANSMITTER;
            set_bit(PORTO, EN);
            clear_bit(ANT_SWITCH, RXTX);
            // Waits T_RT (30 us)
            Wait(T_RT);
            RfifMode = RFIF_TRANSMITTER;
        }
        else if((mode == RF_RECEIVER) && (PreMode == RF_SLEEP)){
            PreMode = RF_RECEIVER;
            set_bit(PORTO, EN);
            set_bit(ANT_SWITCH, RXTX);
            // Waits T_CLK (3.5 ms)
            Wait(T_CLK);
            // Waits T_TW (90 us)
            Wait(T_TW);
            RfifMode = RFIF_RECEIVER;
        }
        else if((mode == RF_RECEIVER) && (PreMode == RF_TRANSMITTER)){
            PreMode = RF_RECEIVER;
            set_bit(PORTO, EN);
            set_bit(ANT_SWITCH, RXTX);
            // Waits T_TR (90 us)
            Wait(T_TR);
            RfifMode = RFIF_RECEIVER;
        }
        else if(mode == RF_SLEEP){
            PreMode = RF_SLEEP;
            clear_bit(PORTO, EN);
            RfifMode = RFIF_DISABLE;
        }
        else{
            PreMode = RF_SLEEP;
            clear_bit(PORTO, EN);
            RfifMode = RFIF_DISABLE;
        }
    }
    SetModeRFIF(RfifMode);
} // void SetRFMode(_U8 mode)

/*******************************************************************
** SetModeRFIF : Sets the BitJockey in the given mode             **
********************************************************************
** In  : mode                                                     **
** Out :                                                          **
*******************************************************************/
void SetModeRFIF(_U8 mode){
    // Disables BitJockey RX and TX Interrupt
    RegIrqEnHig &= ~0xA0;
    // Sets the Bitjockey Baudrate
    RegRfifCmd1 = RfifBaudrate;

    //Clears all BitJockey Irqs and Stops it
    RegRfifCmd3 = RFIF_RX_IRQ_FULL | RFIF_RX_IRQ_NEW | RFIF_RX_IRQ_START;

// Bitjockey work around
    {
	    _U16 timeOut = 100;
        _U8 Stop;
        Stop = false;
        do{
            // Tests if RX busy bit is active
            if(RegRfifRxSta & 0x02){
                // Enables the BitJockey in RX
                RegRfifCmd3 = RFIF_EN_RX;
                // Disables the Bitjockey
                RegRfifCmd3 = 0;
            }
            else{
                Stop = true;
            }
            timeOut--;
        }while(Stop == false && !timeOut);
    }
// End of BitJockey work around

    if (mode == RFIF_DISABLE){ // mode = off
        RFState |= RF_STOP;
        RegRfifCmd1 = 0;
        RegRfifCmd2 = 0;
        RegRfifCmd3 = 0;
    }
    else if (mode == RFIF_TRANSMITTER){ // mode = transmitter
        RFState |= RF_STOP;
        RegRfifCmd2 = 0;
        // Enables BitJockey TX mode
        RegRfifCmd3 = RFIF_EN_TX;
        // Enable BitJockey TX Interrupt
        RegIrqEnHig |= 0x20;
    }
    else if(mode == RFIF_RECEIVER){ // mode = receive
        RFState |= RF_STOP;
        // Bitjockey Pattern detection is internally done
        RegRfifCmd2 = RFIF_EN_START_INTERNAL;
        // Start detection Interrupt and enables Bitjockey RX mode
        RegRfifCmd3 = RFIF_RX_IRQ_EN_START | RFIF_EN_RX;
        // Sets the BitJockey start detection byte
        RegRfifRxSPat = StartByte[0];
        // Enables BitJockey RX Interrupt
        RegIrqEnHig |= 0x80;
    }
    else{ // mode = Standby, sleep
        RFState |= RF_STOP;
        RegRfifCmd1 = 0;
        RegRfifCmd2 = 0;
        RegRfifCmd3 = 0;
    }
} // void SetModeRFIF(_U8 mode)

/*******************************************************************
** WriteRegister : Writes the register value at the given address **
**                  on the XE1201                                 **
********************************************************************
** In  : address, value                                           **
** Out : -                                                        **
*******************************************************************/
void WriteRegister(_U8 address, _U16 value){
    _U16 bitCounter;

    SrtInit();
    clear_bit(PORTO, DE);
    // Writes the Address
    for(bitCounter = 0x02; bitCounter != 0x00; bitCounter >>= 1){
        if (address & bitCounter){
            SrtSetSO(1);
        }
        else{
            SrtSetSO(0);
        }
        SrtSetSCK(1);
        SrtSetSCK(0);
    }
    // Writes the Data
    for(bitCounter = 0x2000; bitCounter != 0x00; bitCounter >>= 1){
        if (value & bitCounter){
            SrtSetSO(1);
        }
        else{
            SrtSetSO(0);
        }
        SrtSetSCK(1);
        SrtSetSCK(0);
    }
    set_bit(PORTO, DE);
    SrtSetSCK(1);
    SrtSetSCK(0);
} // void WriteRegister(_U8 address, _U16 value)

/*******************************************************************
** ReadRegister : Not possible with XE1201                        **
********************************************************************
** ReadRegister : Reads the pre initialized global variable that  **
**               is the image of RF chip registers value          **
********************************************************************
** In  : address                                                  **
** Out : value                                                    **
*******************************************************************/
_U16 ReadRegister(_U8 address){
    return RegistersCfg[address];
} // _U16 ReadRegister(_U8 address)

/*******************************************************************
** Communication functions                                        **
*******************************************************************/

/*******************************************************************
** SendRfFrame : Sends a RF frame                                 **
********************************************************************
** In  : *buffer, size                                            **
** Out : *pReturnCode                                             **
*******************************************************************/
void SendRfFrame( _U8 *buffer, _U8 size, _U8 *pReturnCode){
    if(size > RF_BUFFER_SIZE_MAX){
        RFState |= RF_STOP;
        *pReturnCode = ERROR;
        return;
    }
    SetRFMode(RF_TRANSMITTER);

    RFState |= RF_BUSY;
    RFState &= ~RF_STOP;
    RFFrameSize = size;
    pRFFrame = buffer;

    for(ByteCounter = 0; ByteCounter < 4; ByteCounter++){
        RegRfifTx = 0xAA;
        asm("halt");
    }

    for(ByteCounter = 0; ByteCounter < PatternSize; ByteCounter++){
        RegRfifTx = StartByte[ByteCounter];
        asm("halt");
    }

    RegRfifTx = RFFrameSize;
    asm("halt");

    SyncByte = 0;

    for(ByteCounter = 0, RFFramePos = 0; ByteCounter < RFFrameSize; ByteCounter++){
        if(SyncByte == SYNC_BYTE_FREQ){
            if(EnableSyncByte){
                SyncByte = 0;
                RegRfifTx = 0x55;
                ByteCounter--;
            }
        }
        else{
            if(EnableSyncByte){
                SyncByte++;
            }
            RegRfifTx = pRFFrame[RFFramePos++];
        }
        asm("halt");
    }

    while(((RegRfifTxSta & 0x01) == 0));

    RFState |= RF_STOP;
    RFState &= ~RF_TX_DONE;
    *pReturnCode = OK;
} // void SendRfFrame(_U8 *buffer, _U8 size, _U8 *pReturnCode)

/*******************************************************************
** ReceiveRfFrame : Receives a RF frame                           **
********************************************************************
** In  : -                                                        **
** Out : *buffer, size, *pReturnCode                              **
*******************************************************************/
void ReceiveRfFrame(_U8 *buffer, _U8 *size, _U8 *pReturnCode){
    *pReturnCode = RX_RUNNING;

    if(RFState & RF_STOP){
        pRFFrame = buffer;
        RFFramePos = 0;
        SetRFMode(RF_RECEIVER);
        EnableTimeOut(true);
        RFState |= RF_BUSY;
        RFState &= ~RF_STOP;
        RFState &= ~RF_TIMEOUT;
        return;
    }
    else if(RFState & RF_RX_DONE){
        *size = RFFrameSize;
        *pReturnCode = OK;
        RFState |= RF_STOP;
        EnableTimeOut(false);
        RFState &= ~RF_RX_DONE;
        return;
    }
    else if(RFState & RF_ERROR){
        RFState |= RF_STOP;
        RFState &= ~RF_ERROR;
        EnableTimeOut(false);
        *pReturnCode = ERROR;
        return;
    }
    else if(RFState & RF_TIMEOUT){
        RFState |= RF_STOP;
        RFState &= ~RF_TIMEOUT;
        EnableTimeOut(false);
        *pReturnCode = RX_TIMEOUT;
        return;
    }
} // void ReceiveRfFrame(_U8 *buffer, _U8 size, _U8 *pReturnCode)

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
void AutoFreqControl(_U8 *pReturnCode){
    *pReturnCode = OK;
} // void AutoFreqControl(_U8 *pReturnCode)

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
void Wait(_U16 cntVal){
    RegCntOn &= 0xFC;                              // Disables counter A&B
    RegEvnEn &= 0x7F;                              // Disables events from the counter A&B
    RegEvn = 0x80;                                 // Clears the event from the CntA on the event register
    RegCntCtrlCk =  (RegCntCtrlCk & 0xFC) | 0x01;  // Selects RC frequency as clock source for counter A&B
    RegCntConfig1 |= 0x34;                         // A&B counters count up, counter A&B are in cascade mode
    RegCntA       = (_U8)(cntVal);                 // LSB of cntVal
    RegCntB       = (_U8)(cntVal >> 8);            // MSB of cntVal
    RegEvnEn      |= 0x80;                         // Enables events from CntA
    RegEvn        |= 0x80;                         // Clears the event from the CntA on the event register
    asm("clrb %stat, #0");                         // Clears the event on the CoolRISC status register
    RegCntOn      |= 0x03;                         // Enables counter A&B
    do{
        asm("halt");
    }while ((RegEvn & 0x80) == 0x00);              // Waits the event from counter A
    RegCntOn      &= 0xFE;                         // Disables counter A
    RegEvnEn      &= 0x7F;                         // Disables events from the counter A
    RegEvn        |= 0x80;                         // Clears the event from the CntA on the event register
    asm("clrb %stat, #0");                         // Clears the event on the CoolRISC status register
} // void Wait(_U16 cntVal)

/*******************************************************************
** EnableTimeOut : Enables/Disables the RF frame timeout          **
********************************************************************
** In  : enable                                                   **
** Out : -                                                        **
*******************************************************************/
void EnableTimeOut(_U8 enable){
    RegCntCtrlCk = (RegCntCtrlCk & 0xFC) | 0x03;        // Selects 128 Hz frequency as clock source for counter A&B
    RegCntConfig1 |=  0x34;                             // A&B counters count up, counter A&B  are in cascade mode

    RegCntA = (_U8)RFFrameTimeOut;                      // LSB of RF_FRAME_TIMEOUT
    RegCntB = (_U8)(RFFrameTimeOut >> 8);               // MSB of RF_FRAME_TIMEOUT

    if(enable){
        RegIrqEnHig |= 0x10;                            // Enables IRQ for the counter A&B
        RegCntOn |= 0x03;                               // Enables counter A&B
    }
    else{
        RegIrqEnHig &= ~0x10;                           // Disables IRQ for the counter A&B
        RegCntOn &= ~0x03;                              // Disables counter A&B
    }
} // void EnableTimeOut(_U8 enable)

/*******************************************************************
** BitJockey interrupt handlers                                   **
*******************************************************************/

/*******************************************************************
** Handle_Irq_RfifRx : Handles the interruption from the Rf       **
**                     Interface Rx bit                           **
********************************************************************
** In              : -                                            **
** Out             : -                                            **
*******************************************************************/
void Handle_Irq_RfifRx(void){
    _U8 dummy;;

    if(RFState & RF_BUSY){
        RFState |= RF_BUSY;
        if(RegRfifCmd3 & RFIF_RX_IRQ_START){
            RFFramePos = 0;
            RFFrameSize = 5;
            SyncByte = 0;
            StartDetect = false;
            StartByteStatus = 0x01;
            StartByteCnt = 0;
            RegRfifCmd3 = RFIF_RX_IRQ_EN_NEW | RFIF_RX_IRQ_START | RFIF_EN_RX; // Interrupts are generated every byte
            RegRfifCmd2 = 0;
        }
        else if(RegRfifCmd3 & RFIF_RX_IRQ_NEW){
            RegRfifCmd3 |= RFIF_RX_IRQ_NEW;

            if(!StartDetect){
                if(StartByteCnt == 0 && PatternSize == 1){
                    StartDetect = true;
                }
                else if(StartByteCnt == 0){
                    dummy = RegRfifRx;
                    if(dummy == StartByte[1]){
                        StartByteStatus |= 0x02;
                        if(PatternSize == 2 && StartByteStatus == 0x03){
                            StartDetect = true;
                        }
                    }
                }
                else if(StartByteCnt == 1){
                    dummy = RegRfifRx;
                    if(dummy == StartByte[2]){
                        StartByteStatus |= 0x04;
                        if(PatternSize == 3 && StartByteStatus == 0x07){
                            StartDetect = true;
                        }
                    }
                }
                else if(StartByteCnt == 2){
                    dummy = RegRfifRx;
                    if(dummy == StartByte[3]){
                        StartByteStatus |= 0x08;
                        if(StartByteStatus == 0x0F){
                            StartDetect = true;
                        }
                    }
                }
                else{
					dummy = RegRfifRx;
                }
                StartByteCnt++;
            }
            else{
                if(RFFramePos < RFFrameSize + 1){
                    if(RFFramePos == 0){
                        RFFrameSize = RegRfifRx;
                    }
                    else{
                        if(SyncByte == SYNC_BYTE_FREQ){
                            if(EnableSyncByte){
                                SyncByte = 0;
                                dummy = RegRfifRx;
                                RFFramePos--;
                            }
                        }
                        else{
                            pRFFrame[RFFramePos-1] = RegRfifRx;
                            if(EnableSyncByte){
                                SyncByte++;
                            }
                        }
                    }
                    RFFramePos++;
                }
                else{
                    RFState |= RF_RX_DONE;
                    RFState &= ~RF_BUSY;
                }
            }
        }
        if(RFFrameSize >= RF_BUFFER_SIZE_MAX){
            RFState |= RF_ERROR;
            RFState &= ~RF_BUSY;
        }
    }
} //End Handle_Irq_RfifRx

/*******************************************************************
** Handle_Irq_RfifTx : Handles the interruption from the Rf       **
**                     Interface Tx bit                           **
********************************************************************
** In              : -                                            **
** Out             : -                                            **
*******************************************************************/
void Handle_Irq_RfifTx (void){

}  //End Handle_Irq_RfifTx

/*******************************************************************
** Handle_Irq_CntA : Handles the interruption from the Counter A  **
********************************************************************
** In              : -                                            **
** Out             : -                                            **
*******************************************************************/
void Handle_Irq_CntA (void){
    RFState |= RF_TIMEOUT;
    RFState &= ~RF_BUSY;
} //End Handle_Irq_CntA

