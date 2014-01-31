using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace SemtechLib.General
{
	public class EditableObject : IEditableObject
	{
		private BindingCollectionBase _collection;
		private object[] _originalValues;

		internal void SetCollection(BindingCollectionBase Collection)
		{
			_collection = Collection;
		}

		void IEditableObject.BeginEdit()
		{
			Trace.WriteLine("BeginEdit");
			if (!IsEdit)
			{
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this, (Attribute[])null);
				object[] objArray = new object[properties.Count];
				for (int i = 0; i < properties.Count; i++)
				{
					objArray[i] = NotCopied.Value;
					PropertyDescriptor descriptor = properties[i];
					if (descriptor.PropertyType.IsSubclassOf(typeof(ValueType)))
						objArray[i] = descriptor.GetValue(this);
					else
					{
						object obj2 = descriptor.GetValue(this);
						if (obj2 == null)
							objArray[i] = null;
						else if (!(obj2 is IList) && (obj2 is ICloneable))
							objArray[i] = ((ICloneable)obj2).Clone();
					}
				}
				_originalValues = objArray;
			}
		}

		void IEditableObject.CancelEdit()
		{
			Trace.WriteLine("CancelEdit");
			if (IsEdit)
			{
				if (PendingInsert)
					((IList)_collection).Remove(this);
				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this, (Attribute[])null);
				for (int i = 0; i < properties.Count; i++)
					if (!(_originalValues[i] is NotCopied))
						properties[i].SetValue(this, _originalValues[i]);
				_originalValues = null;
			}
		}

		void IEditableObject.EndEdit()
		{
			Trace.WriteLine("EndEdit");
			if (IsEdit)
			{
				if (PendingInsert)
					_collection.pendingInsert = null;
				_originalValues = null;
			}
		}

		protected BindingCollectionBase Collection
		{
			get { return _collection; }
		}

		private bool IsEdit
		{
			get { return (_originalValues != null); }
		}

		private bool PendingInsert
		{
			get { return (_collection.pendingInsert == this); }
		}

		private class NotCopied
		{
			private static EditableObject.NotCopied value = new EditableObject.NotCopied();

			public static EditableObject.NotCopied Value
			{
				get { return value; }
			}
		}
	}
}