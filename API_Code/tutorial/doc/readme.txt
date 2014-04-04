**************************************************************************************************
**************************************************************************************************
**************************************************************************************************
*******                                                                                    *******
*******              PLEASE COMPILE THIS PROJECT PRIOR TO MODIFYING ANY FILES              *******
*******              THIS WILL UPDATE ALL THE DEPENDENCIES AND THE FILE PATHS !            *******
*******                                                                                    *******
**************************************************************************************************
**************************************************************************************************
**************************************************************************************************

This ReadMe file intends to give the startup state of the microcontroller.

1. Clock 

By default the initialization sets the RC at 2457600 Hz using the Xtal as 
external reference for the DFLL.
Note that this frequency is the one used in the API (see document TN8000.19)

2. Interruptions

The file IrqHandler.c is not used anymore, when an interrupt handler is needed you 
just have to copy the declaration from C:\RIDE\COOLRCTS\WIN32\TEMPLATES\COMMON\IRQHANDLER_xx.c

i.e. if you need to use the UART rx interrupt handler 

2.1 Open the file C:\RIDE\COOLRCTS\WIN32\TEMPLATES\COMMON\IRQHANDLER_UartRX.c
2.2 Copy at least the lines below into your application file

void Handle_Irq_UartRx (void){

}  //End Handle_Irq_UartRx

3. Application specific 
...

