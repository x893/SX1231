namespace TestIListSource2
{
    using DS;
    using System;

    public class ItemConverter : SimpleObjectConverter
    {
        public ItemConverter() : base(typeof(Item))
        {
        }
    }
}

