#ifndef _F149_H
#define _F149_H

#include  <msp430x14x.h>

struct bit_def {
	char b0:1;
	char b1:1;
	char b2:1;
	char b3:1;
	char b4:1;
	char b5:1;
	char b6:1;
	char b7:1;
};
union byte_def {
	struct bit_def bit;
	char byte;
};

/* port 1 */

/*#pragma location=P1IE_	port 1 interrupt enable -- for IAR */
extern volatile union byte_def p1_ie;	// for CCE
#define pie1	p1_ie.byte
#define pie1_0	p1_ie.bit.b0
#define pie1_1	p1_ie.bit.b1
#define pie1_2	p1_ie.bit.b2
#define pie1_3	p1_ie.bit.b3
#define pie1_4	p1_ie.bit.b4
#define pie1_5	p1_ie.bit.b5
#define pie1_6	p1_ie.bit.b6
#define pie1_7	p1_ie.bit.b7

/*#pragma location=P1IES_	-- port 1 interrupt edge sensitivity -- for IAR */
extern volatile union byte_def p1_ies;	// for CCE
#define pies1	p1_ies.byte
#define pies1_0	p1_ies.bit.b0
#define pies1_1	p1_ies.bit.b1
#define pies1_2	p1_ies.bit.b2
#define pies1_3	p1_ies.bit.b3
#define pies1_4	p1_ies.bit.b4
#define pies1_5	p1_ies.bit.b5
#define pies1_6	p1_ies.bit.b6
#define pies1_7	p1_ies.bit.b7

/*#pragma location=P1DIR_	-- for IAR */
extern volatile union byte_def p1_dir;	// for CCE
#define pd1		p1_dir.byte
#define pd1_0	p1_dir.bit.b0
#define pd1_1	p1_dir.bit.b1
#define pd1_2	p1_dir.bit.b2
#define pd1_3	p1_dir.bit.b3
#define pd1_4	p1_dir.bit.b4
#define pd1_5	p1_dir.bit.b5
#define pd1_6	p1_dir.bit.b6
#define pd1_7	p1_dir.bit.b7

/*#pragma location=P1IFG_	-- for IAR */
extern volatile union byte_def p1_ifg;	// for CCE
#define pifg1	p1_ifg.byte
#define pifg1_0	p1_ifg.bit.b0
#define pifg1_1	p1_ifg.bit.b1
#define pifg1_2	p1_ifg.bit.b2
#define pifg1_3	p1_ifg.bit.b3
#define pifg1_4	p1_ifg.bit.b4
#define pifg1_5	p1_ifg.bit.b5
#define pifg1_6	p1_ifg.bit.b6
#define pifg1_7	p1_ifg.bit.b7

/*#pragma location=P1OUT_	-- for IAR */
extern volatile union byte_def p1_out;	// for CCE
#define po1		p1_out.byte
#define po1_0	p1_out.bit.b0
#define po1_1	p1_out.bit.b1
#define po1_2	p1_out.bit.b2
#define po1_3	p1_out.bit.b3
#define po1_4	p1_out.bit.b4
#define po1_5	p1_out.bit.b5
#define po1_6	p1_out.bit.b6
#define po1_7	p1_out.bit.b7

/*#pragma location=P1IN_	-- for IAR */
extern volatile union byte_def p1_in;	// for CCE
#define pi1		p1_in.byte
#define pi1_0	p1_in.bit.b0
#define pi1_1	p1_in.bit.b1
#define pi1_2	p1_in.bit.b2
#define pi1_3	p1_in.bit.b3
#define pi1_4	p1_in.bit.b4
#define pi1_5	p1_in.bit.b5
#define pi1_6	p1_in.bit.b6
#define pi1_7	p1_in.bit.b7

