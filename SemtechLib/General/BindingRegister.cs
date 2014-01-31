using System;

namespace SemtechLib.General
{
	public class BindingRegister : EditableObject
	{
		private uint _address;
		private string _name;
		private bool _readOnly;
		private uint _value;

		public BindingRegister()
		{
			_name = "";
			_address = 0;
			_value = 0;
		}

		public BindingRegister(string name, uint address, uint value)
		{
			_name = name;
			_address = address;
			_value = value;
			_readOnly = false;
		}

		public BindingRegister(string name, uint address, uint value, bool readOnly)
		{
			_name = name;
			_address = address;
			_value = value;
			_readOnly = readOnly;
		}

		public uint Address
		{
			get { return _address; }
			set { _address = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public bool ReadOnly
		{
			get { return _readOnly; }
			set { _readOnly = value; }
		}

		public uint Value
		{
			get { return _value; }
			set { _value = value; }
		}
	}
}
