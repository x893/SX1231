/*****************************************************************************
 *
 * Timer 
 *
 *****************************************************************************
 * FileName:        timer.c
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
 * Functions to setup timer and detect overflow
 *
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Ross Fosler			04/28/03	...	
 * 
 *****************************************************************************/
#include "system.h"

/*********************************************************************
 * Function:        TimerInit
 *
 * PreCondition:    None.
 *
 * Input:       	None.	
 *                  
 * Output:      	None.
 *
 * Overview:        Initializes Timer0 for use.
 *
 ********************************************************************/
void TimerInit(void)
{	
	PR1 = 0x2FF;
	
	IPC0bits.T1IP = 5;
	T1CON = 0b1000000000010000;
	IFS0bits.T1IF = 0;

}

/*********************************************************************
 * Function:        TimerIsOverflowEvent
 *
 * PreCondition:    None.
 *
 * Input:       	None.	
 *                  
 * Output:      	Status.
 *
 * Overview:        Checks for an overflow event, returns TRUE if 
 *					an overflow occured.
 *
 * Note:            This function should be checked at least twice
 *					per overflow period.
 ********************************************************************/
unsigned char TimerIsOverflowEvent(void)
{
	if (IFS0bits.T1IF)
	{		
		IFS0bits.T1IF = 0;
		return(1);
	}
	return(0);
}

/*********************************************************************
 * EOF
 ********************************************************************/
