#include "efm32-stk.h"

/**
  * @brief   This function handles NMI exception.
  * @param  None
  * @retval None
  */
void NMI_Handler(void)
{
}

/**
  * @brief  This function handles Hard Fault exception.
  * @param  None
  * @retval None
  */
//void HardFault_Handler(void)
//{
//  __asm volatile( ".global HardFault_Handler" );
//  __asm volatile( ".extern HardFault_Handler_C" );
//  __asm volatile( "TST LR, #4" );
//  __asm volatile( "ITE EQ" );
//  __asm volatile( "MRSEQ R0, MSP" );
//  __asm volatile( "MRSNE R0, PSP" );
//  __asm volatile( "B HardFault_Handler_C" );
//  /* Go to infinite loop when Hard Fault exception occurs */
//  while (1)
//  {
//  }
//}

#include <stdio.h>

// From Joseph Yiu, minor edits by FVH
// hard fault handler in C,
// with stack frame location as input parameter
// called from HardFault_Handler in file xxx.s
void HardFault_Handler_C( unsigned int *args )
{
#if 0
    int8_t s[100];
    unsigned int stacked_r0;
    unsigned int stacked_r1;
    unsigned int stacked_r2;
    unsigned int stacked_r3;
    unsigned int stacked_r12;
    unsigned int stacked_lr;
    unsigned int stacked_pc;
    unsigned int stacked_psr;

    stacked_r0 = ( ( unsigned long) args[0] );
    stacked_r1 = ( ( unsigned long) args[1] );
    stacked_r2 = ( ( unsigned long) args[2] );
    stacked_r3 = ( ( unsigned long) args[3] );

    stacked_r12 = ( ( unsigned long) args[4] );
    stacked_lr = ( ( unsigned long) args[5] );
    stacked_pc = ( ( unsigned long) args[6] );
    stacked_psr = ( ( unsigned long) args[7] );

    sprintf( s, "\n\n[Hard fault handler - all numbers in hex]\n" );
    sprintf( s, "R0 = %x\n", stacked_r0 );
    sprintf( s, "R1 = %x\n", stacked_r1 );
    sprintf( s, "R2 = %x\n", stacked_r2 );
    sprintf( s, "R3 = %x\n", stacked_r3 );
    sprintf( s, "R12 = %x\n", stacked_r12 );
    sprintf( s, "LR [R14] = %x  subroutine call return address\n", stacked_lr );
    sprintf( s, "PC [R15] = %x  program counter\n", stacked_pc );
    sprintf( s, "PSR = %x\n", stacked_psr );
    sprintf( s, "BFAR = %x\n", ( *( ( volatile unsigned long * )( 0xE000ED38 ) ) ) );
    sprintf( s, "CFSR = %x\n", ( *( ( volatile unsigned long * )( 0xE000ED28 ) ) ) );
    sprintf( s, "HFSR = %x\n", ( *( ( volatile unsigned long * )( 0xE000ED2C ) ) ) );
    sprintf( s, "DFSR = %x\n", ( *( ( volatile unsigned long * )( 0xE000ED30 ) ) ) );
    sprintf( s, "AFSR = %x\n", ( *( ( volatile unsigned long * )( 0xE000ED3C ) ) ) );
    sprintf( s, "SCB_SHCSR = %x\n", SCB->SHCSR );
#endif
    /* Go to infinite loop when Hard Fault exception occurs */
    while( 1 )
    {
    }
}

/**
  * @brief  This function handles Memory Manage exception.
  * @param  None
  * @retval None
  */
void MemManage_Handler(void)
{
  /* Go to infinite loop when Memory Manage exception occurs */
  while (1)
  {
  }
}

/**
  * @brief  This function handles Bus Fault exception.
  * @param  None
  * @retval None
  */
void BusFault_Handler(void)
{
  /* Go to infinite loop when Bus Fault exception occurs */
  while (1)
  {
  }
}

/**
  * @brief  This function handles Usage Fault exception.
  * @param  None
  * @retval None
  */
void UsageFault_Handler(void)
{
  /* Go to infinite loop when Usage Fault exception occurs */
  while (1)
  {
  }
}

/**
  * @brief  This function handles SVCall exception.
  * @param  None
  * @retval None
  */
void SVC_Handler(void)
{
}

/**
  * @brief  This function handles Debug Monitor exception.
  * @param  None
  * @retval None
  */
void DebugMon_Handler(void)
{
}

/**
  * @brief  This function handles PendSVC exception.
  * @param  None
  * @retval None
  */
void PendSV_Handler(void)
{
}

/**
  * @brief  This function handles SysTick Handler.
  * @param  None
  * @retval None
  */
void SysTick_Handler(void)
{
    TickCounter++;
}
