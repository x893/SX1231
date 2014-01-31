using System;
using System.ComponentModel;
using System.Threading;

namespace SemtechLib.General
{
	public class Register : INotifyPropertyChanged
	{
		private uint _address;
		private string _name;
		private uint _oldValue;
		private bool _readOnly;
		private uint _value;
		private bool _visible;

		public event PropertyChangedEventHandler PropertyChanged;

		public Register()
		{
			_name = "";
			_address = 0;
			_value = 0;
			_oldValue = 0;
		}

		public Register(string name, uint address, uint value)
		{
			_name = name;
			_address = address;
			_value = value;
			_oldValue = value;
			_readOnly = false;
		}

		public Register(string name, uint address, uint value, bool readOnly, bool visible)
		{
			_name = name;
			_address = address;
			_value = value;
			_oldValue = value;
			_readOnly = readOnly;
			_visible = visible;
		}

		public void ApplyValue()
		{
			_oldValue = _value;
		}

		public bool IsValueChanged()
		{
			if (_oldValue == _value)
				return false;
			return true;
		}

		private void OnPropertyChanged(string propName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}

		public uint Address
		{
			get { return _address; }
			set
			{
				_address = value;
				OnPropertyChanged("Address");
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged("Name");
			}
		}

		public bool ReadOnly
		{
			get { return _readOnly; }
			set
			{
				_readOnly = value;
				OnPropertyChanged("ReadOnly");
			}
		}

		public uint Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				OnPropertyChanged("Value");
			}
		}

		public bool Visible
		{
			get { return _visible; }
			set
			{
				_visible = value;
				OnPropertyChanged("Visible");
			}
		}
	}
}
