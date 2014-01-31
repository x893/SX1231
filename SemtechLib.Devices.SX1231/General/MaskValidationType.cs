namespace SemtechLib.Devices.SX1231.General
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;

    public class MaskValidationType
    {
        private byte[] arrayValue;
        private static int length = 1;

        public MaskValidationType()
        {
            this.arrayValue = new byte[1];
            length = 1;
        }

        public MaskValidationType(string stringValue)
        {
            this.StringValue = stringValue;
        }

        public MaskValidationType(byte[] array)
        {
            this.ArrayValue = array;
        }

        private static void doParsing(string s, out byte[] bytes)
        {
            s = s.Replace(" ", "");
            string[] strArray = s.Split(new char[] { '-' });
            bytes = new byte[strArray.Length];
            try
            {
                int index = 0;
                foreach (string str in strArray)
                {
                    bytes[index] = Convert.ToByte(str, 0x10);
                    index++;
                }
            }
            catch
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The provided string {0} is not valid", new object[] { s }));
            }
        }

        public static MaskValidationType Parse(string s)
        {
            byte[] buffer;
            doParsing(s, out buffer);
            return new MaskValidationType(buffer);
        }

        public override string ToString()
        {
            string str = "";
            int index = 0;
            while (index < (this.arrayValue.Length - 1))
            {
                str = str + this.arrayValue[index].ToString("X02", CultureInfo.CurrentCulture) + "-";
                index++;
            }
            return (str + this.arrayValue[index].ToString("X02", CultureInfo.CurrentCulture));
        }

        public byte[] ArrayValue
        {
            get
            {
                return this.arrayValue;
            }
            set
            {
                if (this.arrayValue == null)
                {
                    this.arrayValue = new byte[1];
                }
                if (value == null)
                {
                    throw new ArgumentNullException("The array cannot be null.");
                }
                if ((value.Length < 1) && (value.Length > 8))
                {
                    throw new ArgumentException("Array should have as size comprized between 1 and 8.");
                }
                if (this.arrayValue.Length != value.Length)
                {
                    Array.Resize<byte>(ref this.arrayValue, value.Length);
                }
                Array.Copy(value, this.arrayValue, value.Length);
                length = value.Length;
            }
        }

        public static MaskValidationType InvalidMask
        {
            get
            {
                return new MaskValidationType(new byte[Length]);
            }
        }

        public static int Length
        {
            get
            {
                return length;
            }
        }

        public string StringValue
        {
            get
            {
                return this.ToString();
            }
            set
            {
                try
                {
                    string[] strArray = value.Split(new char[] { '-' });
                    this.arrayValue = new byte[strArray.Length];
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        this.arrayValue[i] = Convert.ToByte(strArray[i], 0x10);
                    }
                }
                catch
                {
                }
                finally
                {
                    length = this.arrayValue.Length;
                }
            }
        }
    }
}

