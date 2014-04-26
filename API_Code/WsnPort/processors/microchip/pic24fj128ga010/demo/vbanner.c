/*****************************************************************************
 *
 * Voltage and Temperature 
 *
 *****************************************************************************
 * FileName:        vbanner.h
 * Dependencies:    lcd.h
 *                  adc.h
 * Processor:
 * Compiler:
 * Linker:
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
 * State mashine to print voltage and temperature strings on LCD
 *
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Anton Alkhimenok     10/18/05	...	
 * 
 *****************************************************************************/
#include "system.h"

#define VBNR_CHARWAIT  2

unsigned char 	_uVBannerState;     // current state
char * 	        _pVBanner;          // pointer to string displayed
unsigned char 	_uVBannerLen;       // current position displayed
unsigned int 	_uVBannerCharWait;  // period to display characters


/*****************************************************************************
 * Function: VBannerInit
 *
 * Precondition: None.
 *
 * Overview: VBannerInit initiates the state mashine to display voltage
 * and temperature.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
void   VBannerInit(void){
	_uVBannerLen = LCD_DISPLAY_LEN;
	_uVBannerState = 1;
	_uVBannerCharWait = VBNR_CHARWAIT;
	_pVBanner = _voltage_str;
	
}

/*****************************************************************************
 * Function: VBannerProcessEvents
 *
 * Precondition: VBannerInit must be called first.
 *
 * Overview: This is a state mashine to display voltage and temperature.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
void VBannerProcessEvents(void){
	switch(_uVBannerState){
		case 1: 			      // Wait to put a char
		case 4:
			if (_uVBannerCharWait) _uVBannerCharWait--;
			else {
				_uVBannerState++;
				_uVBannerCharWait = VBNR_CHARWAIT;
			}
			break;
		case 5:
		case 2:				
			if (!mLCDIsBusy()) {  // Put a char on the LCD
				mLCDPutChar(*_pVBanner);
				_pVBanner++;
				_uVBannerLen--;
				if (!_uVBannerLen) _uVBannerState++;
				else _uVBannerState--;
			}
			break;
		case 3:
			if (!mLCDIsBusy()) {  // Move to the second LCD line
				mLCDPutCmd(0xC0); 
				_uVBannerLen = LCD_DISPLAY_LEN;
				_pVBanner = _temperature_str;
				_uVBannerState++;
			}
			break;
		case 6:
			if (!mLCDIsBusy()) {
				mLCDHome();       // Move to the first LCD line
				_uVBannerState = 1;
				_uVBannerLen = LCD_DISPLAY_LEN;
				_pVBanner = _voltage_str;
			}
			break;

		default:
			_uVBannerState = 0;
			break;
	}	
}
/*****************************************************************************
 * EOF
 *****************************************************************************/
