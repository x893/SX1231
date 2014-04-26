/*****************************************************************************
 *  ADC
 *  Modified for PIC24FJ64GA004 family with PPS.
 *****************************************************************************
 * FileName:        adc.c
 * Dependencies:    system.h, spimpol.c, eeprom.c
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
 *  State machine to sample analog inputs for temperature sensor and potentiometer.
 *
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Anton Alkhimenok		10/18/05	...
 * Brant Ivey			 3/14/06	Modified for PIC24FJ64GA004 family with PPS.
 *****************************************************************************/

#include "system.h"

/*****************************************************************************
 * Module Constants Definitions 
 *****************************************************************************/
//ADC channels numbers
#define ADC_TEMPERATURE  ADC_TEMP_CHAN
#define ADC_VOLTAGE      ADC_VOLT_CHAN

// Delay after input switching
#define ADC_SWITCH_DELAY    600

// Temperature scale switching period
#define ADC_CELSIUS_DELAY   100

// Number position
#define ADC_POS_NUMBER      6

// Temperature scale sign position
#define ADC_POS_SCALE       11

// Memory sign position
#define ADC_POS_MEM         13

// Reference voltage, mV
#define AVCC                3300

/*****************************************************************************
 * Arrays: _voltage_str and _temperature_str
 *
 * Overview: These arrays contains voltage and temperature strings.
 *
 *****************************************************************************/
char _voltage_str[16] =     "Vol =      V    ";
char _temperature_str[16] = "Tmp =      C    ";

unsigned char 	_uADCState;
unsigned int	_uADCWait;
// Period to switch temperature scale
unsigned int	_uADCCelsiusWait;
// Current temperature scale
unsigned char 	_uADCCelsius;
// Buffer to filter temperature sensor value
unsigned int 	_uADCTemperature;
// Switch to get temperature data from EEPROM rather than from ADC
unsigned char 	_uADCFromMemory;

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
void ADCInit(){
	AD1CON1 = 0x80E4;				//Turn on, auto sample start, auto-convert
	AD1CON2 = 0;					//AVdd, AVss, int every conversion, MUXA only
	AD1CON3 = 0x1F05;				//31 Tad auto-sample, Tad = 5*Tcy
    
	AD1CHS = ADC_VOLTAGE;

	TRISBbits.TRISB2 = 1;
	TRISBbits.TRISB3 = 1;
	AN_VOLT_PIN = 0;			//Disable digital input on AN5
	AN_TEMP_PIN = 0;          //Disable digital input on AN4
	AD1CSSL = 0;					//No scanned inputs
    _uADCWait = ADC_SWITCH_DELAY;
    _uADCCelsiusWait = ADC_CELSIUS_DELAY;
    _uADCState = 1;
    _uADCCelsius = 0;               //Show celsius scale  
    _uADCFromMemory = 0;            //Show current temperature
}

