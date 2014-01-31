namespace TestIListSource2
{
    using DS;
    using System;

    public class OrderConverter : SimpleObjectConverter
    {
        public OrderConverter() : base(typeof(Order))
        {
        }
    }
}

