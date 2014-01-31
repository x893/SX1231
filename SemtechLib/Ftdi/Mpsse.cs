using FTD2XX_NET;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SemtechLib.Ftdi
{
	public abstract class Mpsse : FtdiIoPort
	{
		protected uint clockDivisor = 0x3B;
		protected byte portAcDir;
		protected byte portAcValue;

		public override event FtdiIoPort.IoChangedEventHandler Io0Changed;
		public override event FtdiIoPort.IoChangedEventHandler Io1Changed;
		public override event FtdiIoPort.IoChangedEventHandler Io2Changed;
		public override event FtdiIoPort.IoChangedEventHandler Io3Changed;
		public override event FtdiIoPort.IoChangedEventHandler Io4Changed;
		public override event FtdiIoPort.IoChangedEventHandler Io5Changed;
		public override event FtdiIoPort.IoChangedEventHandler Io6Changed;
		public override event FtdiIoPort.IoChangedEventHandler Io7Changed;

		public Mpsse(string device)
		{
			Device = device;
			portDir = 0x03;
			portValue = 0x0E;
		}

		public override bool Init(uint frequency)
		{
			lock (syncThread)
			{
				if (frequency < 92 || frequency > 6000000)
					throw new Exception("The frequency value must be in 92 up to 6000000 range.");

				clockDivisor = (12000000 / (2 * frequency)) - 1;
				ftStatus = SetBitMode(0, 0);
				if (ftStatus != FTDI.FT_STATUS.FT_OK)
				{
					isInitialized = false;
					return false;
				}
				ftStatus = SetBitMode(0, 2);
				if (ftStatus != FTDI.FT_STATUS.FT_OK)
				{
					isInitialized = false;
					return false;
				}
				if (Sync2Mpsse())
				{
					txBuffer.Add(0x80);
					txBuffer.Add(PortValue);
					txBuffer.Add(PortDir);

					txBuffer.Add(0x82);
					txBuffer.Add(0);
					txBuffer.Add(0);

					txBuffer.Add(0x86);
					txBuffer.Add((byte)((clockDivisor >> 0) & 0xFF));
					txBuffer.Add((byte)((clockDivisor >> 8) & 0xFF));

					txBuffer.Add(0x85);
					isInitialized = SendBytes();

					readThreadContinue = true;
					readThread = new Thread(new ThreadStart(ReadThread));
					readThread.Start();
					return isInitialized;
				}

				isInitialized = false;
				return false;
			}
		}

		private void OnIo0Changed(bool state)
		{
			if (Io0Changed != null)
				Io0Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		private void OnIo1Changed(bool state)
		{
			if (Io1Changed != null)
				Io1Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		private void OnIo2Changed(bool state)
		{
			if (Io2Changed != null)
				Io2Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		private void OnIo3Changed(bool state)
		{
			if (Io3Changed != null)
				Io3Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		private void OnIo4Changed(bool state)
		{
			if (Io4Changed != null)
				Io4Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		private void OnIo5Changed(bool state)
		{
			if (Io5Changed != null)
				Io5Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		private void OnIo6Changed(bool state)
		{
			if (Io6Changed != null)
				Io6Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		private void OnIo7Changed(bool state)
		{
			if (Io7Changed != null)
				Io7Changed(this, new FtdiIoPort.IoChangedEventArgs(state));
		}

		public override bool ReadBytes(out byte[] rxBuffer, uint bitCount)
		{
			lock (syncThread)
			{
				bool flag = false;
				uint rxQueue = 0;
				uint num2 = (bitCount / 8) + (bitCount % 8);
				List<byte> list = new List<byte>();
				uint numBytesRead = 0;
				uint num4 = 0;
				rxQueue = 0;
				do
				{
					DateTime now = DateTime.Now;
					do
					{
						ftStatus = GetRxBytesAvailable(ref rxQueue);
						if (rxQueue == 0)
							Thread.Sleep(0);
						TimeSpan span = (TimeSpan)(DateTime.Now - now);
						flag = span.TotalMilliseconds >= 1000.0;
					}
					while (rxQueue == 0 && ftStatus == FTDI.FT_STATUS.FT_OK && !flag);
					if (rxQueue > 0)
					{
						byte[] dataBuffer = new byte[rxQueue];
						Read(dataBuffer, (uint)dataBuffer.Length, ref numBytesRead);
						list.AddRange(dataBuffer);
						num4 += numBytesRead;
					}
				}
				while (((num4 < num2) && (ftStatus == FTDI.FT_STATUS.FT_OK)) && !flag);
				if (!flag && ftStatus == FTDI.FT_STATUS.FT_OK)
				{
					rxBuffer = list.ToArray();
					return true;
				}
				rxBuffer = null;
				return false;
			}
		}

		private new void ReadThread()
		{
			byte bitMode = 0;
			while (readThreadContinue)
			{
				if (!isInitialized)
				{
					Application.DoEvents();
					Thread.Sleep(10);
				}
				else
				{
					lock (syncThread)
						ftStatus = GetPinStates(ref bitMode);

					if (ftStatus == FTDI.FT_STATUS.FT_OK)
					{
						OnIo7Changed(((bitMode & 0x80) == 0x80));
						OnIo6Changed(((bitMode & 0x40) == 0x40));
						OnIo5Changed(((bitMode & 0x20) == 0x20));
						OnIo4Changed(((bitMode & 0x10) == 0x10));
						OnIo3Changed(((bitMode & 0x08) == 0x08));
						OnIo2Changed(((bitMode & 0x04) == 0x04));
						OnIo1Changed(((bitMode & 0x02) == 0x02));
						OnIo0Changed(((bitMode & 0x01) == 0x01));
					}
					else
					{
						lock (syncThread)
							Close();
					}
					Thread.Sleep(10);
				}
			}
		}

		public abstract void ScanIn(int bitCount, bool clockOutDataBitsMSBFirst);
		public abstract void ScanInOut(int bitCount, byte[] data, bool clockOutDataBitsMSBFirst);
		public abstract void ScanOut(int bitCount, byte[] data, bool clockOutDataBitsMSBFirst);
		public override bool SendBytes()
		{
			lock (syncThread)
			{
				uint numBytesWritten = 0;
				byte[] array = new byte[txBuffer.Count];
				txBuffer.CopyTo(array);
				ftStatus = Write(array, array.Length, ref numBytesWritten);
				txBuffer.Clear();
				return (ftStatus == FTDI.FT_STATUS.FT_OK);
			}
		}

		public bool SetFrequency(uint frequency)
		{
			lock (syncThread)
			{
				if ((frequency < 0x5c) || (frequency > 0x5b8d80))
					throw new Exception("The frequency value must be in 92 up to 6000000 range.");
				clockDivisor = (0xb71b00 / (2 * frequency)) - 1;
				txBuffer.Add(0x86);
				txBuffer.Add((byte)(clockDivisor & 0xff));
				txBuffer.Add((byte)((clockDivisor >> 8) & 0xff));
				return SendBytes();
			}
		}

		private bool Sync2Mpsse()
		{
			uint rxQueue = 0;
			byte[] dataBuffer = new byte[2];
			uint numBytesRead = 0;
			uint retries;

			if (GetRxBytesAvailable(ref rxQueue) != FTDI.FT_STATUS.FT_OK)
				return false;

			if (rxQueue > 0)
			{
				retries = 1000;
				do
				{
					dataBuffer = new byte[4];
					if ((Read(dataBuffer, (uint)dataBuffer.Length, ref numBytesRead) != FTDI.FT_STATUS.FT_OK) || (retries-- == 0))
						return false;
					rxQueue -= numBytesRead;
				}
				while (rxQueue > 4);
				if (rxQueue > 0)
				{
					dataBuffer = new byte[rxQueue];
					if (Read(dataBuffer, (uint)dataBuffer.Length, ref numBytesRead) != FTDI.FT_STATUS.FT_OK)
						return false;
				}
			}
			retries = 1000;
			do
			{
				txBuffer.Add(250);
				txBuffer.Add(170);
				if (!SendBytes())
					return false;
				Thread.Sleep(100);
				if ((GetRxBytesAvailable(ref rxQueue) != FTDI.FT_STATUS.FT_OK) || (retries-- == 0))
					return false;
			}
			while (rxQueue == 0);

			dataBuffer = new byte[rxQueue];
			if (Read(dataBuffer, (uint)dataBuffer.Length, ref numBytesRead) != FTDI.FT_STATUS.FT_OK)
				return false;

			return (
					numBytesRead > 1
				&& dataBuffer[numBytesRead - 2] == 0xFA
				&& dataBuffer[numBytesRead - 1] == 0xAA
				);
		}

		public byte PortAcDir
		{
			get { return portAcDir; }
			set
			{
				portAcDir = value;
				if (isInitialized)
				{
					txBuffer.Add(0x82);
					txBuffer.Add(PortAcValue);
					txBuffer.Add(portAcDir);
				}
			}
		}

		public byte PortAcValue
		{
			get { return portAcValue; }
			set
			{
				portAcValue = value;
				if (isInitialized)
				{
					txBuffer.Add(0x82);
					txBuffer.Add(portAcValue);
					txBuffer.Add(PortAcDir);
				}
			}
		}

		public override byte PortDir
		{
			get { return portDir; }
			set
			{
				portDir = value;
				if (isInitialized)
				{
					txBuffer.Add(0x80);
					txBuffer.Add(PortValue);
					txBuffer.Add(portDir);
				}
			}
		}

		public override byte PortValue
		{
			get { return portValue; }
			set
			{
				portValue = value;
				if (isInitialized)
				{
					txBuffer.Add(0x80);
					txBuffer.Add(portValue);
					txBuffer.Add(PortDir);
				}
			}
		}
	}
}
