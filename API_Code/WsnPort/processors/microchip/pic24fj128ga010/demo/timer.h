/*****************************************************************************
 *
 * Timer 
 *
 *****************************************************************************
 * FileName:        timer.h
 * Dependencies:    
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
 * This is a simple timer function used to provide quant for state machines
 *
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Ross Fosler			04/28/03	...	
 * 
 *****************************************************************************/

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
extern void TimerInit(void);

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
extern unsigned char TimerIsOverflowEvent(void);

/*********************************************************************
 * EOF
 ********************************************************************/
