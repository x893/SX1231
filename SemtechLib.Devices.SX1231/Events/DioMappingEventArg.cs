namespace SemtechLib.Devices.SX1231.Events
{
    using SemtechLib.Devices.SX1231.Enumerations;
    using System;

    public class DioMappingEventArg : EventArgs
    {
        private byte id;
        private DioMappingEnum value;

        public DioMappingEventArg(byte id, DioMappingEnum value)
        {
            this.id = id;
            this.value = value;
        }

        public byte Id
        {
            get
            {
                return this.id;
            }
        }

        public DioMappingEnum Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

