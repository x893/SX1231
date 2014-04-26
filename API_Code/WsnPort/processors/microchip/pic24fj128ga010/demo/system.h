/*****************************************************************************
 *  System
 * 	Modified for PIC24FJ64GA004 family with PPS.
 *****************************************************************************
 * FileName:        system.h
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
 * The file assembles all header files and
 * contains shared information for all modules
 *
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Anton Alkhimenok		10/21/05	...
 * Brant Ivey			 3/14/06	Modified for PIC24FJ64GA004 family with PPS.
 *****************************************************************************/

// External oscillator frequency
#define SYSCLK          8000000

//Comment the line for 3rd board version 
#define BOARD_VERSION4

//Uncomment if PIC24F part is installed directly on board
//#define PIM_SWAP


#ifdef __PIC24FJ64GA004__	//Defined by MPLAB when using 24FJ64GA004 device
	#include "iomapping.h"
#else
	#define AN_VOLT_PIN  	AD1PCFGbits.PCFG5			//voltage input on AN5
	#define ADC_VOLT_CHAN	5
	#define AN_TEMP_PIN	 	AD1PCFGbits.PCFG4			//temp input on AN4
	#define ADC_TEMP_CHAN	4
#endif

#include <p24fxxxx.h>
#include "spimpol.h"
#include "eeprom.h"
#include "adc.h"
#include "timer.h"
#include "lcd.h"
#include "rtcc.h"
#include "buttons.h"
#include "uart2.h"

/*****************************************************************************
 * EOF
 *****************************************************************************/
