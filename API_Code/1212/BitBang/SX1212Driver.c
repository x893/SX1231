/*******************************************************************
** File        : SX1212driver.c                                   **
********************************************************************
**                                                                **
** Version     : V 1.0                                            **
**                                                                **
** Written by  : Chaouki ROUAISSIA                                **
** Edited by   : Florian KELLER                                   **
**                                                                **
** Date        : 18-08-2009                                       **
**                                                                **
** Project     : API-1212                                         **
**                                                                **
********************************************************************

********************************************************************
** Description : SX1212 transceiver drivers Implementation for the**
**               XE8000 family products !!! Continuous mode !!!   **
*******************************************************************/

/*******************************************************************
** Include files                                                  **
*******************************************************************/
#include "SX1212Driver.h"

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
static   _U8 SyncValue[4];       // Value of sync word
static  _U32 RFFrameTimeOut = RF_FRAME_TIMEOUT(1600); // Reception counter value (full frame timeout generation)


_U16 RegistersCfg[] = { // SX1212 configuration registers values
		DEF_MCPARAM1 | RF_MC1_STANDBY | RF_MC1_BAND_430470 | RF_MC1_SUBBAND_FIRST,                    
		DEF_MCPARAM2 | RF_MC2_DATA_MODE_CONTINUOUS | RF_MC2_MODULATION_FSK | RF_MC2_OOK_THRESH_TYPE_PEAK | RF_MC2_GAIN_IF_00,                      
		DEF_FDEV | RF_FDEV_100,
		DEF_BITRATE_MSB | RF_BITRATE_MSB_25000,
	    DEF_BITRATE_LSB | RF_BITRATE_LSB_25000,                  
		DEF_MCPARAM6,                   
		DEF_R1 | 107,                            
		DEF_P1 | 42,                            
		DEF_S1 | 30,                            
		DEF_R2 | RF_R2_VALUE,                            
		DEF_P2 | RF_P2_VALUE,                            
		DEF_S2 | RF_S2_VALUE,                         
		
		DEF_FIFOCONFIG | RF_FIFO_SIZE_64 | RF_FIFO_THRESH_VALUE,
		DEF_IRQPARAM1 | RF_IRQ0_RX_SYNC | RF_IRQ1_RX_DCLK | RF_IRQ1_TX_DCLK,                     
		DEF_IRQPARAM2 | RF_IRQ2_PLL_LOCK_PIN_ON,                     
		DEF_RSSIIRQTHRESH | RF_RSSIIRQTHRESH_VALUE,                 
		
		DEF_RXPARAM1 | RF_RX1_PASSIVEFILT_378 | RF_RX1_FC_FOPLUS100,                      
		DEF_RXPARAM2 | RF_RX2_FO_100,                      
		DEF_RXPARAM3 | RF_RX3_POLYPFILT_OFF | RF_RX3_BITSYNC_ON | RF_RX3_SYNC_RECOG_ON | RF_RX3_SYNC_SIZE_32 | RF_RX3_SYNC_TOL_0,                      
		DEF_OOKFLOORTHRESH | RF_OOKFLOORTHRESH_VALUE,                       
      //RSSI Value (Read only)               
		DEF_RXPARAM6 | RF_RX6_OOK_THRESH_DECSTEP_000 | RF_RX6_OOK_THRESH_DECPERIOD_000 | RF_RX6_OOK_THRESH_AVERAGING_00,                       
		
		DEF_SYNCBYTE1 | 0x69, // 1st byte of Sync word,                     
		DEF_SYNCBYTE2 | 0x81, // 2nd byte of Sync word,                     
		DEF_SYNCBYTE3 | 0x7E, // 3rd byte of Sync word,                     
		DEF_SYNCBYTE4 | 0x96, // 4th byte of Sync word,                     
		
		DEF_TXPARAM | RF_TX_FC_200 | RF_TX_POWER_PLUS10,                       
		
		DEF_OSCPARAM | RF_OSC_CLKOUT_ON | RF_OSC_CLKOUT_427                     
   
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
    // Initializes SX1212
    SPIInit();
    set_bit(PORTO, (SCK + NSS_CONFIG + MOSI));
    set_bit(PORTP, (SCK + NSS_CONFIG + MOSI));

    for(i = 0; (i + 1) <= REG_OSCPARAM; i++){
        if(i < REG_RSSIVALUE){
            WriteRegister(i, RegistersCfg[i]);
        }
        else{
            WriteRegister(i + 1, RegistersCfg[i]);
        }
    }

    SyncSize = ((RegistersCfg[REG_RXPARAM3] >> 3) & 0x03) + 1;
    for(i = 0; i < SyncSize; i++){
        SyncValue[i] = RegistersCfg[REG_SYNCBYTE1 - 1 + i];
    }

    if((RegistersCfg[REG_BITRATE_MSB] == RF_BITRATE_MSB_1600) && (RegistersCfg[REG_BITRATE_LSB] == RF_BITRATE_LSB_1600)){
        RFFrameTimeOut = RF_FRAME_TIMEOUT(1600);
    }
    else if((RegistersCfg[REG_BITRATE_MSB] == RF_BITRATE_MSB_2000) && (RegistersCfg[REG_BITRATE_LSB] == RF_BITRATE_LSB_2000)){
        RFFrameTimeOut = RF_FRAME_TIMEOUT(2000);
    }
    else if((RegistersCfg[REG_BITRATE_MSB] == RF_BITRATE_MSB_2500) && (RegistersCfg[REG_BITRATE_LSB] == RF_BITRATE_LSB_2500)){
        RFFrameTimeOut = RF_FRAME_TIMEOUT(2500);
    }
    else if((RegistersCfg[REG_BITRATE_MSB] == RF_BITRATE_MSB_5000) && (RegistersCfg[REG_BITRATE_LSB] == RF_BITRATE_LSB_5000)){
        RFFrameTimeOut = RF_FRAME_TIMEOUT(5000);
    }
    else if((RegistersCfg[REG_BITRATE_MSB] == RF_BITRATE_MSB_5000) && (RegistersCfg[REG_BITRATE_LSB] == RF_BITRATE_LSB_5000)){
        RFFrameTimeOut = RF_FRAME_TIMEOUT(8000);
    }
    else if((RegistersCfg[REG_BITRATE_MSB] == RF_BITRATE_MSB_10000) && (RegistersCfg[REG_BITRATE_LSB] == RF_BITRATE_LSB_10000)){
        RFFrameTimeOut = RF_FRAME_TIMEOUT(10000);
    }
    else if((RegistersCfg[REG_BITRATE_MSB] == RF_BITRATE_MSB_20000) && (RegistersCfg[REG_BITRATE_LSB] == RF_BITRATE_LSB_20000)){    
        RFFrameTimeOut = RF_FRAME_TIMEOUT(20000);
    }
    else if((RegistersCfg[REG_BITRATE_MSB] == RF_BITRATE_MSB_25000) && (RegistersCfg[REG_BITRATE_LSB] == RF_BITRATE_LSB_25000)){
        RFFrameTimeOut = RF_FRAME_TIMEOUT(25000);
    }
    else if((RegistersCfg[REG_BITRATE_MSB] == RF_BITRATE_MSB_40000) && (RegistersCfg[REG_BITRATE_LSB] == RF_BITRATE_LSB_40000)){
        RFFrameTimeOut = RF_FRAME_TIMEOUT(40000);
    }
    else if((RegistersCfg[REG_BITRATE_MSB] == RF_BITRATE_MSB_50000) && (RegistersCfg[REG_BITRATE_LSB] == RF_BITRATE_LSB_50000)){
        RFFrameTimeOut = RF_FRAME_TIMEOUT(50000);
    }
    //100kbps and 200kbps not supported in continuous mode (CoolRisk CPU speed limitation)
    else { 
        RFFrameTimeOut = RF_FRAME_TIMEOUT(1600);
    }

    SetRFMode(RF_SLEEP);
}

