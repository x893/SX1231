namespace SemtechLib.Ftdi
{
    using System;

    public class MpsseI2C : Mpsse
    {
        public MpsseI2C(string device) : base(device)
        {
            base.Device = device;
            base.portDir = 0;
            base.portValue = 0;
        }

        public override void ScanIn(int bitCount, bool clockOutDataBitsMSBFirst)
        {
            if ((bitCount - 1) == 0)
            {
                base.txBuffer.Add(0x27);
                base.txBuffer.Add(0);
            }
            else
            {
                int num = bitCount / 8;
                if (num > 0)
                {
                    base.txBuffer.Add(0x25);
                    base.txBuffer.Add((byte) ((num - 1) & 0xff));
                    base.txBuffer.Add((byte) (((num - 1) >> 8) & 0xff));
                }
                num = bitCount % 8;
                if (num > 0)
                {
                    base.txBuffer.Add(0x27);
                    base.txBuffer.Add((byte) ((num - 1) & 0xff));
                }
            }
        }

        public override void ScanInOut(int bitCount, byte[] data, bool clockOutDataBitsMSBFirst)
        {
            throw new NotImplementedException();
        }

        public override void ScanOut(int bitCount, byte[] data, bool clockOutDataBitsMSBFirst)
        {
            int num = bitCount / 8;
            if (num > 0)
            {
                base.txBuffer.Add(0x11);
                base.txBuffer.Add((byte) ((num - 1) & 0xff));
                base.txBuffer.Add((byte) (((num - 1) >> 8) & 0xff));
                for (int i = 0; i < num; i++)
                {
                    base.txBuffer.Add(data[i]);
                }
            }
            num = bitCount % 8;
            if (num > 0)
            {
                base.txBuffer.Add(0x13);
                base.txBuffer.Add((byte) ((num - 1) & 0xff));
                base.txBuffer.Add(data[data.Length - 1]);
            }
        }
    }
}

