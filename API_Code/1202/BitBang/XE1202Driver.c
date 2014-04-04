/*******************************************************************
** File        : XE1202driver.c                                   **
********************************************************************
**                                                                **
** Version     : V 1.0                                            **
**                                                                **
** Written by   : Miguel Luis & Grégoire Guye                     **
**                                                                **
** Date        : 28-03-2003                                       **
**                                                                **
** Project     : API-1202                                         **
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
**               - No change                                      **
**                                                                **
** Changes     : V 2.4 / CRo - 09-01-2007                         **
**               - No change                                      **
**                                                                **
**                                                                **
********************************************************************
** Description : XE1202 transceiver drivers implementation for the**
**               XE8000 family products (BitBang)                 **
*******************************************************************/

/*******************************************************************
** Include files                                                  **
*******************************************************************/
#include "XE1202Driver.h"

/*******************************************************************
** Global variables                                               **
*******************************************************************/
static   _U8 RFState = RF_STOP;     // RF state machine
static   _U8 *pRFFrame;             // Pointer to the RF frame
static   _U8 RFFramePos;            // RF frame current position
static   _U8 RFFrameSize;           // RF frame size
static  _U16 ByteCounter = 0;       // RF frame byte counter
static   _U8 PreMode = RF_SLEEP;    // Previous chip operating mode
volatile _U8 EnableSyncByte = true; // Enables/disables the synchronization byte reception/transmission
static   _U8 SyncByte;              // RF synchronization byte counter
static   _U8 PatternSize = 4;       // Size of pattern detection
static   _U8 StartByte[4];          // Pattern detection values
static  _U32 RFFrameTimeOut = RF_FRAME_TIMEOUT(4800); // Reception counter value (full frame timeout generation)
static  _U32 RFBaudrate = TX_BAUDRATE_GEN_4800;       // Transmission counter value (baudrate generation)

_U16 RegistersCfg[] = { // 1202 configuration registers values
    DEF_RTPARAM1 | RF_RT1_RMODE_MODE_A | RF_RT1_BIT_SYNC_ON | RF_RT1_RSSI_OFF | RF_RT1_FEI_ON | RF_RT1_BW_200 | RF_RT1_POWER_0,
    DEF_RTPARAM2 | RF_RT2_OSC_INT | RF_RT2_WBB_BOOST | RF_RT2_FILTER_OFF | RF_RT2_FSEL_CORRELATOR | RF_RT2_STAIR_10 | RF_RT2_MODUL_ON | RF_RT2_RSSR_LOW | RF_RT2_CLKOUT_OFF,

    DEF_FSPARAM1 | RF_FS1_BAND_915 | RF_FS1_FDEV_100 | RF_FS1_BAUDRATE_4800,
    DEF_FSPARAM2,
    DEF_FSPARAM3,

    DEF_ADPARAM1 | RF_AD1_PATTERN_ON | RF_AD1_P_SIZE_32 | RF_AD1_P_TOL_0 | RF_AD1_CLK_FREQ_1_22_MHZ | RF_AD1_IQA_OFF,
    DEF_ADPARAM2 | RF_AD2_INVERT_OFF | RF_AD2_REG_BW_ON | RF_AD2_REG_FREQ_START_UP | RF_AD2_REG_COND_ON | RF_AD2_WBB_COND_ON | RF_AD2_X_SEL_15_PF,

    DEF_PATTERN1 | 0x69,
    DEF_PATTERN2 | 0x81,
    DEF_PATTERN3 | 0x7E,
    DEF_PATTERN4 | 0x96,
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

    // Initializes XE1202
    SrtInit();

    for(i = 0; (i + 2) <= REG_PATTERN4 + 1; i++){
        if(i < REG_DATAOUT){
            WriteRegister(i, RegistersCfg[i]);
        }
        else{
            WriteRegister(i + 1, RegistersCfg[i]);
        }
    }

    PatternSize = ((RegistersCfg[REG_ADPARAM1 -1] >> 5) & 0x03) + 1;
    for(i = 0; i < PatternSize; i++){
        StartByte[i] = InvertByte(RegistersCfg[REG_PATTERN1 - 1 + i]);
    }

    if((RegistersCfg[REG_FSPARAM1] & 0x07) == RF_FS1_BAUDRATE_4800){
        RFBaudrate = TX_BAUDRATE_GEN_4800;
        RFFrameTimeOut = RF_FRAME_TIMEOUT(4800);
    }
    else if((RegistersCfg[REG_FSPARAM1] & 0x07) == RF_FS1_BAUDRATE_9600){
        RFBaudrate = TX_BAUDRATE_GEN_9600;
        RFFrameTimeOut = RF_FRAME_TIMEOUT(9600);
    }
    else if((RegistersCfg[REG_FSPARAM1] & 0x07) == RF_FS1_BAUDRATE_19200){
        RFBaudrate = TX_BAUDRATE_GEN_19200;
        RFFrameTimeOut = RF_FRAME_TIMEOUT(19200);
    }
    else if((RegistersCfg[REG_FSPARAM1] & 0x07) == RF_FS1_BAUDRATE_38400){
        RFBaudrate = TX_BAUDRATE_GEN_38400;
        RFFrameTimeOut = RF_FRAME_TIMEOUT(38400);
    }
    else if((RegistersCfg[REG_FSPARAM1] & 0x07) == RF_FS1_BAUDRATE_76800){
        RFBaudrate = TX_BAUDRATE_GEN_76800;
        RFFrameTimeOut = RF_FRAME_TIMEOUT(76800);
    }
    else {
        RFBaudrate = TX_BAUDRATE_GEN_4800;
        RFFrameTimeOut = RF_FRAME_TIMEOUT(4800);
    }

    SetRFMode(RF_SLEEP);
} // void InitRFChip (void)

