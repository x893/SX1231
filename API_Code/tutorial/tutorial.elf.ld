
STARTUP("C:\Ride7\Ride\lib\C816\templates\XE88LC06A\crt0.o")
INPUT("D:\Data\SX1212_API\API\API_code\tutorial\tutorial.elf.wrapp.o")
INPUT("obj\sx1212driver.o")
INPUT("obj\main.o")
INPUT("obj\initialisation.o")
INPUT("obj\dflldriver.o")
INPUT("C:\Ride7\Ride\lib\C816\templates\XE88LC06A\IRQHandler.a")
OUTPUT("D:\Data\SX1212_API\API\API_code\tutorial\tutorial.elf")
INCLUDE "C:\Ride7\Ride\lib\C816\templates\XE88LC06A\crt0.ld"
SEARCH_DIR("C:\Ride7\Ride\GNU/c816-gcc\C816\LIB")
SEARCH_DIR("C:\Ride7\Ride\GNU/c816-gcc\LIB\GCC-LIB\C816\EGCS-2.91.02")
INPUT("libc_small_rom_sc_lsc.a")
INPUT("libm_small_rom_sc_lsc.a")
INPUT("libgcc_sc.a")
INPUT("D:\Data\SX1212_API\API\API_code\tutorial\tutorial.elf.DataInit.o")
GEN_INDIRECT_LISTING("D:\Data\SX1212_API\API\API_code\tutorial\tutorial.elf.indcalls")
NOMORE_GEN_INDIRECT_LISTING
