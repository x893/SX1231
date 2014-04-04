/*******************************************************************
** File        : SX1211driver.c                                   **
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
**                                                                **
** Changes     :                                                  **
**                                                                **
**                                                                **
********************************************************************
** Description : SX1230 transmitter drivers Implementation for the**
**               XE8000 family products (MCU mode)						**
*******************************************************************/

/*******************************************************************
** Include files                                                  **
*******************************************************************/
#include "SX1230Driver.h"

/*******************************************************************
** Global variables                                               **
*******************************************************************/
static   _U8 RFState = RF_STOP;     // RF state machine
static   _U8 *pRFFrame;             // Pointer to the RF frame
static   _U8 RFFramePos;            // RF frame current position
static   _U8 RFFrameSize;           // RF frame size
static  _U16 ByteCounter = 0;       // RF frame byte counter
static   _U8 PreMode = RF_STANDBY;  // Previous chip operating mode
static   _U8 SyncSize = 4;          // Size of sync word
static   _U8 SyncValue[4] = {0x69, 0x81, 0x7E, 0x96};       // Value of sync word


_U16 RegistersCfg[] = { // SX1230 configuration registers values
		
		DEF_MODE | RF_STANDBY | RF_MODUL_FSK | RF_SHAPING_OFF,                                              
	   DEF_BRMSB | RF_BITRATE_MSB_25000,                        
		DEF_BRLSB | RF_BITRATE_LSB_25000,                        
		DEF_FDEVMSB | RF_FDEV_MSB_50000,                      
		DEF_FDEVLSB | RF_FDEV_LSB_50000, 
		DEF_RFFREQMSB | RF_RFFREQ_MSB_869,                    
		DEF_RFFREQMID | RF_RFFREQ_MID_869,                    
		DEF_RFFREQLSB | RF_RFFREQ_LSB_869,                    
		DEF_PACTRL | RF_PA_SELECT_PA1 | RF_PA_OUT_PLUS13,                       
		DEF_PAFSKRAMP | RF_PA_RAMPFSK_40,                    
		DEF_PLLCTRL | RF_PLL_DIVRATIO_AUTO,                      
		//VCOCTRL1 have no default value                     
		//VCOCTRL2 have no default value                     
		//VCOCTRL3 have no default value                    
		//VCOCTRL4 have no default value                    
		DEF_CLOCKCTRL | RF_CLK_RC_OFF | RF_CLK_OUT_1,                    
		DEF_EEPROM,// Not used in MCU mode but kept for simplicity
		DEF_CLOCKSEL | RF_CLK_SOURCE_XTAL,                     
		DEF_EOLCTRL | RF_EOL_OFF | RF_EOL_TRIM_1835,                      
		DEF_PAOCPCTRL | RF_PA_OCP_ON | RF_PA_OCP_TRIM_50,                    
		
};

/*******************************************************************
** Configuration functions                                        **
*******************************************************************/

