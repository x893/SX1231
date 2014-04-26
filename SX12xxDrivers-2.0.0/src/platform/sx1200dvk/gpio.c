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
 * \file       gpio.c
 * \brief      GPIO management
 *
 * \version    1.0
 * \date       Jan 1 2011
 * \author     Miguel Luis
 */
#include "mcu.h"

#include "gpio.h"

void LowPowerGpio( void )
{
    GPIO_InitTypeDef GPIO_InitStructure;

	/* Enable all GPIO clocks */
	RCC_APB2PeriphClockCmd( RCC_APB2Periph_GPIOA | RCC_APB2Periph_GPIOB | RCC_APB2Periph_GPIOC | 
	                        RCC_APB2Periph_GPIOD | RCC_APB2Periph_GPIOE | RCC_APB2Periph_GPIOF | 
	                        RCC_APB2Periph_GPIOG, ENABLE );
	
	/* Configure all GPIOs in AIN mode */
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_All;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AIN;
	GPIO_Init(GPIOA, &GPIO_InitStructure);
	GPIO_Init(GPIOB, &GPIO_InitStructure);
	GPIO_Init(GPIOC, &GPIO_InitStructure);
	GPIO_Init(GPIOD, &GPIO_InitStructure);
	GPIO_Init(GPIOE, &GPIO_InitStructure);
	GPIO_Init(GPIOF, &GPIO_InitStructure);
	GPIO_Init(GPIOG, &GPIO_InitStructure);

	/* Disable all GPIO clocks */
	RCC_APB2PeriphClockCmd( RCC_APB2Periph_GPIOA | RCC_APB2Periph_GPIOB | RCC_APB2Periph_GPIOC | 
	                        RCC_APB2Periph_GPIOD | RCC_APB2Periph_GPIOE | RCC_APB2Periph_GPIOF | 
	                        RCC_APB2Periph_GPIOG, DISABLE );
	
}


void GpioInit( void )
{
    GPIO_InitTypeDef GPIO_InitStructure;

    RCC_APB2PeriphClockCmd( RCC_APB2Periph_GPIOA | RCC_APB2Periph_GPIOC |
                            RCC_APB2Periph_GPIOE, ENABLE);

    // Inputs

    // SW1 as input
    GPIO_InitStructure.GPIO_Pin =  GPIO_Pin_6;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;
    GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
    GPIO_Init( GPIOE, &GPIO_InitStructure );

    // SW2 as input
    GPIO_InitStructure.GPIO_Pin =  GPIO_Pin_7;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;
    GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
    GPIO_Init( GPIOE, &GPIO_InitStructure ); 

    // SW3 as input
    GPIO_InitStructure.GPIO_Pin =  GPIO_Pin_0;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;
    GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
    GPIO_Init( GPIOA, &GPIO_InitStructure );  

    // SW4 as input
    GPIO_InitStructure.GPIO_Pin =  GPIO_Pin_13;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_IN_FLOATING;
    GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
    GPIO_Init( GPIOC, &GPIO_InitStructure );  
    
    // Outputs
 
    // Led PE2 as output ( STAT1 )
    GPIO_InitStructure.GPIO_Pin = GPIO_Pin_2;
    GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
    GPIO_Init( GPIOE, &GPIO_InitStructure );

    // Led PE3 as output ( STAT2 )
    GPIO_InitStructure.GPIO_Pin = GPIO_Pin_3;
    GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
    GPIO_Init(GPIOE, &GPIO_InitStructure );

    // Led PE4 as output ( STAT3 )
    GPIO_InitStructure.GPIO_Pin = GPIO_Pin_4;
    GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
    GPIO_Init( GPIOE, &GPIO_InitStructure );

    // Led PE5 as output ( STAT4 )
    GPIO_InitStructure.GPIO_Pin = GPIO_Pin_5;
    GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
    GPIO_InitStructure.GPIO_Mode = GPIO_Mode_Out_PP;
    GPIO_Init( GPIOE, &GPIO_InitStructure );
}

uint16_t LedList[] = { GPIO_Pin_2, GPIO_Pin_3, GPIO_Pin_4, GPIO_Pin_5 };

typedef struct sButton
{
	GPIO_TypeDef *GpioPort;
	uint16_t GpioPin;
	tButtonState GpioPinState;
}tButton;

tButton ButtonList[] = { { GPIOE, GPIO_Pin_6, 0 },
						 { GPIOE, GPIO_Pin_7, 0 },
						 { GPIOA, GPIO_Pin_0, 0 },
						 { GPIOC, GPIO_Pin_13, 0 } };

void GpioSetLedState( tLedId ledId, tLedState ledState )
{
	switch( ledState )
	{
		case OFF:
			GPIO_WriteBit( GPIOE, LedList[ledId], Bit_RESET );
			break;
		case ON:
			GPIO_WriteBit( GPIOE, LedList[ledId], Bit_SET );
			break;
		case TOGGLE:
			GPIO_WriteBit( GPIOE, LedList[ledId], GPIO_ReadOutputDataBit( GPIOE, LedList[ledId] ) ^ 1 );
			break;
	}
}

tButtonState GpioGetButtonState( tButtonId buttonId )
{
	if( ( GPIO_ReadInputDataBit( ButtonList[buttonId].GpioPort, ButtonList[buttonId].GpioPin ) ) == Bit_SET && ButtonList[buttonId].GpioPinState == RELEASED )      // pressed
	{
		ButtonList[buttonId].GpioPinState = PRESSED;
	}
	if( ( GPIO_ReadInputDataBit( ButtonList[buttonId].GpioPort, ButtonList[buttonId].GpioPin ) ) == Bit_RESET && ButtonList[buttonId].GpioPinState != RELEASED )    // released
	{
		ButtonList[buttonId].GpioPinState = RELEASED;
	}  
	return ButtonList[buttonId].GpioPinState;
}