/*******************************************************************
** SetRFMode : Sets the SX1212 operating mode                     **
********************************************************************
** In  : mode                                                     **
** Out : -                                                        **
*******************************************************************/
void SetRFMode(_U8 mode){
    if(mode != PreMode){
        if(mode == RF_TRANSMITTER){
        
        		if (PreMode == RF_SLEEP){
	               WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_STANDBY);        		
        		   Wait(TS_OS);        		
	               WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_SYNTHESIZER);        		
        		   Wait(TS_FS);         		
        		   WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_TRANSMITTER);
        		   Wait(TS_TR);
        		}

        		else if (PreMode == RF_STANDBY){
        		   WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_SYNTHESIZER);        		
        		   Wait(TS_FS);         		
        		   WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_TRANSMITTER);
        		   Wait(TS_TR);
        		}

        		else if (PreMode == RF_SYNTHESIZER){
        		   WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_TRANSMITTER);
        		   Wait(TS_TR);
        		}

        		else if (PreMode == RF_RECEIVER){
        		   WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_TRANSMITTER);
        		   Wait(TS_TR);
        		}		        
        		PreMode = RF_TRANSMITTER;
        }
        
        else if(mode == RF_RECEIVER){
        
            if (PreMode == RF_SLEEP){
        		   WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_STANDBY);        		
        		   Wait(TS_OS);        		
	               WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_SYNTHESIZER);        		
        		   Wait(TS_FS); 
        		   WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_RECEIVER);
        		   Wait(TS_RE);
        		}

        		else if (PreMode == RF_STANDBY){
	               WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_SYNTHESIZER);        		
        		   Wait(TS_FS); 
        		   WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_RECEIVER);
        		   Wait(TS_RE);
        		}

        		else if (PreMode == RF_SYNTHESIZER){
        		   WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_RECEIVER);
        		   Wait(TS_RE);     		
        		}

        		else if (PreMode == RF_TRANSMITTER){	
        		   WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_RECEIVER);
        		   Wait(TS_RE);
        		}
        		PreMode = RF_RECEIVER;
        }
        
        else if(mode == RF_SYNTHESIZER){
        
            if (PreMode == RF_SLEEP){
	            WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_STANDBY);        		
        		   Wait(TS_OS);        		
	            WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_SYNTHESIZER);        		
        		   Wait(TS_FS); 
        		}

        		else if (PreMode == RF_STANDBY){
        	    WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_SYNTHESIZER);        		
        		Wait(TS_FS); 
        		}

        		else {
        	    WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_SYNTHESIZER);        		
        		}
            
            PreMode = RF_SYNTHESIZER;
        }
        
        else if(mode == RF_STANDBY){
        
            if (PreMode == RF_SLEEP){
	            WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_STANDBY);        		
        		Wait(TS_OS);
        		}

        		else {
	            WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_STANDBY);       		
        		}
        		
        		PreMode = RF_STANDBY;
        }
        
        else {// mode == RF_SLEEP
        WriteRegister(REG_MCPARAM1, (RegistersCfg[REG_MCPARAM1] & 0x1F) | RF_SLEEP);
        PreMode = RF_SLEEP;
        }
    }
}

