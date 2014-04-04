/*******************************************************************
** File        : Initialisation.c                                 **
********************************************************************
**                                                                **
** Version     :                                                  **
**                                                                **
** Writen by   :                                                  **
**                                                                **
** Date        : XX-XX-XXXX                                       **
**                                                                **
** Project     : -                                                **
**                                                                **
********************************************************************
** Changes     :                                                  **
********************************************************************
** Description : Initialisation settings for the uController      **
*******************************************************************/
#include "Initialisation.h"

/*******************************************************************
** InitMicro : Initialises the MicroController peripherals        **
********************************************************************
** In        : -                                                  **
** Out       : -                                                  **  
*******************************************************************/
void InitMicro(void)
{
	// Microcontroller peripherals intialisation
	InitPortA();                      // Initialize PORTA
	InitPortB();                      // Initialize PORTB
#if defined(_XE88LC01_) || defined(_XE88LC03_) || defined(_XE88LC05_)
	InitPortC();                      // Initialize PORTC
#endif

#ifdef _XE88LC02_
	InitPortD1();                     // Initialize PORTD1
	InitPortD2();                     // Initialize PORTD2
	InitLCD();						  // Initialize LCD
#endif

#ifdef _XE88LC06A_
	InitPortD();                      // Initialize PORTD
#endif

	InitCounters();                   // Initialize Counter
// The two function below may not be used together since the InitUart() 
// function uses the InitRCOscillator()  
	InitRCOscillator(2457600);		  // Initialises the RC oscillator
    /*******************************************************************
    ** Parameters :                                                   **
    ** Baudrate : 115200, 57600, 38400, 19200, 9600, 4800, 2400,      **
    **             1200,600, 300                                bauds **
    ** Data Len : 1 = 8, 0 = 7                                  bits  **
    ** Parity   : 0 = ODD, 1 = EVEN                                   **
    ** ParityEn : Enables or disables the parity                      **
    ** rcFactor : 1, 2, 4, 8, 16 times the default rc frequency       **
    *******************************************************************/	
//    InitUART(19200, 1, 0, 0, 2);      // Initialises the UART communications
}

/*******************************************************************
** Common Peripherals to all the XE8000 family	                  **
********************************************************************/

/*******************************************************************
**  PA7  *  PA6  *  PA5  *  PA4  *  PA3  *  PA2  *  PA1  *  PA0   **
**   -   *   -   *   -   *   -   *   -   *   -   *   -   *   -    **
*******************************************************************/
/*******************************************************************
** InitPortA : Initialises the Port A                             **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitPortA(void){
    RegPADebounce   = 0x00; // debounce
    RegPAEdge       = 0x00; // Rising Edge
    RegPAPullup     = 0x00; // Pullups
    RegPARes0       = 0x00; // No reset selection
    RegPARes1       = 0x00; // No reset selection
}

/*******************************************************************
**  PB7  *  PB6  *  PB5  *  PB4  *  PB3  *  PB2  *  PB1  *  PB0   **
**   -   *   -   *  SDA  *  SCL  *   -   *   -   *   -   *   -    **
*******************************************************************/
/*******************************************************************
** InitPortB : Initialises the Port B                             **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitPortB(void){
    RegPBOut        = 0x00; // 
    RegPBDir        = 0xFF; // Sets PortB pins as outputs
    RegPBOpen       = 0x00; // Port B hasn't open drain outputs
    RegPBPullup     = 0x00; // No Pullups
    RegPBAna        = 0x00; // Port B in Digital mode
}

/*******************************************************************
** InitCounters : Initialises the Counters                        **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitCounters(void){
	RegCntCtrlCk	= 0x00;
	RegCntConfig1	= 0x00;
	RegCntConfig2	= 0x00;
	RegCntA			= 0x00;
	RegCntB			= 0x00;
	RegCntC			= 0x00;
	RegCntD			= 0x00;
}


/*******************************************************************
** InitRCOscillator : Initialises the RC oscillator (DFLL)        **
********************************************************************
** In  : Frequency (in Hz)                                        **
** Out : -                                                        **
*******************************************************************/
void InitRCOscillator(unsigned long Frequency){
	// Enable XTAL
	RegSysClock = (RegSysClock & 0xFC) | 0x03;
	// Wait for Cold XTAL
	while ((RegSysClock & 0x08) == 0x08);

	DFLLRun(Frequency);
}

