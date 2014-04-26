/*******************************************************************
** File        : main.c                                           **
********************************************************************
**                                                                **
** Version     : V 1.0                                            **
**                                                                **
** Written by   : Miguel Luis                                     **
**                                                                **
** Date        : 15-01-2004                                       **
**                                                                **
** Project     : API - Tutorial                                   **
**                                                                **
********************************************************************
** Changes     :                                                  **
********************************************************************
** Description : Main program                                     **
*******************************************************************/
#include "Globals.h"

/*******************************************************************
** Global variables declaration                                   **
*******************************************************************/
_U8 RFbuffer[RF_BUFFER_SIZE];       // RF buffer
volatile _U8 RFbufferSize;          // RF buffer size
volatile _U8 EnableMaster = true;   // Master/Slave selection
volatile _U8 SendNextPacket = true; // Indicates when to send the next frame

/*******************************************************************
** OnMaster :                                                     **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void OnMaster(void)
{
	_U8 ReturnCode = -1;
	// Test if a PING needs to be sent
	if (SendNextPacket)
	{
		// Copy the PING string to the buffer used to send the frame
		strcpy(RFbuffer, "PING");
		// Sends the frame to the RF chip
		SendRfFrame(RFbuffer, strlen(RFbuffer), &ReturnCode);
		// Indicates that we want to wait for an answer
		SendNextPacket = false;
	}
	else
	{
		// Receives the frame from the RF chip
		ReceiveRfFrame(RFbuffer, (_U8*)&RFbufferSize, &ReturnCode);
		// Tests if there is a TimeOut or if we have received a frame
		if (ReturnCode == OK)
		{
			// Sets the last byte of received buffer to 0. Needed by strcmp function
			RFbuffer[RFbufferSize] = '\0';
			// Tests if the received buffer size is greater than 0
			if (RFbufferSize > 0)
			{
				// Tests if the received buffer value is PONG
				if (strcmp(RFbuffer, "PONG") == 0)
				{
					// Indicates on a LED that the received frame is a PONG
					toggle_bit(RegPBOut, 0x01);
				}
			}
			// Indicates that we can send the next PING frame
			SendNextPacket = true;
		}
		else if (ReturnCode == RX_TIMEOUT)
		{
			// Indicates that we can send the next PING frame
			SendNextPacket = true;
		}
	}
}

/*******************************************************************
** OnSlave :                                                      **
********************************************************************
** In  : -                                                        **
** Out : -                                                        **
*******************************************************************/
void OnSlave(void)
{
	_U8 ReturnCode = -1;
	// Receives the frame from the RF chip
	ReceiveRfFrame(RFbuffer, (_U8*)&RFbufferSize, &ReturnCode);
	// Tests if we have received a frame
	// Tests if the received buffer size is greater than 0
	if (ReturnCode == OK && RFbufferSize > 0)
	{
		// Sets the last byte of received buffer to 0. Needed by strcmp function
		RFbuffer[RFbufferSize] = '\0';
		// Tests if the received buffer value is PING
		if (strcmp(RFbuffer, "PING") == 0)
		{
			// Indicates on a LED that the received frame is a PING
			toggle_bit(RegPBOut, 0x01);
			// Copy the PONG string to the buffer used to send the frame
			strcpy(RFbuffer, "PONG");
			// Sends the frame to the RF chip
			SendRfFrame(RFbuffer, strlen(RFbuffer), &ReturnCode);
		}
	}
}

/*******************************************************************
** main : Main program function                                   **
********************************************************************
** In   : -                                                       **
** Out  : -                                                       **
*******************************************************************/
int main(void)
{
	InitMicro();

	_Monitor_Init();
	_Monitor_SoftBreak;

	InitRFChip();

	// Main Loop
	while (1)
	{
		if (EnableMaster)
			OnMaster();
		else
			OnSlave();
	}
	return 0;
}
