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

/*******************************************************************
** Include files                                                  **
*******************************************************************/
#include "XE1209Driver.h"

/*******************************************************************
** Global variables                                               **
*******************************************************************/
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
static  _U32 RFFrameTimeOut = RF_FRAME_TIMEOUT(1820); // Reception counter value (full frame timeout generation)
static  _U32 RfifBaudrate = RFIF_BAUDRATE_1820;       // BitJockeyTM baudrate setting

static   _U8 RfifMode = RFIF_DISABLE;// BitJockeyTM mode
static   _U8 StartDetect = false;    // Indicates when a start detection has been made
static   _U8 StartByteStatus = 0;    // Indicates which start byte has been detected
static   _U8 StartByteCnt = 0;       // Start byte counter


_U16 RegistersCfg[] = { // XE1209 configuration registers values
    DEF_REG_A | RF_A_OSC_INT | RF_A_TEST_NORMAL | RF_A_SENS_200 | RF_A_PWR_1_8 | RF_A_TR_0 | RF_A_FC_36_86
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
    // Initializes XE1209
    SrtInit();

    WriteRegister(REG_A, RegistersCfg[REG_A]);

    SetRFMode(RF_SLEEP);
} // void InitRFChip(void)

/*******************************************************************
** SetRFMode : Sets the XE1209 operating mode (sleep,             **
**            carrier_detector, receiver, Transmitter)            **
********************************************************************
** In  : mode                                                     **
** Out : -                                                        **
*******************************************************************/
void SetRFMode(_U8 mode){
    if(mode != PreMode){
        if((mode == RF_TRANSMITTER) && (PreMode == RF_SLEEP)){
            PreMode = RF_TRANSMITTER;
			WriteRegister(REG_A, (ReadRegister(REG_A) | RF_A_TR_1));
			clear_bit(PORTO, RE);
			RfifMode = RFIF_TRANSMITTER;
        }
        else if((mode == RF_TRANSMITTER) && (PreMode == RF_RECEIVER)){
            PreMode = RF_TRANSMITTER;
			WriteRegister(REG_A, (ReadRegister(REG_A) | RF_A_TR_1));
			clear_bit(PORTO, RE);
			RfifMode = RFIF_TRANSMITTER;
        }
        else if((mode == RF_RECEIVER) && (PreMode == RF_SLEEP)){
            PreMode = RF_RECEIVER;
			WriteRegister(REG_A, (ReadRegister(REG_A) | RF_A_TR_1));
			set_bit(PORTO, RE);
			RfifMode = RFIF_RECEIVER;
        }
        else if((mode == RF_RECEIVER) && (PreMode == RF_TRANSMITTER)){
            PreMode = RF_RECEIVER;
			WriteRegister(REG_A, (ReadRegister(REG_A) | RF_A_TR_1));
			set_bit(PORTO, RE);
			RfifMode = RFIF_RECEIVER;
        }
        else if(mode == RF_SLEEP){
            PreMode = RF_SLEEP;
			WriteRegister(REG_A, (ReadRegister(REG_A) & (~RF_A_TR_1)));
            clear_bit(PORTO, RE);
			RfifMode = RFIF_DISABLE;
        }
        else{
            PreMode = RF_SLEEP;
			WriteRegister(REG_A, (ReadRegister(REG_A) & (~RF_A_TR_1)));
            clear_bit(PORTO, RE);
			RfifMode = RFIF_DISABLE;
        }
    }
	SetModeRFIF(RfifMode);
}

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
** WriteRegister : Writes the register value on the               **
**                 XE1209                                         **
********************************************************************
** In  : address, value                                           **
** Out : -                                                        **
*******************************************************************/
void WriteRegister(_U8 address, _U16 value){
    _U8 bitCounter;
	
	set_bit(PORTO, DE);
	SrtInit();
	
	for(bitCounter = 0x01; bitCounter != 0x00; bitCounter <<= 1){	       
        if (value & bitCounter){
			SrtSetSO(1);
		}
		else{
            SrtSetSO(0);
        }
        
		if(bitCounter == 0x80){
			SrtSetSCK(1);		
			clear_bit(PORTO, (DE+SC));
		}
		else{
			SrtSetSCK(1);
			SrtSetSCK(0);		
		}
    }
	clear_bit(PORTO, (DE+SD+SC));
}

/*******************************************************************
** ReadRegister : Not possible with XE1209                        **
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
