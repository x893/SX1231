/******************************************************************************/
/* lnk_msp430f149.cmd - LINKER COMMAND FILE FOR LINKING MSP430F149 PROGRAMS */
/*                                                                            */
/*  Ver | dd mmm yyyy | Who  | Description of changes                         */
/* =====|=============|======|=============================================   */
/*  0.01| 08 Mar 2004 | A.D. | First prototype                                */
/*  0.02| 26 Mai 2004 | A.D. | Leading symbol underscores removed,            */
/*      |             |      | Interrupt vector definition changed            */
/*  0.03| 22 Jun 2004 | A.D. | File reformatted                               */
/*                                                                            */
/*   Usage:  lnk430 <obj files...>    -o <out file> -m <map file> lnk.cmd     */
/*           cl430  <src files...> -z -o <out file> -m <map file> lnk.cmd     */
/*                                                                            */
/*----------------------------------------------------------------------------*/

/* These linker options are for command line linking only.  For IDE linking,  */
/* you should set your linker options in Project Properties                   */
/* -c                                               LINK USING C CONVENTIONS  */
/* -stack  0x0100                                   SOFTWARE STACK SIZE       */
/* -heap   0x0100                                   HEAP AREA SIZE            */

/*----------------------------------------------------------------------------*/
/* 'Allocate' peripheral registers at given addresses                         */
/*----------------------------------------------------------------------------*/

/************************************************************
* STANDARD BITS
************************************************************/
/************************************************************
* STATUS REGISTER BITS
************************************************************/
/************************************************************
* PERIPHERAL FILE MAP
************************************************************/
/************************************************************
* SPECIAL FUNCTION REGISTER ADDRESSES + CONTROL BITS
************************************************************/
IE1                = 0x0000;
IFG1               = 0x0002;
ME1                = 0x0004;
IE2                = 0x0001;
IFG2               = 0x0003;
ME2                = 0x0005;
/************************************************************
* WATCHDOG TIMER
************************************************************/
WDTCTL             = 0x0120;
/************************************************************
* HARDWARE MULTIPLIER
************************************************************/
MPY                = 0x0130;
MPYS               = 0x0132;
MAC                = 0x0134;
MACS               = 0x0136;
OP2                = 0x0138;
RESLO              = 0x013A;
RESHI              = 0x013C;
SUMEXT             = 0x013E;
/************************************************************
* DIGITAL I/O Port1/2
************************************************************/
P1IN               = 0x0020;
p1_in               = 0x0020;
P1OUT              = 0x0021;
P1DIR              = 0x0022;
P1IFG              = 0x0023;
P1IES              = 0x0024;
P1IE               = 0x0025;
P1SEL              = 0x0026;
P2IN               = 0x0028;
P2OUT              = 0x0029;
p2_out              = 0x0029;
P2DIR              = 0x002A;
p2_dir              = 0x002A;
P2IFG              = 0x002B;
P2IES              = 0x002C;
P2IE               = 0x002D;
P2SEL              = 0x002E;
/************************************************************
* DIGITAL I/O Port3/4
************************************************************/
P3IN               = 0x0018;
P3OUT              = 0x0019;
P3DIR              = 0x001A;
P3SEL              = 0x001B;
P4IN               = 0x001C;
P4OUT              = 0x001D;
p4_out              = 0x001D;
P4DIR              = 0x001E;
p4_dir              = 0x001E;
P4SEL              = 0x001F;
/************************************************************
* DIGITAL I/O Port5/6
************************************************************/
P5IN               = 0x0030;
P5OUT              = 0x0031;
P5DIR              = 0x0032;
P5SEL              = 0x0033;
p5_sel              = 0x0033;
P6IN               = 0x0034;
P6OUT              = 0x0035;
P6DIR              = 0x0036;
P6SEL              = 0x0037;
/************************************************************
* USART
************************************************************/
/************************************************************
* USART 0
************************************************************/
U0CTL              = 0x0070;
U0TCTL             = 0x0071;
U0RCTL             = 0x0072;
U0MCTL             = 0x0073;
U0BR0              = 0x0074;
U0BR1              = 0x0075;
U0RXBUF            = 0x0076;
U0TXBUF            = 0x0077;
/************************************************************
* USART 1
************************************************************/
U1CTL              = 0x0078;
U1TCTL             = 0x0079;
U1RCTL             = 0x007A;
U1MCTL             = 0x007B;
U1BR0              = 0x007C;
U1BR1              = 0x007D;
U1RXBUF            = 0x007E;
U1TXBUF            = 0x007F;
/************************************************************
* Timer A3
************************************************************/
TAIV               = 0x012E;
TACTL              = 0x0160;
TACCTL0            = 0x0162;
TACCTL1            = 0x0164;
TACCTL2            = 0x0166;
TAR                = 0x0170;
TACCR0             = 0x0172;
TACCR1             = 0x0174;
TACCR2             = 0x0176;
/************************************************************
* Timer B7
************************************************************/
TBIV               = 0x011E;
TBCTL              = 0x0180;
TBCCTL0            = 0x0182;
TBCCTL1            = 0x0184;
TBCCTL2            = 0x0186;
TBCCTL3            = 0x0188;
TBCCTL4            = 0x018A;
TBCCTL5            = 0x018C;
TBCCTL6            = 0x018E;
TBR                = 0x0190;
TBCCR0             = 0x0192;
TBCCR1             = 0x0194;
TBCCR2             = 0x0196;
TBCCR3             = 0x0198;
TBCCR4             = 0x019A;
TBCCR5             = 0x019C;
TBCCR6             = 0x019E;
/************************************************************
* Basic Clock Module
************************************************************/
DCOCTL             = 0x0056;
BCSCTL1            = 0x0057;
BCSCTL2            = 0x0058;
/*************************************************************
* Flash Memory
*************************************************************/
FCTL1              = 0x0128;
FCTL2              = 0x012A;
FCTL3              = 0x012C;
/************************************************************
* Comparator A
************************************************************/
CACTL1             = 0x0059;
CACTL2             = 0x005A;
CAPD               = 0x005B;
/************************************************************
* ADC12
************************************************************/
ADC12CTL0          = 0x01A0;
ADC12CTL1          = 0x01A2;
ADC12IFG           = 0x01A4;
ADC12IE            = 0x01A6;
ADC12IV            = 0x01A8;
ADC12MEM0          = 0x0140;
ADC12MEM1          = 0x0142;
ADC12MEM2          = 0x0144;
ADC12MEM3          = 0x0146;
ADC12MEM4          = 0x0148;
ADC12MEM5          = 0x014A;
ADC12MEM6          = 0x014C;
ADC12MEM7          = 0x014E;
ADC12MEM8          = 0x0150;
ADC12MEM9          = 0x0152;
ADC12MEM10         = 0x0154;
ADC12MEM11         = 0x0156;
ADC12MEM12         = 0x0158;
ADC12MEM13         = 0x015A;
ADC12MEM14         = 0x015C;
ADC12MEM15         = 0x015E;
ADC12MCTL0         = 0x0080;
ADC12MCTL1         = 0x0081;
ADC12MCTL2         = 0x0082;
ADC12MCTL3         = 0x0083;
ADC12MCTL4         = 0x0084;
ADC12MCTL5         = 0x0085;
ADC12MCTL6         = 0x0086;
ADC12MCTL7         = 0x0087;
ADC12MCTL8         = 0x0088;
ADC12MCTL9         = 0x0089;
ADC12MCTL10        = 0x008A;
ADC12MCTL11        = 0x008B;
ADC12MCTL12        = 0x008C;
ADC12MCTL13        = 0x008D;
ADC12MCTL14        = 0x008E;
ADC12MCTL15        = 0x008F;
/************************************************************
* Interrupt Vectors (offset from 0xFFE0)
************************************************************/
/************************************************************
* End of Modules
************************************************************/

