/***************************************************************************//**
 * @file
 * @brief i2c driver for the Si1147 
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

#include "i2cdrv_zero.h"
#include "si1147_i2c.h"

/*******************************************************************************
 *******************************   DEFINES   ***********************************
 ******************************************************************************/


/*******************************************************************************
 **************************   GLOBAL FUNCTIONS   *******************************
 ******************************************************************************/



/**************************************************************************//**
 * @brief
  *  Reads register from the Si1147 sensor.
 * @param[in] i2C
 *   The I2C peripheral to use (not used).
 * @param[in] addr
 *   The I2C address of the sensor.
 * @param[out] data
 *   The data read from the sensor.
 * @param[in] reg
 *   The register address to read from in the sensor.
 * @return 
 *   Returns number of bytes read on success. Otherwise returns error codes
 *   based on the I2CDRV.
 *****************************************************************************/
int Si1147_Read_Register (I2C_TypeDef *i2c,uint8_t addr, uint8_t reg, uint8_t *data)
{
  I2C_TransferSeq_TypeDef    seq;
  I2C_TransferReturn_TypeDef ret;
  uint8_t i2c_write_data[1];
  
  /* Unused parameter */
  (void) i2c;

  seq.addr  = addr;
  seq.flags = I2C_FLAG_WRITE_READ;
  /* Select register to start reading from */
  i2c_write_data[0] = reg;
  seq.buf[0].data = i2c_write_data;
  seq.buf[0].len  = 1;
  /* Select length of data to be read */
  seq.buf[1].data = data;
  seq.buf[1].len  = 1;

  ret = I2CDRV_Transfer(&seq);
  if (ret != i2cTransferDone)
  {
    *data = 0xff;
    return((int) ret);
  }
  
  return((int) 1);
}

/**************************************************************************//**
 * @brief
  *  Writes register in the Si1147 sensor.
 * @param[in] i2C
 *   The I2C peripheral to use (not used).
 * @param[in] addr
 *   The I2C address of the sensor.
 * @param[in] data
 *   The data to write to the sensor.
 * @param[in] reg
 *   The register address to write to in the sensor.
 * @return 
 *   Returns zero on success. Otherwise returns error codes
 *   based on the I2CDRV.
 *****************************************************************************/
int Si1147_Write_Register (I2C_TypeDef *i2c,uint8_t addr, uint8_t reg, uint8_t data)
{
  I2C_TransferSeq_TypeDef    seq;
  I2C_TransferReturn_TypeDef ret;
  uint8_t i2c_write_data[2];
  uint8_t i2c_read_data[1];
  /* Unused parameter */
  (void) i2c;

  seq.addr  = addr;
  seq.flags = I2C_FLAG_WRITE;
  /* Select register and data to write */
  i2c_write_data[0] = reg;
  i2c_write_data[1] = data;
  seq.buf[0].data = i2c_write_data;
  seq.buf[0].len  = 2;
  seq.buf[1].data = i2c_read_data;
  seq.buf[1].len  = 0;

  ret = I2CDRV_Transfer(&seq);
  if (ret != i2cTransferDone)
  {
    return((int) ret);
  }
  
  return((int) 0);
}

/**************************************************************************//**
 * @brief
  *  Writes a block of data to the Si1147 sensor.
 * @param[in] i2C
 *   The I2C peripheral to use (not used).
 * @param[in] addr
 *   The I2C address of the sensor.
 * @param[in] data
 *   The data to write to the sensor.
 * @param[in] length
 *   The number of bytes to write to the sensor.
 * @param[in] reg
 *   The first register to begin writing to.
 * @return 
 *   Returns zero on success. Otherwise returns error codes
 *   based on the I2CDRV.
 *****************************************************************************/
int Si1147_Write_Block_Register (I2C_TypeDef *i2c,uint8_t addr, uint8_t reg, uint8_t length, uint8_t const *data)
{
  I2C_TransferSeq_TypeDef    seq;
  I2C_TransferReturn_TypeDef ret;
  uint8_t i2c_write_data[10];
  uint8_t i2c_read_data[1];
  int i;
  /* Unused parameter */
  (void) i2c;

  seq.addr  = addr;
  seq.flags = I2C_FLAG_WRITE;
  /* Select register to start writing to*/
  i2c_write_data[0] = reg;
  for (i=0; i<length;i++)
  {
    i2c_write_data[i+1] = data[i];
  }
  seq.buf[0].data = i2c_write_data;
  seq.buf[0].len  = 1+length;
  seq.buf[1].data = i2c_read_data;
  seq.buf[1].len  = 0;

  ret = I2CDRV_Transfer(&seq);
  if (ret != i2cTransferDone)
  {
    return((int) ret);
  }
  
  return((int) 0);
}

/**************************************************************************//**
 * @brief
  *  Reads a block of data from the Si1147 sensor.
 * @param[in] i2C
 *   The I2C peripheral to use (not used).
 * @param[in] addr
 *   The I2C address of the sensor.
 * @param[out] data
 *   The data read from the sensor.
 * @param[in] length
 *   The number of bytes to write to the sensor.
 * @param[in] reg
 *   The first register to begin reading from.
 * @return 
 *   Returns number of bytes read on success. Otherwise returns error codes
 *   based on the I2CDRV.
 *****************************************************************************/
int Si1147_Read_Block_Register (I2C_TypeDef *i2c,uint8_t addr, uint8_t reg, uint8_t length, uint8_t *data)
{
  I2C_TransferSeq_TypeDef    seq;
  I2C_TransferReturn_TypeDef ret;
  uint8_t i2c_write_data[1];
  uint8_t i2c_read_data[10];
  int i;
  /* Unused parameter */
  (void) i2c;

  seq.addr  = addr;
  seq.flags = I2C_FLAG_WRITE_READ;
  /* Select register to start reading from */
  i2c_write_data[0] = reg;
  seq.buf[0].data = i2c_write_data;
  seq.buf[0].len  = 1;
  /* Select length of data to be read */
  seq.buf[1].data = i2c_read_data;
  seq.buf[1].len  = length;
  for (i=0; i<length;i++)
  {
    data[i] = i2c_read_data[i];
  }
  ret = I2CDRV_Transfer(&seq);
  if (ret != i2cTransferDone)
  {
    return((int) ret);
  }
  
  return((int) length);
}

