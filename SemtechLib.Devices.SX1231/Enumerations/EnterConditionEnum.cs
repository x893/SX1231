namespace SemtechLib.Devices.SX1231.Enumerations
{
    using System;

    public enum EnterConditionEnum
    {
        OFF,
        RisingEdgeFifoNotEmpty,
        RisingEdgeFifoLevel,
        RisingEdgeCrcOk,
        RisingEdgePayloadReady,
        RisingEdgeSyncAddress,
        RisingEdgePacketSent,
        FallingEdgeFifoNotEmpty
    }
}

