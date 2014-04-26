#ifndef __BSPCONFIG_H__
#define __BSPCONFIG_H__

#if   ( PLATFORM == STK3200 )

	#include "..\..\EFM32ZG_STK3200\config\bspconfig.h"

#elif ( PLATFORM == STK3700 )

	#include "..\..\EFM32GG_STK3700\config\bspconfig.h"

#else

	#error Unknown board

#endif

#endif
