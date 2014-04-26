/*****************************************************************************
 *  Testing
 *****************************************************************************
 * FileName:        adc.c
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
 * Buttons processing
 *
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * XXX                  XXX  
 *****************************************************************************/


/*****************************************************************************
 * INCLUDES
 *****************************************************************************/
#include "store.h"
#include "spimpol.h"
#include "lcd.h"
#include "p24FJ128GA010.h"

/*****************************************************************************
 * DEFINITIONS
 *****************************************************************************/
int debounce_count1;
int button1;
int debounce_count2;
int button2;
int addr;
int strLen;
/*****************************************************************************
 * Function: 
 *
 * Preconditon: none
 *
 * Overview: 
 *
 * Input: 
 *
 * Output: 
 *
 *****************************************************************************/
void BtnInit(){
	debounce_count1 = 128;
	TRISDBits.RD6 = 1;
	debounce_count2 = 128;
	TRISDBits.RD7 = 1;
}

void BtnDebounce(){

	if(!PORTDBits.RD6;){
		debounce_count1++;
		if(debounce_count1 > (128 + DEBOUNCE_MAX)){
			debounce_count1 = (128 + DEBOUNCE_MAX);
			button1 = 1;
		}
	else
		debounce_count1--;
		if(debounce_count1 < (128 - DEBOUNCE_MAX)){
			debounce_count1 = (128 - DEBOUNCE_MAX);
			button1 = 0;
		}	
	}
	if(!PORTDBits.RD7;){
		debounce_count2++;
		if(debounce_count2 > (128 + DEBOUNCE_MAX)){
			debounce_count2 = (128 + DEBOUNCE_MAX);
			button2 = 1;
		}
	else
		debounce_count2--;
		if(debounce_count2 < (128 - DEBOUNCE_MAX)){
			debounce_count2 = (128 - DEBOUNCE_MAX);
			button2 = 0;
		}	
	}
}


void BtnProcessEvents(){

	char temp;

	BtnDebounce();

	if(button1){
		addr = 0x0000;
		strLen = 16;
		EEPromWrite(VoltStr, addr,strLen);
		//displayStored(0x0000);
		while(button1) BtnDebounce();
	}
	if(button2){
		mLCDPutCmd(0xC0);
		displayingString = 1;
		while(button2) BtnDebounce();
	}
	
	if(displayingString){	
		if(StrLoc >= strLen){
			displayingString = 0;
			StrLoc = 0;
			break;
		}
		if(!mLCDIsBusy()){
			temp = EEPromRead(addr + StrLoc);
			mLCDPutChar(temp);
			StrLoc++;
		}
	}		
}

