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
 * \file        gpio.c
 * \brief       GPIO management
 *
 * \version     1.0
 * \date        Jan 1 2011
 * \author      Miguel Luis
 */
#include "mcu.h"
#include "timer.h"
#include "gpio.h"

void GpioInit( void )
{

    /* MCU dependant Set GPIO pins as analog or digital */
    /* In our case, all GPIO pins are used as Digital   */
    GPIO_ANALOG_RA4 = 0;
    GPIO_ANALOG_RC0 = 0;
    GPIO_ANALOG_RC1 = 0;
    GPIO_ANALOG_RC2 = 0;
    GPIO_ANALOG_RC3 = 0;
    
    GPIO_ANALOG_RB5 = 0;
    GPIO_ANALOG_RC6 = 0;


    SW1_DIR = INPUT_PIN;
    SW2_DIR = INPUT_PIN;
    SW3_DIR = INPUT_PIN;
    SW4_DIR = INPUT_PIN;

    LED1 = LED_OFF;
    LED2 = LED_OFF;

    LED1_DIR = OUTPUT_PIN;
    LED2_DIR = OUTPUT_PIN;

}

U8 GpioProcess( void )
{
    U8 return_val = 0;

    if (SW1 == SW_ON)
    {
        return_val = 1;
    }
    else if (SW2 == SW_ON)
    {
        return_val = 2;
    }
    else if (SW3 == SW_ON)
    {
        return_val = 3;
    }
    else if (SW4 == SW_ON)
    {
        return_val = 4;
    }
    else return_val = 0;

    return return_val;

}
