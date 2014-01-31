namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class FifoFillConditionEventArg : EventArgs
    {
        private FifoFillConditionEnum value;

        public FifoFillConditionEventArg(FifoFillConditionEnum value)
        {
            this.value = value;
        }

        public FifoFillConditionEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

