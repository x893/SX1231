#ifndef __STK3200_H__
#define __STK3200_H__

#include <stdio.h>
#include <stdint.h>
#include <stdbool.h>

#include "bsp.h"

#if   defined( BSP_STK_2010 )

	#define SK_NAME		"EFM32-STK3200"

#elif defined( BSP_STK_2200 )

	#define SK_NAME		"EFM32-STK3700"

#else

	#error Unknown board

#endif

#define FW_VERSION		"2.0.B2"

/*!
 * Functions return codes definition
 */
typedef enum
{
    SX_OK,
    SX_ERROR,
    SX_BUSY,
    SX_EMPTY,
    SX_DONE,
    SX_TIMEOUT,
    SX_UNSUPPORTED,
    SX_WAIT,
    SX_CLOSE,
    SX_YES,
    SX_NO,          
} tReturnCodes;

extern volatile uint32_t TickCounter;

/**
  * @brief   Small printf for GCC/RAISONANCE
  */
#ifdef __GNUC__
	/* With GCC/RAISONANCE, small printf (option LD Linker->Libraries->Small printf
	   set to 'Yes') calls __io_putchar() */
	#define PUTCHAR_PROTOTYPE int __io_putchar(int ch)

#endif /* __GNUC__ */

/*!
 * Initializes board peripherals
 */
void BoardInit( void );


/*!
 * Delay code execution for "delay" ms
 */
void Delay ( uint16_t delay );

/*!
 * Delay code execution for "delay" s
 */
void LongDelay ( uint8_t delay );

/*!
 * \brief Computes a random number between min and max
 *
 * \param [IN] min range minimum value
 * \param [IN] max range maximum value
 * \retval random random value in range min..max
 */
uint32_t randr( uint32_t min, uint32_t max );

#endif // __STK3200_H__
