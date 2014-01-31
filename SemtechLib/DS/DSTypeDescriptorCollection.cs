namespace DS
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    public class DSTypeDescriptorCollection : CollectionBase, ITypedList
    {
        private DSTypeDescriptor _Tables = new DSTypeDescriptor();

        public DSTypeDescriptorCollection()
        {
            base.List.Add(this._Tables);
        }

        public void Add(object Target, string PropertyName, Type ElementType)
        {
            this._Tables.Add(new DSPropertyDescriptor(Target, PropertyName, ElementType));
        }

        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            if ((listAccessors == null) || (listAccessors.Length == 0))
            {
                return ((ICustomTypeDescriptor) this._Tables).GetProperties();
            }
            Type elementType = this._Tables.GetElementType(listAccessors);
            if (elementType == null)
            {
                return new PropertyDescriptorCollection(null);
            }
            return TypeDescriptor.GetProperties(elementType);
        }

        string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
        {
            if ((listAccessors == null) || (listAccessors.Length == 0))
            {
                return "SUX";
            }
            if (this._Tables.GetElementType(listAccessors) == null)
            {
                return "";
            }
            return listAccessors[listAccessors.Length - 1].Name;
        }
    }
}

