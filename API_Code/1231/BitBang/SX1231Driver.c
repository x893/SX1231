/*******************************************************************
** File        : SX1231driver.c                                   **
********************************************************************
**                                                                **
** Version     : V 2.0                                            **
**                                                                **
** Written by  : Chaouki ROUAISSIA                                **
**                                                                **
** Date        : 07-10-2009                                       **
**                                                                **
** Project     : API-1231                                         **
**                                                                **
********************************************************************
** Description : SX1231 transceiver drivers Implementation for the**
**               XE8000 family products !!! Continuous mode !!!   **
*******************************************************************/

/*******************************************************************
** Include files                                                  **
*******************************************************************/
#include "SX1231Driver.h"

/*******************************************************************
** Global variables                                               **
*******************************************************************/
static   _U8 RFState = RF_STOP;     // RF state machine
static   _U8 *pRFFrame;             // Pointer to the RF frame
static   _U8 RFFramePos;            // RF frame current position
static   _U8 RFFrameSize;           // RF frame size
static  _U16 ByteCounter = 0;       // RF frame byte counter
static   _U8 PreMode = RF_STANDBY;  // Previous chip operating mode
static   _U8 SyncSize = 8;          // Size of sync word
static   _U8 SyncValue[8];          // Value of sync word
static  _U32 RFFrameTimeOut = RF_FRAME_TIMEOUT(1200); // Reception counter value (full frame timeout generation)


