/*****************************************************************************
 *
 * Show Start Banners
 *
 *****************************************************************************
 * FileName:        banner.c
 * Dependencies:    system.h
 * Processor:       PIC24
 * Compiler:        MPLAB C30
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
 * State mashine to display  PIC24 Features. 
 *
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * XXXX                 XXX         ...
 * Anton Alkhimenok     10/18/05	Several additions
 *****************************************************************************/
#include "system.h"

/*****************************************************************************
*  Banners strings.
 *****************************************************************************/
//                  "XXXXXXXXXXXXXXXX";
const char _T1[]  = "Microchip       ";
const char _T2[]  = "Technology, Inc ";

const char _T3[]  = "Presenting the  ";
const char _T4[]  = "PIC24FJ128GA010 ";

const char _T5[]  = "Copyright 2005  ";
const char _T6[]  = "                ";

const char _T7[]  = "16-bit          ";
const char _T8[]  = "Microcontroller ";

const char _T9[]  = "16MIPS / 32MHz  ";
const char _T10[] = "2.0V - 3.6V     ";

const char _T11[] = "Features:       ";
const char _T12[] = "2 SPI modules   ";

const char _T13[] = "2 I2C modules   ";   
const char _T14[] = "2 UARTs w/ IrDA ";

const char _T15[] = "New Parallel    ";
const char _T16[] = "Master Port     ";

const char _T17[] = "500k sample     ";
const char _T18[] = "10-bit A/D      ";

const char _T19[] = "5 PWM or        ";
const char _T20[] = "Output compare  ";

const char _T21[] = "5 Input Capture ";
const char _T22[] = "Real-time clock ";

const char _T23[] = "and calendar    ";
const char _T24[] = "Watchdog Timer  ";

const char _T25[] = "On-chip voltage ";
const char _T26[] = "regulator       ";

const char _T27[] = "5 16-bit timers ";
const char _T28[] = "32-bit options  ";

const char _T29[] = "Many oscillator ";
const char _T30[] = "modes           ";

const char _T31[] = "8MHz internal   ";
const char _T32[] = "oscillator      ";

// Last banner is showed at start only 
const char _T33[] = "  Explorer 16   ";
const char _T34[] = "Development Brd ";

// Specify delay between banners
#define BNR_CHANGE_DELAY 1000
// Quantity of Banners - 2 (last banner is showed at start only and never counted again)
#define BNR_COUNT   32

/*****************************************************************************
*  Array of pointers to banners strings
******************************************************************************/
const char* _pBannersArray[] =
{_T1,_T2,_T3,_T4,_T5,_T6,_T7,_T8,
_T9,_T10,_T11,_T12,_T13,_T14,_T15,_T16,
_T17,_T18,_T19,_T20,_T21,_T22,_T23,_T24,
_T25,_T26,_T27,_T28,_T29,_T30,_T31,_T32,
_T33,_T34};

unsigned char 	_uBannerNum;
unsigned char 	_uBannerState;
const char * 	_pBanner;
unsigned char 	_uBannerLen;
unsigned int	_uBannerWait;
unsigned int 	_uBannerCharWait;

/*****************************************************************************
* Function: BannerStart
*
* Preconditions: None.
*
* Overview: The function starts to show banners from the last one. The banner
* is displayed once.
*
* Input: None.
*
* Output: None.
*
******************************************************************************/
void BannerStart(void){
	_uBannerState = 3;
	_uBannerNum = 32;
	_uBannerWait = 2000;	
}

/*****************************************************************************
* Function: BannerInit
*
* Preconditions: None.
*
* Overview: The function starts to show banners  from the first one.
*
* Input: None.
*
* Output: None.
*
******************************************************************************/
void BannerInit(void){
	_uBannerState = 9;
	_uBannerNum = 0;
	_uBannerWait = 20;	
}

/*****************************************************************************
* Function: BannerProcessEvents
*
* Preconditions: BannerInit or BannerStart must be called before.
*
* Overview: The function implements a state mashine to display banners sequence.
* Must be called periodically to output the strings.
*
* Input: None.
*
* Output: None.
*
******************************************************************************/
void BannerProcessEvents(void)
{

	switch(_uBannerState){
        case 4: 			// Wait to put a char
        case 7:
			if (_uBannerCharWait)
                 _uBannerCharWait--;
			else {
				_uBannerState++;
				_uBannerCharWait = 1;
			}
			break;

        case 5:				// Put a char on the LCD
        case 8:
			if (!mLCDIsBusy()) {
				mLCDPutChar(*_pBanner);
				_pBanner++;
				_uBannerLen--;
				if (!_uBannerLen)
                     _uBannerState++;
			}
			break;

		case 3:             // Put the first line
			if (!mLCDIsBusy()) {
            	mLCDHome();
				_pBanner = _pBannersArray[_uBannerNum];
                _uBannerNum++;
				_uBannerLen = LCD_DISPLAY_LEN;
				_uBannerState++;
			}
			break;

		case 6:				// Put the second line
            if (!mLCDIsBusy()){
			    mLCDPutCmd(0xC0);
    			_pBanner = _pBannersArray[_uBannerNum];
                _uBannerNum++;
	    		_uBannerLen = LCD_DISPLAY_LEN;
    			_uBannerState++;
            }
			break;

		case 9:				// Wait at the end of each banner
			if(_uBannerWait--)
                break;
			if(_uBannerNum >= BNR_COUNT)
				_uBannerNum = 0;
   			_uBannerWait = BNR_CHANGE_DELAY;
    		_uBannerState = 3;
            break;

		default:
			_uBannerState = 3;
	}	
}
/*****************************************************************************
* EOF
******************************************************************************/