/*******************************************************************
** SetRFMode : Sets the XE1202 operating mode (Sleep, Receiver,   **
**           Transmitter)                                         **
********************************************************************
** In  : mode                                                     **
** Out : -                                                        **
*******************************************************************/
void SetRFMode(_U8 mode){
    if(mode != PreMode){
        clear_bit(PORTO, EN);
        if((mode == RF_TRANSMITTER) && (PreMode == RF_SLEEP)){
			PreMode = RF_TRANSMITTER;
            clear_bit(RegPBOut, (MODE2+MODE1));
            set_bit(RegPBOut, MODE0);
            // wait TS_OS
            Wait(TS_OS);
            clear_bit(RegPBOut, MODE0);
            set_bit(RegPBOut, (MODE2+MODE1));
            // wait TS_FS 200us
            Wait(TS_FS);
            set_bit(RegPBOut, (MODE2+MODE1+MODE0));
            set_bit(ANT_SWITCH, TX);
            clear_bit(ANT_SWITCH, RX);
            // wait TS_TR 100us
            Wait(TS_TR);
        }
        else if((mode == RF_TRANSMITTER) && (PreMode == RF_RECEIVER)){
            PreMode = RF_TRANSMITTER;
            set_bit(RegPBOut, (MODE2+MODE1+MODE0));
            set_bit(ANT_SWITCH, TX);
            clear_bit(ANT_SWITCH, RX);
            // wait TS_TR
            Wait(TS_TR);
        }
        else if((mode == RF_RECEIVER) && (PreMode == RF_SLEEP)){
            PreMode = RF_RECEIVER;
            clear_bit(RegPBOut, (MODE2+MODE1));
            set_bit(RegPBOut, MODE0);
            // wait TS_OS
            Wait(TS_OS);
            clear_bit(RegPBOut, (MODE2+MODE0));
            set_bit(RegPBOut, MODE1);
            // wait TS_BBR
            Wait(TS_BBR);
            clear_bit(RegPBOut, MODE2);
            set_bit(RegPBOut, (MODE1+MODE0));
            // wait TS_FS
            Wait(TS_FS);
            clear_bit(RegPBOut, (MODE1+MODE0));
            set_bit(RegPBOut, MODE2);
            set_bit(ANT_SWITCH, RX);
            clear_bit(ANT_SWITCH, TX);
            // wait TS_BB2
            Wait(TS_BB2);
        }
        else if((mode == RF_RECEIVER) && (PreMode == RF_TRANSMITTER)){
            PreMode = RF_RECEIVER;
            clear_bit(RegPBOut, (MODE1+MODE0));
            set_bit(RegPBOut, MODE2);
            set_bit(ANT_SWITCH, RX);
            clear_bit(ANT_SWITCH, TX);
            // wait TS_BB2
            Wait(TS_BB2);
        }
        else if(mode == RF_SLEEP){
            PreMode = RF_SLEEP;
            clear_bit(RegPBOut, (MODE2+MODE1+MODE0));
            clear_bit(ANT_SWITCH, (RX+TX));
        }
        else{
            PreMode = RF_SLEEP;
            clear_bit(RegPBOut, (MODE2+MODE1+MODE0));
            clear_bit(ANT_SWITCH, (RX+TX));
        }
        set_bit(PORTO, EN);
    }
} // void SetRFMode(_U8 mode)

