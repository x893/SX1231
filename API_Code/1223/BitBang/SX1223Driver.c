/*******************************************************************
** File        : SX1223driver.c                                   **
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

/*******************************************************************
** Include files                                                  **
*******************************************************************/
#include "SX1223Driver.h"

/*******************************************************************
** Global variables                                               **
*******************************************************************/
static   _U8 RFState = RF_STOP;     // RF state machine
static   _U8 *pRFFrame;             // Pointer to the RF frame
static   _U8 RFFramePos;            // RF frame current position
static   _U8 RFFrameSize;           // RF frame size
static  _U16 ByteCounter = 0;       // RF frame byte counter
static   _U8 PreMode = RF_CFG0_MODE_STANDBY; // Previous chip operating mode
volatile _U8 EnableSyncByte = true; // Enables/disables the synchronization byte reception/transmission
static   _U8 SyncByte;              // RF synchronization byte counter

static   _U8 PatternSize = 4;       // Size of pattern detection
static   _U8 StartByte[4] = {0x69, 0x81, 0x7E, 0x96}; // Pattern detection values
static  _U32 RFBaudrate = TX_BAUDRATE_GEN_1200;       // Transmission counter value (baudrate generation)


_U16 RegistersCfg[] = { // 1223 configuration registers values
	DEF_CONFIG0  | RF_CFG0_MODE_STANDBY | RF_CFG0_POWER_PLUS10 | RF_CFG0_CLKOUT_ON | RF_CFG0_DCLK_ON,             
	DEF_CONFIG1  | RF_CFG1_MODULATION_MW2 | RF_CFG1_BAND_900 | RF_CFG1_XCOINTCAPS_ON | RF_CFG1_LDO_ON | RF_CFG1_OLOPAMP_ON | RF_CFG1_PALDC_OFF | RF_CFG1_LD_ON,              
	DEF_CONFIG2  | RF_CFG2_PACAP_INT | RF_CFG2_XCOQUICKSTART_OFF | RF_CFG2_XCOHIGHCURRENT_OFF | RF_CFG2_CPHIGHCURRENT_OFF | RF_CFG2_VCOFREQ_915,              
	DEF_CONFIG3  | RF_CFG3_MODI_9 | RF_CFG3_MODA_3,              
	DEF_CONFIG4  | RF_CFG4_BRN_1 | RF_CFG4_MODF_4,              
	DEF_CONFIG5  | RF_CFG5_REFCLKK_VALUE,              
	DEF_A0       | RF_A0_VALUE,                   
	DEF_N0MSB    | RF_N0MSB_VALUE,                
	DEF_N0LSB    | RF_N0LSB_VALUE,                
	DEF_M0MSB    | RF_M0MSB_VALUE,                
	DEF_M0LSB    | RF_M0LSB_VALUE,                
	DEF_A1       | RF_A1_VALUE,                   
	DEF_N1MSB    | RF_N1MSB_VALUE,                
	DEF_N1LSB    | RF_N1LSB_VALUE,                
	DEF_M1MSB    | RF_M1MSB_VALUE,                
	DEF_M1LSB    | RF_M1LSB_VALUE,                
	DEF_TESTOPT0 | RF_TESTOPT0_VCOBIAS_950 | RF_TESTOPT0_PRE_PHASESEL | RF_TESTOPT0_VCOBYPASS_OFF,             
	DEF_TESTOPT1 | RF_TESTOPT1_PABIASSOURCE_INT2 | RF_TESTOPT1_PABIASLEVEL_1 | RF_TESTOPT1_PABBIASSOURCE_INT2 | RF_TESTOPT1_PABBIASLEVEL_1,           
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
void InitRFChip (void){
    _U16 i;

    // Initializes SX1223
    SrtInit();

    for(i = 0; i <= REG_TESTOPT1; i++){
        WriteRegister(i, RegistersCfg[i]);
    }

    SetRFMode(RF_CFG0_MODE_STANDBY);
}

/*******************************************************************
** SetRFMode : Sets the SX1223 operating mode (Sleep, Standby,    **
**           Transmit   ,...)                                     **
********************************************************************
** In  : mode                                                     **
** Out : -                                                        **
*******************************************************************/
void SetRFMode(_U8 mode){


    if(mode != PreMode){

            if(mode == RF_CFG0_MODE_TRANSMIT){
                 
            	 if(PreMode == RF_CFG0_MODE_SLEEP){  
                   WriteRegister(REG_CONFIG0, (RegistersCfg[REG_CONFIG0] & 0x9F) | RF_CFG0_MODE_STANDBY);
                   Wait(TS_OS);
                   WriteRegister(REG_CONFIG0, (RegistersCfg[REG_CONFIG0] & 0x9F) | RF_CFG0_MODE_SYNTHESIZER);
                   Wait(TS_FS);
                   WriteRegister(REG_CONFIG0, (RegistersCfg[REG_CONFIG0] & 0x9F) | RF_CFG0_MODE_TRANSMIT);
                   Wait(TS_TR);
                }
                
                else if(PreMode == RF_CFG0_MODE_STANDBY){
                   WriteRegister(REG_CONFIG0, (RegistersCfg[REG_CONFIG0] & 0x9F) | RF_CFG0_MODE_SYNTHESIZER);
                   Wait(TS_FS);
                   WriteRegister(REG_CONFIG0, (RegistersCfg[REG_CONFIG0] & 0x9F) | RF_CFG0_MODE_TRANSMIT);
                   Wait(TS_TR);
                }
                
                else if(PreMode == RF_CFG0_MODE_SYNTHESIZER){
                   WriteRegister(REG_CONFIG0, (RegistersCfg[REG_CONFIG0] & 0x9F) | RF_CFG0_MODE_TRANSMIT);
                   Wait(TS_TR);
                }                 
                
                PreMode = RF_CFG0_MODE_TRANSMIT;
            }

            else if(mode == RF_CFG0_MODE_SYNTHESIZER){
                 
            	 if(PreMode == RF_CFG0_MODE_SLEEP){
                   WriteRegister(REG_CONFIG0, (RegistersCfg[REG_CONFIG0] & 0x9F) | RF_CFG0_MODE_STANDBY);
                   Wait(TS_OS);
                   WriteRegister(REG_CONFIG0, (RegistersCfg[REG_CONFIG0] & 0x9F) | RF_CFG0_MODE_SYNTHESIZER);
                   Wait(TS_FS);
                }
                
                else if(PreMode == RF_CFG0_MODE_STANDBY){
                   WriteRegister(REG_CONFIG0, (RegistersCfg[REG_CONFIG0] & 0x9F) | RF_CFG0_MODE_SYNTHESIZER);
                   Wait(TS_FS);
                }
                
                else if(PreMode == RF_CFG0_MODE_TRANSMIT){
                   WriteRegister(REG_CONFIG0, (RegistersCfg[REG_CONFIG0] & 0x9F) | RF_CFG0_MODE_SYNTHESIZER);
                }                 
                
                PreMode = RF_CFG0_MODE_SYNTHESIZER;
            }
                        
            else if(mode == RF_CFG0_MODE_STANDBY){
                 
            	 if(PreMode == RF_CFG0_MODE_SLEEP){
                   WriteRegister(REG_CONFIG0, (RegistersCfg[REG_CONFIG0] & 0x9F) | RF_CFG0_MODE_STANDBY);
                   Wait(TS_OS);
                }
                
                else {
                   WriteRegister(REG_CONFIG0, (RegistersCfg[REG_CONFIG0] & 0x9F) | RF_CFG0_MODE_STANDBY);
                }              
                
                PreMode = RF_CFG0_MODE_STANDBY;
            }
            
            else if(mode == RF_CFG0_MODE_SLEEP){
   
                WriteRegister(REG_CONFIG0, (RegistersCfg[REG_CONFIG0] & 0x9F) | RF_CFG0_MODE_SLEEP);
                 
                PreMode = RF_CFG0_MODE_SLEEP;
            }            
    }

} // void SetRFMode(_U8 mode)


/*******************************************************************
** WriteRegister : Writes the register value at the given address **
**                  on the SX1223                                 **
********************************************************************
** In  : address, value                                           **
** Out : -                                                        **
*******************************************************************/
void WriteRegister(_U8 address, _U16 value){
    _U8 i;

    clear_bit(PORTO, EN);
    SrtInit();
    set_bit(PORTO, SI+SCK);

    SrtSetSCK(1);
    SrtSetSCK(0);

    // Start
    SrtSetSCK(1);
    SrtSetSI(0);
    SrtSetSCK(0);

    // Write
    SrtSetSCK(1);
    SrtSetSCK(0);

    // Write Address
    for (i = 0x10; i != 0; i >>= 1){
        SrtSetSCK(1);

        if (address & i)
            SrtSetSI(1);
        else
            SrtSetSI(0);

        SrtSetSCK(0);
    }

    for (i = 0x80; i != 0; i >>= 1){
        SrtSetSCK(1);
        if (value & i)
            SrtSetSI(1);
        else
            SrtSetSI(0);

        SrtSetSCK(0);
    }

    SrtSetSI(1);

    SrtSetSCK(1);
    SrtSetSCK(0);

    SrtSetSCK(1);
    SrtSetSCK(0);

    SrtSetSCK(1);
    SrtSetSCK(0);

    set_bit(PORTO, SI+EN+SCK);
}

/*******************************************************************
** ReadRegister : Reads the register value at the given address on**
**                the SX1223                                      **
********************************************************************
** In  : address                                                  **
** Out : value                                                    **
*******************************************************************/
_U16 ReadRegister(_U8 address){
    _U8 i;
    _U8 value = 0;

    clear_bit(PORTO, EN);
    SrtInit();
    set_bit(PORTO, SI+SCK);

    SrtSetSCK(1);
    SrtSetSCK(0);

    // Start
    SrtSetSCK(1);
    SrtSetSI(0);
    SrtSetSCK(0);

    // Write
    SrtSetSCK(1);
    SrtSetSI(1);
    SrtSetSCK(0);

    // Write Address
    for (i = 0x10; i != 0x00; i >>= 1){
        SrtSetSCK(1);

        if (address & i)
            SrtSetSI(1);
        else
            SrtSetSI(0);

        SrtSetSCK(0);
    }

    SrtSetSI(1);

    for (i = 0x80; i != 0x00; i >>= 1){
        SrtSetSCK(1);
        SrtSetSCK(0);
        if (SrtCheckSO())
            value |= i;
    }

    SrtSetSCK(1);
    SrtSetSCK(0);

    SrtSetSCK(1);
    SrtSetSCK(0);
    
    set_bit(PORTO, SI+EN+SCK);

    return value;
}

/*******************************************************************
** Communication functions                                        **
*******************************************************************/

/*******************************************************************
** SendRfFrame : Sends a RF frame                                 **
********************************************************************
** In  : *buffer, size                                            **
** Out : *pReturnCode                                             **
*******************************************************************/
void SendRfFrame(_U8 *buffer, _U8 size, _U8 *pReturnCode){
    if(size > RF_BUFFER_SIZE_MAX){
        RFState |= RF_STOP;
        *pReturnCode = ERROR;
        return;
    }
    SetRFMode(RF_CFG0_MODE_TRANSMIT);

    RFState |= RF_BUSY;
    RFState &= ~RF_STOP;
    RFFrameSize = size;
    pRFFrame = buffer;

    TxEventsOn();                                 // Enable Tx events
    clear_bit(PORTO, DATAIN);

    for(ByteCounter = 0; ByteCounter < 4; ByteCounter++){
        SendByte(0xAA);
    }

    for(ByteCounter = 0; ByteCounter < PatternSize; ByteCounter++){
        SendByte(StartByte[ByteCounter]);
    }


    SendByte(RFFrameSize);

    SyncByte = 0;

    for(ByteCounter = 0, RFFramePos = 0; ByteCounter < RFFrameSize; ByteCounter++){
        if(SyncByte == SYNC_BYTE_FREQ){
            if(EnableSyncByte){
                SyncByte = 0;
                SendByte(0x55);
                ByteCounter--;
            }
        }
        else{
            if(EnableSyncByte){
                SyncByte++;
            }
            SendByte(pRFFrame[RFFramePos++]);
        }
    }

    clear_bit(PORTO, DATAIN);
    TxEventsOff();                                // Disable Tx events

    RFState |= RF_STOP;
    RFState &= ~RF_TX_DONE;
    *pReturnCode = OK;
} // void SendRfFrame(_U8 *buffer, _U8 size, _U8 *pReturnCode)


/*******************************************************************
** SendByte : Send a data of 8 bits to the transceiver LSB first  **
********************************************************************
** In  : b                                                        **
** Out : -                                                        **
*******************************************************************/
void SendByte(_U8 b){
    _U8 bitCounter;

	for(bitCounter = 0x01; bitCounter != 0x00; bitCounter <<= 1){
        do{
            asm("halt");
        }while ((RegEvn & 0x80) == 0x00);         // Waits the event from the counter A
        if (b & bitCounter){
            set_bit(PORTO, DATAIN);
        }
        else{
            clear_bit(PORTO, DATAIN);
        }
        RegEvn = 0x80;                           // Clears the event from the counter A on the event register
        asm("clrb %stat, #0");                   // Clears the event on the CoolRISC status register
    } // for(bitCounter = 0x01; bitCounter != 0x00; bitCounter <<= 1)
} // void Sendbyte(_U8 b)

/*******************************************************************
** Transceiver specific functions                                 **
*******************************************************************/


/*******************************************************************
** Utility functions                                              **
*******************************************************************/

/*******************************************************************
** TxEventsOn : Initializes the timers and the events related to  **
**             the TX routines PA1 CntA&B                         **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void TxEventsOn(void){
    RegCntCtrlCk =  (RegCntCtrlCk & 0xFC) | 0x01; // Selects RC frequency as clock source for counter A&B
    RegCntConfig1 |=  0x34;                       // A&B counters count up, counter A&B  are in cascade mode
    RegCntA       = (char)RFBaudrate;             // LSB of RFBaudrate
    RegCntB       = (char)(RFBaudrate >> 8);      // MSB of RFBaudrate
    RegEvnEn      |= 0x80;                        // Enables events for the counter A&B
    RegEvn        |= 0x80;                        // Clears the event from the CntA on the event register
    asm("and %stat, #0xDE");                      // Clears the event on the CoolRISC status register, and disable all interrupts
    RegCntOn      |= 0x03;                        // Enables counter A&B
} // void TxEventsOn(void)

/*******************************************************************
** TxEventsOff : Initializes the timers and the events related to **
**             the TX routines PA1 CntA&B                         **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void TxEventsOff(void){
    RegCntOn &= 0xFC;                              // Disables counter A&B
    RegEvnEn &= 0x7F;                              // Disables events from the counter A&B
    RegEvn = 0x80;                                 // Clears the event from the CntA on the event register
    asm("clrb %stat, #0");                         // Clears the event on the CoolRISC status register
    asm("setb %stat, #5");                         // Enable all interrupts
} // void TxEventsOff(void)


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
** InvertByte : Inverts a byte. MSB -> LSB, LSB -> MSB            **
********************************************************************
** In  : b                                                        **
** Out : b                                                        **
*******************************************************************/
_U8 InvertByte(_U8 b){
    asm("   move %r0, #0x08");
    asm("LoopInvertByte:");
    asm("   shl  %r3");
    asm("   shrc %r2");
    asm("   dec  %r0");
    asm("   jzc  LoopInvertByte");
} // _U8 InvertByte(_U8 b)








