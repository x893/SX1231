;***************************************************************************
; C Compiler for R8C/Tiny, M16C/60,30,20,10
; Copyright(C) 1999(2000-2006). Renesas Technology Corp.
; and Renesas Solutions Corp., All rights reserved.
;
; sect30.inc : section definition for R8C/Tiny
;
; $Date: 2007/11/29 09:02:11 $
; $Revision: 1.1.14.1 $
;***************************************************************************

;=====================================================================
;
; 	Arrangement of section
;
;---------------------------------------------------------------------
; Near RAM data area
;---------------------------------------------------------------------
; SBDATA area
		.section	data_SE,DATA
		.org		400H
data_SE_top:

		.section	bss_SE,DATA,ALIGN
bss_SE_top:

		.section	data_SO,DATA
data_SO_top:

		.section	bss_SO,DATA
bss_SO_top:

; SBDATA area definition
		.glb		__SB__
__SB__	.equ		data_SE_top

; near RAM area
		.section	data_NE,DATA,ALIGN
data_NE_top:

		.section	bss_NE,DATA,ALIGN
bss_NE_top:

		.section	data_NO,DATA
data_NO_top:

		.section	bss_NO,DATA
bss_NO_top:

;---------------------------------------------------------------------
; Stack area
;---------------------------------------------------------------------
		.section	istack,DATA,ALIGN
		.blkb		__ISTACKSIZE__
istack_top:

.if __STACKSIZE__ != 0
		.section	stack,DATA,ALIGN
		.blkb		__STACKSIZE__
stack_top:
.endif

;---------------------------------------------------------------------
; heap section
;---------------------------------------------------------------------
.if __HEAPSIZE__ != 0
		.section	heap,DATA
heap_top:
		.blkb		__HEAPSIZE__
.endif

;---------------------------------------------------------------------
; Near ROM data area
;---------------------------------------------------------------------
		.section	rom_NE,ROMDATA
		.org		__ROM_TOPADR__
rom_NE_top:

		.section	rom_NO,ROMDATA
rom_NO_top:

;---------------------------------------------------------------------
; Initial data of 'data' section
;---------------------------------------------------------------------
		.section	data_SEI,ROMDATA,ALIGN
data_SEI_top:

		.section	data_SOI,ROMDATA
data_SOI_top:

		.section	data_NEI,ROMDATA,ALIGN
data_NEI_top:
 
		.section	data_NOI,ROMDATA
data_NOI_top:

;---------------------------------------------------------------------
; Switch Table Section
;---------------------------------------------------------------------
;		.section	switch_table,ROMDATA
;switch_table_top:

;---------------------------------------------------------------------
; code area
;---------------------------------------------------------------------
		.section	program,CODE,ALIGN
		.section	interrupt,CODE,ALIGN

;---------------------------------------------------------------------
; variable vector section
;---------------------------------------------------------------------
		.section	vector,ROMDATA
		.org		__VECTOR_ADR__

.if	1
		.lword	dummy_int		; vector  0 BRK instruction
		.lword	dummy_int		; vector  1
		.lword	dummy_int		; vector  2
		.lword	dummy_int		; vector  3
		.lword	dummy_int		; vector  4
		.lword	dummy_int		; vector  5
		.lword	dummy_int		; vector  6
		.lword	dummy_int		; vector  7
		.lword	dummy_int		; vector  8
		.lword	dummy_int		; vector  9
		.lword	dummy_int		; vector 10
		.lword	dummy_int		; vector 11
		.lword	dummy_int		; vector 12
		.lword	dummy_int		; vector 13 key input
		.lword	dummy_int		; vector 14 ADC
		.lword	dummy_int		; vector 15 synchronous serial
		.lword	dummy_int		; vector 16 compare1 Timer-C
		.glb	_uart_tx_isr
		.lword	_uart_tx_isr	; vector 17 UART0 transmit
		.lword	dummy_int		; vector 18 UART0 receive
		.lword	dummy_int		; vector 19
		.lword	dummy_int		; vector 20
		.lword	dummy_int		; vector 21 (reserved)
		.lword	dummy_int		; vector 22 Timer-X
		.lword	dummy_int		; vector 23 (reserved)
		.glb	_TimerZ_isr
		.lword	_TimerZ_isr		; vector 24	Timer-Z
		.lword	dummy_int		; vector 25 /INT1
		.lword	dummy_int		; vector 26 /INT3
		.lword	dummy_int		; vector 27 Timer-C
