namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class PaRampEventArg : EventArgs
    {
        private PaRampEnum value;

        public PaRampEventArg(PaRampEnum value)
        {
            this.value = value;
        }

        public PaRampEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