_U16 RegistersCfg[] = { // SX1231 configuration registers values
	DEF_FIFO, // Left for convenience, not to be changed
	DEF_OPMODE | RF_OPMODE_SEQUENCER_ON | RF_OPMODE_LISTEN_OFF | RF_OPMODE_STANDBY,                       
	DEF_DATAMODUL | RF_DATAMODUL_DATAMODE_CONTINUOUS | RF_DATAMODUL_MODULATIONTYPE_FSK | RF_DATAMODUL_MODULATIONSHAPING_00,                    
	DEF_BITRATEMSB | RF_BITRATEMSB_4800,                   
	DEF_BITRATELSB | RF_BITRATELSB_4800,                   
	DEF_FDEVMSB | RF_FDEVMSB_5000,                       
	DEF_FDEVLSB | RF_FDEVLSB_5000,                      
	DEF_FRFMSB | RF_FRFMSB_865,                       
	DEF_FRFMID | RF_FRFMID_865,                       
	DEF_FRFLSB | RF_FRFLSB_865,                       
	DEF_OSC1,                         
	DEF_OSC2,                         
	DEF_LOWBAT | RF_LOWBAT_OFF | RF_LOWBAT_TRIM_1835,                                               
	DEF_LISTEN1 | RF_LISTEN1_RESOL_4100 | RF_LISTEN1_CRITERIA_RSSI | RF_LISTEN1_END_01,                      
	DEF_LISTEN2 | RF_LISTEN2_COEFIDLE_VALUE,                      
	DEF_LISTEN3 | RF_LISTEN3_COEFRX_VALUE, 
	DEF_VERSION, 			// Read Only                      

	DEF_PALEVEL | RF_PALEVEL_PA0_ON | RF_PALEVEL_PA1_OFF | RF_PALEVEL_PA2_OFF | RF_PALEVEL_OUTPUTPOWER_11111,                          
	DEF_PARAMP | RF_PARAMP_40,                          
	DEF_OCP | RF_OCP_ON | RF_OCP_TRIM_100,                           

	DEF_AGCREF | RF_AGCREF_AUTO_ON | RF_AGCREF_LEVEL_MINUS80,                          
	DEF_AGCTHRESH1 | RF_AGCTHRESH1_SNRMARGIN_101 | RF_AGCTHRESH1_STEP1_16,                          
	DEF_AGCTHRESH2 | RF_AGCTHRESH2_STEP2_3 | RF_AGCTHRESH2_STEP3_11,                          
	DEF_AGCTHRESH3 | RF_AGCTHRESH3_STEP4_9 | RF_AGCTHRESH3_STEP5_11,                          
	DEF_LNA | RF_LNA_ZIN_200 | RF_LNA_LOWPOWER_OFF | RF_LNA_GAINSELECT_AUTO,                           
	DEF_RXBW | RF_RXBW_DCCFREQ_010 | RF_RXBW_MANT_24 | RF_RXBW_EXP_5,                          
	DEF_AFCBW | RF_AFCBW_DCCFREQAFC_100 | RF_AFCBW_MANTAFC_20 | RF_AFCBW_EXPAFC_3,                        
	DEF_OOKPEAK | RF_OOKPEAK_THRESHTYPE_PEAK | RF_OOKPEAK_PEAKTHRESHSTEP_000 | RF_OOKPEAK_PEAKTHRESHDEC_000,                         
	DEF_OOKAVG | RF_OOKAVG_AVERAGETHRESHFILT_10,								  
	DEF_OOKFIX | RF_OOKFIX_FIXEDTHRESH_VALUE,								  
	DEF_AFCFEI | RF_AFCFEI_AFCAUTOCLEAR_OFF | RF_AFCFEI_AFCAUTO_OFF,							  
	DEF_AFCMSB, 			// Read Only							  
	DEF_AFCLSB, 			// Read Only							  
	DEF_FEIMSB, 			// Read Only							  
	DEF_FEILSB, 			// Read Only							  
	DEF_RSSICONFIG | RF_RSSI_FASTRX_OFF,						  
	DEF_RSSIVALUE,  		// Read Only						  

	DEF_DIOMAPPING1 | RF_DIOMAPPING1_DIO0_00 | RF_DIOMAPPING1_DIO1_00 | RF_DIOMAPPING1_DIO2_00 | RF_DIOMAPPING1_DIO3_00,
	DEF_DIOMAPPING2 | RF_DIOMAPPING2_DIO4_00 | RF_DIOMAPPING2_DIO5_01 | RF_DIOMAPPING2_CLKOUT_OFF,						  
	DEF_IRQFLAGS1,
	DEF_IRQFLAGS2,								  							  
	DEF_RSSITHRESH | 228,// Must be set to (-Sensitivity x 2)						  
	DEF_RXTIMEOUT1 | RF_RXTIMEOUT1_RXSTART_VALUE,						  
	DEF_RXTIMEOUT2 | RF_RXTIMEOUT2_RSSITHRESH_VALUE,						  
 
	DEF_PREAMBLEMSB,     // Not used in continuous mode						  		
	DEF_PREAMBLELSB,     // Not used in continuous mode						  		
	DEF_SYNCCONFIG | RF_SYNC_ON | RF_SYNC_FIFOFILL_AUTO | RF_SYNC_SIZE_4 | RF_SYNC_TOL_0,		   		     
	DEF_SYNCVALUE1 | 0x69,						  
	DEF_SYNCVALUE2 | 0x81,						  
	DEF_SYNCVALUE3 | 0x7E,						  
	DEF_SYNCVALUE4 | 0x96,						  
	DEF_SYNCVALUE5 | RF_SYNC_BYTE5_VALUE,						  
	DEF_SYNCVALUE6 | RF_SYNC_BYTE6_VALUE,						  
	DEF_SYNCVALUE7 | RF_SYNC_BYTE7_VALUE,						  
	DEF_SYNCVALUE8 | RF_SYNC_BYTE8_VALUE,						  
	DEF_PACKETCONFIG1,   // Not used in continuous mode					  
	DEF_PAYLOADLENGTH,	// Not used in continuous mode				  		
	DEF_NODEADRS,			// Not used in continuous mode				  		
	DEF_BROADCASTADRS,	// Not used in continuous mode				  		
	DEF_AUTOMODES,			// Not used in continuous mode			  		
	DEF_FIFOTHRESH,		// Not used in continuous mode				  
	DEF_PACKETCONFIG2,	// Not used in continuous mode (except RF_PACKET2_RXRESTART)				  
	DEF_AESKEY1,			// Not used in continuous mode				  
	DEF_AESKEY2,			// Not used in continuous mode				  
	DEF_AESKEY3,			// Not used in continuous mode				  
	DEF_AESKEY4,			// Not used in continuous mode				  
	DEF_AESKEY5,			// Not used in continuous mode				  
	DEF_AESKEY6,			// Not used in continuous mode				  
	DEF_AESKEY7,			// Not used in continuous mode				  
	DEF_AESKEY8,			// Not used in continuous mode				  
	DEF_AESKEY9,			// Not used in continuous mode				  
	DEF_AESKEY10,			// Not used in continuous mode				  
	DEF_AESKEY11,			// Not used in continuous mode				  
	DEF_AESKEY12,			// Not used in continuous mode				  
	DEF_AESKEY13,			// Not used in continuous mode				  
	DEF_AESKEY14,			// Not used in continuous mode				  
	DEF_AESKEY15,			// Not used in continuous mode				  
	DEF_AESKEY16,			// Not used in continuous mode				  
	
	DEF_TEMP1 | RF_TEMP1_ADCLOWPOWER_ON,					  		      
	DEF_TEMP2							     
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
    // Initializes SX1231
    SPIInit();
    set_bit(PORTO, NSS);
	 
	 /////// RC CALIBRATION (Once at POR) ///////
	 SetRFMode(RF_STANDBY);
	 WriteRegister(0x57, 0x80); 
	 WriteRegister(REG_OSC1, ReadRegister(REG_OSC1) | RF_OSC1_RCCAL_START); 
	 while ((ReadRegister(REG_OSC1) & RF_OSC1_RCCAL_DONE) == 0x00); 
 	 WriteRegister(REG_OSC1, ReadRegister(REG_OSC1) | RF_OSC1_RCCAL_START); 
	 while ((ReadRegister(REG_OSC1) & RF_OSC1_RCCAL_DONE) == 0x00);
	 WriteRegister(0x57, 0x00);
	 ////////////////////////////////////////////
	 
    for(i = 1; i <= REG_TEMP2; i++){
    	  WriteRegister(i, RegistersCfg[i]);
    }

    SyncSize = ((RegistersCfg[REG_SYNCCONFIG] >> 3) & 0x07) + 1;
    for(i = 0; i < SyncSize; i++){
        SyncValue[i] = InvertByte(RegistersCfg[REG_SYNCVALUE1 + i]);
    }

    RFFrameTimeOut = RF_FRAME_TIMEOUT(1200); // Worst case bitrate

    SetRFMode(RF_SLEEP);
}