;		.glb	_TimerC_0_isr (vector 28)
		.lword	dummy_int		; vector 28 compare0 timer-C
		.lword	dummy_int		; vector 29 /INT0
		.lword	dummy_int		; vector 30 (reserved)
		.lword	dummy_int		; vector 31 (reserved)
		.lword	dummy_int		; vector 32 software interrupts
		.lword	dummy_int		; vector 33 software interrupts
		.lword	dummy_int		; vector 34 "
		.lword	dummy_int		; vector 35
		.lword	dummy_int		; vector 36
		.lword	dummy_int		; vector 37
		.lword	dummy_int		; vector 38
		.lword	dummy_int		; vector 39
		.lword	dummy_int		; vector 40
		.lword	dummy_int		; vector 41
		.lword	dummy_int		; vector 42
		.lword	dummy_int		; vector 43
		.lword  dummy_int		; vector 44
		.lword	dummy_int		; vector 45
		.lword	dummy_int		; vector 46
		.lword	dummy_int 		; vector 47
		.lword	dummy_int 		; vector 48
		.lword	dummy_int 		; vector 49
		.lword	dummy_int 		; vector 50
		.lword	dummy_int 		; vector 51
		.lword	dummy_int 		; vector 52
		.lword	dummy_int 		; vector 53
		.lword	dummy_int 		; vector 54
		.lword	dummy_int 		; vector 55
		.lword	dummy_int 		; vector 56
		.lword	dummy_int 		; vector 57
		.lword	dummy_int 		; vector 58
		.lword	dummy_int 		; vector 59
		.lword	dummy_int 		; vector 60
		.lword	dummy_int 		; vector 61
		.lword	dummy_int 		; vector 62
		.lword	dummy_int 		; vector 63
.endif

;---------------------------------------------------------------------
; fixed vector section
;---------------------------------------------------------------------
		.section	fvector,ROMDATA
		.org		0ffdcH
UDI:
		.lword		dummy_int
OVER_FLOW:
		.lword		dummy_int
BRKI:
		.lword		dummy_int
ADDRESS_MATCH:
		.lword		dummy_int
SINGLE_STEP:
		.lword		dummy_int
WDT:
		.lword		dummy_int
; reserved
		.lword		dummy_int
; reserved
		.lword		dummy_int
RESET:
		.lword		start

;=====================================================================
; ID code & Option function select register
;---------------------------------------------------------------------
; ID code check function
		.id "#FFFFFFFFFFFFFF"

; option function select register
		.ofsreg	0FFH

;---------------------------------------------------------------------
; far ROM data area
;---------------------------------------------------------------------
;	
;		.section	rom_FE,ROMDATA
;		.org		10000H
;
;		.section	rom_FO,ROMDATA
; 
;		.section	data_FEI,ROMDATA,ALIGN
;data_FEI_top:
;
;		.section	data_FOI,ROMDATA
;data_FOI_top:
;

;=====================================================================
; Initialize Macro declaration
;---------------------------------------------------------------------
N_BZERO	.macro		TOP_ ,SECT_
		mov.b		#00H, R0L
		mov.w		#(TOP_ & 0FFFFH), A1
		mov.w		#sizeof SECT_ , R3
		sstr.b
		.endm

N_BCOPY .macro		FROM_,TO_,SECT_
		mov.w		#(FROM_ & 0FFFFH),A0
		mov.b		#(FROM_ >>16),R1H
		mov.w		#TO_ ,A1
		mov.w		#sizeof SECT_ , R3
		smovf.b
		.endm

;***************************************************************************
;	C Compiler for R8C/Tiny, M16C/60,30,20,10
; Copyright(C) 1999(2000-2006). Renesas Technology Corp.
; and Renesas Solutions Corp., All rights reserved.
;***************************************************************************

