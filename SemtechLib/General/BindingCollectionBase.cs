using System;
using System.Collections;
using System.ComponentModel;

namespace SemtechLib.General
{
	public abstract class BindingCollectionBase : IBindingList, IList, ICollection, IEnumerable
	{
		private ArrayList list = new ArrayList();
		internal object pendingInsert = null;

		public event ListChangedEventHandler ListChanged;

		protected BindingCollectionBase()
		{
		}

		public void Clear()
		{
			OnClear();

			for (int i = 0; i < list.Count; i++)
				((EditableObject)list[i]).SetCollection(null);

			list.Clear();
			pendingInsert = null;
			OnClearComplete();

			if (ListChanged != null)
				ListChanged(this, new ListChangedEventArgs(ListChangedType.Reset, 0));
		}

		protected virtual object CreateInstance()
		{
			return Activator.CreateInstance(ElementType);
		}

		public IEnumerator GetEnumerator()
		{
			return list.GetEnumerator();
		}

		protected virtual void OnClear()
		{
		}

		protected virtual void OnClearComplete()
		{
		}

		protected virtual void OnInsert(int index, object value)
		{
		}

		protected virtual void OnInsertComplete(int index, object value)
		{
		}

		protected virtual void OnRemove(int index, object value)
		{
		}

		protected virtual void OnRemoveComplete(int index, object value)
		{
		}

		protected virtual void OnSet(int index, object oldValue, object newValue)
		{
		}

		protected virtual void OnSetComplete(int index, object oldValue, object newValue)
		{
		}

		protected virtual void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
		}

		public void RemoveAt(int index)
		{
			if ((index < 0) || (index >= list.Count))
				throw new ArgumentOutOfRangeException();

			object obj2 = list[index];
			OnValidate(obj2);
			OnRemove(index, obj2);
			((EditableObject)list[index]).SetCollection(null);
			if (pendingInsert == obj2)
				pendingInsert = null;

			list.RemoveAt(index);
			OnRemoveComplete(index, obj2);

			if (ListChanged != null)
				ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
		}

		void ICollection.CopyTo(Array array, int index)
		{
			list.CopyTo(array, index);
		}

		int IList.Add(object value)
		{
			OnValidate(value);
			OnInsert(list.Count, value);
			int index = list.Add(value);
			try
			{
				OnInsertComplete(index, value);
			}
			catch
			{
				list.RemoveAt(index);
				throw;
			}
			((EditableObject)value).SetCollection(this);
			if (ListChanged != null)
			{
				ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemAdded, index));
			}
			return index;
		}

		bool IList.Contains(object value)
		{
			return list.Contains(value);
		}

		int IList.IndexOf(object value)
		{
			return list.IndexOf(value);
		}

		void IList.Insert(int index, object value)
		{
			if ((index < 0) || (index > list.Count))
				throw new ArgumentOutOfRangeException();

			OnValidate(value);
			OnInsert(index, value);
			list.Insert(index, value);
			try
			{
				OnInsertComplete(index, value);
			}
			catch
			{
				list.RemoveAt(index);
				throw;
			}
			((EditableObject)value).SetCollection(this);
			if (ListChanged != null)
				ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemAdded, index));
		}

		void IList.Remove(object value)
		{
			OnValidate(value);
			int index = list.IndexOf(value);
			if (index < 0)
				throw new ArgumentException();

			OnRemove(index, value);
			list.RemoveAt(index);
			OnRemoveComplete(index, value);
			if (ListChanged != null)
			{
				ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
			}
		}

		void IBindingList.AddIndex(PropertyDescriptor property)
		{
			throw new NotSupportedException();
		}

		object IBindingList.AddNew()
		{
			if (pendingInsert != null)
				((IEditableObject)pendingInsert).CancelEdit();

			object obj2 = CreateInstance();
			((IList)this).Add(obj2);
			pendingInsert = obj2;
			return obj2;
		}

		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			throw new NotSupportedException();
		}

		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			throw new NotSupportedException();
		}

		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
			throw new NotSupportedException();
		}

		void IBindingList.RemoveSort()
		{
			throw new NotSupportedException();
		}

		public int Count
		{
			get { return list.Count; }
		}

		protected virtual Type ElementType
		{
			get { return typeof(object); }
		}

		protected IList List
		{
			get { return this; }
		}

		bool ICollection.IsSynchronized
		{
			get { return list.IsSynchronized; }
		}

		object ICollection.SyncRoot
		{
			get { return list.SyncRoot; }
		}

		bool IList.IsFixedSize
		{
			get { return list.IsFixedSize; }
		}

		bool IList.IsReadOnly
		{
			get { return list.IsReadOnly; }
		}

		object IList.this[int index]
		{
			get
			{
				if ((index < 0) || (index >= list.Count))
					throw new ArgumentOutOfRangeException();
				return list[index];
			}
			set
			{
				if ((index < 0) || (index >= list.Count))
					throw new ArgumentOutOfRangeException();

				OnValidate(value);
				object oldValue = list[index];
				OnSet(index, oldValue, value);
				list[index] = value;
				try
				{
					OnSetComplete(index, oldValue, value);
				}
				catch
				{
					list[index] = oldValue;
					throw;
				}
				if (ListChanged != null)
					ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemChanged, index));
			}
		}

		bool IBindingList.AllowEdit
		{
			get
			{
				return true;
			}
		}

		bool IBindingList.AllowNew
		{
			get { return true; }
		}

		bool IBindingList.AllowRemove
		{
			get { return true; }
		}

		bool IBindingList.IsSorted
		{
			get { return false; }
		}

		ListSortDirection IBindingList.SortDirection
		{
			get { throw new NotSupportedException(); }
		}

		PropertyDescriptor IBindingList.SortProperty
		{
			get { throw new NotSupportedException(); }
		}

		bool IBindingList.SupportsChangeNotification
		{
			get { return true; }
		}

		bool IBindingList.SupportsSearching
		{
			get { return false; }
		}

		bool IBindingList.SupportsSorting
		{
			get { return false; }
		}
	}
}
