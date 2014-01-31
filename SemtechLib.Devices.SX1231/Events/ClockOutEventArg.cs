using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class ClockOutEventArg : EventArgs
	{
		private ClockOutEnum _value;

		public ClockOutEventArg(ClockOutEnum value)
		{
			_value = value;
		}

		public ClockOutEnum Value
		{
			get { return _value; }
		}
	}
}
