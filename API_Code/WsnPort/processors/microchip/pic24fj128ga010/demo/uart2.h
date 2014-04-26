/*****************************************************************************
 *
 * UART Driver for PIC24.
 * Modified for PIC24FJ64GA004 family with PPS.
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
 * Brant Ivey			 3/14/06	Modified for PIC24FJ64GA004 family with PPS.
 *****************************************************************************/

/*****************************************************************************
 * DEFINITIONS
 *****************************************************************************/
// Baudrate
#define BAUDRATE2		19200

// UART IOs
#ifdef  __PIC24FJ64GA004__
#define UART2_TX_TRIS   PPS_UART2_TX_TRIS
#define UART2_RX_TRIS   PPS_UART2_RX_TRIS
#else
#define UART2_TX_TRIS   TRISFbits.TRISF5
#define UART2_RX_TRIS   TRISFbits.TRISF4
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
extern void UART2Init();

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
extern void  UART2PutChar(char Ch);

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
extern char UART2IsPressed();

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
extern char UART2GetChar();

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
extern void  UART2PutDec(unsigned char Dec);

/*****************************************************************************
 * EOF
 *****************************************************************************/
