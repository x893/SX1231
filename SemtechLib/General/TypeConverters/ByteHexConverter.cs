namespace SemtechLib.General.TypeConverters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class ByteHexConverter : ByteConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type t)
        {
            return ((t == typeof(string)) || base.CanConvertFrom(context, t));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value)
        {
            if (!(value is string))
            {
                return base.ConvertFrom(context, info, value);
            }
            try
            {
                string str = (string) value;
                if ((str.StartsWith("0x", true, info) && (str.Length <= 4)) || (str.Length <= 2))
                {
                    return Convert.ToByte(str, 0x10);
                }
            }
            catch
            {
            }
            throw new ArgumentException("Can not convert '" + ((string) value) + "' to type ByteHexConverter");
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            if ((destType == typeof(string)) && (value is byte))
            {
                byte num = (byte) value;
                return ("0x" + num.ToString("X02"));
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }
}

