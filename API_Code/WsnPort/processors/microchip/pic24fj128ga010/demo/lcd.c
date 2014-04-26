/*****************************************************************************
 *
 * LCD Driver for PIC24.
 *
 *****************************************************************************
 * FileName:        lcd.c
 * Dependencies:    system.h, uart2.c
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
 *
 * A simple LCD driver for LCDs interface through the PMP
 * 
 *
 *
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Ross Fosler			08/13/04	...	
 * Anton Alkhimenok		10/18/05	added message copying to UART	
 * Brant Ivey			3/14/06		Changed PMPEN register to PMAEN
 *****************************************************************************/
#include "system.h"

// Define a fast instruction execution time in terms of loop time
// typically > 43us
#define	LCD_F_INSTR		10

// Define a slow instruction execution time in terms of loop time
// typically > 1.35ms
#define	LCD_S_INSTR		150

// Define the startup time for the LCD in terms of loop time
// typically > 30ms
#define	LCD_STARTUP		2000


#define		_LCD_IDLE(__cnt)	_uLCDstate = 1; _uLCDloops = __cnt;
#define		_LCD_INIT(__cnt)	_uLCDstate++; _uLCDloops = __cnt;


unsigned int	_uLCDloops;
unsigned char	_uLCDstate;
unsigned char 	_uLCDchar;

/*****************************************************************************
 * Function: LCDProcessEvents
 *
 * Preconditions: None.
 *
 * Overview: This is a state mashine to issue commands and data to LCD. Must be
 * called periodically to make LCD message processing.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
void LCDProcessEvents(void)
{
	switch (_uLCDstate) {
		case 1:								// *** wait *** 
			if (_uLCDloops) _uLCDloops--;
			else _uLCDstate = 0;
			break;
		case 2:								// *** init ***
			PMCON = 0x83BF;				    // Setup the PMP
			PMMODE = 0x3FF;
			PMAEN = 0x0001;
			PMADDR = 0x0000;
			_uLCDstate = 64;				// Set the next state
			_uLCDloops = LCD_STARTUP;		// Set the entry delay
			break;
		case 3:								// *** put ***
			_LCD_IDLE(LCD_F_INSTR);
			PMADDR = 0x0001;
			PMDIN1 = _uLCDchar;
            UART2PutChar(_uLCDchar);        // Copy character to UART
			break;
		case 4:								// *** clear ***
			_LCD_IDLE(LCD_S_INSTR);
			PMADDR = 0x0000;
			PMDIN1 = 0b00000001;
            UART2PutChar('\r');             // Send return to UART
			break;
		case 5:								// *** home ***
			_LCD_IDLE(LCD_S_INSTR);
			PMADDR = 0x0000;
			PMDIN1 = 0b00000010;
            UART2PutChar('\r');             // Send return to UART
			break;	
		case 6:								// *** command ***
			_LCD_IDLE(LCD_F_INSTR);
			PMADDR = 0x0000;
			PMDIN1 = _uLCDchar;
			break;

		// This is the LCD init state machine
		case 64:							// Standard delay states for the init
		case 66:
		case 68:
		case 70:							// Programmable delay loop
			if (_uLCDloops) _uLCDloops--;
			else _uLCDstate++;
			break;
		case 65:
			_LCD_INIT(LCD_F_INSTR);
			PMDIN1 = 0b00111100;			// Set the default function
			break;
		case 67:
			_LCD_INIT(LCD_F_INSTR);
			PMDIN1 = 0b00001100;            // Set the display control
			break;
		case 69:	
			_LCD_INIT(LCD_S_INSTR);
			PMDIN1 = 0b00000001;			// Clear the display
			break;
		case 71:
			_LCD_INIT(LCD_S_INSTR);
			PMDIN1 = 0b00000110;			// Set the entry mode
			break;
		case 72:
			_uLCDstate = 0;
			break;
		default:
			_uLCDstate = 0;
			break;
	}
}
/*****************************************************************************
 * EOF
 *****************************************************************************/
