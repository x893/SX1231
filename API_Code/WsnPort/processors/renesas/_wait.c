
void far main() {
	asm( "\tFCLR I"); // Disable interrupts
	pd8 = 0x03; // Enable P8 pins 0 & 1 as outputs and the rest as inputs
	pd9 = 0x00; // Enable P9 all pins as inputs
	int0ic = 0x11;// Enable INT0 & set priority level at 1
	asm( "\tFSET I"); // Enable interrupts
	while(1) {
		p8_0 = 0; // Turn LED2 on
		while (p9_0); // Wait until SW1 is pressed
		p8_0 = 1; // Turn LED2 off
		prc0 = 1; // Enable write to CM registers
		cm02 = 1; // Enable peripheral clock stop bit
		asm( "\tWAIT"); // Enter wait mode
	}
}
// Initialize SFRs SW1 been pressed? Turn on LED2 Turn off LED2 & Enter WAIT or STOP mode Yes No Figure 2.1 Flowchart for WAIT and STOP routines Using the Power Down Modes for the M16C M16C Dec-98 Mitsubishi Electronics 3 APNM16CN1298A STOP mode is similar to WAIT mode where the clock mode register CM1 is controlled to enter the power down mode. 
