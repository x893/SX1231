namespace SemtechLib.General.Events
{
    using System;

    public class DoubleEventArg : EventArgs
    {
        private double value;

        public DoubleEventArg(double value)
        {
            this.value = value;
        }

        public double Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