/*******************************************************************
** InitRFChip : This routine initializes the RFChip registers     **
**              Using Pre Initialized variables                   **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitRFChip (void){
    _U16 i;
      
    // Initialize SPI
    SPIInit();
    set_bit(PORTO, (SCK + NSS + MOSI));
    set_bit(PORTP, (SCK + NSS + MOSI));
    
    // Initialize Registers
    for(i = 0; (i + 4) <= REG_PAOCPCTRL; i++){
        if(i < REG_VCOCTRL1){
            WriteRegister(i, RegistersCfg[i]);
        }
        else{
            WriteRegister(i + 4, RegistersCfg[i]);
        }
    }

    SetRFMode(RF_SLEEP);
}

/*******************************************************************
** SetRFMode : Sets the SX1230 operating mode                     **
********************************************************************
** In  : mode                                                     **
** Out : -                                                        **
*******************************************************************/
void SetRFMode(_U8 mode){
    if(mode != PreMode){
        if(mode == RF_TRANSMITTER){
        
        		WriteRegister(REG_MODE, (RegistersCfg[REG_MODE] & 0x8F) | RF_TRANSMITTER);
        		while ((ReadRegister(REG_PLLCTRL) & RF_PLL_LOCK_FLAG) == 0x00); //Wait for PLL_LOCK
        		Wait(TS_TR);	        
        		
        		PreMode = RF_TRANSMITTER;
        }
        
        else if(mode == RF_SYNTHESIZER){
        		
        		WriteRegister(REG_MODE, (RegistersCfg[REG_MODE] & 0x8F) | RF_SYNTHESIZER);
        		while ((ReadRegister(REG_PLLCTRL) & RF_PLL_LOCK_FLAG) == 0x00); //Wait for PLL_LOCK
            
            PreMode = RF_SYNTHESIZER;
        }
        
        else if(mode == RF_STANDBY){
        
            if (PreMode == RF_SLEEP){
	            WriteRegister(REG_MODE, (RegistersCfg[REG_MODE] & 0x8F) | RF_STANDBY);        		
        		   Wait(TS_OS);
        		}

        		else {
	            WriteRegister(REG_MODE, (RegistersCfg[REG_MODE] & 0x8F) | RF_STANDBY);       		
        		}
        		
        		PreMode = RF_STANDBY;
        }
        
        else {// mode == RF_SLEEP
            WriteRegister(REG_MODE, (RegistersCfg[REG_MODE] & 0x8F) | RF_SLEEP);
        		
        		PreMode = RF_SLEEP;
        }
    }
}

/*******************************************************************
** WriteRegister : Writes the register value at the given address **
**                  on the SX1230                                 **
********************************************************************
** In  : address, value                                           **
** Out : -                                                        **
*******************************************************************/
void WriteRegister(_U8 address, _U16 value){
    
    SPIInit();
    set_bit(PORTO, (SCK + NSS + MOSI));
    set_bit(PORTP, (SCK + NSS + MOSI));
    
    address = address | 0x80;
    SPINssConfig(0);
    SpiInOut(address);
    SpiInOut(value);
    SPINssConfig(1);
}

/*******************************************************************
** ReadRegister : Reads the register value at the given address on**
**                the SX1230                                      **
********************************************************************
** In  : address                                                  **
** Out : value                                                    **
*******************************************************************/
_U16 ReadRegister(_U8 address){
    _U8 value = 0;

    SPIInit();
    set_bit(PORTO, (SCK + NSS + MOSI));
    set_bit(PORTP, (SCK + NSS + MOSI));
    
    address = address & 0x7F;
    SPINssConfig(0);
    SpiInOut(address);
    value = SpiInOut(0);
    SPINssConfig(1);

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
    SetRFMode(RF_TRANSMITTER);
    set_bit(PORTDIR, DATA);
    
    RFState |= RF_BUSY;
    RFState &= ~RF_STOP;
    RFFrameSize = size;
    pRFFrame = buffer;

    TxEventsOn();                     // Enable Tx events

    for(ByteCounter = 0; ByteCounter < 4; ByteCounter++){
        SendByte(0xAA);
    }

    for(ByteCounter = 0; ByteCounter < SyncSize; ByteCounter++){
        SendByte(SyncValue[ByteCounter]);
    }

    SendByte(RFFrameSize);

    for(ByteCounter = 0, RFFramePos = 0; ByteCounter < RFFrameSize; ByteCounter++){
        SendByte(pRFFrame[RFFramePos++]);  
    }

    // Wait for last bit to be actually transmitted
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A

    TxEventsOff();                    // Disable Tx events

    RFState |= RF_STOP;
    RFState &= ~RF_TX_DONE;
    *pReturnCode = OK;
} // void SendRfFrame(_U8 *buffer, _U8 size, _U8 *pReturnCode)


