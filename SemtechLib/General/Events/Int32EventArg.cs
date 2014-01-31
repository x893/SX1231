namespace SemtechLib.General.Events
{
    using System;

    public class Int32EventArg : EventArgs
    {
        private int value;

        public Int32EventArg(int value)
        {
            this.value = value;
        }

        public int Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

