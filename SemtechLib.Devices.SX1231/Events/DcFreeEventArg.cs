using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class DcFreeEventArg : EventArgs
	{
		private DcFreeEnum _value;

		public DcFreeEventArg(DcFreeEnum value)
		{
			_value = value;
		}

		public DcFreeEnum Value
		{
			get { return _value; }
		}
	}
}
