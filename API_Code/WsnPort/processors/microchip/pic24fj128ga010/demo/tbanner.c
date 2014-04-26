/*****************************************************************************
 *
 * State mashine to display/setup Real Time Clock Calender
 *
 *****************************************************************************
 * FileName:        tbanner.c
 * Dependencies:    
 * Processor:       PIC24
 * Compiler:       	MPLAB C30
 * Linker:          MPLINK 03.20.01 or higher
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
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * XXX                  XXX         ...	
 * Anton Alkhimenok     10-21-2005  Get/Set related changings
 *****************************************************************************/

#include "system.h"

unsigned char 	_uTBannerState;
char * 	        _pTBanner;          // Pointer to character dislayed.
unsigned char 	_uTBannerLen;       // Position displayed.
unsigned int 	_uTBannerCharWait;
unsigned int 	_uTBannerBlinkWait;
unsigned char 	_uTBannerBlink = 0;
unsigned char   _uTBannerSetup = 0; // Setup mode indicator.
unsigned char 	_uTBannerCurPos;    // Current field selected.


/*****************************************************************************
 * Clock setup related definitions.
 *****************************************************************************/
#define TBNR_BLINK_PERIOD   20000
#define TBNR_POS_MAX        6
#define TBNR_POS_WKDAY      0
#define TBNR_POS_HOUR       1
#define TBNR_POS_MIN        2
#define TBNR_POS_SEC        3
#define TBNR_POS_MONTH      4
#define TBNR_POS_DAY        5
#define TBNR_POS_YEAR       6

/*****************************************************************************
 * Function: TBannerInit
 *
 * Precondition: None.
 *
 * Overview: The function setup state mashine to display clock.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
void TBannerInit(void)
{
	_uTBannerLen = LCD_DISPLAY_LEN;
	_uTBannerState = 1;
	_uTBannerCharWait = 2;
	_pTBanner = _time_str;
    _uTBannerCurPos = TBNR_POS_HOUR;
}

/*****************************************************************************
 * Function: TBannerSetup
 *
 * Precondition: None.
 *
 * Overview: The function toggles between CLOCK and SETUP mode.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
void TBannerSetup(){
    _uTBannerSetup ^=1;
    if(_uTBannerSetup)
        _uTBannerCurPos = TBNR_POS_HOUR;
}

/*****************************************************************************
 * Function: TBannerIsSetup
 *
 * Precondition: None.
 *
 * Overview: The function checks for current mode.
 *
 * Input: None.
 *
 * Output: Not zero if current mode is SETUP.
 *
 *****************************************************************************/
char TBannerIsSetup(){
    return _uTBannerSetup;
}

/*****************************************************************************
 * Function: TBannerNext
 *
 * Precondition: None.
 *
 * Overview: The function moves to the next clock field to be changed.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
void TBannerNext(){
    _uTBannerCurPos++;
    if(_uTBannerCurPos > TBNR_POS_MAX)
        _uTBannerCurPos = TBNR_POS_HOUR;
}

/*****************************************************************************
 * Function: TBannerClearField
 *
 * Precondition: None.
 *
 * Overview: The function blanks the selected clock field.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
void TBannerClearField(){
char counter;
//Array containing screen start and end positions of clock fields.
const static char _uTBannerDataPosLen[][2] = {
{0,3},   // week days
{4,6},   // hours
{7,9},   // minutes   
{10,12}, // seconds
{0,3},   // month
{4,6},   // date
{8,12}   // year
};
if(_uTBannerSetup){
    // Clear field shosen
    if(_uTBannerCurPos < 4){
        for(counter = _uTBannerDataPosLen[_uTBannerCurPos][0];
             counter < _uTBannerDataPosLen[_uTBannerCurPos][1]; counter++)
            _time_str[counter] = ' ';
    }else
        for(counter = _uTBannerDataPosLen[_uTBannerCurPos][0];
             counter < _uTBannerDataPosLen[_uTBannerCurPos][1]; counter++)
            _date_str[counter] = ' ';
}
}

/*****************************************************************************
 * Function: TBannerChangeField
 *
 * Precondition: None.
 *
 * Overview: The function increases/decreases the selected clock field.
 *
 * Input: Direction of changing: 0 to decrement otherwise increment.
 *
 * Output: None.
 *
 *****************************************************************************/
void TBannerChangeField(char increment){
unsigned char data;
if(_uTBannerSetup){
	mRTCCUnlock();
    switch(_uTBannerCurPos){
        case TBNR_POS_WKDAY:
            break;
        case TBNR_POS_HOUR:
            data = mRTCCGetBinHour();
            if(increment) data++; else data--;
            RTCCSetBinHour(data);
            break;
        case TBNR_POS_MIN:
            data = mRTCCGetBinMin();
            if(increment) data++; else data--;
            RTCCSetBinMin(data);
            break;
        case TBNR_POS_SEC:
            data = mRTCCGetBinSec();
            if(increment) data++; else data--;
            RTCCSetBinSec(data);
            break;
        case TBNR_POS_MONTH:
            data = mRTCCGetBinMonth();
            if(increment) data++; else data--;
            RTCCSetBinMonth(data);
            RTCCCalculateWeekDay();
            break;
        case TBNR_POS_DAY:
            data = mRTCCGetBinDay();
            if(increment) data++; else data--;
            RTCCSetBinDay(data);
            RTCCCalculateWeekDay();
            break;
        case TBNR_POS_YEAR:
            data = mRTCCGetBinYear();
            if(increment) data++; else data--;
            RTCCSetBinYear(data);
            RTCCCalculateWeekDay();
            break;
        default:
            ;
    }// End of switch(_uTBannerCurPos ...
    mRTCCSet();
}
}

/*****************************************************************************
 * Function: TBannerProcessEvents
 *
 * Precondition: TBannerInit must be called before.
 *
 * Overview: This is a state mashine to display time and date strings and
 * show the blinking cursor in SETUP mode.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
void TBannerProcessEvents(void)
{
   
	switch(_uTBannerState){
		case 1: 			// Wait to put a char
		case 4:
			if (_uTBannerCharWait) _uTBannerCharWait--;
			else {
				_uTBannerState++;
				_uTBannerCharWait = 2;
			}
			break;
		case 5:
		case 2:				// Put a char on the LCD
			if (!mLCDIsBusy()) {
				mLCDPutChar(*_pTBanner);
				_pTBanner++;
				_uTBannerLen--;
				if (!_uTBannerLen) _uTBannerState++;
				else _uTBannerState--;
			}
			break;
		case 3:
			if (!mLCDIsBusy()) {
				mLCDPutCmd(0xC0);
				_uTBannerLen = LCD_DISPLAY_LEN;

                if(_uTBannerBlinkWait--){
                    _uTBannerBlink ^= 1;
                    _uTBannerBlinkWait = TBNR_BLINK_PERIOD;
                }

                 RTCCProcessEvents();

                if(_uTBannerBlink)
                    TBannerClearField();
   				_pTBanner = _date_str;
				_uTBannerState++;
			}
			break;
		case 6:
			if (!mLCDIsBusy()) {
				mLCDHome();
				_uTBannerState = 1;
				_uTBannerLen = LCD_DISPLAY_LEN;
                if(_uTBannerBlink)
                     TBannerClearField();
   				_pTBanner = _time_str;
			}
			break;
		default:
			_uTBannerState = 0;
			break;
	}	
}
/*****************************************************************************
 * EOF
 *****************************************************************************/
