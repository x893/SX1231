namespace SemtechLib.Devices.SX1231.Events
{
    using System;

    public class PacketStatusEventArg : EventArgs
    {
        private int max;
        private int number;

        public PacketStatusEventArg(int number, int max)
        {
            this.number = number;
            this.max = max;
        }

        public int Max
        {
            get
            {
                return this.max;
            }
        }

        public int Number
        {
            get
            {
                return this.number;
            }
        }
    }
}

