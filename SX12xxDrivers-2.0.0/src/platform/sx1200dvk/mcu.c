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
 *    \file       mcu.c
 *    \brief      Implements the functions that manage the microcontroller
 *
 *    \version    1.0
 *    \date       Jan 1 2011
 *    \author     Miguel Luis
 */
#include "mcu.h"
#include "sx1200dvk.h"
#include "timer.h"
#include "irqHandler.h"
#include "uart.h"
#include "gpio.h"
#include "spi.h"
#include "lcd.h"
#include "joystick.h"
#include "usb_lib.h"
#include "usb_desc.h"
#include "usb_pwr.h"

void McuInit( void )
{
	// LowPower
	LowPowerGpio( );

    SetSystem( );

	Set_USBClock();
	USB_Interrupts_Config();
	USB_Init();

    // Initialize timer tick 1 ms
    TmrInit( );
	
    // Initialize SPI
    SpiInit( );

    // Initialize UART
    UartInit( );

    // Initialize IRQ
    IrqInit( );

    // Initialize GPIO
    GpioInit( );

    // Initialize LCD display
    LCDInit( );

    // Initialize Joystick
    JoystickInit( );
}
