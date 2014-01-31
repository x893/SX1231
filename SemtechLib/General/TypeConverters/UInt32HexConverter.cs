namespace SemtechLib.General.TypeConverters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class UInt32HexConverter : UInt32Converter
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
                if ((str.StartsWith("0x", true, info) && (str.Length <= 10)) || (str.Length <= 8))
                {
                    return Convert.ToUInt32(str, 0x10);
                }
            }
            catch
            {
            }
            throw new ArgumentException("Can not convert '" + ((string) value) + "' to type UInt32HexConverter");
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            if ((destType == typeof(string)) && (value is uint))
            {
                uint num = (uint) value;
                return ("0x" + num.ToString("X08"));
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }
}

