/*****************************************************************************
 *
 * LCD Driver for PIC24.
 *
 *****************************************************************************
 * FileName:        lcd.h
 * Dependencies:    
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
 * Alkhimenok           10/28/05    added some comments
 *****************************************************************************/

// Display line length.
#define LCD_DISPLAY_LEN 16

// Interface variables used by control macros.
extern unsigned int     _uLCDloops;
extern unsigned char 	_uLCDchar;
extern unsigned char	_uLCDstate;

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
void LCDProcessEvents(void);

/*****************************************************************************
 * Macro: mLCDIsBusy
 *
 * Preconditions: None.
 *
 * Overview: Query if the LCD is busy processing.
 *
 * Input: None.
 *
 * Output: Macro returns zero if LCD is not busy.
 *
 *****************************************************************************/
#define mLCDIsBusy() 				_uLCDstate

/*****************************************************************************
 * Macro: mLCDInit
 *
 * Preconditions: None.
 *
 * Overview: Init the LCD.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
#define mLCDInit() 					_uLCDstate = 2;

/*****************************************************************************
 * Macro: mLCDPutChar
 *
 * Preconditions: Call of mLCDInit must be done.
 *
 * Overview: Put a character on the display.
 *
 * Input: Character.
 *
 * Output: None.
 *
 *****************************************************************************/
#define mLCDPutChar(__lcd_char) 	_uLCDchar = __lcd_char; _uLCDstate = 3;

/*****************************************************************************
 * Macro: mLCDPutChar
 *
 * Preconditions: None.
 *
 * Overview: Send a generic command.
 *
 * Input: Command.
 *
 * Output: None.
 *
 *****************************************************************************/
#define mLCDPutCmd(__lcd_cmd)		_uLCDchar = __lcd_cmd; _uLCDstate = 6;

/*****************************************************************************
 * Macro: mLCDClear
 *
 * Preconditions: None.
 *
 * Overview: Clear the display.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
#define mLCDClear() 				_uLCDstate = 4;

/*****************************************************************************
 * Macro: mLCDHome
 *
 * Preconditions: None.
 *
 * Overview: Home the display.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
#define mLCDHome() 					_uLCDstate = 5; _uLCDchar = 0;

/*****************************************************************************
 * Macro: mLCDEMode
 *
 * Preconditions: None.
 *
 * Overview: Set the mode, dir and shift.
 *
 * Input: Command.
 *
 * Output: None.
 *
 *****************************************************************************/
#define mLCDEMode(__lcd_cmd)		_uLCDchar = __lcd_cmd | 0x04; _uLCDstate = 6;

/*****************************************************************************
 * Macro: mLCDCtl
 *
 * Preconditions: None.
 *
 * Overview: Set the display control, on/off, cursor.
 *
 * Input: Command.
 *
 * Output: None.
 *
 *****************************************************************************/
#define mLCDCtl(__lcd_cmd)			_uLCDchar = __lcd_cmd | 0x08; _uLCDstate = 6;

// 
#define mLCDShift(__lcd_cmd)		_uLCDchar = __lcd_cmd | 0x10; _uLCDstate = 6;

/*****************************************************************************
 * Macro: mLCDFSet
 *
 * Preconditions: None.
 *
 * Overview: Set the interface.
 *
 * Input: Address.
 *
 * Output: None.
 *
 *****************************************************************************/
#define mLCDFSet(__lcd_cmd)			_uLCDchar = __lcd_cmd | 0x20; _uLCDstate = 6;

/*****************************************************************************
 * Macro: mLCDCDAddr
 *
 * Preconditions: None.
 *
 * Overview: Set the CGRAM address.
 *
 * Input: Address.
 *
 * Output: None.
 *
 *****************************************************************************/
#define mLCDCDAddr(__lcd_addr)		_uLCDchar = __lcd_addr | 0x40; _uLCDstate = 6;

/*****************************************************************************
 * Macro: mLCDAddr
 *
 * Preconditions: None.
 *
 * Overview: Set the DDRAM address.
 *
 * Input: Address.
 *
 * Output: None.
 *
 *****************************************************************************/
#define mLCDAddr(__lcd_addr) 		_uLCDchar = __lcd_addr | 0x80; _uLCDstate = 6;

/*****************************************************************************
 * EOF
 *****************************************************************************/

