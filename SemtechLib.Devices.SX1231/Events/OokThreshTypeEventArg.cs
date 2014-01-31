namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class OokThreshTypeEventArg : EventArgs
    {
        private OokThreshTypeEnum value;

        public OokThreshTypeEventArg(OokThreshTypeEnum value)
        {
            this.value = value;
        }

        public OokThreshTypeEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

