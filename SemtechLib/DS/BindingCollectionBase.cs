namespace DS
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    public abstract class BindingCollectionBase : IBindingList, IList, ICollection, IEnumerable
    {
        private ArrayList _List = new ArrayList();
        internal object _PendingInsert = null;

        public event ListChangedEventHandler ListChanged;

        protected BindingCollectionBase()
        {
        }

        public void Clear()
        {
            OnClear();
            for (int i = 0; i < _List.Count; i++)
            {
                ((EditableObject)_List[i]).SetCollection(null);
            }
            _List.Clear();
            _PendingInsert = null;
            OnClearComplete();
            if (ListChanged != null)
            {
                ListChanged(this, new ListChangedEventArgs(ListChangedType.Reset, 0));
            }
        }

        protected virtual object CreateInstance()
        {
            return Activator.CreateInstance(ElementType);
        }

        public IEnumerator GetEnumerator()
        {
            return _List.GetEnumerator();
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
            if ((index < 0) || (index >= _List.Count))
            {
                throw new ArgumentOutOfRangeException();
            }
            object obj2 = _List[index];
            OnValidate(obj2);
            OnRemove(index, obj2);
            ((EditableObject)_List[index]).SetCollection(null);
            if (_PendingInsert == obj2)
            {
                _PendingInsert = null;
            }
            _List.RemoveAt(index);
            OnRemoveComplete(index, obj2);
            if (ListChanged != null)
            {
                ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            _List.CopyTo(array, index);
        }

        int IList.Add(object value)
        {
            OnValidate(value);
            OnInsert(_List.Count, value);
            int index = _List.Add(value);
            try
            {
                OnInsertComplete(index, value);
            }
            catch
            {
                _List.RemoveAt(index);
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
            return _List.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return _List.IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            if ((index < 0) || (index > _List.Count))
            {
                throw new ArgumentOutOfRangeException();
            }
            OnValidate(value);
            OnInsert(index, value);
            _List.Insert(index, value);
            try
            {
                OnInsertComplete(index, value);
            }
            catch
            {
                _List.RemoveAt(index);
                throw;
            }
            ((EditableObject)value).SetCollection(this);
            if (ListChanged != null)
            {
                ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemAdded, index));
            }
        }

        void IList.Remove(object value)
        {
            OnValidate(value);
            int index = _List.IndexOf(value);
            if (index < 0)
            {
                throw new ArgumentException();
            }
            OnRemove(index, value);
            _List.RemoveAt(index);
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
            if (_PendingInsert != null)
            {
                ((IEditableObject)_PendingInsert).CancelEdit();
            }
            object obj2 = CreateInstance();
            ((IList)this).Add(obj2);
            _PendingInsert = obj2;
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
            get
            {
                return _List.Count;
            }
        }

        protected virtual Type ElementType
        {
            get
            {
                return typeof(object);
            }
        }

        protected IList List
        {
            get
            {
                return this;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return _List.IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return _List.SyncRoot;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return _List.IsFixedSize;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return _List.IsReadOnly;
            }
        }

        object IList.this[int index]
        {
            get
            {
                if ((index < 0) || (index >= _List.Count))
                {
                    throw new ArgumentOutOfRangeException();
                }
                return _List[index];
            }
            set
            {
                if ((index < 0) || (index >= _List.Count))
                {
                    throw new ArgumentOutOfRangeException();
                }
                OnValidate(value);
                object oldValue = _List[index];
                OnSet(index, oldValue, value);
                _List[index] = value;
                try
                {
                    OnSetComplete(index, oldValue, value);
                }
                catch
                {
                    _List[index] = oldValue;
                    throw;
                }
                if (ListChanged != null)
                {
                    ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemChanged, index));
                }
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
            get
            {
                return true;
            }
        }

        bool IBindingList.AllowRemove
        {
            get
            {
                return true;
            }
        }

        bool IBindingList.IsSorted
        {
            get
            {
                return false;
            }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get
            {
                return true;
            }
        }

        bool IBindingList.SupportsSearching
        {
            get
            {
                return false;
            }
        }

        bool IBindingList.SupportsSorting
        {
            get
            {
                return false;
            }
        }
    }
}
