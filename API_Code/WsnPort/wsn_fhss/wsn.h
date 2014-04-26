/**
 * \file wsn.h
 * 
 *
 */
#include "platform.h"
#include "FHSSapi.h"


extern volatile char Slave_Needs_Hop;	// flag
extern hw_address_e hw_address;

void new_hw_address(void);

