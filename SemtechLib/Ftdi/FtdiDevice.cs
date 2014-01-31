using System;
using System.Threading;

namespace SemtechLib.Ftdi
{
	public class FtdiDevice : IDisposable
	{
		public enum MpsseProtocol
		{
			SPI,
			I2C
		}

		private Mpsse portA;
		private IoPort portB;

		public event EventHandler Closed;
		public event EventHandler Opened;

		public FtdiDevice(MpsseProtocol protocol)
		{
			switch (protocol)
			{
				case MpsseProtocol.SPI:
					portA = new MpsseSPI("A");
					break;

				case MpsseProtocol.I2C:
					portA = new MpsseI2C("A");
					break;
			}
			portB = new IoPort("B");

			portA.Opened += new EventHandler(ports_Opened);
			portA.Closed += new EventHandler(ports_Closed);

			portB.Opened += new EventHandler(ports_Opened);
			portB.Closed += new EventHandler(ports_Closed);
		}

		public bool Close()
		{
			if (portA.IsOpen)
				portA.Close();
			if (portB.IsOpen)
				portB.Close();
			OnClosed();
			return true;
		}

		public void Dispose()
		{
			if (IsOpen)
				Close();
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
			if (portA.Open(name) && portB.Open(name))
			{
				OnOpened();
				return true;
			}
			return false;
		}

		private void ports_Closed(object sender, EventArgs e)
		{
			Close();
		}

		private void ports_Opened(object sender, EventArgs e)
		{
			if (IsOpen)
				OnOpened();
		}

		public bool IsOpen
		{
			get { return (portA.IsOpen && portB.IsOpen); }
		}

		public Mpsse PortA
		{
			get { return portA; }
			set { portA = value; }
		}

		public IoPort PortB
		{
			get { return portB; }
			set { portB = value; }
		}
	}
}