/*******************************************************************
** WriteRegister : Writes the register value at the given address **
**                  on the SX1212                                 **
********************************************************************
** In  : address, value                                           **
** Out : -                                                        **
*******************************************************************/
void WriteRegister(_U8 address, _U16 value){
    
    SPIInit();
    address = (address << 1) & 0x3E ;
    SPINssConfig(0);
    SpiInOut(address);
    SpiInOut(value);
    SPINssConfig(1);
}

/*******************************************************************
** ReadRegister : Reads the register value at the given address on**
**                the SX1212                                      **
********************************************************************
** In  : address                                                  **
** Out : value                                                    **
*******************************************************************/
_U16 ReadRegister(_U8 address){
    _U8 value = 0;

    SPIInit();
    address = ((address << 1) & 0x7E) | 0x40;
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

    TxEventsOn();                                 // Enable Tx events


    for(ByteCounter = 0; ByteCounter < 4; ByteCounter++){
        SendByte(0x55);
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

    TxEventsOff();                                // Disable Tx events

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

    pRFFrame = buffer;
    RFFramePos = 0;
    RFFrameSize = 2;

    SetRFMode(RF_RECEIVER);
    clear_bit(PORTDIR, DATA);
    RxEventsOn();                           // Enable events (timeout)

    do{
        if((RegEvn & 0x80) == 0x80){        // Tests if counter A generates an event (timeout)
            RxEventsOff();
            *size = RFFrameSize;
            *pReturnCode = RX_TIMEOUT;
            return;                         // Returns the status TimeOut
        }
    }while((RegPAIn & IRQ_0) == 0x00);      // Waits The Sync word detection of the SX1212

    RegEvn = 0x90;                         // Clears the event from the CntA and PA1 on the event register
    asm("and %stat, #0xDE");               // Clears the event on the CoolRISC status register, and disable all interrupts

    RFFrameSize = ReceiveByte();
    RFFramePos++;

    // The following lines performs the same behavior as RxEventsOff then RxEventsOn inlined
    RegCntOn &= 0xFC;                           // Disables counters A&B
    RegEvnEn &= 0x6F;                           // Disables events from PortA bit 1 and Cnt A
    asm("clrb %stat, #0");                      // Clears the event on the CoolRISC status register
    RegCntA       = (_U8)(RFFrameTimeOut);      // LSB of RFFrameTimeOut
    RegCntB       = (_U8)(RFFrameTimeOut >> 8); // MSB of RFFrameTimeOut
    RegEvnEn      |= 0x90;                      // Enables events from PA1 and CntA
    RegEvn        |= 0x90;                      // Clears the event from the CntA and PA1 on the event register
    asm("and %stat, #0xDE");                    // Clears the event on the CoolRISC status register, and disable all interrupts
    RegCntOn      |= 0x03;                      // Enables counter A&B
    // End of RxEventsOff then RxEventsOn inlined

    if(RFFrameSize >= RF_BUFFER_SIZE_MAX){
        *size = RFFrameSize;
        *pReturnCode = ERROR;
        return;
    }

    while(RFFramePos < RFFrameSize + 1){

        pRFFrame[RFFramePos-1] = ReceiveByte();

        if ((RegEvn & 0x80) == 0x80){       // Tests if counter A generates an event
            RxEventsOff();
            *size = RFFrameSize;
            *pReturnCode = RX_TIMEOUT;
            return ;                        // Returns the status TimeOut
        }
        RFFramePos++;
    }

    *size = RFFrameSize;
    *pReturnCode = OK;
    return;
} // void ReceiveRfFrame(_U8 *buffer, _U8 size, _U8 *pReturnCode)

/*******************************************************************
** SendByte : Send a data of 8 bits to the transceiver MSB first  **
********************************************************************
** In  : b                                                        **
** Out : -                                                        **
*******************************************************************/
void SendByte(_U8 b){
    // Bit 7
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
    
    
    // Bit 6
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
    
    // Bit 5
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
    
    // Bit 4
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
    
    // Bit 3
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
    
    // Bit 2
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
    
    // Bit 1
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
    
    // Bit 0
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
** ReceiveByte : Receives a data of 8 bits from the transceiver   **
**              MSB first                                         **
********************************************************************
** In  : -                                                        **
** Out : b                                                        **
*******************************************************************/
_U8 ReceiveByte(void){
    _U8 b = 0;
    // Bit 7
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPDIn & DATA){
        b |= 0x80;
    }
    // Bit 6
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPDIn & DATA){
        b |= 0x40;
    }
    // Bit 5
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPDIn & DATA){
        b |= 0x20;
    }
    // Bit 4
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPDIn & DATA){
        b |= 0x10;
    }
    // Bit 3
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPDIn & DATA){
        b |= 0x08;
    }
    // Bit 2
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPDIn & DATA){
        b |= 0x04;
    }
    // Bit 1
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPDIn & DATA){
        b |= 0x02;
    }
    // Bit 0
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPDIn & DATA){
        b |= 0x01;
    }
    return b;
} // _U8 ReceiveByte(void)

