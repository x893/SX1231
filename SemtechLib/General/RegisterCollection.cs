using System;
using System.Collections;
using System.Reflection;
using System.Threading;

namespace SemtechLib.General
{
	public class RegisterCollection : CollectionBase
	{
		public event EventHandler DataInserted;

		public RegisterCollection()
		{
		}

		public RegisterCollection(RegisterCollection value)
		{
			AddRange(value);
		}

		public RegisterCollection(Register[] value)
		{
			AddRange(value);
		}

		public int Add(Register value)
		{
			return base.List.Add(value);
		}

		public void AddRange(Register[] value)
		{
			for (int i = 0; i < value.Length; i++)
				Add(value[i]);
		}

		public void AddRange(RegisterCollection value)
		{
			for (int i = 0; i < value.Count; i++)
				Add(value[i]);
		}

		public bool Contains(Register value)
		{
			return base.List.Contains(value);
		}

		public void CopyTo(Register[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		public new RegisterEnumerator GetEnumerator()
		{
			return new RegisterEnumerator(this);
		}

		public int IndexOf(Register value)
		{
			return base.List.IndexOf(value);
		}

		public void Insert(int index, Register value)
		{
			base.List.Insert(index, value);
		}

		protected override void OnInsert(int index, object value)
		{
			base.OnInsert(index, value);
			if (DataInserted != null)
				DataInserted(this, EventArgs.Empty);
		}

		public void Remove(Register value)
		{
			base.Capacity--;
			base.List.Remove(value);
		}

		public Register this[int index]
		{
			get { return (Register)base.List[index]; }
			set { base.List[index] = value; }
		}

		public Register this[string name]
		{
			get
			{
				foreach (Register register in base.List)
				{
					if (register.Name == name)
					{
						return register;
					}
				}
				return null;
			}
			set
			{
				foreach (Register register in base.List)
				{
					if (register.Name == name)
					{
						base.List[(int)register.Address] = value;
					}
				}
			}
		}

		public class RegisterEnumerator : IEnumerator
		{
			private IEnumerator baseEnumerator;
			private IEnumerable temp;

			public RegisterEnumerator(RegisterCollection mappings)
			{
				temp = mappings;
				baseEnumerator = temp.GetEnumerator();
			}

			public bool MoveNext()
			{
				return baseEnumerator.MoveNext();
			}

			public void Reset()
			{
				baseEnumerator.Reset();
			}

			bool IEnumerator.MoveNext()
			{
				return baseEnumerator.MoveNext();
			}

			void IEnumerator.Reset()
			{
				baseEnumerator.Reset();
			}

			public Register Current
			{
				get { return (Register)baseEnumerator.Current; }
			}

			object IEnumerator.Current
			{
				get { return baseEnumerator.Current; }
			}
		}
	}
}