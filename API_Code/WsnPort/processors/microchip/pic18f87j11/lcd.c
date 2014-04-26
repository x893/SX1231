#include "platform.h"
#ifdef ENABLE_LCD

rom const uint16_t LCD_DELAY = HIRES_TIMEOUT(10);	// used only in init()
//rom const uint16_t LCD_DELAY_BUSY = HIRES_TIMEOUT(1000);

typedef struct {
	unsigned char bit0 : 1;
	unsigned char bit1 : 1;
	unsigned char bit2 : 1;
	unsigned char bit3 : 1;
	unsigned char bit4 : 1;
	unsigned char bit5 : 1;
	unsigned char e : 1;	// LCD E (enable) clock
	unsigned char rs : 1;	// LCD register select line
	// rs: 1=data, 0=instruction
} lcd_bits_t;

union {
	lcd_bits_t bits;
	unsigned char octet;
} lcd_u;

uint8_t lcd_cmd_buf[64];
int lcd_cmd_buf_in;
int lcd_cmd_buf_out;

#define DELAY_COUNT_1MS		30	// measured with 10MHz cpu clock

#define CMD_IWRITE	0xff

typedef enum {
	LCD_STATE__IDLE = 0,
	LCD_STATE__DELAY,
	LCD_STATE__DO_IWRITE,
	LCD_STATE__DWRITE
} lcd_state_e;

static lcd_state_e lcd_state;
static lcd_state_e lcd_state_after_delay;
static int lcd_delay_count;
static char char_to_put;

#if 0
static int
num_avail()
{
	//return lcd_cmd_buf_in - lcd_cmd_buf_out;
	int num_used;
	int my_in = lcd_cmd_buf_in;

	if (lcd_cmd_buf_out > my_in)
		my_in += sizeof(lcd_cmd_buf);

	num_used = lcd_cmd_buf_in - lcd_cmd_buf_out;

	return sizeof(lcd_cmd_buf) - num_used;
}
#endif

static void
WritePortA(void)
{
	LCD_CS = 0;
	SpiInOut(0x40);
	SpiInOut(0x12);
	SpiInOut(lcd_u.octet);
	LCD_CS = 1;
}

static void
WritePortB(uint8_t arg)
{
	LCD_CS = 0;
	SpiInOut(0x40);
	SpiInOut(0x13);
	SpiInOut(arg);
	LCD_CS = 1;
}

static void
InitWrite(uint8_t temp_wr)
{
	lcd_u.bits.e = FALSE;
	lcd_u.bits.rs = FALSE;
	WritePortA();
	WritePortB(temp_wr);
    _asm 
	nop
	nop
	nop
	_endasm

	lcd_u.bits.e = TRUE;
	WritePortA();
    _asm 
	nop
	nop
	nop
	nop
	nop
	nop
	nop
	nop
	nop
	_endasm
	lcd_u.bits.e = FALSE;
	WritePortA();
}

void
lcd_init()
{
	LCD_CS_DIR = OUTPUT;
	LCD_CS = 1;

	LCD_RST_DIR = OUTPUT;
	LCD_RST_DIR = 0;
	Wait(LCD_DELAY);
	LCD_RST_DIR = 1;

	/**** init port A *******/
	LCD_CS = 0;
	SpiInOut(0x40);
	SpiInOut(0x00);
	SpiInOut(0x00);
	LCD_CS = 1;

	/**** init port A *******/
	LCD_CS = 0;
	SpiInOut(0x40);
	SpiInOut(0x01);
	SpiInOut(0x00);
	LCD_CS = 1;

	lcd_u.bits.e = FALSE;
	lcd_u.bits.rs = FALSE;
	WritePortA();

	Wait(LCD_DELAY);
	InitWrite(0x3c);	// function set

	Wait(LCD_DELAY);
	InitWrite(0x0c);	// display off

	Wait(LCD_DELAY);
	InitWrite(0x01);	// display clear

	Wait(LCD_DELAY);
	InitWrite(0x06);	// entry mode


	lcd_cmd_buf_in = 0;
	lcd_cmd_buf_out = 0;
}

