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
 * \file       mcu.c
 * \brief      Implements the functions that manage the microcontroller
 *
 * \version    1.0
 * \date       Jan 1 2011
 * \author     Miguel Luis
 */


#include "mcu.h"
#include "timer.h"
#include "gpio.h"
#include "sx1243-Hal.h"

#pragma config CPUDIV = NOCLKDIV
#pragma config USBDIV = OFF
#pragma config FOSC   = HS
#pragma config PLLEN  = ON
#pragma config FCMEN  = OFF
#pragma config IESO   = OFF
#pragma config PWRTEN = OFF
#pragma config BOREN  = OFF
#pragma config BORV   = 30
#pragma config WDTEN  = OFF
#pragma config WDTPS  = 32768
#pragma config MCLRE  = OFF
#pragma config HFOFST = OFF
#pragma config STVREN = ON
#pragma config LVP    = OFF
#pragma config XINST  = OFF
#pragma config BBSIZ  = OFF
#pragma config CP0    = OFF
#pragma config CP1    = OFF
#pragma config CPB    = OFF
#pragma config WRT0   = OFF
#pragma config WRT1   = OFF
#pragma config WRTB   = OFF
#pragma config WRTC   = OFF
#pragma config EBTR0  = OFF
#pragma config EBTR1  = OFF
#pragma config EBTRB  = OFF

#define RANDL_MAX 2147483647

static unsigned long next = 1;

void McuInit( void )
{
    VOLTAGE_REF |= 0x0F; // Default all pins to digital

    // Initialize timer tick 1 ms
    TmrInit( );

    // Initialize GPIO
    GpioInit( );
}

int randl( void )
{
    return ( ( next = next * 1103515245 + 12345 ) % RANDL_MAX );
}

void srandl( unsigned int seed )
{
    next = seed;
}

U32 randr( U32 min, U32 max )
{
    return ( U32 )randl( ) % ( max - min + 1 ) + min;
}
