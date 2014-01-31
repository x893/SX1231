namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class RfPaSwitchSelEventArg : EventArgs
    {
        private RfPaSwitchSelEnum value;

        public RfPaSwitchSelEventArg(RfPaSwitchSelEnum value)
        {
            this.value = value;
        }

        public RfPaSwitchSelEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