/*******************************************************************
** SetRFMode : Sets the SX1231 operating mode                     **
********************************************************************
** In  : mode                                                     **
** Out : -                                                        **
*******************************************************************/
void SetRFMode(_U8 mode){
    if(mode != PreMode){
        if(mode == RF_TRANSMITTER){
        		WriteRegister(REG_OPMODE, (RegistersCfg[REG_OPMODE] & 0xE3) | RF_TRANSMITTER);        
        		while ((ReadRegister(REG_IRQFLAGS1) & RF_IRQFLAGS1_MODEREADY) == 0x00); // Wait for ModeReady
        		PreMode = RF_TRANSMITTER;
        }
        
        else if(mode == RF_RECEIVER){
        		WriteRegister(REG_OPMODE, (RegistersCfg[REG_OPMODE] & 0xE3) | RF_RECEIVER);        
        		while ((ReadRegister(REG_IRQFLAGS1) & RF_IRQFLAGS1_MODEREADY) == 0x00); // Wait for ModeReady
        		PreMode = RF_RECEIVER;
        }
        
        else if(mode == RF_SYNTHESIZER){
        		WriteRegister(REG_OPMODE, (RegistersCfg[REG_OPMODE] & 0xE3) | RF_SYNTHESIZER);        
        		while ((ReadRegister(REG_IRQFLAGS1) & RF_IRQFLAGS1_MODEREADY) == 0x00); // Wait for ModeReady
        		PreMode = RF_SYNTHESIZER;
        }
        
        else if(mode == RF_STANDBY){
        		WriteRegister(REG_OPMODE, (RegistersCfg[REG_OPMODE] & 0xE3) | RF_STANDBY);        
        		while ((ReadRegister(REG_IRQFLAGS1) & RF_IRQFLAGS1_MODEREADY) == 0x00); // Wait for ModeReady
        		PreMode = RF_STANDBY;
        }
        
        else {// mode == RF_SLEEP
        		WriteRegister(REG_OPMODE, (RegistersCfg[REG_OPMODE] & 0xE3) | RF_SLEEP);        
        		while ((ReadRegister(REG_IRQFLAGS1) & RF_IRQFLAGS1_MODEREADY) == 0x00); // Wait for ModeReady
        		PreMode = RF_SLEEP;
        }
    }
}

/*******************************************************************
** WriteRegister : Writes the register value at the given address **
**                  on the SX1231                                 **
********************************************************************
** In  : address, value                                           **
** Out : -                                                        **
*******************************************************************/
void WriteRegister(_U8 address, _U16 value){
    
    SPIInit();
    SPINss(1);
    address = address | 0x80;
    SPINss(0);
    SpiInOut(address);
    SpiInOut(value);
    SPINss(1);
}

