using System;

namespace SemtechLib.General
{
	public class PropertyOrderPair : IComparable
	{
		private string _name = string.Empty;
		private int _order;

		public PropertyOrderPair()
		{
		}

		public PropertyOrderPair(string name, int order)
		{
			_order = order;
			_name = name;
		}

		public int CompareTo(object obj)
		{
			int order = ((PropertyOrderPair)obj)._order;
			if (order == _order)
			{
				string name = ((PropertyOrderPair)obj)._name;
				return string.Compare(_name, name);
			}
			if (order > _order)
				return -1;
			return 1;
		}

		public string Name
		{
			get { return _name; }
		}
	}
}
