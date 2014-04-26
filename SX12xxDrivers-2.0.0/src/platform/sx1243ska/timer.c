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
 * \file        timer.c
 * \brief       Implements the functions that manage the hardware timers
 *
 * \version     1.0
 * \date        Feb 12 2010
 * \author      Miguel Luis
 */
#include "mcu.h"
#include "irqHandler.h"
#include "timer.h"

U32 TickCounter = 0;

void TmrInit( void )
{
    TickCounter = 0;

    // Counters initialisation
    TIMER0_PRESCALER_SLCT   = 0;    // Internal clock
    TIMER0_PRESCALER_OFF    = 0;    // Prescaler On

    TMR0L = 209;                    // With default prescaler setting and 48MHz clock TMR0 will loop around every 1ms

    FLAG_IRQ_CTRL_TIMER0    = 0;    // Clear interrupt flag
    EN_IRQ_CTRL_TIMER0      = 1;    // Enable TMR0 interrupt
    TIMER0_ON               = 1;    // Enable TMR0
}
  
void TmrIrqHandler( void )
{
    TMR0L = 209;                    // TMR0 reload

    TickCounter++;
}

U32 TmrGetTickCounter( void )
{
    U32 tick;

    EN_IRQ_CTRL_TIMER0 = 0;     // Disable interrupt while reading the tick counter

    tick = TickCounter;

    EN_IRQ_CTRL_TIMER0 = 1;     // re-enable the interrupt

    return tick;
}


U32 TmrGetDeltaTickCounter( U32 oldTick )
{
    U32 curTick;

    EN_IRQ_CTRL_TIMER0 = 0;     // Disable interrupt while reading the tick counter

    curTick = TickCounter;

    EN_IRQ_CTRL_TIMER0 = 1;     // re-enable the interrupt 

    return curTick - oldTick;
}
