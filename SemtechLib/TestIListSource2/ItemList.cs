namespace TestIListSource2
{
    using DS;
    using System;
    using System.Reflection;

    public class ItemList : BindingCollectionBase
    {
        public int Add(Item Item)
        {
            return base.List.Add(Item);
        }

        protected override Type ElementType
        {
            get
            {
                return typeof(Item);
            }
        }

        public Item this[int Index]
        {
            get
            {
                return (base.List[Index] as Item);
            }
            set
            {
                base.List[Index] = value;
            }
        }
    }
}

