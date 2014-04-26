/*****************************************************************************
 *  Buttons
 * 	Modified for PIC24FJ64GA004 family with PPS.
 *****************************************************************************
 * FileName:        button.c
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
 *
 * Buttons processing
 *
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * XXX                  XXX  
 * Brant Ivey			 3/14/06	Modified for PIC24FJ64GA004 family with PPS.
 *****************************************************************************/
#include "system.h"

/*****************************************************************************
 * Structure: BUTTONS _button_press
 *
 * Overview: the structure provides a n access to button bit indicators.
 *
 *****************************************************************************/
unsigned char _b1_cnt;
unsigned char _b2_cnt;
unsigned char _b3_cnt;
unsigned char _b4_cnt;
BUTTON _button_press;

//store old tris values for button I/O pins
unsigned int b1tris;
unsigned int b2tris;
unsigned int b3tris;
unsigned int b4tris;

/*****************************************************************************
 * Function: BtnInit
 *
 * Preconditon: None.
 *
 * Overview: Setup debounce.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
void BtnInit(void)
{
	_b1_cnt = 128;
	_b2_cnt = 128;
	_b3_cnt = 128;
	_b4_cnt = 128;
}

/*****************************************************************************
 * Function: BtnProcessEvents
 *
 * Preconditon: None.
 *
 * Overview: Must be called periodically to proccess buttons.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
void BtnProcessEvents(void)
{
//	AD1CON1bits.ADON = 0;		//Workaround for PIC24FJ64GA004 A1 rev bug preventing reads from AN0/AN1

	#ifdef __PIC24FJ64GA004__
	//Store old tris values and set buttons as inputs
	b1tris = BUTTON1_TRIS;
	b2tris = BUTTON2_TRIS;
	b3tris = BUTTON3_TRIS;
	b4tris = BUTTON4_TRIS;

	BUTTON1_TRIS = 1;	
	BUTTON2_TRIS = 1;
	BUTTON3_TRIS = 1;
	BUTTON4_TRIS = 1;
	#endif

	if (!BUTTON1) {
		_b1_cnt++;
		if (_b1_cnt > (128 + BUTTON_MAX_DEBOUCE)){
			_b1_cnt = (128 + BUTTON_MAX_DEBOUCE);
			_button_press.b1 = 1;
		}		
	}
	else {
		_b1_cnt--;
		if (_b1_cnt < (128 - BUTTON_MAX_DEBOUCE)){
			_b1_cnt = (128 - BUTTON_MAX_DEBOUCE);
			_button_press.b1 = 0;
		}
	}
 

	if (!BUTTON2) {
		_b2_cnt++;
		if (_b2_cnt > (128 + BUTTON_MAX_DEBOUCE)){
			_b2_cnt = (128 + BUTTON_MAX_DEBOUCE);
			_button_press.b2 = 1;
		}
	}
	else {
		_b2_cnt--;
		if (_b2_cnt < (128 - BUTTON_MAX_DEBOUCE)){
			_b2_cnt = (128 - BUTTON_MAX_DEBOUCE);
			_button_press.b2 = 0;
		}
	}


	if (!BUTTON3) {
		_b3_cnt++;
		if (_b3_cnt > (128 + BUTTON_MAX_DEBOUCE)){
			_b3_cnt = (128 + BUTTON_MAX_DEBOUCE);
			_button_press.b3 = 1;
		}
	}
	else {
		_b3_cnt--;
		if (_b3_cnt < (128 - BUTTON_MAX_DEBOUCE)){
			_b3_cnt = (128 - BUTTON_MAX_DEBOUCE);
			_button_press.b3 = 0;
		}
	}

	if (!BUTTON4) {
		_b4_cnt++;
		if (_b4_cnt > (128 + BUTTON_MAX_DEBOUCE)){
			_b4_cnt = (128 + BUTTON_MAX_DEBOUCE);
			_button_press.b4 = 1;
		}
	}
	else {
		_b4_cnt--;
		if (_b4_cnt < (128 - BUTTON_MAX_DEBOUCE)){
			_b4_cnt = (128 - BUTTON_MAX_DEBOUCE);
			_button_press.b4 = 0;
		}
	}

	#ifdef __PIC24FJ64GA004__
	//Restore tris values for button I/O pins 
	BUTTON1_TRIS = b1tris;
	BUTTON2_TRIS = b2tris;
	BUTTON3_TRIS = b3tris;
	BUTTON4_TRIS = b4tris;
	#endif

//	AD1CON1bits.ADON = 1;		//Workaround for PIC24FJ64GA004 A1 issue
}
/*****************************************************************************
 * EOF
 *****************************************************************************/

