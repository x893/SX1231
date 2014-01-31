namespace TestIListSource2
{
    using DS;
    using System;
    using System.ComponentModel;

    [TypeConverter(typeof(OrderConverter))]
    public class Order : EditableObject
    {
        private string _A;
        private string _B;
        private ItemList _Items = new ItemList();

        public string A
        {
            get
            {
                return this._A;
            }
            set
            {
                this._A = value;
            }
        }

        public string B
        {
            get
            {
                return this._B;
            }
            set
            {
                this._B = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ItemList Items
        {
            get
            {
                return this._Items;
            }
        }
    }
}

