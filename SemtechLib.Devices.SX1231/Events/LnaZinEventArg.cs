namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class LnaZinEventArg : EventArgs
    {
        private LnaZinEnum value;

        public LnaZinEventArg(LnaZinEnum value)
        {
            this.value = value;
        }

        public LnaZinEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

