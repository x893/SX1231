namespace SemtechLib.General.Events
{
    using System;

    public class ByteArrayEventArg : EventArgs
    {
        private byte[] value;

        public ByteArrayEventArg(byte[] value)
        {
            this.value = value;
        }

        public byte[] Value
        {
            get
            {
                return this.value;
            }
        }
    }
}

