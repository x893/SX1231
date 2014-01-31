using SemtechLib.Devices.SX1231.Enumerations;
using System;

namespace SemtechLib.Devices.SX1231.Events
{
	public class AddressFilteringEventArg : EventArgs
	{
		private AddressFilteringEnum _value;

		public AddressFilteringEventArg(AddressFilteringEnum value)
		{
			_value = value;
		}

		public AddressFilteringEnum Value
		{
			get { return _value; }
		}
	}
}
