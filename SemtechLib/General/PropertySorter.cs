using System;
using System.Collections;
using System.ComponentModel;

namespace SemtechLib.General
{
	public class PropertySorter : ExpandableObjectConverter
	{
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value, attributes);
			ArrayList list = new ArrayList();
			foreach (PropertyDescriptor descriptor in properties)
			{
				Attribute attribute = descriptor.Attributes[typeof(PropertyOrderAttribute)];
				if (attribute != null)
				{
					PropertyOrderAttribute attribute2 = (PropertyOrderAttribute)attribute;
					list.Add(new PropertyOrderPair(descriptor.Name, attribute2.Order));
				}
				else
					list.Add(new PropertyOrderPair(descriptor.Name, 0));
			}
			list.Sort();
			ArrayList list2 = new ArrayList();
			foreach (PropertyOrderPair pair in list)
			{
				list2.Add(pair.Name);
			}
			return properties.Sort((string[])list2.ToArray(typeof(string)));
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}