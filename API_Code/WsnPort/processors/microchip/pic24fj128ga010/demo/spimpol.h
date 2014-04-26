/************************************************************************
*                                                                       *
* This implements a generic library functionality to support SPI Master *
* for dsPIC/PIC24 family                                                *
*                                                                       *
*************************************************************************
* Company:             Microchip Technology, Inc.                       *
*                                                                       *
* Software License Agreement                                            *
*                                                                       *
* The software supplied herewith by Microchip Technology Incorporated   *
* (the "Company") for its PICmicro® Microcontroller is intended and     *
* supplied to you, the Company's customer, for use solely and           *
* exclusively on Microchip PICmicro Microcontroller products. The       *
* software is owned by the Company and/or its supplier, and is          *
* protected under applicable copyright laws. All rights are reserved.   *
* Any use in violation of the foregoing restrictions may subject the    *
* user to criminal sanctions under applicable laws, as well as to       *
* civil liability for the breach of the terms and conditions of this    *
* license.                                                              *
*                                                                       *
* THIS SOFTWARE IS PROVIDED IN AN "AS IS" CONDITION. NO WARRANTIES,     *
* WHETHER EXPRESS, IMPLIED OR STATUTORY, INCLUDING, BUT NOT LIMITED     *
* TO, IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A           *
* PARTICULAR PURPOSE APPLY TO THIS SOFTWARE. THE COMPANY SHALL NOT,     *
* IN ANY CIRCUMSTANCES, BE LIABLE FOR SPECIAL, INCIDENTAL OR            *
* CONSEQUENTIAL DAMAGES, FOR ANY REASON WHATSOEVER.                     *
*                                                                       *
* Author               Date            Comment                          *
*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
* Alkhimenok           Oct 26, 2005    ....                             *
************************************************************************/
//   USE ONY ONCE   // 
#ifndef _spimpol_h_
#define _spimpol_h_

#ifndef BOARD_VERSION4
#define SPIM_SPI1
#else
#define SPIM_SPI2
#endif

#define SPIM_PIC24
#define SPIM_BLOCKING_FUNCTION
#define SPIM_MODE16 0
#define SPIM_SMP 1
#define SPIM_CKE 0
#define SPIM_CKP 1
#define SPIM_PPRE (unsigned)0
#define SPIM_SPRE (unsigned)0


/************************************************************************
*                                                                       *
* This section defines names of control registers of SPI Module.        *
* Names depends of processor type and module number.                    *
*                                                                       *
************************************************************************/
#ifdef SPIM_SPI1

    #define SPIBUF  SPI1BUF
    #define SPISTAT SPI1STAT
    #define SPIBUFbits  SPI1BUFbits
    #define SPISTATbits SPI1STATbits
    #define SPIINTEN IEC0 
    #define SPIINTFLG IFS0
    #define SPIINTENbits IEC0bits
    #define SPIINTFLGbits IFS0bits
    #define SPIIF SPI1IF
    #define SPIIE SPI1IE
    #define SPICON SPI1CON1
    #define SPICONbits SPI1CON1bits
    #define SPICON2 SPI1CON2
    #define SPICON2bits SPI1CON2bits

#else

    #define SPIBUF  SPI2BUF
    #define SPISTAT SPI2STAT
    #define SPIBUFbits  SPI2BUFbits
    #define SPISTATbits SPI2STATbits
    #define SPIINTEN IEC2 
    #define SPIINTFLG IFS2
    #define SPIINTENbits IEC2bits
    #define SPIINTFLGbits IFS2bits
    #define SPIIF SPI2IF
    #define SPIIE SPI2IE
    #define SPICON SPI2CON1
    #define SPICONbits SPI2CON1bits
    #define SPICON2 SPI2CON2
    #define SPICON2bits SPI2CON2bits

#endif

/************************************************************************
* Error and Status Flags                                                *
* SPIM_STS_WRITE_COLLISION indicates that, Write collision has occurred *
* while trying to transmit the byte.                                    *
*                                                                       *    
* SPIM_STS_TRANSMIT_NOT_OVER indicates that, the transmission is        *
* not yet over. This is to be checked only when non Blocking            *
* option is opted.                                                      *
*                                                                       *    
* SPIM_STS_DATA_NOT_READY indicates that reception SPI buffer is empty  *
* and there's no data avalable yet.                                     * 
*                                                                       *    
************************************************************************/
#define SPIM_STS_WRITE_COLLISION    1
#define SPIM_STS_TRANSMIT_NOT_OVER  2  
#define SPIM_STS_DATA_NOT_READY     3  

/************************************************************************
* Macro: mSPIMPolGet                                                    *
*                                                                       *
* PreCondition: 'SPIMPolIsTransmitOver' should return a '0'.            *
*                                                                       *
* Overview: This macro reads a data received                            *
*                                                                       *
* Input: None                                                           *
*                                                                       *
* Output: Data received                                                 *
*                                                                       *
************************************************************************/
#define  mSPIMPolGet() SPIBUF

/************************************************************************
* Function: SPIMPolInit                                                 *
*                                                                       *
* Preconditions: TRIS bits of SCK and SDO should be made output.        *
* TRIS bit of SDI should be made input. TRIS bit of Slave Chip Select   *
* pin (if any used) should be made output. Overview This function is    *
* used for initializing the SPI module. It initializes the module       *
* according to Application Maestro options.                             *
*                                                                       *
* Input: Application Maestro options                                    *
*                                                                       *
* Output: None                                                          *
*                                                                       *
************************************************************************/           
extern void SPIMPolInit();
/************************************************************************
* Function SPIMPolPut                                                   *
*                                                                       *
* Preconditions: 'SPIMPolInit' should have been called.                 *
* Overview: in non Blocking Option this function sends the byte         *
* over SPI bus and checks for Write Collision; in Blocking Option       *
* it waits for a free transmission buffer.                              *
*                                                                       *
* Input: Data to be sent.                                               *
*                                                                       *
* Output: 'This function returns ‘0’  on proper initialization of       *
* transmission and ‘SPIM_STS_WRITE_COLLISION’ on occurrence of          *
* the Write Collision error.                                            *
*                                                                       *
************************************************************************/           
extern unsigned SPIMPolPut(unsigned Data);
/************************************************************************
* Function: SPIMPolIsTransmitOver                                       *
*                                                                       *
* Preconditions: ‘SPIMPolPut’ should have been called.                  *
* Overview: in non Blocking Option this function checks whether         *
* the transmission of the byte is completed; in Blocking Option         *
* it waits till the transmission of the byte is completed.              *
*                                                                       *
* Input: None                                                           *
*                                                                       *
* Output: in Blocking Option none and in non Blocking Option            *
* it returns: ’0’ - if the transmission is over,                        *
* SPIM_STS_TRANSMIT_NOT_OVER - if the transmission is not yet over.     *
*                                                                       *
************************************************************************/           
extern unsigned SPIMPolIsTransmitOver();

#endif