/*static void
LCDBusy(void)
{
	Wait(LCD_DELAY_BUSY);
}	*/

/*static void
i_write(uint8_t temp_wr)
{
	lcd_u.bits.e = FALSE;
	lcd_u.bits.rs = FALSE;
	WritePortA();

	LCDBusy();
	WritePortB(temp_wr);
    _asm 
	nop
	nop
	nop
	nop
	_endasm
	lcd_u.bits.e = TRUE;
	WritePortA();
    _asm 
	nop
	nop
	nop
	nop
	nop
	nop
	_endasm
	lcd_u.bits.e = FALSE;
	lcd_u.bits.rs = FALSE;
	WritePortA();
} */

void
LCDLine_1(void)
{
	//i_write(0x80);
	lcd_cmd_buf[lcd_cmd_buf_in++] = CMD_IWRITE;
	if (lcd_cmd_buf_in == sizeof(lcd_cmd_buf))
		lcd_cmd_buf_in = 0;
	if (lcd_cmd_buf_in == lcd_cmd_buf_out) {
		for (;;) {
			_asm nop _endasm
		}
	}

	lcd_cmd_buf[lcd_cmd_buf_in++] = 0x80;	// line1 command
	if (lcd_cmd_buf_in == sizeof(lcd_cmd_buf))
		lcd_cmd_buf_in = 0;

	if (lcd_cmd_buf_in == lcd_cmd_buf_out) {
		for (;;) {
			_asm nop _endasm
		}
	}
}

void
LCDLine_2(void)
{
	//i_write(0xc0);
	lcd_cmd_buf[lcd_cmd_buf_in++] = CMD_IWRITE;
	if (lcd_cmd_buf_in == sizeof(lcd_cmd_buf))
		lcd_cmd_buf_in = 0;
	if (lcd_cmd_buf_in == lcd_cmd_buf_out) {
		for (;;) {
			_asm nop _endasm
		}
	}

	lcd_cmd_buf[lcd_cmd_buf_in++] = 0xc0;	// line2 command
	if (lcd_cmd_buf_in == sizeof(lcd_cmd_buf))
		lcd_cmd_buf_in = 0;
	if (lcd_cmd_buf_in == lcd_cmd_buf_out) {
		for (;;) {
			_asm nop _endasm
		}
	}
}

void
ClearLCD()
{
	//i_write(0x01);
	lcd_cmd_buf[lcd_cmd_buf_in++] = CMD_IWRITE;
	if (lcd_cmd_buf_in == sizeof(lcd_cmd_buf))
		lcd_cmd_buf_in = 0;
	if (lcd_cmd_buf_in == lcd_cmd_buf_out) {
		for (;;) {
			_asm nop _endasm
		}
	}

	lcd_cmd_buf[lcd_cmd_buf_in++] = 0x01;	// ClearLCD command
	if (lcd_cmd_buf_in == sizeof(lcd_cmd_buf))
		lcd_cmd_buf_in = 0;
	if (lcd_cmd_buf_in == lcd_cmd_buf_out) {
		for (;;) {
			_asm nop _endasm
		}
	}
}

void
LCD_dwrite(uint8_t temp_wr)
{
#if 0
	//SLAVE_SLEEP_INDICATOR = 1;	// XXX debug
	LCDBusy();
	lcd_u.bits.e = FALSE;
	lcd_u.bits.rs = TRUE;
	WritePortA();
	WritePortB(temp_wr);
    _asm 
	nop
	nop
	nop
	nop
	_endasm
	lcd_u.bits.e = TRUE;
	WritePortA();
    _asm 
	nop
	nop
	nop
	nop
	nop
	nop
	_endasm
	lcd_u.bits.e = FALSE;
	lcd_u.bits.rs = FALSE;
	WritePortA();
	//SLAVE_SLEEP_INDICATOR = 0;	// XXX debug
#endif

	if (temp_wr == CMD_IWRITE)	// reserved value
		temp_wr = 'x';	

	lcd_cmd_buf[lcd_cmd_buf_in++] = temp_wr;
	if (lcd_cmd_buf_in == sizeof(lcd_cmd_buf))
		lcd_cmd_buf_in = 0;

	if (lcd_cmd_buf_in == lcd_cmd_buf_out) {
		for (;;) {
			_asm nop _endasm
		}
	}
}

