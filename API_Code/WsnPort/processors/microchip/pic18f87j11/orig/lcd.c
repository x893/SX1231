#include "platform.h"
#ifdef ENABLE_LCD

rom const uint16_t LCD_DELAY = HIRES_TIMEOUT(10);
rom const uint16_t LCD_DELAY_BUSY = HIRES_TIMEOUT(1000);

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

}

static void
LCDBusy(void)
{
	Wait(LCD_DELAY_BUSY);
}

static void
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
}

void
LCDLine_1(void)
{
	i_write(0x80);
}

void
LCDLine_2(void)
{
	i_write(0xc0);
}

void
ClearLCD()
{
	i_write(0x01);
}

void
LCD_dwrite(uint8_t temp_wr)
{
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

#endif /* ENABLE_LCD */