/* port 2 */
/*#pragma location=P2IE_	-- port 2 interrupt enable -- for IAR */
extern volatile union byte_def p2_ie;	// for CCE
#define pie2	p2_ie.byte
#define pie2_0	p2_ie.bit.b0
#define pie2_1	p2_ie.bit.b1
#define pie2_2	p2_ie.bit.b2
#define pie2_3	p2_ie.bit.b3
#define pie2_4	p2_ie.bit.b4
#define pie2_5	p2_ie.bit.b5
#define pie2_6	p2_ie.bit.b6
#define pie2_7	p2_ie.bit.b7

/*#pragma location=P2IES_	-- port 2 interrupt edge sensitivity -- for IAR */
extern volatile union byte_def p2_ies;	// for CCE
#define pies2	p2_ies.byte
#define pies2_0	p2_ies.bit.b0
#define pies2_1	p2_ies.bit.b1
#define pies2_2	p2_ies.bit.b2
#define pies2_3	p2_ies.bit.b3
#define pies2_4	p2_ies.bit.b4
#define pies2_5	p2_ies.bit.b5
#define pies2_6	p2_ies.bit.b6
#define pies2_7	p2_ies.bit.b7

/*#pragma location=P2DIR_	-- for IAR */
extern volatile union byte_def p2_dir;	// for CCE
#define pd2		p2_dir.byte
#define pd2_0	p2_dir.bit.b0
#define pd2_1	p2_dir.bit.b1
#define pd2_2	p2_dir.bit.b2
#define pd2_3	p2_dir.bit.b3
#define pd2_4	p2_dir.bit.b4
#define pd2_5	p2_dir.bit.b5
#define pd2_6	p2_dir.bit.b6
#define pd2_7	p2_dir.bit.b7

/*#pragma location=P2IFG_	-- for IAR */
extern volatile union byte_def p2_ifg;	// for CCE
#define pifg2	p2_ifg.byte
#define pifg2_0	p2_ifg.bit.b0
#define pifg2_1	p2_ifg.bit.b1
#define pifg2_2	p2_ifg.bit.b2
#define pifg2_3	p2_ifg.bit.b3
#define pifg2_4	p2_ifg.bit.b4
#define pifg2_5	p2_ifg.bit.b5
#define pifg2_6	p2_ifg.bit.b6
#define pifg2_7	p2_ifg.bit.b7

/*#pragma location=P2OUT_	-- for IAR */
extern volatile union byte_def p2_out;	// for CCE
#define po2		p2_out.byte
#define po2_0	p2_out.bit.b0
#define po2_1	p2_out.bit.b1
#define po2_2	p2_out.bit.b2
#define po2_3	p2_out.bit.b3
#define po2_4	p2_out.bit.b4
#define po2_5	p2_out.bit.b5
#define po2_6	p2_out.bit.b6
#define po2_7	p2_out.bit.b7

/*#pragma location=P2IN_	-- for IAR */
extern volatile union byte_def p2_in;	// for CCE
#define pi2		p2_in.byte
#define pi2_0	p2_in.bit.b0
#define pi2_1	p2_in.bit.b1
#define pi2_2	p2_in.bit.b2
#define pi2_3	p2_in.bit.b3
#define pi2_4	p2_in.bit.b4
#define pi2_5	p2_in.bit.b5
#define pi2_6	p2_in.bit.b6
#define pi2_7	p2_in.bit.b7

/* port 3 */

/*#pragma location=P3DIR_	-- for IAR */
extern volatile union byte_def p3_dir;	// for CCE
#define pd3		p3_dir.byte
#define pd3_0	p3_dir.bit.b0
#define pd3_1	p3_dir.bit.b1
#define pd3_2	p3_dir.bit.b2
#define pd3_3	p3_dir.bit.b3
#define pd3_4	p3_dir.bit.b4
#define pd3_5	p3_dir.bit.b5
#define pd3_6	p3_dir.bit.b6
#define pd3_7	p3_dir.bit.b7

/*#pragma location=P3OUT_	-- for IAR */
extern volatile union byte_def p3_out;	// for CCE
#define po3		p3_out.byte
#define po3_0	p3_out.bit.b0
#define po3_1	p3_out.bit.b1
#define po3_2	p3_out.bit.b2
#define po3_3	p3_out.bit.b3
#define po3_4	p3_out.bit.b4
#define po3_5	p3_out.bit.b5
#define po3_6	p3_out.bit.b6
#define po3_7	p3_out.bit.b7

