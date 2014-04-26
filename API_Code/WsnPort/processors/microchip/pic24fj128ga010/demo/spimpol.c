/************************************************************************
*                                                                       *
* This implements a generic library functionality to support            *
* SPI Master for dsPIC/PIC24 family                                     *
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
#include "system.h"

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
void SPIMPolInit(){
    SPISTAT = 0;
    SPICON = (SPIM_PPRE|SPIM_SPRE); 
    SPICONbits.MSTEN = 1; 
    SPICON2 = 0;
    SPICONbits.MODE16 = SPIM_MODE16;
    SPICONbits.CKE = SPIM_CKE;
    SPICONbits.CKP = SPIM_CKP;
    SPICONbits.SMP = SPIM_SMP;
    SPIINTENbits.SPIIE = 0;
    SPIINTFLGbits.SPIIF = 0;
    SPISTATbits.SPIEN = 1;
}

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
unsigned SPIMPolPut(unsigned Data)
{
#ifndef SPIM_BLOCKING_FUNCTION

    if(SPISTATbits.SPITBF)
        return SPIM_STS_WRITE_COLLISION;
    SPIBUF = Data;
    return 0;    

#else

    // Wait for a data byte reception
    while(SPISTATbits.SPITBF);
    SPIBUF = Data;
    return 0;

#endif
}

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
unsigned SPIMPolIsTransmitOver(){
#ifndef  SPIM_BLOCKING_FUNCTION

    if(SPISTATbits.SPIRBF == 0)
        return SPIM_STS_TRANSMIT_NOT_OVER;
    return 0;

#else

    // Wait for a data byte reception
    while(SPISTATbits.SPIRBF == 0);
    return 0;

#endif
}


