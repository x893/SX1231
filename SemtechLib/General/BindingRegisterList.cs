using System;
using System.Reflection;

namespace SemtechLib.General
{
	public class BindingRegisterList : BindingCollectionBase
	{
		public int Add(BindingRegister Item)
		{
			return base.List.Add(Item);
		}

		protected override Type ElementType
		{
			get { return typeof(BindingRegister); }
		}

		public BindingRegister this[int Index]
		{
			get { return (base.List[Index] as BindingRegister); }
			set { base.List[Index] = value; }
		}
	}
}