/*******************************************************************
** WriteRegister : Writes the register value at the given address **
**                  on the XE1202                                 **
********************************************************************
** In  : address, value                                           **
** Out : -                                                        **
*******************************************************************/
void WriteRegister(_U8 address, _U16 value){
    _U8 i;

    clear_bit(PORTO, EN);
    SrtInit();
    set_bit(PORTO, SO+SI+SCK);

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

    set_bit(PORTO, SO+SI+EN+SCK);
} // void WriteRegister(_U8 address, _U16 value)

/*******************************************************************
** ReadRegister : Reads the register value at the given address on**
**                the XE1202                                      **
********************************************************************
** In  : address                                                  **
** Out : value                                                    **
*******************************************************************/
_U16 ReadRegister(_U8 address){
    _U8 i;
    _U8 value = 0;

    clear_bit(PORTO, EN);
    SrtInit();
    set_bit(PORTO, SO+SI+SCK);

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

    set_bit(PORTO, SO+SI+EN+SCK);

    return value;
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
void SendRfFrame(_U8 *buffer, _U8 size, _U8 *pReturnCode){
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
** ReceiveRfFrame : Receives a RF frame                           **
********************************************************************
** In  : -                                                        **
** Out : *buffer, size, *pReturnCode                              **
*******************************************************************/
void ReceiveRfFrame(_U8 *buffer, _U8 *size, _U8 *pReturnCode){
    _U8  dummy = 0;

    pRFFrame = buffer;
    RFFramePos = 0;
    RFFrameSize = 2;
    SyncByte = 0;

    SetRFMode(RF_RECEIVER);
    RxEventsOn();                           // Enable events (timeout)

	do{
		if((RegEvn & 0x80) == 0x80){        // Tests if counter A generates an event (timeout)
			RxEventsOff();
			*size = RFFrameSize;
			*pReturnCode = RX_TIMEOUT;
			return;                         // Returns the status TimeOut                     
		}
	}while((RegPAIn & PATTERN) == 0x00);	// Waits The Pattern detection of the XE1202 

	RegEvn = 0x90;                           // Clears the event from the CntA and PA1 on the event register
	asm("and %stat, #0xDE");                 // Clears the event on the CoolRISC status register, and disable all interrupts

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

    while(RFFramePos < RFFrameSize + 1){
        if(SyncByte == SYNC_BYTE_FREQ){
            if(EnableSyncByte){
                SyncByte = 0;
                dummy = ReceiveByte();
                RFFramePos--;
            }
        }
        else{
            pRFFrame[RFFramePos-1] = ReceiveByte();
            if(EnableSyncByte){
                SyncByte++;
            }
        }
        if ((RegEvn & 0x80) == 0x80){       // Tests if counter A generates an event
            RxEventsOff();
            *size = RFFrameSize;
            *pReturnCode = RX_TIMEOUT;
            return ;                        // Returns the status TimeOut
        }
        RFFramePos++;
    }

    if(RFFrameSize >= RF_BUFFER_SIZE_MAX){
        *size = RFFrameSize;
        *pReturnCode = ERROR;
        return;
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
    if(RegPAIn & DATAOUT){
        b |= 0x01;
    }
	// Bit 1
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPAIn & DATAOUT){
        b |= 0x02;
    }
	// Bit 2
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPAIn & DATAOUT){
        b |= 0x04;
    }
	// Bit 3
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPAIn & DATAOUT){
        b |= 0x08;
    }
	// Bit 4
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPAIn & DATAOUT){
        b |= 0x10;
    }
	// Bit 5
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPAIn & DATAOUT){
        b |= 0x20;
    }
	// Bit 6
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPAIn & DATAOUT){
        b |= 0x40;
    }
	// Bit 7
    do{
        asm("halt");
    }while ((RegEvn & 0x90) == 0x00); // Waits the event from the PortA or the counter A
    RegEvn = 0x10;                    // Clears the event from the PortA on the event register
    asm("clrb %stat, #0");            // Clears the event on the CoolRISC status register
    if(RegPAIn & DATAOUT){
        b |= 0x80;
    }
    return b;
} // _U8 ReceiveByte(void)

/*******************************************************************
** Transceiver specific functions                                 **
*******************************************************************/

