namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class PaModeEventArg : EventArgs
    {
        private PaModeEnum value;

        public PaModeEventArg(PaModeEnum value)
        {
            this.value = value;
        }

        public PaModeEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

