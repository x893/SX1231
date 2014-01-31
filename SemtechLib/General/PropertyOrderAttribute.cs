using System;

namespace SemtechLib.General
{

	[AttributeUsage(AttributeTargets.Property)]
	public class PropertyOrderAttribute : Attribute
	{
		private int _order;

		public PropertyOrderAttribute()
		{
		}

		public PropertyOrderAttribute(int order)
		{
			_order = order;
		}

		public int Order
		{
			get { return _order; }
		}
	}
}