namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class ListenResolEventArg : EventArgs
    {
        private ListenResolEnum value;

        public ListenResolEventArg(ListenResolEnum value)
        {
            this.value = value;
        }

        public ListenResolEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

