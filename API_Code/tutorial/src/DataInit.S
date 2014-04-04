	.global DataInit

	.text

_DataInit_MemClear:
	move	r1,	#0
_DataInit_MemSet:
	move	(i0)+, r1
	dec	r0
	jzc	_DataInit_MemSet
	rets


DataInit:
	move	-(i3),	ipl
	move	-(i3),	iph
	
	move	i0h, #HIWORD(_spage0data)
	move	i0l, #LOWORD(_spage0data)
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x80
	move	r0,	#2
	move	r1,	#0x01
	calls	_DataInit_MemSet	; Addr = 0x81 -> 0x82
	move	r0,	#3
	calls	_DataInit_MemClear
	; Addr = 0x83 -> 0x85
	move	a, #0x01
	move	(i0)+, a	; Addr = 0x86
	move	a, #0x04
	move	(i0)+, a	; Addr = 0x87
	move	r0,	#3
	calls	_DataInit_MemClear
	; Addr = 0x88 -> 0x8a
	move	a, #0x45
	move	(i0)+, a	; Addr = 0x8b
	move	r0,	#3
	calls	_DataInit_MemClear
	; Addr = 0x8c -> 0x8e
	move	a, #0x37
	move	(i0)+, a	; Addr = 0x8f
	move	i0h, #HIWORD(_sdata)
	move	i0l, #LOWORD(_sdata)
	move	r0,	#3
	calls	_DataInit_MemClear
	; Addr = 0x140 -> 0x142
	move	a, #0x8C
	move	(i0)+, a	; Addr = 0x143
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x144
	move	a, #0x07
	move	(i0)+, a	; Addr = 0x145
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x146
	move	a, #0x64
	move	(i0)+, a	; Addr = 0x147
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x148
	move	a, #0x1F
	move	(i0)+, a	; Addr = 0x149
	move	r0,	#9
	calls	_DataInit_MemClear
	; Addr = 0x14a -> 0x152
	move	a, #0x08
	move	(i0)+, a	; Addr = 0x153
	move	r0,	#5
	calls	_DataInit_MemClear
	; Addr = 0x154 -> 0x158
	move	a, #0xC0
	move	(i0)+, a	; Addr = 0x159
	move	r0,	#5
	calls	_DataInit_MemClear
	; Addr = 0x15a -> 0x15e
	move	a, #0x80
	move	(i0)+, a	; Addr = 0x15f
	move	r0,	#3
	calls	_DataInit_MemClear
	; Addr = 0x160 -> 0x162
	move	a, #0x69
	move	(i0)+, a	; Addr = 0x163
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x164
	move	a, #0x81
	move	(i0)+, a	; Addr = 0x165
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x166
	move	a, #0x7E
	move	(i0)+, a	; Addr = 0x167
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x168
	move	a, #0x96
	move	(i0)+, a	; Addr = 0x169
	move	a, #0x50
	move	(i0)+, a	; Addr = 0x16a
	move	a, #0x4F
	move	(i0)+, a	; Addr = 0x16b
	move	a, #0x4E
	move	(i0)+, a	; Addr = 0x16c
	move	a, #0x47
	move	(i0)+, a	; Addr = 0x16d
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x16e
	move	a, #0x50
	move	(i0)+, a	; Addr = 0x16f
	move	a, #0x49
	move	(i0)+, a	; Addr = 0x170
	move	a, #0x4E
	move	(i0)+, a	; Addr = 0x171
	move	a, #0x47
	move	(i0)+, a	; Addr = 0x172
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x1032b2
	move	iph,	(i3)+
	move	ipl,	(i3)+
	rets
