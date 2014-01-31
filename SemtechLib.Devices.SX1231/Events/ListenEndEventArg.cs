namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class ListenEndEventArg : EventArgs
    {
        private ListenEndEnum value;

        public ListenEndEventArg(ListenEndEnum value)
        {
            this.value = value;
        }

        public ListenEndEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

