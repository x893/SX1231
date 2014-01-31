namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class OokPeakThreshDecEventArg : EventArgs
    {
        private OokPeakThreshDecEnum value;

        public OokPeakThreshDecEventArg(OokPeakThreshDecEnum value)
        {
            this.value = value;
        }

        public OokPeakThreshDecEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