/*****************************************************************************
* Function: ADCProcessEvents
*
* Preconditions: ADCInit must be called before.
*
* Overview: This is a state mashine to grab analog data from potentiometer
* and temperature sensor.
*
* Input: None.
*
* Output: None.
*
******************************************************************************/
void ADCProcessEvents(){
unsigned long Result;
unsigned long rlt;

    switch(_uADCState){

        case 1:          // Convert result for potentiometer
          // Wait for conversion to complete
          while(!AD1CON1bits.DONE);          
          Result = (long) ADC1BUF0;   
  
          Result = (Result*AVCC)/1024;

          ADCShortToString((int)Result, 1, _voltage_str+ADC_POS_NUMBER);
          // Sweep least significant digit
          _voltage_str[ADC_POS_NUMBER+4] = ' ';
          // Switch input to temperature sensor  
          AD1CHS = ADC_TEMPERATURE;
          _uADCState++;
          break;

        case 3:          // Convert result for temperature  
          // Wait for conversion to complete
          while(!AD1CON1bits.DONE);

          Result = (long) ADC1BUF0;

          // filter temperature Value
          // _uADCTemperature = 15/16*_uADCTemperature + 1/16*New Sample   
          _uADCTemperature -= _uADCTemperature>>4;
          _uADCTemperature += Result;
          Result = _uADCTemperature>>4;

          // Read temperature stored into EEPROM  
          _temperature_str[ADC_POS_MEM] = ' ';  
          if(ADCIsFromMemory()){
            Result = ADCLoadTemperature();          
            _temperature_str[ADC_POS_MEM] = 'M';
          }

          if(0 == _uADCCelsiusWait--){
            _uADCCelsiusWait = ADC_CELSIUS_DELAY;
            _uADCCelsius ^= 1;
            if(_uADCCelsius)
                _temperature_str[ADC_POS_SCALE]='C';
            else
                _temperature_str[ADC_POS_SCALE]='F';
          }
          if(_uADCCelsius)
              ADCShortToString((int)Result,3,_temperature_str+ADC_POS_NUMBER);
          else{
              // Convert Celsius temperatures into Fahrenheit
              // Begin by multiplying the Celsius temperature by 9. 
              // Divide the answer by 5. 
              // Now add 32. 
              Result *= 9; Result /= 5; Result += 320;
              ADCShortToString((int)Result, 3,_temperature_str+ADC_POS_NUMBER);
          }  
          // Switch input to potentiometer
          AD1CHS = ADC_VOLTAGE;
          _uADCState++; break;

        case 2:
        case 4:
            // Delay slot between channel switching
            if(0 == _uADCWait--){
                _uADCWait = ADC_SWITCH_DELAY;
                _uADCState++;
            }
            break;
        break;
        default: 
           _uADCState = 1;
    }
}

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
void ADCShortToString(int Value, char DotPos, char* Buffer){
char Result;
char Pos;

    // Clean Buffer (4 digits + Dot)
    for(Pos = 0; Pos < 5; Pos++) Buffer[Pos] = ' ';

    Pos = 0;
    if(Pos == DotPos){ *Buffer++ = '0';*Buffer++ = '.';}
    Pos++;
    Result = Value/1000;
    Value -= 1000*Result;
    if(Result) *Buffer++ = Result + '0';
    else if(Pos >= DotPos) *Buffer++ = '0';

    if(Pos == DotPos) *Buffer++ = '.'; Pos++;
    Result = Value/100;
    Value -= 100*Result;
    if(Result) *Buffer++ = Result + '0';
    else if(Pos >= DotPos) *Buffer++ = '0';

    if(Pos == DotPos) *Buffer++ = '.'; Pos++;
    Result = Value/10;
    Value -= 10*Result;
    if(Result) *Buffer++ = Result + '0';
    else if(Pos >= DotPos) *Buffer++ = '0';

    if(Pos == DotPos) *Buffer++ = '.'; Pos++;
    if(Value) *Buffer++ = Value + '0';
    else if(Pos >= DotPos) *Buffer++ = '0';
}			    

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
void ADCStoreTemperature(){
unsigned Temp;
    // Get temperature
    Temp = _uADCTemperature>>4;
    // Write MS byte into EEPROM address = 0
    EEPROMWriteByte(Hi(Temp),0);
    // Write LS byte into EEPROM address = 1
    EEPROMWriteByte(Lo(Temp),1);
}

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
unsigned ADCLoadTemperature(){
unsigned Temp;
    // Read MS byte from EEPROM address = 0
    Temp =  EEPROMReadByte(0);
    Temp = (Temp<<8)&0xff00;
    // Read LS byte from EEPROM address = 1
    Temp |= (EEPROMReadByte(1)&0x00ff);
    // If there's not valid value replace it with 25.0 C
    if(Temp > 990)
        Temp = 250;
    return Temp;
}

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
void ADCSetFromMemory(){
    _uADCFromMemory ^=1; // Toggle switch
}

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
unsigned char ADCIsFromMemory(){
    return _uADCFromMemory;
}

/*****************************************************************************
 * EOF
 *****************************************************************************/
