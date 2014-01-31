namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class EnterConditionEventArg : EventArgs
    {
        private EnterConditionEnum value;

        public EnterConditionEventArg(EnterConditionEnum value)
        {
            this.value = value;
        }

        public EnterConditionEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

