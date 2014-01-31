namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class PacketFormatEventArg : EventArgs
    {
        private PacketFormatEnum value;

        public PacketFormatEventArg(PacketFormatEnum value)
        {
            this.value = value;
        }

        public PacketFormatEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