/*******************************************************************
** AutoFreqControl : Calibrates the receiver LO frequency to the  **
**               transmitter LO frequency                         **
********************************************************************
** In  : -                                                        **
** Out : *pReturnCode                                             **
*******************************************************************/
void AutoFreqControl(_U8 *pReturnCode){
    _U8 done = false;
    _U8 timeOut = 20;
    _U8 temp, mode, fDev = 10;
    _U32 bitRate;
    _F32 fErr = 4.8;                // fErr(nb step) = bitRate * 0.5 / SynthesizerStep, SynthesizerStep = 500

    // Reads the RTParam2 register to know which kind of FEI we will use
    mode    = (ReadRegister(REG_RTPARAM2) >> 4) & 0x01;
    // Reads the FSParam1 register to know the bitRate and the
    // frequency deviation
    temp    = ReadRegister(REG_FSPARAM1);
    switch(temp & 0x07){
    case 0:
        bitRate = 4800;
        fErr = 4.8;
        break;
    case 1:
        bitRate = 9600;
        fErr = 9.6;
        break;
    case 2:
        bitRate = 19200;
        fErr = 19.2;
        break;
    case 3:
        bitRate = 38400;
        fErr = 38.4;
        break;
    case 4:
        bitRate = 76800;
        fErr = 76.8;
        break;
    case 5:
        bitRate = 100000;
        fErr = 100.0;
        break;
    }
    temp    = (temp >> 3) & 0x07;
    switch(temp){
    case 0:
        fDev = 10;
        break;
    case 1:
        fDev = 20;
        break;
    case 2:
        fDev = 40;
        break;
    case 3:
        fDev = 80;
        break;
    case 4:
        fDev = 200;
        break;
    }

    // Initializes the FEI to enable his reading
    InitFei();

    if(mode){
        // Correlator
        _S8 result = 0, prevResult = 0;
        _S16 loRegisterValue = 0;

        // First FEI and LO register reading
        result = (_U8)ReadFei();
        loRegisterValue = (_S16)ReadLO();
        prevResult = result;
        // Do while the FEI doesn't returns OK
        do{
            InitFei();
            result = (_U8)ReadFei();
            // FEI reading result test
            if(result == 2){ // LOW
                if(prevResult == result){
                    loRegisterValue = (_S16)((_F32)loRegisterValue - (_F32)fErr);
                }
                else{
                    loRegisterValue = (_S16)((_F32)loRegisterValue - (_F32)fErr/(_F32)2);
                }
            }
            else if(result == 3){ // HIGH
                if(prevResult == result){
                    loRegisterValue = (_S16)((_F32)loRegisterValue + (_F32)fErr);
                }
                else{
                    loRegisterValue = (_S16)((_F32)loRegisterValue + (_F32)fErr/(_F32)2);
                }
            }
            else if(result == 0){ // OK
                done = true;
            }
            prevResult = result;
            // Updates the LO register with the new value
            WriteLO(loRegisterValue);
            timeOut --;
            if(timeOut == 0){
                *pReturnCode = RX_TIMEOUT;
                return;
            }
        }while(!done);
    }
    else{
        // Demodulator
        _S16 result = 0;
        _S16 loRegisterValue = 0;

        timeOut = 20;
        do{
            result = ReadFei();

            loRegisterValue = ReadLO();
            if(result <= -120){
                loRegisterValue = (_S16)((_F32)loRegisterValue + (_F32)fDev);
            }
            if((result <= -20) && (result > -120)){
                loRegisterValue = (_S16)((_F32)loRegisterValue - (((_F32)0.1 / (_F32)107) * (_F32)result - (_F32)0.88 * (_F32)fDev));
            }
            if((result > -20) && (result < 20)){
                loRegisterValue = (_S16)((_F32)loRegisterValue - (((_F32)0.9 / (_F32)20) * (_F32)fDev * (_F32)result));
            }
            if((result >= 20) && (result < 120)){
                loRegisterValue = (_S16)((_F32)loRegisterValue - (((_F32)0.1 / (_F32)107) * (_F32)result + (_F32)0.88 * (_F32)fDev));
            }
            if(result >= 120){
                loRegisterValue = (_S16)((_F32)loRegisterValue - (_F32)fDev);
            }
            if((result >= -1) && (result <= 1)){
               done = true;
            }
            WriteLO(loRegisterValue);

            timeOut --;
            if(timeOut == 0){
                *pReturnCode = RX_TIMEOUT;
                return;
            }
        }while(!done);
    }
    *pReturnCode = OK;
} // void AutoFreqControl(_U8 *pReturnCode)
 
/*******************************************************************
** ReadLO : Reads the LO frequency value from  XE1202             **
********************************************************************
** In  : -                                                        **
** Out : value                                                    **
*******************************************************************/
_U16 ReadLO(void){
    _U16 value;
    value = ReadRegister(REG_FSPARAM2) << 8;
    value |= ReadRegister(REG_FSPARAM3) & 0xFF;
    return value;
} // _U16 ReadLO(void)

