namespace SemtechLib.Devices.SX1231.Enumerations
{
    using System;

    public enum ExitConditionEnum
    {
        OFF,
        FallingEdgeFifoNotEmpty,
        RisingEdgeFifoLevel,
        RisingEdgeCrcOk,
        RisingEdgePayloadReadyOrTimeout,
        RisingEdgeSyncAddressOrTimeout,
        RisingEdgePacketSent,
        RisingEdgeTimeout
    }
}

