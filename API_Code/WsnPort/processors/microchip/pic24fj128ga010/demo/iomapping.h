/************************************************************************
*                                                                       *
* This redefines I/O mapping for a PIC24FJ64GA004 44-pin PIM            *
*                                                                       *
*************************************************************************
* Company:             Microchip Technology, Inc.                       *
*                                                                       *
* Software License Agreement                                            *
*                                                                       *
* The software supplied herewith by Microchip Technology Incorporated   *
* (the "Company") for its PICmicro® Microcontroller is intended and     *
* supplied to you, the Company's customer, for use solely and           *
* exclusively on Microchip PICmicro Microcontroller products. The       *
* software is owned by the Company and/or its supplier, and is          *
* protected under applicable copyright laws. All rights are reserved.   *
* Any use in violation of the foregoing restrictions may subject the    *
* user to criminal sanctions under applicable laws, as well as to       *
* civil liability for the breach of the terms and conditions of this    *
* license.                                                              *
*                                                                       *
* THIS SOFTWARE IS PROVIDED IN AN "AS IS" CONDITION. NO WARRANTIES,     *
* WHETHER EXPRESS, IMPLIED OR STATUTORY, INCLUDING, BUT NOT LIMITED     *
* TO, IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A           *
* PARTICULAR PURPOSE APPLY TO THIS SOFTWARE. THE COMPANY SHALL NOT,     *
* IN ANY CIRCUMSTANCES, BE LIABLE FOR SPECIAL, INCIDENTAL OR            *
* CONSEQUENTIAL DAMAGES, FOR ANY REASON WHATSOEVER.                     *
*                                                                       *
* Author               Date            Comment                          *
*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
* Brant Ivey           Mar 14, 2006    ....                             *
************************************************************************/

#ifndef IOMAPPING_H	
#define IOMAPPING_H

//ADC input mapping
#define AN_VOLT_PIN  	AD1PCFGbits.PCFG7			//voltage input on AN7
#define ADC_VOLT_CHAN	7

#define AN_TEMP_PIN	 	AD1PCFGbits.PCFG6 			//temp input on AN6
#define ADC_TEMP_CHAN	6


//Push Button I/O Mapping
#define BUTTON1_IO		PORTAbits.RA10
#define BUTTON2_IO		PORTAbits.RA9
#define BUTTON3_IO		PORTCbits.RC6
#define BUTTON4_IO		PORTAbits.RA7

#define BUTTON1_TRIS	TRISAbits.TRISA10
#define BUTTON2_TRIS	TRISAbits.TRISA9
#define BUTTON3_TRIS	TRISCbits.TRISC6
#define BUTTON4_TRIS	TRISAbits.TRISA7


//LED I/O Mapping
#define LED0_IO			LATAbits.LATA10
#define LED1_IO			LATAbits.LATA7
#define LED2_IO			LATBbits.LATB8
#define LED3_IO			LATBbits.LATB9
#define LED4_IO			LATAbits.LATA9
#define LED5_IO			LATAbits.LATA8
#define LED6_IO			LATBbits.LATB12
#define LED7_IO			LATCbits.LATC6

#define LED0_TRIS		TRISAbits.TRISA10
#define LED1_TRIS		TRISAbits.TRISA7			
#define LED2_TRIS		TRISBbits.TRISB8
#define LED3_TRIS		TRISBbits.TRISB9
#define LED4_TRIS		TRISAbits.TRISA9
#define LED5_TRIS		TRISAbits.TRISA8
#define LED6_TRIS		TRISBbits.TRISB12
#define LED7_TRIS		TRISCbits.TRISC6


//SPI I/O Mapping
#define PPS_SPI_SS_IO		LATAbits.LATA8
#define PPS_SPI_SS_TRIS	    TRISAbits.TRISA8
#define PPS_SPI_SCK_TRIS	TRISCbits.TRISC8
#define PPS_SPI_SDI_TRIS	TRISCbits.TRISC4
#define PPS_SPI_SDO_TRIS	TRISCbits.TRISC5


//UART I/O Mapping
#define PPS_UART2_TX_TRIS		TRISCbits.TRISC9
#define PPS_UART2_RX_TRIS		TRISCbits.TRISC3


//PPS Outputs
#define NULL_IO		0
#define C1OUT_IO	1
#define C2OUT_IO	2
#define U1TX_IO		3
#define U1RTS_IO	4
#define U2TX_IO		5
#define U2RTS_IO	6
#define SDO1_IO		7
#define SCK1OUT_IO	8
#define SS1OUT_IO	9
#define SDO2_IO		10
#define SCK2OUT_IO	11
#define SS2OUT_IO	12
#define OC1_IO		18
#define OC2_IO		19
#define OC3_IO		20
#define OC4_IO		21
#define OC5_IO		22

extern void ioMap();
extern void unlock();
extern void lockIO();

#endif