/*******************************************************************
** Parameters :                                                   **
** Baudrate : 115200, 57600, 38400, 19200, 9600, 4800, 2400,      **
**             1200,600, 300                                bauds **
** Data Len : 1 = 8, 0 = 7                                  bits  **
** Parity   : 0 = ODD, 1 = EVEN                                   **
** ParityEn : Enables or disables the parity                      **
** rcFactor : 1, 2, 4, 8, 16 times the default rc frequency       **
*******************************************************************/

/*******************************************************************
** InitUART : Initialises the UART                                **
********************************************************************
** In  : baudrate, dataLen, parity, parityEn, rcFactor            **
** Out : -                                                        **
*******************************************************************/
void InitUART(_U32 baudrate, _U8 dataLen, _U8 parity, _U8 parityEn,
              _U8 rcFactor){
    RegUartCmd = 0;
    RegUartCtrl = 0;

    switch(baudrate){
        case 115200:
            InitRCOscillator(1843200*rcFactor);
            RegUartCmd &= ~UART_SELXTAL;                                // XTAL is not selected
            switch(rcFactor){
                case 1:
                    RegUartCmd &= ~UART_RCSEL_2 & ~UART_RCSEL_1 & ~UART_RCSEL_0;
                    break;
                case 2:
                    RegUartCmd |= UART_RCSEL_0;
                    break;
                case 4:
                    RegUartCmd |= UART_RCSEL_1;
                    break;
                case 8:
                    RegUartCmd |= UART_RCSEL_1 | UART_RCSEL_0;
                    break;
                case 16:
                    RegUartCmd |= UART_RCSEL_2;
                    break;
            }
            RegUartCtrl |= UART_BR_2 | UART_BR_1 | UART_BR_0;
            break;
        case 57600:
            InitRCOscillator(1843200*rcFactor);
            RegUartCmd &= ~UART_SELXTAL;
            switch(rcFactor){
                case 1:
                    RegUartCmd &= ~UART_RCSEL_2 & ~UART_RCSEL_1 & ~UART_RCSEL_0;
                    break;
                case 2:
                    RegUartCmd |= UART_RCSEL_0;
                    break;
                case 4:
                    RegUartCmd |= UART_RCSEL_1;
                    break;
                case 8:
                    RegUartCmd |= UART_RCSEL_1 | UART_RCSEL_0;
                    break;
                case 16:
                    RegUartCmd |= UART_RCSEL_2;
                    break;
            }
            RegUartCtrl |= UART_BR_2 | UART_BR_1;
            break;
        case 38400:
            InitRCOscillator(614400*rcFactor);
            RegUartCmd &= ~UART_SELXTAL;
            switch(rcFactor){
                case 1:
                    RegUartCmd  &= ~UART_RCSEL_2 & ~UART_RCSEL_1 & ~UART_RCSEL_0;
                    break;
                case 2:
                    RegUartCmd  |= UART_RCSEL_0;
                    break;
                case 4:
                    RegUartCmd  |= UART_RCSEL_1;
                    break;
                case 8:
                    RegUartCmd  |= UART_RCSEL_1 | UART_RCSEL_0;
                    break;
                case 16:
                    RegUartCmd  |= UART_RCSEL_2;
                    break;
            }
            RegUartCtrl |= UART_BR_2 | UART_BR_1 | UART_BR_0;
            break;
        case 19200:
            InitRCOscillator(614400*rcFactor);
            RegUartCmd &= ~UART_SELXTAL;
            switch(rcFactor){
                case 1:
                    RegUartCmd  &= ~UART_RCSEL_2 & ~UART_RCSEL_1 & ~UART_RCSEL_0;
                    break;
                case 2:
                    RegUartCmd  |= UART_RCSEL_0;
                    break;
                case 4:
                    RegUartCmd  |= UART_RCSEL_1;
                    break;
                case 8:
                    RegUartCmd  |= UART_RCSEL_1 | UART_RCSEL_0;
                    break;
                case 16:
                    RegUartCmd  |= UART_RCSEL_2;
                    break;
            }
            RegUartCtrl |= UART_BR_2 | UART_BR_1;
            break;
        case 9600:
            InitRCOscillator(614400*rcFactor);
            RegUartCmd &= ~UART_SELXTAL;
            switch(rcFactor){
                case 1:
                    RegUartCmd  &= ~UART_RCSEL_2 & ~UART_RCSEL_1 & ~UART_RCSEL_0;
                    break;
                case 2:
                    RegUartCmd  |= UART_RCSEL_0;
                    break;
                case 4:
                    RegUartCmd  |= UART_RCSEL_1;
                    break;
                case 8:
                    RegUartCmd  |= UART_RCSEL_1 | UART_RCSEL_0;
                    break;
                case 16:
                    RegUartCmd  |= UART_RCSEL_2;
                    break;
            }
            RegUartCtrl |= UART_BR_2 | UART_BR_0;
            break;
        case 4800:
            InitRCOscillator(614400*rcFactor);
            RegUartCmd &= ~UART_SELXTAL;
            switch(rcFactor){
                case 1:
                    RegUartCmd  &= ~UART_RCSEL_2 & ~UART_RCSEL_1 & ~UART_RCSEL_0;
                    break;
                case 2:
                    RegUartCmd  |= UART_RCSEL_0;
                    break;
                case 4:
                    RegUartCmd  |= UART_RCSEL_1;
                    break;
                case 8:
                    RegUartCmd  |= UART_RCSEL_1 | UART_RCSEL_0;
                    break;
                case 16:
                    RegUartCmd  |= UART_RCSEL_2;
                    break;
            }
            RegUartCtrl |= UART_BR_2;
            break;
        case 2400:
            InitRCOscillator(1228800);
            RegUartCmd |= UART_SELXTAL | UART_RCSEL_0;
            RegUartCtrl |= UART_BR_1 | UART_BR_0;
            break;
        case 1200:
            InitRCOscillator(1228800);
            RegUartCmd |= UART_SELXTAL | UART_RCSEL_0;
            RegUartCtrl |= UART_BR_1;
            break;
        case 600:
            InitRCOscillator(1228800);
            RegUartCmd |= UART_SELXTAL | UART_RCSEL_0;
            RegUartCtrl |= UART_BR_0;
            break;
        case 300:
            InitRCOscillator(1228800);
            RegUartCmd |= UART_SELXTAL | UART_RCSEL_0;
            RegUartCtrl &= UART_BR_2 & ~UART_BR_1 & ~UART_BR_0;
            break;
        default: // Baudrate = 38400
            InitRCOscillator(1228800);
            RegUartCmd |= 0;
            RegUartCtrl |= UART_BR_2 | UART_BR_1 | UART_BR_0;
            break;
    }

    if(parity == 0){
        RegUartCmd &= ~UART_PM;
    }
    else{
        RegUartCmd |= UART_PM;
    }

    if(parityEn == 0){
        RegUartCmd &= ~UART_PE;
    }
    else{
        RegUartCmd |= UART_PE;
    }

    if(dataLen == 0){
        RegUartCmd &= ~UART_WL;
    }
    else{
        RegUartCmd |= UART_WL;
    }

    RegUartCtrl &= ~UART_ECHO;                       // No ECHO
    RegUartCtrl |= UART_ENRX;                        // Enables the reception
    RegUartCtrl |= UART_ENTX;                        // Enables the transmission
    RegUartCtrl &= ~UART_XRX;                        // Don't inverts
    RegUartCtrl &= ~UART_XTX;                        // Don't inverts
}

