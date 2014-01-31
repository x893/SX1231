namespace DS
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;

    public class DSTypeDescriptor : ICustomTypeDescriptor
    {
        private ArrayList _TableDesc = new ArrayList();

        public void Add(DSPropertyDescriptor Desc)
        {
            this._TableDesc.Add(Desc);
        }

        public Type GetElementType(PropertyDescriptor[] listAccessors)
        {
            PropertyDescriptor descriptor = listAccessors[0];
            foreach (DSPropertyDescriptor descriptor2 in this._TableDesc)
            {
                if (descriptor2.Name == descriptor.Name)
                {
                    if (listAccessors.Length == 1)
                    {
                        return descriptor2.ComponentType;
                    }
                    Type componentType = descriptor2.ComponentType;
                    for (int i = 1; i < listAccessors.Length; i++)
                    {
                        PropertyInfo property = componentType.GetProperty(listAccessors[1].Name);
                        if (property == null)
                        {
                            return null;
                        }
                        property = property.PropertyType.GetProperty("Item");
                        if (property == null)
                        {
                            return null;
                        }
                        componentType = property.PropertyType;
                    }
                    return componentType;
                }
            }
            return null;
        }

        AttributeCollection ICustomTypeDescriptor.GetAttributes()
        {
            return new AttributeCollection(null);
        }

        string ICustomTypeDescriptor.GetClassName()
        {
            return null;
        }

        string ICustomTypeDescriptor.GetComponentName()
        {
            return null;
        }

        TypeConverter ICustomTypeDescriptor.GetConverter()
        {
            return null;
        }

        EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
        {
            return null;
        }

        PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
        {
            return null;
        }

        object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return null;
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
        {
            return new EventDescriptorCollection(null);
        }

        EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return new EventDescriptorCollection(null);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
        {
            return ((ICustomTypeDescriptor) this).GetProperties(null);
        }

        PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            PropertyDescriptor[] array = new PropertyDescriptor[this._TableDesc.Count];
            this._TableDesc.CopyTo(array);
            return new PropertyDescriptorCollection(array);
        }

        object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
    }
}

