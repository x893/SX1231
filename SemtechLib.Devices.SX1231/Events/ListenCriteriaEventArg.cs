namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class ListenCriteriaEventArg : EventArgs
    {
        private ListenCriteriaEnum value;

        public ListenCriteriaEventArg(ListenCriteriaEnum value)
        {
            this.value = value;
        }

        public ListenCriteriaEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

