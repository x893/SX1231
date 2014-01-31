namespace SemtechLib.Ftdi
{
	using System;

	public class MpsseSPI : Mpsse
	{
		public MpsseSPI(string device)
			: base(device)
		{
			Device = device;
			portDir = 0xFB;
			portValue = 30;
		}

		public override void ScanIn(int bitCount, bool clockOutDataBitsMSBFirst)
		{
			int count = bitCount / 8;
			if (count > 0)
			{	// Bytes count
				if (clockOutDataBitsMSBFirst)
					txBuffer.Add(0x24);
				else
					txBuffer.Add(0x2C);
				txBuffer.Add((byte)(((count - 1) >> 0) & 0xFF));
				txBuffer.Add((byte)(((count - 1) >> 8) & 0xFF));
			}
			count = bitCount % 8;
			if (count > 0)
			{
				// Bit remaining
				if (clockOutDataBitsMSBFirst)
					txBuffer.Add(0x26);
				else
					txBuffer.Add(0x2E);
				txBuffer.Add((byte)((count - 1) & 0xFF));
			}
		}

		public override void ScanInOut(int bitCount, byte[] data, bool clockOutDataBitsMSBFirst)
		{
			int count = bitCount / 8;
			if (count > 0)
			{
				if (clockOutDataBitsMSBFirst)
					txBuffer.Add(0x35);
				else
					txBuffer.Add(0x3D);
				txBuffer.Add((byte)(((count - 1) >> 0) & 0xFF));
				txBuffer.Add((byte)(((count - 1) >> 8) & 0xFF));
				for (int i = 0; i < count; i++)
					txBuffer.Add(data[i]);
			}
			count = bitCount % 8;
			if (count > 0)
			{
				if (clockOutDataBitsMSBFirst)
					txBuffer.Add(0x37);
				else
					txBuffer.Add(0x3F);
				txBuffer.Add((byte)((count - 1) & 0xFF));
				txBuffer.Add(data[data.Length - 1]);
			}
		}

		public override void ScanOut(int bitCount, byte[] data, bool clockOutDataBitsMSBFirst)
		{
			int count = bitCount / 8;
			if (count > 0)
			{
				if (clockOutDataBitsMSBFirst)
					txBuffer.Add(0x11);
				else
					txBuffer.Add(0x19);
				txBuffer.Add((byte)(((count - 1) >> 0) & 0xFF));
				txBuffer.Add((byte)(((count - 1) >> 8) & 0xFF));
				for (int i = 0; i < count; i++)
					txBuffer.Add(data[i]);
			}
			count = bitCount % 8;
			if (count > 0)
			{
				if (clockOutDataBitsMSBFirst)
					txBuffer.Add(0x13);
				else
					txBuffer.Add(0x1B);
				txBuffer.Add((byte)((count - 1) & 0xFF));
				txBuffer.Add(data[data.Length - 1]);
			}
		}
	}
}
