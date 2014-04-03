using FTD2XX_NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace SemtechLib.Ftdi
{
	public abstract class FtdiIoPort : FTDI, IDisposable
	{
		public class IoChangedEventArgs : EventArgs
		{
			private bool _state;

			public IoChangedEventArgs(bool state)
			{
				_state = state;
			}

			public bool State
			{
				get { return _state; }
			}
		}

		public delegate void IoChangedEventHandler(object sender, FtdiIoPort.IoChangedEventArgs e);

		public event EventHandler Opened;
		public event EventHandler Closed;

		public abstract event IoChangedEventHandler Io0Changed;
		public abstract event IoChangedEventHandler Io1Changed;
		public abstract event IoChangedEventHandler Io2Changed;
		public abstract event IoChangedEventHandler Io3Changed;
		public abstract event IoChangedEventHandler Io4Changed;
		public abstract event IoChangedEventHandler Io5Changed;
		public abstract event IoChangedEventHandler Io6Changed;
		public abstract event IoChangedEventHandler Io7Changed;

		private string device = "";
		private FTDI.FT_DEVICE_INFO_NODE[] deviceList;

		protected FTDI.FT_STATUS ftStatus;
		protected FtdiInfo info;
		protected bool isInitialized;
		protected byte portDir;
		protected byte portValue;
		protected Thread readThread;
		protected bool readThreadContinue;
		protected object syncThread = new object();
		protected List<byte> txBuffer = new List<byte>();

		public new bool Close()
		{
			readThreadContinue = false;
			if (base.IsOpen)
			{
				base.Close();
				OnClosed();
			}
			return true;
		}

		public void Dispose()
		{
			Close();
		}

		public virtual bool Init(uint frequency)
		{
			throw new NotImplementedException();
		}
		protected void ReadThread()
		{
			throw new NotImplementedException();
		}
		public virtual bool SendBytes()
		{
			throw new NotImplementedException();
		}
		public virtual bool ReadBytes(out byte[] rxBuffer, uint bitCount)
		{
			throw new NotImplementedException();
		}

		private void OnClosed()
		{
			if (Closed != null)
				Closed(this, EventArgs.Empty);
		}

		private void OnOpened()
		{
			if (Opened != null)
				Opened(this, EventArgs.Empty);
		}

		public bool Open(string name)
		{
			if (SearchDevice(name + " " + device))
			{
				ftStatus = base.OpenBySerialNumber(info.SerialNumber);
				if (ftStatus == FTDI.FT_STATUS.FT_OK)
				{
					OnOpened();
					return true;
				}
			}
			return false;
		}

		private bool SearchDevice(string name)
		{
			uint devcount = 0;
			ftStatus = base.GetNumberOfDevices(ref devcount);
			if (ftStatus == FTDI.FT_STATUS.FT_OK)
			{
				if (devcount == 0)
					return false;
				deviceList = new FTDI.FT_DEVICE_INFO_NODE[devcount];
				ftStatus = base.GetDeviceList(deviceList);
				if (ftStatus == FTDI.FT_STATUS.FT_OK)
					for (uint i = 0; i < devcount; i++)
					{
						string str = deviceList[i].Description.ToString();
						if (str.Length != 0 && name == str)
						{
							info.DeviceIndex = i;
							info.Flags = deviceList[i].Flags;
							info.Type = deviceList[i].Type.ToString();
							info.Id = deviceList[i].ID;
							info.LocId = deviceList[i].LocId;
							info.SerialNumber = deviceList[i].SerialNumber;
							info.Description = deviceList[i].Description;
							return true;
						}
					}
			}
			return false;
		}

		public virtual void TxBufferAdd(byte data)
		{
			txBuffer.Add(data);
		}

		public virtual void TxBufferAdd(byte[] data)
		{
			txBuffer.AddRange(Enumerable.AsEnumerable<byte>(data));
		}

		public string Device
		{
			get { return device; }
			set { device = value; }
		}

		public virtual byte PortDir
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public virtual byte PortValue
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}
	}
}