void
LCD_puts(char line, const char *str)
{
	int i;

	if (line == 1)
		LCDLine_1();
	else
		LCDLine_2();

	for (i = 0; str[i] != 0; i++)
		LCD_dwrite(str[i]);	// write new

	for (; i < 16; i++)
		LCD_dwrite(' ');	// wipe off any stale characters
}

void
LCDLine_2_clear()
{
	int i;

	LCDLine_2();

	for (i = 0; i < 16; i++)
		LCD_dwrite(' ');	// wipe off any stale characters

	LCDLine_2();
}

/* LCD_adds(): append chars to the current line */
void
LCD_adds(const char *str)
{
	int i;

	for (i = 0; str[i] != 0; i++)
		LCD_dwrite(str[i]);	// write new
}

void
LCD_puts_from_rom(char line, rom_ptr const char *str)
{
	int i;

	if (line == 1)
		LCDLine_1();
	else
		LCDLine_2();

	for (i = 0; str[i] != 0; i++)
		LCD_dwrite(str[i]);	// write new

	for (; i < 16; i++)
		LCD_dwrite(' ');	// wipe off any stale characters
}

void
lcd_task()
{
	uint8_t cmd;

	switch (lcd_state) {
		case LCD_STATE__IDLE:
			if (lcd_cmd_buf_in != lcd_cmd_buf_out) {
				/* theres something new to do */
				cmd = lcd_cmd_buf[lcd_cmd_buf_out++];
				if (lcd_cmd_buf_out == sizeof(lcd_cmd_buf))
					lcd_cmd_buf_out = 0;

				if (cmd == CMD_IWRITE) {
					// start the i_write here
					lcd_u.bits.e = FALSE;
					lcd_u.bits.rs = FALSE;
					WritePortA();

					lcd_state = LCD_STATE__DELAY;
					lcd_state_after_delay = LCD_STATE__DO_IWRITE;
					lcd_delay_count = DELAY_COUNT_1MS;
					LATGbits.LATG1 = 1;
				} else {
					// cmd is a char to put
					char_to_put = cmd;
					lcd_state = LCD_STATE__DELAY;
					lcd_state_after_delay = LCD_STATE__DWRITE;
					lcd_delay_count = DELAY_COUNT_1MS;
					LATGbits.LATG1 = 1;
				}
			}
			break;
		case LCD_STATE__DO_IWRITE:
			cmd = lcd_cmd_buf[lcd_cmd_buf_out++];
			if (lcd_cmd_buf_out == sizeof(lcd_cmd_buf))
				lcd_cmd_buf_out = 0;

			// cmd is the arg to i_write()
			// we are now after the LCDBusy delay
			WritePortB(cmd);

    		_asm nop nop nop nop _endasm

			lcd_u.bits.e = TRUE;
			WritePortA();

    		_asm nop nop nop nop nop nop _endasm

			lcd_u.bits.e = FALSE;
			lcd_u.bits.rs = FALSE;
			WritePortA();

			lcd_state = LCD_STATE__IDLE;
			break;
		case LCD_STATE__DWRITE:
			lcd_u.bits.e = FALSE;
			lcd_u.bits.rs = TRUE;
			WritePortA();
			WritePortB(char_to_put);

    		_asm nop nop nop nop _endasm

			lcd_u.bits.e = TRUE;
			WritePortA();

    		_asm nop nop nop nop nop nop _endasm

			lcd_u.bits.e = FALSE;
			lcd_u.bits.rs = FALSE;
			WritePortA();

			lcd_state = LCD_STATE__IDLE;
			break;
		case LCD_STATE__DELAY:
			if (--lcd_delay_count == 0) {
				LATGbits.LATG1 = 0;
				lcd_state = lcd_state_after_delay;
			}
			break;
	} /* switch (lcd_state) */
}

#endif /* ENABLE_LCD */


