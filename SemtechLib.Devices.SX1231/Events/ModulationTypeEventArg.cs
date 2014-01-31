namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class ModulationTypeEventArg : EventArgs
    {
        private ModulationTypeEnum value;

        public ModulationTypeEventArg(ModulationTypeEnum value)
        {
            this.value = value;
        }

        public ModulationTypeEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

