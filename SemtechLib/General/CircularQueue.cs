using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SemtechLib.General
{
	public class CircularQueue<T> : Queue<T>
	{
		private int m_MaxLength;

		public CircularQueue(int maxLength)
		{
			MaxLength = maxLength;
		}

		public void Add(T item)
		{
			if (base.Count < MaxLength)
				base.Enqueue(item);
			else
			{
				base.Dequeue();
				base.Enqueue(item);
			}
		}

		public int MaxLength
		{
			get { return m_MaxLength; }
			set { m_MaxLength = value; }
		}
	}
}