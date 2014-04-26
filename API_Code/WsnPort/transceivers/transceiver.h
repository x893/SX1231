#include "types.h"

#define RF_BUFFER_SIZE		64

typedef enum {
	RF_MODE_SLEEP,			/**< xosc off */
	RF_MODE_STANDBY,		/**< xosc on */
	RF_MODE_SYNTHESIZER,	/**< PLL on */
	RF_MODE_RECEIVER,		/**< RF receiver mode */
	RF_MODE_TRANSMITTER		/**< RF transmitting mode */
} rf_mode_e;

typedef enum {
	RADIO_OK,			/**< completed successfully */
	RADIO_RX_RUNNING,	/**< not yet completed */
	RADIO_RX_TIMEOUT,	/**< nothing received within the specified time period */
	RADIO_ERROR			/**< fail */
} radio_return_e;

/**
 * sets the RF transceiver's operating mode (RX, TX, standby etc)
 * \param mode the desired operating mode
 */
void SetRFMode(uint8_t mode);

/**
 * transmit a packet in RF transmit mode
 * \param buffer pointer to the message to transmit
 * \param size the number of payload bytes in the message buffer.  The length passed here is not to include the Node_adrs or the length byte itself, only the length of bytes in your payload buffer.
 * \param Node_adrs the node address byte to send with the packet.  Will be decoded by packet engine on the remote side.
 * \param immediate_rx if true, go directly to RX mode after transmission complete.  Eliminates the possibilty of missing a received message due to receiver not being enabled quickly enough.
 */
uint8_t SendRfFrame(const uint8_t *buffer, uint8_t size, uint8_t Node_adrs, char immediate_rx);

/**
 * receive an RF packet
 * \param buffer pointer to buffer which will hold the received message
 * \param size pointer written to by this function which indicates the length of payload received
 * \param Timeout how long to wait for a packet to be received. Use the macro HIRES_TIMEOUT() to obtain the value to pass here.
 * \return RADIO_OK when packet has been received, or RADIO_RX_TIMEOUT when nothing has been received within the specified period.  RADIO_RX_RUNNING will be returned if the timeout period has not yet expired when nothing has been received.
 */
uint8_t ReceiveRfFrame(uint8_t *buffer, uint8_t *size, uint16_t Timeout);

/**
 * set the receiver filtering node address.  Only messages which contain this Node_Adrs will be received.
 */
void RadioSetNodeAddress(uint8_t node_adrs);

/**
 * run once at power-up.  Configures the RF transceivers registers for operation.
 */
void InitRFChip(void);

