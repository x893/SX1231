/*****************************************************************************
 *  Buttons
 *  Modified for PIC24FJ64GA004 family with PPS.
 *****************************************************************************
 * FileName:        button.h
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
 * Buttons processing defintions 
 *
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Brant Ivey			 3/14/06	Modified for PIC24FJ64GA004 family with PPS.
 *****************************************************************************/

/*****************************************************************************
 * Buttons IOs PORT definitions for different versions
 * of Explorer 16 Development Board.
 *****************************************************************************/


#ifdef  __PIC24FJ64GA004__
	//I/O for FJ64 PIM
	#define BUTTON1   BUTTON1_IO
	#define BUTTON2   BUTTON2_IO
	#define BUTTON3   BUTTON3_IO
	#define BUTTON4   BUTTON4_IO		

#else

	#ifdef  BOARD_VERSION4

		#ifdef  PIM_SWAP

			#define BUTTON1   PORTAbits.RA2
			#define BUTTON2   PORTAbits.RA3
			#define BUTTON3   PORTAbits.RA7
			#define BUTTON4   PORTDbits.RD13

		#else

			#define BUTTON1   PORTDbits.RD6
			#define BUTTON2   PORTDbits.RD7
			#define BUTTON3   PORTAbits.RA7
			#define BUTTON4   PORTDbits.RD13

		#endif

	#else

		#define BUTTON1   PORTBbits.RB0
		#define BUTTON2   PORTBbits.RB1
		#define BUTTON3   PORTBbits.RB2
		#define BUTTON4   PORTBbits.RB3

	#endif

#endif
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
void BtnInit(void);

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
void BtnProcessEvents(void);

/*****************************************************************************
 * Structure: BUTTONS _button_press
 *
 * Overview: the structure provides a n access to button bit indicators.
 *
 *****************************************************************************/
typedef struct tagButton {
	unsigned b1:1;
	unsigned b2:1;
	unsigned b3:1;
	unsigned b4:1;
}BUTTON;
extern BUTTON _button_press;

/*****************************************************************************
 * Debounce button counters
 *
 *****************************************************************************/
#define	BUTTON_MAX_DEBOUCE 	20
extern unsigned char _b1_cnt;
extern unsigned char _b2_cnt;
extern unsigned char _b3_cnt;
extern unsigned char _b4_cnt;

/*****************************************************************************
 * Function: BtnIsPressed
 *
 * Preconditon: None.
 *
 * Overview: Macro detects if button is pressed
 *
 * Input: None.
 *
 * Output: Macro returns zero if button is not pressed.
 *
 *****************************************************************************/
#define BtnIsPressed(__btn)	_button_press.b##__btn

/*****************************************************************************
 * Function: BtnIsNotPressed
 *
 * Preconditon: None.
 *
 * Overview: Macro detects if button is not pressed
 *
 * Input: None.
 *
 * Output: Macro returns zero if button is pressed.
 *
 *****************************************************************************/
#define BtnIsNotPressed(__btn)	!_button_press.b##__btn

/*****************************************************************************
 * EOF
 *****************************************************************************/
