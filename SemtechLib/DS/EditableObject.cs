namespace DS
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;

    public class EditableObject : IEditableObject
    {
        private BindingCollectionBase _Collection;
        private object[] _OriginalValues;

        internal void SetCollection(BindingCollectionBase Collection)
        {
            this._Collection = Collection;
        }

        void IEditableObject.BeginEdit()
        {
            Trace.WriteLine("BeginEdit");
            if (!this.IsEdit)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this, (Attribute[]) null);
                object[] objArray = new object[properties.Count];
                for (int i = 0; i < properties.Count; i++)
                {
                    objArray[i] = NotCopied.Value;
                    PropertyDescriptor descriptor = properties[i];
                    if (descriptor.PropertyType.IsSubclassOf(typeof(ValueType)))
                    {
                        objArray[i] = descriptor.GetValue(this);
                    }
                    else
                    {
                        object obj2 = descriptor.GetValue(this);
                        if (obj2 == null)
                        {
                            objArray[i] = null;
                        }
                        else if (!(obj2 is IList) && (obj2 is ICloneable))
                        {
                            objArray[i] = ((ICloneable) obj2).Clone();
                        }
                    }
                }
                this._OriginalValues = objArray;
            }
        }

        void IEditableObject.CancelEdit()
        {
            Trace.WriteLine("CancelEdit");
            if (this.IsEdit)
            {
                if (this.PendingInsert)
                {
                    ((IList) this._Collection).Remove(this);
                }
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this, (Attribute[]) null);
                for (int i = 0; i < properties.Count; i++)
                {
                    if (!(this._OriginalValues[i] is NotCopied))
                    {
                        properties[i].SetValue(this, this._OriginalValues[i]);
                    }
                }
                this._OriginalValues = null;
            }
        }

        void IEditableObject.EndEdit()
        {
            Trace.WriteLine("EndEdit");
            if (this.IsEdit)
            {
                if (this.PendingInsert)
                {
                    this._Collection._PendingInsert = null;
                }
                this._OriginalValues = null;
            }
        }

        protected BindingCollectionBase Collection
        {
            get
            {
                return this._Collection;
            }
        }

        private bool IsEdit
        {
            get
            {
                return (this._OriginalValues != null);
            }
        }

        private bool PendingInsert
        {
            get
            {
                return (this._Collection._PendingInsert == this);
            }
        }

        private class NotCopied
        {
            private static EditableObject.NotCopied _Value = new EditableObject.NotCopied();

            public static EditableObject.NotCopied Value
            {
                get
                {
                    return _Value;
                }
            }
        }
    }
}

