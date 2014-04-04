#ifndef __GLOBALS__
#define __GLOBALS__

/*******************************************************************
** Soft generation control (RAISONANCE monitor usage)             **
*******************************************************************/
#define usemonitor 0
#if usemonitor
	#include <Monit.h>
#else
	#define _Monitor_Init()
	#define _Monitor_SoftBreak
#endif

/*******************************************************************
** Soft generation control target type                            **
** modify the TARGET value only                                   **
*******************************************************************/

/*******************************************************************
** Uncoment if using Phyton PICE-XE                               **
*******************************************************************/
//#define _XE88LC06A_

/*******************************************************************
** Uncoment if using Phyton PICE-XE                               **
*******************************************************************/
#include <Types.h>

/*******************************************************************
** Target specific Include files                                  **
** dont need to be modified                                       **
*******************************************************************/
#ifdef _XE88LC01_
    #include <XE88LC01.h>
#endif
#ifdef _XE88LC02_
    #include <XE88LC02.h>
#endif
#ifdef _XE88LC03_
    #include <XE88LC03.h>
#endif
#ifdef _XE88LC05_
    #include <XE88LC05.h>
#endif
#ifdef _XE88LC06A_
    #include <XE88LC06A.h>
#endif

/*******************************************************************
** Application specific Include files                             **
*******************************************************************/
#include "Initialisation.h"
#include "DFLLDriver.h"
#include "SX1212Driver.h"

/*******************************************************************
** Global definitions                                             **
*******************************************************************/


/*******************************************************************
** Protocol commands definitions                                  **
*******************************************************************/

/*******************************************************************
** Macros definitions                                             **
*******************************************************************/
#define set_bit(reg, bit) reg |= bit
#define clear_bit(reg, bit) reg &= ~(bit)
#define toggle_bit(reg, bit) reg ^= bit
#define check_bit(reg, bit) (reg & bit) ? 1 : 0

#define InitializeWatchDog() RegSysCtrl |= 0x10
#define ResetWatchDog() asm("move %0, #0x0A"::"m"(RegSysWd));\
                        asm("move %0, #0x03"::"m"(RegSysWd))

/*******************************************************************
** Types definitions                                              **
*******************************************************************/
#endif /* __GLOBALS__ */

