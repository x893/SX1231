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
 * \file        timer.h
 * \brief       Implements the functions that manage the global timer
 *
 * \version     1.0
 * \date        Feb 12 2010
 * \author      Miguel Luis
 */
#ifndef __TIMER_H__
#define __TIMER_H__

/*!
 * Initialises the main application timer
 */
void TmrInit( void );


/*!
 * \brief Returns the tick counter value.
 * \brief One unit = 1 us.
 * \remark See the GetDeltaTickCounter( U32 oldTick ) function.
 * \retval Tick 32 bits counter value
 */
U32 TmrGetTickCounter( void );


/*!
 * Timer interrupt handler
 */
void TmrIrqHandler( void );


/*!
 * \brief Return the number of tick between the current value of the tick counter and
 *        an older value of the tick counter. The older value must be the return of GetDeltaCounter().
 *
 * \param[in] oldTick	Old tick value of the counter 
 * \retval Delta between the two values. 
 * \remark One unit = 1 ms
 */
U32 TmrGetDeltaTickCounter( U32 oldTick );

#endif // __TIMER_H__