/*******************************************************************
** Transceiver specific functions                                 **
*******************************************************************/

/*******************************************************************
** ReadRssi : Reads the Rssi value from the SX1212                **
********************************************************************
** In  : -                                                        **
** Out : value                                                    **
*******************************************************************/
_U16 ReadRssi(void){
	_U16 value;
	value = ReadRegister(REG_RSSIVALUE);  // Reads the RSSI result
	return value;
}

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
    RegCntA       = (_U8)RFFrameTimeOut;           // LSB of RFFrameTimeOut
    RegCntB       = (_U8)(RFFrameTimeOut >> 8);    // MSB of RFFrameTimeOut
    RegPAEdge     |= 0x02;                         // Set Evt on falling edge of DCLK (IRQ_1)
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
** RxEventsOn : Initializes the timers and the events related to  **
**             the RX routines PA1 CntA&B                         **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void RxEventsOn(void){
    RegCntCtrlCk =  (RegCntCtrlCk & 0xFC) | 0x03;  // Selects 128 Hz frequency as clock source for counter A&B
    RegCntConfig1 |= 0x34;                         // A&B counters count up, counter A&B are in cascade mode
    RegCntA       = (_U8)RFFrameTimeOut;           // LSB of RFFrameTimeOut
    RegCntB       = (_U8)(RFFrameTimeOut >> 8);    // MSB of RFFrameTimeOut
    RegPAEdge     &= 0xFD;                         // Set Evt on rising edge of DCLK (IRQ_1)
    RegEvnEn      |= 0x90;                         // Enables events from PA1 and CntA
    RegEvn        |= 0x90;                         // Clears the event from the CntA and PA1 on the event register
    asm("and %stat, #0xDE");                       // Clears the event on the CoolRISC status register, and disable all interrupts
    RegCntOn      |= 0x03;                         // Enables counter A&B
} // void RxEventsOn(void)

/*******************************************************************
** RxEventsOff : Initializes the timers and the events related to **
**             the RX routines PA1 CntA&B                         **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void RxEventsOff(void){
    RegCntOn &= 0xFC;                              // Disables counters A&B
    RegEvnEn &= 0x6F;                              // Disables events from PortA bit 1 and Cnt A
    RegEvn |= 0x90;                                // Clears the event from the CntA and PA1 on the event register
    asm("clrb %stat, #0");                         // Clears the event on the CoolRISC status register
    asm("setb %stat, #5");                         // Enable all interrupts
} // void RxEventsOff(void)

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

