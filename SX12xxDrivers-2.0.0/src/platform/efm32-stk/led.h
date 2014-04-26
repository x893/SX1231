#ifndef __LED_H__
#define __LED_H__

typedef enum 
{
    LED_GREEN = 0,
    LED_RED = 1,
} tLed;

#define LedOn( led )		BSP_LedSet( led )
#define LedOff( led )		BSP_LedClear( led )
#define LedToggle( led )	BSP_LedToggle( led )

#endif // __LED_H__