/*******************************************************************
** WriteLO : Writes the LO frequency value on the XE1202          **
********************************************************************
** In  : value                                                    **
** Out : -                                                        **
*******************************************************************/
void WriteLO(_U16 value){
    WriteRegister(REG_FSPARAM2, (_U8) (value >> 8));
    WriteRegister(REG_FSPARAM3, (_U8) value);
} // void WriteLO(_U16 value)

/*******************************************************************
** InitFei : Initializes the XE1202 to enable the FEI reading     **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitFei(void){
    _U8 bitRate = 0;
    _U8 mode = 0;

    // Reads the RTParam2 register to know which kind of FEI we will use
    mode = (ReadRegister(REG_RTPARAM2) >> 4) & 0x01;
    // Reads the FSParam1 register to know the bitRate
    bitRate = ReadRegister(REG_FSPARAM1) & 0x07;

    if(mode){
        // Correlator
        clear_bit(PORTO, EN);
        set_bit(PORTO, EN);
        switch(bitRate){
            case 0:
                Wait(TS_FE_04_8_KB_CORR);
                break;
            case 1:
                Wait(TS_FE_09_6_KB_CORR);
                break;
            case 2:
                Wait(TS_FE_19_2_KB_CORR);
                break;
            case 3:
                Wait(TS_FE_38_4_KB_CORR);
                break;
            case 4:
                Wait(TS_FE_76_8_KB_CORR);
                break;
        }
    }
    else{
        // Demodulator
        clear_bit(PORTO, EN);
        set_bit(PORTO, EN);
    }
} // void InitFei(void)

/*******************************************************************
** ReadFei : Reads the FEI value from  XE1202                     **
********************************************************************
** In  : -                                                        **
** Out : value                                                    **
*******************************************************************/
_S16 ReadFei(void){
    _S16 value;
    _U16 tempValue;
    _U8 mode = 0;
    _U8 bitRate = 0;
    _U16 i, tsFeDelay = 0;

    // Reads the RTParam2 register to know which kind of FEI we will use
    mode = (ReadRegister(REG_RTPARAM2) >> 4) & 0x01;

    if(!mode){
        // Demodulator
        // Reads the FSParam1 register to know the bitRate
        bitRate = ReadRegister(REG_FSPARAM1) & 0x07;
        switch(bitRate){
            case 0:
                tsFeDelay = TS_FE_04_8_KB_DEMOD;
                break;
            case 1:
                tsFeDelay = TS_FE_09_6_KB_DEMOD;
                break;
            case 2:
                tsFeDelay = TS_FE_19_2_KB_DEMOD;
                break;
            case 3:
                tsFeDelay = TS_FE_38_4_KB_DEMOD;
                break;
            case 4:
                tsFeDelay = TS_FE_76_8_KB_DEMOD;
                break;
        }
        for(i = 0, value = 0; i < 4; i++){
            set_bit(PORTO, DATAIN);
            Wait(50); //Wait 20 us min for DATAIN
            clear_bit(PORTO, DATAIN);
            Wait(tsFeDelay);
            if((tempValue = ReadRegister(REG_DATAOUT)) & 0x80){
                tempValue |= 0xFF00;
            }
            else{
                tempValue &= 0x00FF;
            }
            value += tempValue;
        }
        value = ((_F32)value / (_F32)4);
        return value;
    }
    // Correlator
    set_bit(PORTO, DATAIN);
    Wait(50); //Wait 20 us min for DATAIN
    clear_bit(PORTO, DATAIN);
    value = (ReadRegister(REG_DATAOUT) >> 4) & 0x0003;
    return value;
} // _S16 ReadFei(void)

/*******************************************************************
** InitRssi : Initializes the XE1202 to enable the RSSI reading   **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitRssi(void){
    clear_bit(PORTO, EN);
    set_bit(PORTO, EN);
    Wait(TS_RS);
} // void InitRssi(void)

/*******************************************************************
** ReadRssi : Reads the Rssi value from  XE1202                   **
********************************************************************
** In  : -                                                        **
** Out : value                                                    **
*******************************************************************/
_U16 ReadRssi(void){
    _U16 value;
    toggle_bit(PORTO, DATAIN);
    Wait(50); //Wait 20 us min for DATAIN
    toggle_bit(PORTO, DATAIN);
    value = ReadRegister(REG_DATAOUT) >> 6;  // Reads the RSSI result
    return value;
} // _U16 ReadRssi(void)

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