/*#pragma location=P3IN_	-- for IAR */
extern volatile union byte_def p3_in;	// for CCE
#define pi3		p3_in.byte
#define pi3_0	p3_in.bit.b0
#define pi3_1	p3_in.bit.b1
#define pi3_2	p3_in.bit.b2
#define pi3_3	p3_in.bit.b3
#define pi3_4	p3_in.bit.b4
#define pi3_5	p3_in.bit.b5
#define pi3_6	p3_in.bit.b6
#define pi3_7	p3_in.bit.b7
/* port 3 */

/*#pragma location=P4DIR_	-- for IAR */
extern volatile union byte_def p4_dir;	// for CCE
#define pd4		p4_dir.byte
#define pd4_0	p4_dir.bit.b0
#define pd4_1	p4_dir.bit.b1
#define pd4_2	p4_dir.bit.b2
#define pd4_3	p4_dir.bit.b3
#define pd4_4	p4_dir.bit.b4
#define pd4_5	p4_dir.bit.b5
#define pd4_6	p4_dir.bit.b6
#define pd4_7	p4_dir.bit.b7

/*#pragma location=P4OUT_	-- for IAR */
extern volatile union byte_def p4_out;	// for CCE
#define po4		p4_out.byte
#define po4_0	p4_out.bit.b0
#define po4_1	p4_out.bit.b1
#define po4_2	p4_out.bit.b2
#define po4_3	p4_out.bit.b3
#define po4_4	p4_out.bit.b4
#define po4_5	p4_out.bit.b5
#define po4_6	p4_out.bit.b6
#define po4_7	p4_out.bit.b7

/*#pragma location=P4IN_	-- for IAR */
extern volatile union byte_def p4_in;	// for CCE
#define pi4		p4_in.byte
#define pi4_0	p4_in.bit.b0
#define pi4_1	p4_in.bit.b1
#define pi4_2	p4_in.bit.b2
#define pi4_3	p4_in.bit.b3
#define pi4_4	p4_in.bit.b4
#define pi4_5	p4_in.bit.b5
#define pi4_6	p4_in.bit.b6
#define pi4_7	p4_in.bit.b7

extern volatile union byte_def p5_dir;	// for CCE
#define pd5		p5_dir.byte
#define pd5_0	p5_dir.bit.b0
#define pd5_1	p5_dir.bit.b1
#define pd5_2	p5_dir.bit.b2
#define pd5_3	p5_dir.bit.b3
#define pd5_4	p5_dir.bit.b4
#define pd5_5	p5_dir.bit.b5
#define pd5_6	p5_dir.bit.b6
#define pd5_7	p5_dir.bit.b7

extern volatile union byte_def p5_sel;	// for CCE
#define psel5		p5_sel.byte
#define psel5_0	p5_sel.bit.b0
#define psel5_1	p5_sel.bit.b1
#define psel5_2	p5_sel.bit.b2
#define psel5_3	p5_sel.bit.b3
#define psel5_4	p5_sel.bit.b4
#define psel5_5	p5_sel.bit.b5
#define psel5_6	p5_sel.bit.b6	// ACLK out
#define psel5_7	p5_sel.bit.b7


/*#pragma location=P6IN_	-- for IAR */
extern volatile union byte_def p6_in;	// for CCE
#define pi6		p6_in.byte
#define pi6_0	p6_in.bit.b0
#define pi6_1	p6_in.bit.b1
#define pi6_2	p6_in.bit.b2
#define pi6_3	p6_in.bit.b3
#define pi6_4	p6_in.bit.b4
#define pi6_5	p6_in.bit.b5
#define pi6_6	p6_in.bit.b6
#define pi6_7	p6_in.bit.b7

#endif /* #ifndef _F149_H */

