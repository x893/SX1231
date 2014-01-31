namespace SemtechLib.General.TypeConverters
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Globalization;
    using System.Reflection;

    public class ObjectConverter : TypeConverter
    {
        private Type objectType;

        protected ObjectConverter(Type ObjectType)
        {
            this.objectType = ObjectType;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return ((destinationType == typeof(InstanceDescriptor)) || base.CanConvertTo(context, destinationType));
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if ((destinationType == typeof(InstanceDescriptor)) && (value.GetType() == this.objectType))
            {
                ConstructorInfo constructor = this.objectType.GetConstructor(new Type[0]);
                if (constructor != null)
                {
                    return new InstanceDescriptor(constructor, new object[0], false);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

