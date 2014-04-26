/**
 * \file FHSSapi.h
 *
 *
 */


#define _CPU_SLEEP_		1

#define NUM_CHANNELS	50

//State machine
#define		NOT_SYNC	         0x00
#define		SYNC_OK		         0x02
#define		SYNC_RX_RUN	         0x03
#define		SYNC_TX_RUN			 0x04
#define		SYNC_RX_TIMEOUT		 0x05
#define		SYNC_RX_OK			 0x06
#define		SYNC_TX_DIALOG_CH	 0x07
#define		SYNC_END			 0xFA

//Node ID (Address byte in register 29)
#define		BroadCast_ID		0x00
#define		Master_ID			0x01
#define		Slave0_ID			0x02
#define		Slave1_ID			0x03
#define		Slave2_ID			0x04
#define		Slave3_ID			0x05

/* SyncState exported to allow state to be reset upon mode change */
extern uint8_t SyncState;

#ifdef SLAVE_ANSWER_ALL
extern volatile char hop_on_next_wakeup;
#endif
