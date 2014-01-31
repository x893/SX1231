using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class AgcStepEventArg : EventArgs
	{
		private byte _id;
		private byte _value;

		public AgcStepEventArg(byte id, byte value)
		{
			_id = id;
			_value = value;
		}

		public byte Id
		{
			get { return _id; }
		}

		public byte Value
		{
			get { return _value; }
		}
	}
}
