/*****************************************************************************
 *  ADC
 *****************************************************************************
 * FileName:        adc.h
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
 * External functions and data declaration to display
 * temperature sensor and potentiometer
 *
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Anton Alkhimenok		10/21/05	...
 *****************************************************************************/
/*****************************************************************************
 * Arrays: _voltage_str and _temperature_str
 *
 * Overview: These arrays contains voltage and temperature strings.
 *
 *****************************************************************************/
extern char _voltage_str[16];
extern char _temperature_str[16];

/*****************************************************************************
* Function: ADCInit
*
* Preconditions: None.
*
* Overview: This function initiates ADC and state mashine.
*
* Input: None.
*
* Output: None.
*
******************************************************************************/
extern void ADCInit();

/*****************************************************************************
* Function: ADCProcessEvents
*
* Preconditions: ADCInit must be called before.
*
* Overview: This is a state mashine to grab analog data from potentiometer
* and temperature sensor. Must be called periodically to refresh voltage
* and temperature strings.
*
* Input: None.
*
* Output: None.
*
******************************************************************************/
extern void ADCProcessEvents();

/*****************************************************************************
 * Function: ADCStoreTemperature()
 *
 * Preconditions: SPIMPolInit and EEPROMInit must be called before.
 *
 * Overview: The function stores the current temperature into EEPROM.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
extern void ADCStoreTemperature();

/*****************************************************************************
 * Function: ADCSetFromMemory
 *
 * Preconditions: None.
 *
 * Overview: The function toggles switch to display temperature stored into EEPROM
 * rather than current one.
 *
 * Input: None.
 *
 * Output: None.
 *
 *****************************************************************************/
extern void ADCSetFromMemory();

/*****************************************************************************
 * Function: ADCIsFromMemory
 *
 * Preconditions: None.
 *
 * Overview: The function returns mode of EEPROM displaying.
 *
 * Input: None.
 *
 * Output: It returns zero if the current temperature is displyed.
 *
 *****************************************************************************/
extern unsigned char ADCIsFromMemory();

/*****************************************************************************
 * Function: ADCLoadTemperature()
 *
 * Preconditions:  SPIMPolInit and EEPROMInit must be called before.
 *
 * Overview: The function returns temperature stored by ADCStoreTemperature
 * into EEPROM.
 *
 * Input: None.
 *
 * Output: Temperature value read from EEPROM.
 *
 *****************************************************************************/
extern unsigned ADCLoadTemperature();

/*****************************************************************************
 * Function: ADCShortToString
 *
 * Preconditions : None.
 *
 * Overview: The function converts integer into string.
 *
 * Input: Value - value to be converted; DotPos - dot position ( can be 
 * between 0 and 3, DOTPOS_NONE = 4, if equals DOTPOS_TRAIL_ZEROS = -1 will not
 * put a dot and insert leading zeros); Buffer - receives the result string
 *
 * Output: None.
 *
 *****************************************************************************/
#define DOTPOS_NONE          4 // ADCShortToString will not show dot
#define DOTPOS_TRAIL_ZEROS  -1 // ADCShortToString will not show dot and fill front space with zeros
extern void ADCShortToString(int Value, char DotPos, char* Buffer);

/*****************************************************************************
 * EOF
 *****************************************************************************/