/*******************************************************************
** SendByte : Send a data of 8 bits to the transceiver MSB first  **
********************************************************************
** In  : b                                                        **
** Out : -                                                        **
*******************************************************************/
void SendByte(_U8 b){
    // Bit 0
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    if(b & 0x80) {
    	RegPDOut |= DATA;
    } 
    else {
    	RegPDOut &= ~DATA;
    }
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    
    
    // Bit 1
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    if(b & 0x40) {
    	RegPDOut |= DATA;
    } 
    else {
    	RegPDOut &= ~DATA;
    }
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    
    // Bit 2
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    if(b & 0x20) {
    	RegPDOut |= DATA;
    } 
    else {
    	RegPDOut &= ~DATA;
    }
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    
    // Bit 3
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    if(b & 0x10) {
    	RegPDOut |= DATA;
    } 
    else {
    	RegPDOut &= ~DATA;
    }
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    
    // Bit 4
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    if(b & 0x08) {
    	RegPDOut |= DATA;
    } 
    else {
    	RegPDOut &= ~DATA;
    }
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    
    // Bit 5
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    if(b & 0x04) {
    	RegPDOut |= DATA;
    } 
    else {
    	RegPDOut &= ~DATA;
    }
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    
    // Bit 6
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    if(b & 0x02) {
    	RegPDOut |= DATA;
    } 
    else {
    	RegPDOut &= ~DATA;
    }
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    
    // Bit 7
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    if(b & 0x01) {
    	RegPDOut |= DATA;
    } 
    else {
    	RegPDOut &= ~DATA;
    }
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
   
} // void Sendbyte(_U8 b)


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
** TxEventsOn : Initializes the timers and the events related to  **
**             the TX routines PA1 CntA&B                         **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void TxEventsOn(void){
    RegCntCtrlCk =  (RegCntCtrlCk & 0xFC) | 0x03;  // Selects 128 Hz frequency as clock source for counter A&B
    RegCntConfig1 |= 0x34;                         // A&B counters count up, counter A&B are in cascade mode
    RegCntA       = 0x3F;                          // LSB of TimeOut (0.5s)
    RegCntB       = 0x00;                          // MSB of TimeOut (0.5s)
    RegPAEdge     |= 0x02;                         // Set Evt on falling edge of DCLK
    RegEvnEn      |= 0x90;                         // Enables events from PA1 and CntA
    RegEvn        |= 0x90;                         // Clears the event from the CntA and PA1 on the event register
    asm("and %stat, #0xDE");                       // Clears the event on the CoolRISC status register, and disable all interrupts
    RegCntOn      |= 0x03;                         // Enables counter A&B
} // void TxEventsOn(void)

/*******************************************************************
** TxEventsOff : Initializes the timers and the events related to **
**             the TX routines PA1 CntA&B                         **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void TxEventsOff(void){
    RegCntOn &= 0xFC;                              // Disables counters A&B
    RegEvnEn &= 0x6F;                              // Disables events from PortA bit 1 and Cnt A
    RegEvn |= 0x90;                                // Clears the event from the CntA and PA1 on the event register
    asm("clrb %stat, #0");                         // Clears the event on the CoolRISC status register
    asm("setb %stat, #5");                         // Enable all interrupts
} // void TxEventsOff(void)

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

/*******************************************************************
** SpiInOut : Sends and receives a byte from the SPI bus          **
********************************************************************
** In  : outputByte                                               **
** Out : inputByte                                                **
*******************************************************************/
_U8 SpiInOut (_U8 outputByte){
    _U8 bitCounter;
    _U8 inputByte = 0;

    SPIClock(0);
    for(bitCounter = 0x80; bitCounter != 0x00; bitCounter >>= 1){
        if (outputByte & bitCounter){
            SPIMosi(1);
        }
        else{
            SPIMosi(0);
        }
        SPIClock(1);
        if (SPIMisoTest()){
            inputByte |= bitCounter;
        }
        SPIClock(0);
    }  // for(BitCounter = 0x80; BitCounter != 0x00; BitCounter >>= 1)
    SPIMosi(0);

    return inputByte;
} // _U8 SpiInOut (_U8 outputByte)


