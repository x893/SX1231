/************************************************************************
*                                                                       *
*  Basic access to SPI EEPROM 25LC256.                                  *
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
* Function: EEPROMInit                                                  *
*                                                                       *
* Preconditions: SPI module must be configured to operate with          *
*                 parameters: Master, MODE16 = 0, CKP = 1, SMP = 1.     *
*                                                                       *
* Overview: This function setup SPI IOs connected to EEPROM.            *
*                                                                       *
* Input: None.                                                          *
*                                                                       *
* Output: None.                                                         *
*                                                                       *
************************************************************************/
void EEPROMInit()
{
    // Set IOs directions for EEPROM SPI
    EEPROM_SS_PORT = 1;
    EEPROM_SS_TRIS = 0;
    EEPROM_SCK_TRIS = 0;
    EEPROM_SDO_TRIS = 0;
    EEPROM_SDI_TRIS = 1;
}

/************************************************************************
* Function: EEPROMWriteByte()                                           *
*                                                                       *
* Preconditions: SPI module must be configured to operate with  EEPROM. *
*                                                                       *
* Overview: This function writes a new value to address specified.      *
*                                                                       *
* Input: Data to be written and address.                                *
*                                                                       *
* Output: None.                                                         *
*                                                                       *
************************************************************************/
void EEPROMWriteByte(unsigned Data, unsigned Address)
{
    EEPROMWriteEnable();
    mEEPROMSSLow();

    SPIMPolPut(EEPROM_CMD_WRITE);
    SPIMPolIsTransmitOver();
    mSPIMPolGet();

    SPIMPolPut(Hi(Address));
    SPIMPolIsTransmitOver();
    mSPIMPolGet();

    SPIMPolPut(Lo(Address));
    SPIMPolIsTransmitOver();
    mSPIMPolGet();

    SPIMPolPut(Data);
    SPIMPolIsTransmitOver();
    mSPIMPolGet();

    mEEPROMSSHigh();

    // wait for completion of previous write operation
    while(EEPROMReadStatus().Bits.WIP);
}

/************************************************************************
* Function: EEPROMReadByte()                                            *
*                                                                       *
* Preconditions: SPI module must be configured to operate with  EEPROM. *
*                                                                       *
* Overview: This function reads a value from address specified.         *
*                                                                       *
* Input: Address.                                                       *
*                                                                       *
* Output: Data read.                                                    *
*                                                                       *
************************************************************************/
unsigned EEPROMReadByte(unsigned Address){
unsigned Temp;
    mEEPROMSSLow();

    SPIMPolPut(EEPROM_CMD_READ);
    SPIMPolIsTransmitOver();
    mSPIMPolGet();

    SPIMPolPut(Hi(Address));
    SPIMPolIsTransmitOver();
    mSPIMPolGet();

    SPIMPolPut(Lo(Address));
    SPIMPolIsTransmitOver();
    mSPIMPolGet();

    SPIMPolPut(0);
    SPIMPolIsTransmitOver();
    Temp = mSPIMPolGet();

    mEEPROMSSHigh();
    return Temp;
}

/************************************************************************
* Function: EEPROMWriteEnable()                                         *
*                                                                       *
* Preconditions: SPI module must be configured to operate with EEPROM.  *
*                                                                       *
* Overview: This function allows a writing into EEPROM. Must be called  *
* before every writing command.                                         *
*                                                                       *
* Input: None.                                                          *
*                                                                       *
* Output: None.                                                         *
*                                                                       *
************************************************************************/
void EEPROMWriteEnable(){
    mEEPROMSSLow();
    SPIMPolPut(EEPROM_CMD_WREN);
    SPIMPolIsTransmitOver();
    mSPIMPolGet();
    mEEPROMSSHigh();
}

/************************************************************************
* Function: EEPROMReadStatus()                                          *
*                                                                       *
* Preconditions: SPI module must be configured to operate with  EEPROM. *
*                                                                       *
* Overview: This function reads status register from EEPROM.            *
*                                                                       *
* Input: None.                                                          *
*                                                                       *
* Output: Status register value.                                        *
*                                                                       *
************************************************************************/
union _EEPROMStatus_ EEPROMReadStatus(){
char Temp;

    mEEPROMSSLow();
    SPIMPolPut(EEPROM_CMD_RDSR);
    SPIMPolIsTransmitOver();
    mSPIMPolGet();
    SPIMPolPut(0);
    SPIMPolIsTransmitOver();
    Temp = mSPIMPolGet();
    mEEPROMSSHigh();

    return (union _EEPROMStatus_)Temp;
}

/************************************************************************
* EOF
************************************************************************/
