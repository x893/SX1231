namespace SemtechLib.General.Events
{
    using System;

    public class DecimalEventArg : EventArgs
    {
        private decimal value;

        public DecimalEventArg(decimal value)
        {
            this.value = value;
        }

        public decimal Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

