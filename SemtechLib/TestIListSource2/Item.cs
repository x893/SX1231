namespace TestIListSource2
{
    using DS;
    using System;
    using System.ComponentModel;

    [TypeConverter(typeof(ItemConverter))]
    public class Item : EditableObject
    {
        private string _C;

        public string C
        {
            get
            {
                return this._C;
            }
            set
            {
                this._C = value;
            }
        }

        public bool Sux
        {
            get
            {
                return (this.C != null);
            }
        }
    }
}

