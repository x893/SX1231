using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class DataModeEventArg : EventArgs
	{
		private DataModeEnum _value;

		public DataModeEventArg(DataModeEnum value)
		{
			_value = value;
		}

		public DataModeEnum Value
		{
			get { return _value; }
		}
	}
}