/*******************************************************************
** Specific Peripherals 						                  **
*******************************************************************/

#ifdef _XE88LC02_ 
/*******************************************************************
** PD1_7 * PD1_6 * PD1_5 * PD1_4 * PD1_3 * PD1_2 * PD1_1 * PD1_0  **
**   -   *   -   *   -   *   -   *   -   *   -   *   -   *   -    **
*******************************************************************/
/*******************************************************************
** InitPortD1 : Initialises the Port D1                           **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitPortD1(void){
    RegPD1Out        = 0x00;
    RegPD1Dir        = 0xFF;
    RegPD1Pullup     = 0x00;
}

/*******************************************************************
** PD2_7 * PD2_6 * PD2_5 * PD2_4 * PD2_3 * PD2_2 * PD2_1 * PD2_0  **
**   -   *   -   *   -   *   -   *   -   *   -   *   -   *   -    **
*******************************************************************/
/*******************************************************************
** InitPortD2 : Initialises the Port D2                           **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitPortD2(void){
    RegPD2Out        = 0x00;
    RegPD2Dir        = 0xFF;
    RegPD2Pullup     = 0x00;
}

/*******************************************************************
** InitLCD : Initialises the LCD                                  **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitLCD(void){
	RegLcdSe        = 0xFF; // LCD function
	RegVgenCfg0     = 0x21; // Multiplier on, vref on. 1/3 bias
	RegLcdClkFrame  = 0xe3; // 4096Hz, Div 8
	RegLcdOn        = 0x03; // Start LCD, mux 1:4
}
#endif //Target LC02

#if defined(_XE88LC01_) || defined(_XE88LC03_) || defined(_XE88LC05_)
/*******************************************************************
**  PC7  *  PC6  *  PC5  *  PC4  * PC3   * PC2   * PC1   * PC0    **
**   -   *   -   *   -   *   -   *   -   *   -   *   -   *   -    **
*******************************************************************/
/*******************************************************************
** InitPortC : Initialises the Port C                             **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitPortC(void){
    RegPCOut        = 0x00;
    RegPCDir        = 0xFF;
}
#endif //Traget LC01, LC03, LC05

#ifdef _XE88LC05_
/*******************************************************************
** ------------------------ Parameters ---------------------------**
** NsOrder  : 0x00 - 0x02             [0 (PWM), 1, 2            ] **
** CodeImax : 0x00 - 0x07             [4, 5, 6, 7, 8, 9, 10, 11 ] **
** Enable   : 0x00 - 0x03             [DAC, AMP                 ] **
** Fin      : 0x00 - 0x01             [RC clock, RC clock div 2 ] **
** BW       : 0x00 - 0x01             [CL0, CL1                 ] **
** Inv      : 0x00 - 0x01             [HIGH, LOW                ] **
*******************************************************************/
/*******************************************************************
** InitDAS : Initialises the D/A Signal                           **
********************************************************************
** In  : NsOrder, CodeImax, Enable, Fin, BW, Inv                  **
** Out : -                                                        **
*******************************************************************/
void InitDAS(_U8 NsOrder, _U8 CodeImax, _U8 Enable, _U8 Fin,
	         _U8 BW, _U8 Inv){
	RegDasCfg0 = (NsOrder << 6) + (CodeImax << 3) + (Enable << 1) +
	             Fin;
	RegDasCfg1 = (BW << 1) + Inv;
}

/*******************************************************************
** ------------------------ Parameters ---------------------------**
** 0x00                                 [DAC OFF, AMP OFF       ] **
** 0x01                                 [DAC ON , AMP OFF       ] **
** 0x02                                 [DAC OFF, AMP ON        ] **
** 0x03                                 [DAC ON , AMP ON        ] **
*******************************************************************/
/*******************************************************************
** InitDAB : Initialises the D/A Bias                             **
********************************************************************
** In  : Enable                                                   **
** Out : -                                                        **
*******************************************************************/
void InitDAB(_U8 Enable){
	RegDab1Cfg = Enable;		
}
#endif //Target LC05

#ifdef _XE88LC06A_
/*******************************************************************
**  PD7  *  PD6  *  PD5  *  PD4  * PD3   * PD2   * PD1   * PD0    **
**   -   *   -   *   -   *   -   *   -   *   -   *   -   *   -    **
*******************************************************************/
/*******************************************************************
** InitPortD : Initialises the Port D                             **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void InitPortD(void){
    RegPDOut        = 0x00;
    RegPDDir        = 0xFF;
    RegPDPullup     = 0x00;
}
#endif //Target LC06


