namespace SemtechLib.General.Events
{
    using System;

    public class ByteEventArg : EventArgs
    {
        private byte value;

        public ByteEventArg(byte value)
        {
            this.value = value;
        }

        public byte Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

