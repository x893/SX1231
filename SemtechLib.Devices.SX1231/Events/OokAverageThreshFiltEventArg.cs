namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class OokAverageThreshFiltEventArg : EventArgs
    {
        private OokAverageThreshFiltEnum value;

        public OokAverageThreshFiltEventArg(OokAverageThreshFiltEnum value)
        {
            this.value = value;
        }

        public OokAverageThreshFiltEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

