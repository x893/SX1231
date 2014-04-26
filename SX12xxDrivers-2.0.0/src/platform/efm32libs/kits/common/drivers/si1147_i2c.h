/***************************************************************************//**
 * @file
 * @brief Driver for the Si1147 Proximity sensor
 * @version 3.20.2
 *******************************************************************************
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
#ifndef __SI1147_H
#define __SI1147_H

#include "em_device.h"

/***************************************************************************//**
 * @addtogroup Drivers
 * @{
 ******************************************************************************/

/***************************************************************************//**
 * @addtogroup Si114x
 * @{
 ******************************************************************************/

#ifdef __cplusplus
extern "C" {
#endif

/*******************************************************************************
 *******************************   DEFINES   ***********************************
 ******************************************************************************/
  

/*******************************************************************************
 ********************************   ENUMS   ************************************
 ******************************************************************************/

/*******************************************************************************
 *******************************   STRUCTS   ***********************************
 ******************************************************************************/



/*******************************************************************************
 *****************************   PROTOTYPES   **********************************
 ******************************************************************************/
int Si1147_Write_Register (I2C_TypeDef *i2c,uint8_t addr, uint8_t reg, uint8_t data);
int Si1147_Write_Block_Register (I2C_TypeDef *i2c,uint8_t addr, uint8_t reg, uint8_t length, uint8_t const *data);
int Si1147_Read_Block_Register (I2C_TypeDef *i2c,uint8_t addr, uint8_t reg, uint8_t length, uint8_t  *data);
int Si1147_Read_Register (I2C_TypeDef *i2c,uint8_t addr, uint8_t reg, uint8_t *data);


#ifdef __cplusplus
}
#endif

/** @} (end group Si114x) */
/** @} (end group Drivers) */

#endif /* __TEMPSENS_H */
