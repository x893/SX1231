namespace SemtechLib.General.Events
{
    using System;

    public class BooleanEventArg : EventArgs
    {
        private bool value;

        public BooleanEventArg(bool value)
        {
            this.value = value;
        }

        public bool Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

