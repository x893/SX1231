namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class ExitConditionEventArg : EventArgs
    {
        private ExitConditionEnum value;

        public ExitConditionEventArg(ExitConditionEnum value)
        {
            this.value = value;
        }

        public ExitConditionEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

