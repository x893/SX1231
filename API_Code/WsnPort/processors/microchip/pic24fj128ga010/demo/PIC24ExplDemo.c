/*****************************************************************************
 *
 * Explorer 16 Development Board Demo Program.
 * Modified for PIC24FJ64GA004 family with PPS.
 *
 *****************************************************************************
 * FileName:        PIC24ExplDemo.c
 * Dependencies:    system.h
 * Processor:       PIC24
 * Compiler:       	MPLAB C30 v3.00 or later
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
 * Author               Date        Comment
 *~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 * Ross Fosler	        XXX         ...	
 * Anton Alkhimenok     10/21/05    ...
 * Brant Ivey			3/14/06    Modified for PIC24FJ64GA004 family
 *****************************************************************************/

#include "system.h"

// Setup configuration bits
#ifdef __PIC24FJ64GA004__ //Defined by MPLAB when using 24FJ64GA004 device
_CONFIG1( JTAGEN_OFF & GCP_OFF & GWRP_OFF & COE_OFF & FWDTEN_OFF & ICS_PGx1 & IOL1WAY_ON) 
_CONFIG2( FCKSM_CSDCMD & OSCIOFNC_OFF & POSCMOD_HS & FNOSC_PRI & I2C1SEL_SEC)
#else
_CONFIG1( JTAGEN_OFF & GCP_OFF & GWRP_OFF & COE_OFF & FWDTEN_OFF & ICS_PGx2) 
_CONFIG2( FCKSM_CSDCMD & OSCIOFNC_OFF & POSCMOD_HS & FNOSC_PRI)
#endif


#define     DISP_MAX        2
#define     DISP_HELLO      0
#define     DISP_CLOCK      2
#define     DISP_VOLTAGE    1

unsigned char _display_state;

int main(void)
{

    // Start from displaying of PIC24 banners
	_display_state = DISP_HELLO;

    // Setup PortA IOs as digital
    AD1PCFG = 0xffff;

	//IO Mapping for PIC24FJ64GA004 
	#ifdef __PIC24FJ64GA004__ //Defined by MPLAB when using 24FJ64GA004 device
		ioMap();
		lockIO();
	#endif

    // Setup SPI to communicate to EEPROM
    SPIMPolInit();

    // Setup EEPROM IOs
    EEPROMInit();

    // Setup the UART
    UART2Init();

	// Setup the timer
	TimerInit();
    
	// Setup the LCD
	mLCDInit();

	// Setup debounce processing
	BtnInit(); 

    // Setup the ADC
    ADCInit();

	// Setup the banner processing
	BannerStart();

	// Setup the RTCC
    RTCCInit();

	while (1) {
		LCDProcessEvents();
        ADCProcessEvents();

		if (TimerIsOverflowEvent()){

			// Button debounce processing
			BtnProcessEvents();
			// State dependent processing
			switch (_display_state) {
                // Show Microchip banners
				case DISP_HELLO: BannerProcessEvents(); break;
                // Show clock
				case DISP_CLOCK: TBannerProcessEvents(); break;
                // Show voltage and temperature
				case DISP_VOLTAGE: VBannerProcessEvents(); break;

                default: _display_state = DISP_HELLO;
			}// End of switch (_display_state)...

            // If S6 is pressed show the next example
			if (BtnIsPressed(4)) {

                // Change state and clear display 
                if(!TBannerIsSetup()){
       				_display_state++;
                    if(_display_state > DISP_MAX)
                         _display_state = 0;

                    // Initialize state
                    switch (_display_state) {
                        // Microchip banners
                 	    case DISP_HELLO: BannerInit(); break;
                        // Clock
    				    case DISP_CLOCK: TBannerInit(); break;
                        // Voltage and temperature
	        			case DISP_VOLTAGE: VBannerInit(); break;
                        default:
                             _display_state = 0;
        		    }// End of switch (_display_state)...
                    mLCDClear();
                }else
                    TBannerNext();

                // wait for button released
                while (BtnIsPressed(4)){
					BtnProcessEvents();
				}
			}// End of 	if (BtnIsPressed(4)){...

            if(_display_state == DISP_CLOCK){

        		if (BtnIsPressed(1)){
                        TBannerSetup();
                    // wait for button released
                    while (BtnIsPressed(1))	BtnProcessEvents();
                }// End of if (BtnIsPressed(1 ...

                if(TBannerIsSetup()){
	           		if (BtnIsPressed(2)) {
                        TBannerChangeField(1);
                        // wait for button released
                        while (BtnIsPressed(2))	BtnProcessEvents();
    		    	}// End of if (BtnIsPressed(2)){...
    
	    		    if (BtnIsPressed(3)) {
                        // wait for button released
                        TBannerChangeField(0);
                        while (BtnIsPressed(3))	BtnProcessEvents();
        			}// End of if (BtnIsPressed(3)){...
                }// End of if(TBannerIsSetup( ...

            }// End of if(_display_state == DISP_SET_CLOCK ...



            if(_display_state == DISP_VOLTAGE){

        		if (BtnIsPressed(2)){
                    ADCSetFromMemory();
                    // wait for button released
                    while (BtnIsPressed(2)){
						BtnProcessEvents();
					}
                }// End of if (BtnIsPressed(2 ...

           		if (BtnIsPressed(3)){
                    ADCStoreTemperature();
                    // wait for button released
                    while (BtnIsPressed(3)){
						BtnProcessEvents();
					}
  		    	}// End of if (BtnIsPressed(3)){...

            }// End of if(_display_state ...




    	}// End of if (TimerIsOverflowEvent()...
    }// End of while(1)...
}// End of main()...

