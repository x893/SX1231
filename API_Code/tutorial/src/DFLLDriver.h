/*******************************************************************
** File        : DFLLDriver.h                                     **
********************************************************************
**                                                                **
** Version     : V 1.0                                            **
**                                                                **
** Written by   : Miguel Luis                                     **
**                                                                **
** Date        : 14-02-2002                                       **
**                                                                **
** Project     : -                                                **
**                                                                **
********************************************************************
** Changes     : V 1.1 - Add the 2 following lines:               **
**                       StaticRegCntOn = RegCntOn;               **
**                       RegCntOn = 0;                            **
**               V 1.2 - Add the instruction "FREQ div2" and      **
**                       "Freq nodiv"                             **
********************************************************************
** Description : DFLL (Digital Frequency Locked Loop)             **
**               This DFLL implementation is precise at 2%        **
**               The tests were made at ambient temperature       **
**               (See Excel sheet for more information)           **
********************************************************************
** Code Size   : Tested version Optimization None                 **
**               Optimization None    : 254 instructions          **
**               Optimization Level 1 : 182 instructions          **
**               Optimization Level 2 : 185 instructions          **
**               Optimization Level 3 : 230 instructions          **
*******************************************************************/
#ifndef __DFLLDRIVER__
#define __DFLLDRIVER__
/*******************************************************************
** Include files                                                  **
*******************************************************************/
#include "Globals.h"

/*******************************************************************
** DFLLRun : Makes measurement of RC oscillator until the wished  **
**           frequency is found                                   **
********************************************************************
** In      : Frequency (in Hz)                                    **
** Out     : -                                                    **
*******************************************************************/
void DFLLRun (_U32 Frequency);

#endif /* __DFLLDRIVER__ */
