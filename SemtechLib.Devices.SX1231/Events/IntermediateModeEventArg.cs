namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class IntermediateModeEventArg : EventArgs
    {
        private IntermediateModeEnum value;

        public IntermediateModeEventArg(IntermediateModeEnum value)
        {
            this.value = value;
        }

        public IntermediateModeEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

