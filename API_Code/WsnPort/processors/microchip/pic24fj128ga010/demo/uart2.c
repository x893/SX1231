/*****************************************************************************
 *
 * UART Driver for PIC24.
 *
 *****************************************************************************
 * FileName:        uart2.c
 * Dependencies:    system.h
 * Processor:       PIC24
 * Compiler:       	MPLAB C30
 * Linker:          MPLAB LINK30
 * Company:         Microchip Technology Incorporated
 *
 * Software License Agreement
 *
 * The software supplied herewith by Microchip Technology Incorporated
 * (the "Company") is intended and supplied to you, the Company's
 * customer, for use solely and exclusively with products manufactured
 * by the Company. 
 *
 * The software is owned by the Company and/or its supplier, and is 
 * protected under applicable copyright laws. All rights are reserved. 
 * Any use in violation of the foregoing restrictions may subject the 
 * user to criminal sanctions under applicable laws, as well as to 
 * civil liability for the breach of the terms and conditions of this 
 * license.
 *
 * THIS SOFTWARE IS PROVIDED IN AN "AS IS" CONDITION. NO WARRANTIES, 
 * WHETHER EXPRESS, IMPLIED OR STATUTORY, INCLUDING, BUT NOT LIMITED 
 * TO, IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
 * PARTICULAR PURPOSE APPLY TO THIS SOFTWARE. THE COMPANY SHALL NOT, 
 * IN ANY CIRCUMSTANCES, BE LIABLE FOR SPECIAL, INCIDENTAL OR 
 * CONSEQUENTIAL DAMAGES, FOR ANY REASON WHATSOEVER.
 *
 * A simple UART polled driver 
 *
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Anton Alkhimenok		10/18/05	...
 * Brant Ivey			 3/14/06	Added support for PIC24FJ64004 PIM
 *****************************************************************************/
#include "system.h"

/*****************************************************************************
 * U2BRG register value and baudrate mistake calculation
 *****************************************************************************/
#define BAUDRATEREG2 SYSCLK/32/BAUDRATE2-1

#if BAUDRATEREG2 > 255
#error Cannot set up UART2 for the SYSCLK and BAUDRATE.\
 Correct values in main.h and uart2.h files.
#endif

#define BAUDRATE_MISTAKE 1000*(BAUDRATE2-SYSCLK/32/(BAUDRATEREG2+1))/BAUDRATE2
#if (BAUDRATE_MISTAKE > 2)||(BAUDRATE_MISTAKE < -2)
#error UART2 baudrate mistake is too big  for the SYSCLK\
 and BAUDRATE2. Correct values in uart2.c file.
#endif 

/*****************************************************************************
 * Function: UART2Init
 *
 * Precondition: None.
 *
 * Overview: Setup UART2 module.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
void UART2Init()
{
    // Set directions of UART IOs
	UART2_TX_TRIS = 0;
	UART2_RX_TRIS = 1;
	U2BRG = BAUDRATEREG2;
	U2MODE = 0;
	U2STA = 0;
	U2MODEbits.UARTEN = 1;
	U2STAbits.UTXEN = 1;
  	// reset RX flag
 	IFS1bits.U2RXIF = 0;
}

/*****************************************************************************
 * Function: UART2PutChar
 *
 * Precondition: UART2Init must be called before.
 *
 * Overview: Wait for free UART transmission buffer and send a byte.
 *
 * Input: Byte to be sent.
 *
 * Output: None.
 *
 *****************************************************************************/
void  UART2PutChar(char Ch){
    // wait for empty buffer  
    while(U2STAbits.UTXBF == 1);
      U2TXREG = Ch;
}

/*****************************************************************************
 * Function: UART2IsPressed
 *
 * Precondition: UART2Init must be called before.
 *
 * Overview: Check if there's a new byte in UART reception buffer.
 *
 * Input: None.
 *
 * Output: Zero if there's no new data received.
 *
 *****************************************************************************/
char UART2IsPressed()
{
    if(IFS1bits.U2RXIF == 1)
        return 1;
    return 0;
}

/*****************************************************************************
 * Function: UART2GetChar
 *
 * Precondition: UART2Init must be called before.
 *
 * Overview: Wait for a byte.
 *
 * Input: None.
 *
 * Output: Byte received.
 *
 *****************************************************************************/
char UART2GetChar(){
char Temp;
    while(IFS1bits.U2RXIF == 0);
    Temp = U2RXREG;
    IFS1bits.U2RXIF = 0;
    return Temp;
}

/*****************************************************************************
 * Function: UART2PutDec
 *
 * Precondition: UART2Init must be called before.
 *
 * Overview: This function converts decimal data into a string
 * and outputs it into UART.
 *
 * Input: Binary data.
 *
 * Output: None.
 *
 *****************************************************************************/
void  UART2PutDec(unsigned char Dec){
unsigned char Res;
    Res = Dec;

    if(Res/100) 
        UART2PutChar(Res/100+'0');
    Res = Res - (Res/100)*100;

    if(Res/10) 
        UART2PutChar(Res/10+'0');
    Res = Res - (Res/10)*10;
 
    UART2PutChar(Res+'0');
}

/*****************************************************************************
 * EOF
 *****************************************************************************/
