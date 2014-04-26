/*
 * THE FOLLOWING FIRMWARE IS PROVIDED: (1) "AS IS" WITH NO WARRANTY; AND 
 * (2)TO ENABLE ACCESS TO CODING INFORMATION TO GUIDE AND FACILITATE CUSTOMER.
 * CONSEQUENTLY, SEMTECH SHALL NOT BE HELD LIABLE FOR ANY DIRECT, INDIRECT OR
 * CONSEQUENTIAL DAMAGES WITH RESPECT TO ANY CLAIMS ARISING FROM THE CONTENT
 * OF SUCH FIRMWARE AND/OR THE USE MADE BY CUSTOMERS OF THE CODING INFORMATION
 * CONTAINED HEREIN IN CONNECTION WITH THEIR PRODUCTS.
 * 
 * Copyright (C) SEMTECH S.A.
 */
/*! 
 * \file       gpio.h
 * \brief       
 *
 * \version    1.0
 * \date       Jan 1 2011
 * \author     Miguel Luis
 */
#ifndef __GPIO_H__
#define __GPIO_H__

typedef enum eLedState
{
	OFF = 0,
	ON,
	TOGGLE,
}tLedState;

typedef enum eLedId
{
	STAT1 = 0,
	STAT2,
	STAT3,
	STAT4,
}tLedId;

typedef enum eButtonState
{
	RELEASED = 0,
	PRESSED,
}tButtonState;

typedef enum eButtonId
{
	SW1 = 0,
	SW2,
	SW3,
	SW4,
}tButtonId;

void LowPowerGpio( void );

void GpioInit( void );

void GpioSetLedState( tLedId ledId, tLedState ledState );

tButtonState GpioGetButtonState( tButtonId buttonId );

#endif // __GPIO_H__