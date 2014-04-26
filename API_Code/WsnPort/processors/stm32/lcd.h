#ifndef __LCD_H__
#define __LCD_H__

#ifndef ENABLE_LCD

	#define lcd_init()
	#define LCDProcessEvents()
	#define LCD_puts_from_rom(line, msg)
	#define LCDLine_1()
	#define LCDLine_2()
	#define LCDLine_2_clear()
	#define LCD_dwrite(ch)
	#define LCD_puts(line, msg)
	#define LCD_puts_from_rom(line, msg)
	#define LCD_adds(line)
#else
	void lcd_init(void);
	void LCDLine_1(void);
	void LCDLine_2(void);
	void LCDLine_2_clear(void);
	void LCD_dwrite(uint8_t ch);
	void LCD_puts(char line, const char *str);
	void LCD_puts_from_rom(char line, rom_ptr const char *str);
	void LCD_adds(const char *str);

#endif /* ENABLE_LCD */

#endif
