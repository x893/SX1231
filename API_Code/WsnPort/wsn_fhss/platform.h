#ifndef _PLATFORM_H_
#define _PLATFORM_H_
/**
 * \file platform.h
 * prototypes of cpu-specific functions
 *
 */

#include "types.h"
#include "cpu.h"
#include "timers.h"
#include "io_port_mapping.h"
#include "lcd.h"

typedef enum {
	HW_ADDRESS__MASTER,
	HW_ADDRESS__SLAVE0,
	HW_ADDRESS__SLAVE1,
	HW_ADDRESS__SLAVE2,
	HW_ADDRESS__SLAVE3,

	HW_ADDRESS__MAX
} hw_address_e;



/**
 * configure the radio transceiver with the next radio channel to be used, and increment the hop count
 * \param channel_num pointer the the running hop count. The current value is used as the channel number to used, and is then post-incremented.
 * \ingroup wsn_fhss
 */
void Fhss_Hop(uint8_t *channel_num);

/**
 * Implements the synchronization mode for both master and slave.
 * For master, it sweeps all the radio channels with the synchronization message
 * For slave, it attempts to aquire the synchronization messages from the master.
 * \ingroup wsn_fhss
 */
uint8_t Sync_fhss(void);

extern uint8_t radio_channel_dialog;	// the running hop counter
extern uint8_t current_radio_channel;	// indicate where the radio is now

extern char text[];
extern uint8_t RFbuffer[];       // RF buffer
extern volatile uint8_t RFbufferSize;          // RF buffer size
extern uint8_t Node_adrs;


/**
 * \defgroup UART_functions UART driver implemented in uart.c
 * \defgroup SPI_functions SPI driver implemented in spi.c
 * \defgroup TIMER_functions TIMER driver implemented in timers.c
 */

/**
 * send bytes out the UART
 * \param buffer pointer to byte to send
 * \param size the count of bytes to send
 * \ingroup UART_functions
 */
//void USART_send(const uint8_t *buffer, uint8_t size);
//void USART_send(const unsigned char *buffer, unsigned char size);

/**
 * send a string out the UART
 * \param str the null-terminated string to send
 * \ingroup UART_functions
 */
void USART_send_str(const char *str);
void USART_send_str_from_rom(const char rom_ptr *str);
void _usart_putc(char c);


/**
 * receive a character from the UART
 * \return the byte received or 0xff for nothing received
 * \ingroup UART_functions
 */
uint8_t USART_Receive(void);

/**
 * initialize the UART
 * if external clock is used instead of crystal, this function should be called
 * after configuring the external clock
 * \ingroup UART_functions
 */
void uart_init(void);

/**
 * initialize the SPI port as master
 * \ingroup SPI_functions
 */
void SPIInit(void);

/**
 * transfer a byte across the SPI
 * SPI is full duplex, meaning as a byte is sent, a byte is also received.
 * \param outputByte the byte to send
 * \return the byte received
 * \ingroup SPI_functions
 */
uint8_t SpiInOut(uint8_t outputByte);

/**
 * blocks execution until the hi-resolution timer has expired
 * \param compare_value the amount of time to suspend. Use the macro HIRES_TIMEOUT() to convert microseconds to the value needed by this function.
 * \ingroup TIMER_functions
 */
void Wait(uint16_t compare_value);

/**
 * start the hi-resolutiont timer
 * The caller will poll HIRES_COMPARE_B_FLAG to determine when this timer has expired.
 * \param compare_value the amount of time until the HIRES_COMPARE_B_FLAG will be raised. Use the macro HIRES_TIMEOUT() to convert microseconds to the value needed by this function.
 * \ingroup TIMER_functions
 */
void EnableClock_HiRes(uint16_t compare_value);

/**
 * put the transceiver and cpu into low power wait/sleep mode
 * This function enables the interrupt on the low-resolution timer.  The ISR resides in the cpu-specific file timers.c
 * The caller is expected to first call EnableClock() to configure the period until wakeup.
 * \ingroup TIMER_functions
 */
void go_sleep(void);

/**
 * set the time until the low-resolution timer expires
 * This function enables the interrupt on the low-resolution timer.  The ISR resides in the cpu-specific file timers.c
 * \param timeout the amount of time before experation. Use the macro LOWRES_TIMEOUT() to convert milliseconds to the value required.
 * \ingroup TIMER_functions
 */
void EnableClock(uint16_t timeout);

/**
 * one-time power-up initialization of both low-resolution and hi-resolution timers.
 * \ingroup TIMER_functions
 */
void timers_init(void);

void poll_hardware_address(void);

#endif /* _PLATFORM_H_ */
