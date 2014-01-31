namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class LowBatTrimEventArg : EventArgs
    {
        private LowBatTrimEnum value;

        public LowBatTrimEventArg(LowBatTrimEnum value)
        {
            this.value = value;
        }

        public LowBatTrimEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

