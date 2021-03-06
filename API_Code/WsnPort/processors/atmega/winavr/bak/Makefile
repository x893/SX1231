###############################################################################
# Makefile for the project WSN_ATMEL
###############################################################################

## General Flags
PROJECT = WSN_ATMEL
#MCU = atmega644p
MCU = atmega644
TARGET = $(PROJECT).elf
CC = avr-gcc.exe

## Options common to compile, link and assembly rules
COMMON = -mmcu=$(MCU)

## Compile options common for all C compilation units.
CFLAGS = -DREADBACK_VERIFY -D_CPU_SLEEP_ $(COMMON)
CFLAGS += -Wall -gdwarf-2 -D$(MCU) -DF_CPU=8000000UL -O1 -funsigned-char -funsigned-bitfields -fpack-struct -fshort-enums
CFLAGS += -MD -MP -MT $(*F).o -MF dep/$(@F).d 

## Assembly specific flags
ASMFLAGS = $(COMMON)
ASMFLAGS += $(CFLAGS)
ASMFLAGS += -x assembler-with-cpp -Wa,-gdwarf2

## Linker flags
LDFLAGS = $(COMMON)
LDFLAGS += -Wl,-Map=$(PROJECT).map,--cref


## Intel Hex file production flags
HEX_FLASH_FLAGS = -R .eeprom

HEX_EEPROM_FLAGS = -j .eeprom
HEX_EEPROM_FLAGS += --set-section-flags=.eeprom="alloc,load"
HEX_EEPROM_FLAGS += --change-section-lma .eeprom=0 --no-change-warnings

WSN_DIR = ../../../wsn_fhss
XCVR_DIR = ../../../transceivers
SX1211_DIR = $(XCVR_DIR)/sx1211
INCLUDES = -I.. -I$(WSN_DIR) -I$(XCVR_DIR) -I$(SX1211_DIR)

## Objects that must be built in order to link
OBJECTS =	\
	../cpu.o	\
	../timers.o	\
	../spi.o	\
	../uart.o	\
	$(WSN_DIR)/FHSSapi.o	\
	$(SX1211_DIR)/SX1211driver.o	\
	$(WSN_DIR)/wsn.o

## Objects explicitly added by the user
LINKONLYOBJECTS = 

## Build
all: $(TARGET) WSN_ATMEL.hex WSN_ATMEL.eep size

## Compile
$(OBJECTS): %.o : %.c
	$(CC) -DSX1211 -DSX1211_CRYSTAL_12_8MHZ $(INCLUDES) $(CFLAGS) -c $< -o $@


##Link
$(TARGET): $(OBJECTS)
	 $(CC) $(LDFLAGS) $(OBJECTS) $(LINKONLYOBJECTS) $(LIBDIRS) $(LIBS) -o $(TARGET)

%.hex: $(TARGET)
	avr-objcopy -O ihex $(HEX_FLASH_FLAGS)  $< $@

%.eep: $(TARGET)
	-avr-objcopy $(HEX_EEPROM_FLAGS) -O ihex $< $@ || exit 0

%.lss: $(TARGET)
	avr-objdump -h -S $< > $@

size: ${TARGET}
	@echo
	@avr-size -C --mcu=${MCU} ${TARGET}

# dependencies:
$(WSN_DIR)/wsn.o: $(XCVR_DIR)/transceiver.h $(WSN_DIR)/WSN.h 
$(WSN_DIR)/WSN.h: $(WSN_DIR)/platform.h $(WSN_DIR)/FHSSapi.h
$(WSN_DIR)/FHSSapi.o: $(XCVR_DIR)/transceiver.h $(WSN_DIR)/FHSSapi.h $(WSN_DIR)/WSN.h
$(WSN_DIR)/sx1211driver.o: $(SX1211_DIR)/SX1211driver.h $(WSN_DIR)/FHSSapi.h
$(WSN_DIR)/SX1211driver.h: $(WSN_DIR)/platform.h
$(WSN_DIR)/platform.h: ../types.h ../cpu.h ../timers.h ../io_port_mapping.h

## Clean target
.PHONY: clean
clean:
	-rm -rf dep/* $(OBJECTS) $(PROJECT).elf $(PROJECT).hex $(PROJECT).eep $(PROJECT).map

## Other dependencies
#-include $(shell mkdir dep 2>/dev/null) $(wildcard dep/*)


# $ avarice.exe --mkII --erase --program --file WSN_ATMEL.elf --part atmega644 --jtag /dev/ttyS0 :4242

