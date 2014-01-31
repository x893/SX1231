namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class LnaGainEventArg : EventArgs
    {
        private LnaGainEnum value;

        public LnaGainEventArg(LnaGainEnum value)
        {
            this.value = value;
        }

        public LnaGainEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

