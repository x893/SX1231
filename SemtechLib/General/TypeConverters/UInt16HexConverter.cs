namespace SemtechLib.General.TypeConverters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class UInt16HexConverter : UInt16Converter
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
                if ((str.StartsWith("0x", true, info) && (str.Length <= 6)) || (str.Length <= 4))
                {
                    return Convert.ToUInt16(str, 0x10);
                }
            }
            catch
            {
            }
            throw new ArgumentException("Can not convert '" + ((string) value) + "' to type UInt16HexConverter");
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            if ((destType == typeof(string)) && (value is ushort))
            {
                ushort num = (ushort) value;
                return ("0x" + num.ToString("X04"));
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }
}

