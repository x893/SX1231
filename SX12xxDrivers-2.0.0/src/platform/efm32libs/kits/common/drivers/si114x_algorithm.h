/**************************************************************************//**
 * @file
 * @brief Swipe algorithm for Si114x
 * @version 3.20.3
 ******************************************************************************
 * @section License
 * <b>(C) Copyright 2014 Silicon Labs, http://www.silabs.com</b>
 *******************************************************************************
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 *
 * DISCLAIMER OF WARRANTY/LIMITATION OF REMEDIES: Silicon Labs has no
 * obligation to support this Software. Silicon Labs is providing the
 * Software "AS IS", with no express or implied warranties of any kind,
 * including, but not limited to, any implied warranties of merchantability
 * or fitness for any particular purpose or warranties against infringement
 * of any proprietary rights of a third party.
 *
 * Silicon Labs will not be liable for any consequential, incidental, or
 * special damages, or any other relief, or for any claim by any third party,
 * arising from your use of this Software.
 *
 *****************************************************************************/
#ifndef __SI114X_ALGORITHM_H
#define __SI114X_ALGORITHM_H

#include "em_device.h"
#include "si114x_functions.h"

#ifdef __cplusplus
extern "C" {
#endif

/***************************************************************************//**
 * @addtogroup Drivers
 * @{
 ******************************************************************************/

/***************************************************************************//**
 * @addtogroup Si114x
 * @{
 ******************************************************************************/

/*******************************************************************************
 *******************************   DEFINES   ***********************************
 ******************************************************************************/
/** I2C device address for Si1147 on weather station board. */
#define SI1147_ADDR    0xc0

/*******************************************************************************
 *******************************   STRUCTS   ***********************************
 ******************************************************************************/  
  
/** Interrupt Sample */
typedef struct
{
  u32 timestamp;         /* Timestamp to record */
  u16 vis;               /* VIS */
  u16 ir;                /* IR */
  u16 ps1;               /* PS1 */
  u16 ps2;               /* PS2 */
  u16 ps3;               /* PS3 */
  u16 aux;               /* AUX */
} Si114x_Sample_TypeDef;

/*******************************************************************************
 ********************************   ENUMS   ************************************
 ******************************************************************************/
/** Si114x gestures */
typedef enum
{
  NONE,
  UP,
  DOWN,
  LEFT,
  RIGHT,
  TAP
} gesture_t;

/*******************************************************************************
 *****************************   PROTOTYPES   **********************************
 ******************************************************************************/
gesture_t Si1147_NewSample(I2C_TypeDef *i2c, uint8_t addr, uint32_t timestamp);
int Si1147_ConfigureDetection(I2C_TypeDef *i2c, uint8_t addr, int slow);
int Si1147_SetInterruptOutputEnable(I2C_TypeDef *i2c, uint8_t addr, int enable);
int Si1147_GetInterruptOutputEnable(I2C_TypeDef *i2c, uint8_t addr, int *enable);
int Si1147_Detect_Device(I2C_TypeDef *i2c, uint8_t addr);
int Si1147_MeasureUVAndObjectPresent(I2C_TypeDef *i2c, uint8_t addr, uint16_t *uvIndex, int* objectDetect);

/** @} (end addtogroup Drivers) */
/** @} (end addtogroup Si114x) */

#ifdef __cplusplus
}
#endif

#endif /* #define SI114X_ALGORITHM_H */
