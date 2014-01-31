namespace TestIListSource2
{
    using DS;
    using System;
    using System.Reflection;

    public class OrderList : BindingCollectionBase
    {
        public int Add(Order Item)
        {
            return base.List.Add(Item);
        }

        protected override Type ElementType
        {
            get
            {
                return typeof(Order);
            }
        }

        public Order this[int Index]
        {
            get
            {
                return (base.List[Index] as Order);
            }
            set
            {
                base.List[Index] = value;
            }
        }
    }
}