/*******************************************************************
** ReadRegister : Reads the register value at the given address on**
**                the SX1231                                      **
********************************************************************
** In  : address                                                  **
** Out : value                                                    **
*******************************************************************/
_U16 ReadRegister(_U8 address){
    _U8 value = 0;

    SPIInit();
    SPINss(1);
    address = address & 0x7F;
    SPINss(0);
    SpiInOut(address);
    value = SpiInOut(0);
    SPINss(1);

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
    
    set_bit(PORTDIR, DATA); // DATA is an output 
    WriteRegister(REG_DIOMAPPING1, (RegistersCfg[REG_DIOMAPPING1] & 0xCF) | RF_DIOMAPPING1_DIO1_00); // DIO1 mapped to DCLK
    
    RFState |= RF_BUSY;
    RFState &= ~RF_STOP;
    RFFrameSize = size;
    pRFFrame = buffer;

    TxEventsOn();                                 // Enable Tx events

    SetRFMode(RF_TRANSMITTER);

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
    }while ((RegEvn & 0x90) == 0x00); // Waits for the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits for the event from the PortA or the counter A

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

    clear_bit(PORTDIR, DATA); // DATA is an input
    WriteRegister(REG_DIOMAPPING1, (RegistersCfg[REG_DIOMAPPING1] & 0x3F) | RF_DIOMAPPING1_DIO0_00); // DIO0 mapped to SYNC
    WriteRegister(REG_DIOMAPPING1, (RegistersCfg[REG_DIOMAPPING1] & 0xCF) | RF_DIOMAPPING1_DIO1_00); // DIO1 mapped to DCLK
    
    RxEventsOn();                           // Enable events (timeout)
    
    SetRFMode(RF_RECEIVER);
    WriteRegister(REG_PACKETCONFIG2, RF_PACKET2_RXRESTART);
    
    do{
        if((RegEvn & 0x80) == 0x80){        // Tests if counter A generates an event (timeout)
            RxEventsOff();
            *size = RFFrameSize;
            *pReturnCode = RX_TIMEOUT;
            return;                         // Returns the status TimeOut
        }
    }while((RegPAIn & DIO0) == 0x00);      // Waits for the Sync word detection of the SX1231

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
** SendByte : Send a data of 8 bits to the transceiver LSB first  **
********************************************************************
** In  : b                                                        **
** Out : -                                                        **
*******************************************************************/
void SendByte(_U8 b){
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
   
} // void Sendbyte(_U8 b)

/*******************************************************************
** ReceiveByte : Receives a data of 8 bits from the transceiver   **
**              LSB first                                         **
********************************************************************
** In  : -                                                        **
** Out : b                                                        **
*******************************************************************/
_U8 ReceiveByte(void){
    _U8 b = 0;
    // Bit 0
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPDIn & DATA){
        b |= 0x01;
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
    // Bit 2
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPDIn & DATA){
        b |= 0x04;
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
    // Bit 4
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPDIn & DATA){
        b |= 0x10;
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
    // Bit 6
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPDIn & DATA){
        b |= 0x40;
    }
    // Bit 7
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPDIn & DATA){
        b |= 0x80;
    }
    return b;
} // _U8 ReceiveByte(void)

/*******************************************************************
** Transceiver specific functions                                 **
*******************************************************************/

/*******************************************************************
** ReadRssi : Reads the Rssi value from the SX1231                **
********************************************************************
** In  : -                                                        **
** Out : value                                                    **
*******************************************************************/
_U16 ReadRssi(void){ // Must be called while in RX
	_U16 value;
	WriteRegister(REG_RSSICONFIG, RegistersCfg[REG_RSSICONFIG] | RF_RSSI_START); // Triggers RSSI measurement
	while ((ReadRegister(REG_RSSICONFIG) & RF_RSSI_DONE) == 0x00);               // Waits for RSSI measurement to be completed
	value = ReadRegister(REG_RSSIVALUE);                                         // Reads the RSSI result
	return value;
}

/*******************************************************************
** ReadFei : Triggers FEI measurement and returns its value       **
********************************************************************
** In  : -                                                        **
** Out : value                                                    **
*******************************************************************/
_S16 ReadFei(void){ // Must be called while in RX
	_S16 value;
	WriteRegister(REG_AFCFEI, RegistersCfg[REG_AFCFEI] | RF_AFCFEI_FEI_START);   // Triggers FEI measurement
	while ((ReadRegister(REG_AFCFEI) & RF_AFCFEI_FEI_DONE) == 0x00);             // Waits for FEI measurement to be completed
	value = ((ReadRegister(REG_FEIMSB) << 8) | ReadRegister(REG_FEILSB));        // Reads the FEI result
	return value;
}

/*******************************************************************
** AutoFreqControl : Calibrates the receiver frequency to the     **
**               transmitter frequency                            **
********************************************************************
** In  : -                                                        **
** Out : *pReturnCode                                             **
*******************************************************************/
void AutoFreqControl(_U8 *pReturnCode){ // Must be called while in RX
	WriteRegister(REG_AFCFEI, RegistersCfg[REG_AFCFEI] | RF_AFCFEI_AFC_START);   // Triggers AFC measurement
	while ((ReadRegister(REG_AFCFEI) & RF_AFCFEI_AFC_DONE) == 0x00);             // Waits for AFC measurement to be completed
   *pReturnCode = OK;
}

/*******************************************************************
** Utility functions                                              **
*******************************************************************/

/*******************************************************************
** Wait : This routine uses the counter A&B to create a delay     **
**        using the RC ck source                                  **
** cntVal = RC * wanted_time  = 2 457 600  * wanted_time          ** 
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
    RegPAEdge     |= 0x02;                         // Set Evt on falling edge of DCLK (DIO1)
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
    RegPAEdge     &= 0xFD;                         // Set Evt on rising edge of DCLK (DIO1)
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



