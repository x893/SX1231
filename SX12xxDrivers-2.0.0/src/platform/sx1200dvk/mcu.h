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
 * \file       mcu.h
 * \brief      Implements the functions that manage the microcontroller
 *
 * \version    1.0
 * \date       Jan 1 2011
 * \author     Miguel Luis
 */
#ifndef __MCU_H__
#define __MCU_H__

#include "stm32f10x.h"
#include "types.h"


#define FW_VERSION									"1.0.0"

//we want to use the functions putchar and getchar
// and not some big machinery with stdout and stdin
#ifdef putchar
#undef putchar
#endif

#ifdef getchar
#undef getchar
#endif

//we could leave putchar=putchar but the following lines prevent errors in case 
// stdio.h is included again later in the compilation.
//also, this is a little faster. (by one function call and return)
#define putchar __io_putchar
#define getchar __io_getchar

#ifdef __GNUC__
    /* With GCC/RAISONANCE, small printf (option LD Linker->Libraries->Small printf
       set to 'Yes') calls __io_putchar() */
    #define PUTCHAR_PROTOTYPE int __io_putchar(int ch)
#else
    #define PUTCHAR_PROTOTYPE int fputc(int ch, FILE *f)
#endif /* __GNUC__ */

/*!
 * Functions return codes definition
 */
#define SX_OK                                       ( U8 ) 0x00
#define SX_ERROR                                    ( U8 ) 0x01
#define SX_BUSY                                     ( U8 ) 0x02
#define SX_EMPTY                                    ( U8 ) 0x03
#define SX_DONE                                     ( U8 ) 0x04
#define SX_TIMEOUT                                  ( U8 ) 0x05
#define SX_UNSUPPORTED                              ( U8 ) 0x06
#define SX_WAIT                                     ( U8 ) 0x07
#define SX_CLOSE                                    ( U8 ) 0x08
#define SX_ACK                                      ( U8 ) 0x09
#define SX_NACK                                     ( U8 ) 0x0A
#define SX_YES                                      ( U8 ) 0x0B
#define SX_NO                                       ( U8 ) 0x0C

/*!
 * Watchdog reset function
 */
#define ResetWatchdog( )

/*!
 * \brief Initializes the microcontroller
 */
void McuInit( void );

#endif // __MCU_H__