/****************************************************************************/
/* SPECIFY THE SYSTEM MEMORY MAP                                            */
/****************************************************************************/

MEMORY
{
    SFR                     : origin = 0x0000, length = 0x0010
    PERIPHERALS_8BIT        : origin = 0x0010, length = 0x00F0
    PERIPHERALS_16BIT       : origin = 0x0100, length = 0x0100
    RAM                     : origin = 0x0200, length = 0x0800
    INFOA                   : origin = 0x1080, length = 0x0080
    INFOB                   : origin = 0x1000, length = 0x0080
    FLASH                   : origin = 0x1100, length = 0xEEE0
    INT00                   : origin = 0xFFE0, length = 0x0002
    INT01                   : origin = 0xFFE2, length = 0x0002
    INT02                   : origin = 0xFFE4, length = 0x0002
    INT03                   : origin = 0xFFE6, length = 0x0002
    INT04                   : origin = 0xFFE8, length = 0x0002
    INT05                   : origin = 0xFFEA, length = 0x0002
    INT06                   : origin = 0xFFEC, length = 0x0002
    INT07                   : origin = 0xFFEE, length = 0x0002
    INT08                   : origin = 0xFFF0, length = 0x0002
    INT09                   : origin = 0xFFF2, length = 0x0002
    INT10                   : origin = 0xFFF4, length = 0x0002
    INT11                   : origin = 0xFFF6, length = 0x0002
    INT12                   : origin = 0xFFF8, length = 0x0002
    INT13                   : origin = 0xFFFA, length = 0x0002
    INT14                   : origin = 0xFFFC, length = 0x0002
    RESET                   : origin = 0xFFFE, length = 0x0002
}

/****************************************************************************/
/* SPECIFY THE SECTIONS ALLOCATION INTO MEMORY                              */
/****************************************************************************/

SECTIONS
{
    .bss       : {} > RAM                /* GLOBAL & STATIC VARS              */
    .sysmem    : {} > RAM                /* DYNAMIC MEMORY ALLOCATION AREA    */
    .stack     : {} > RAM (HIGH)         /* SOFTWARE SYSTEM STACK             */

    .text      : {} > FLASH              /* CODE                              */
    .cinit     : {} > FLASH              /* INITIALIZATION TABLES             */
    .const     : {} > FLASH              /* CONSTANT DATA                     */
    .cio       : {} > RAM                /* C I/O BUFFER                      */

    .pinit     : {} > FLASH              /* C++ CONSTRUCTOR TABLES            */

    .infoA     : {} > INFOA              /* MSP430 INFO FLASH MEMORY SEGMENTS */
    .infoB     : {} > INFOB

    .int00   : {} > INT00                /* MSP430 INTERRUPT VECTORS          */
    .int01   : {} > INT01
    .int02   : {} > INT02
    .int03   : {} > INT03
    .int04   : {} > INT04
    .int05   : {} > INT05
    .int06   : {} > INT06
    .int07   : {} > INT07
    .int08   : {} > INT08
    .int09   : {} > INT09
    .int10   : {} > INT10
    .int11   : {} > INT11
    .int12   : {} > INT12
    .int13   : {} > INT13
    .int14   : {} > INT14
    .reset   : {} > RESET              /* MSP430 RESET VECTOR               */ 
}

