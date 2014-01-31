; File created on Thu Aug 20 11:49:36 2009

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
	move	a, #0x01
	move	(i0)+, a	; Addr = 0x99
	move	r0,	#2
	calls	_DataInit_MemClear
	; Addr = 0x9a -> 0x9b
	move	a, #0x20
	move	(i0)+, a	; Addr = 0x9c
	move	a, #0x04
	move	(i0)+, a	; Addr = 0x9d
	move	r0,	#3
	calls	_DataInit_MemClear
	; Addr = 0x9e -> 0xa0
	move	a, #0x34
	move	(i0)+, a	; Addr = 0xa1
	move	r0,	#2
	move	r1,	#0x01
	calls	_DataInit_MemSet	; Addr = 0xa2 -> 0xa3
	move	i0h, #HIWORD(_sdata)
	move	i0l, #LOWORD(_sdata)
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x140
	move	a, #0x30
	move	(i0)+, a	; Addr = 0x141
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x142
	move	a, #0x24
	move	(i0)+, a	; Addr = 0x143
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x144
	move	a, #0x03
	move	(i0)+, a	; Addr = 0x145
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x146
	move	a, #0x07
	move	(i0)+, a	; Addr = 0x147
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x148
	move	a, #0x1F
	move	(i0)+, a	; Addr = 0x149
	move	r0,	#3
	calls	_DataInit_MemClear
	; Addr = 0x14a -> 0x14c
	move	a, #0x6B
	move	(i0)+, a	; Addr = 0x14d
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x14e
	move	a, #0x2A
	move	(i0)+, a	; Addr = 0x14f
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x150
	move	a, #0x1E
	move	(i0)+, a	; Addr = 0x151
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x152
	move	a, #0x77
	move	(i0)+, a	; Addr = 0x153
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x154
	move	a, #0x2F
	move	(i0)+, a	; Addr = 0x155
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x156
	move	a, #0x19
	move	(i0)+, a	; Addr = 0x157
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x158
	move	a, #0xCF
	move	(i0)+, a	; Addr = 0x159
	move	r0,	#3
	calls	_DataInit_MemClear
	; Addr = 0x15a -> 0x15c
	move	a, #0x09
	move	(i0)+, a	; Addr = 0x15d
	move	r0,	#3
	calls	_DataInit_MemClear
	; Addr = 0x15e -> 0x160
	move	a, #0xA3
	move	(i0)+, a	; Addr = 0x161
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x162
	move	a, #0x38
	move	(i0)+, a	; Addr = 0x163
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x164
	move	a, #0x38
	move	(i0)+, a	; Addr = 0x165
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x166
	move	a, #0x0C
	move	(i0)+, a	; Addr = 0x167
	move	r0,	#3
	calls	_DataInit_MemClear
	; Addr = 0x168 -> 0x16a
	move	a, #0x69
	move	(i0)+, a	; Addr = 0x16b
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x16c
	move	a, #0x81
	move	(i0)+, a	; Addr = 0x16d
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x16e
	move	a, #0x7E
	move	(i0)+, a	; Addr = 0x16f
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x170
	move	a, #0x96
	move	(i0)+, a	; Addr = 0x171
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x172
	move	a, #0x72
	move	(i0)+, a	; Addr = 0x173
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x174
	move	a, #0xBC
	move	(i0)+, a	; Addr = 0x175
	move	a, #0x50
	move	(i0)+, a	; Addr = 0x176
	move	a, #0x49
	move	(i0)+, a	; Addr = 0x177
	move	a, #0x4E
	move	(i0)+, a	; Addr = 0x178
	move	a, #0x47
	move	(i0)+, a	; Addr = 0x179
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x17a
	move	a, #0x50
	move	(i0)+, a	; Addr = 0x17b
	move	a, #0x4F
	move	(i0)+, a	; Addr = 0x17c
	move	a, #0x4E
	move	(i0)+, a	; Addr = 0x17d
	move	a, #0x47
	move	(i0)+, a	; Addr = 0x17e
	move	a, #0x00
	move	(i0)+, a	; Addr = 0x103250
	move	iph,	(i3)+
	move	ipl,	(i3)+
	rets
