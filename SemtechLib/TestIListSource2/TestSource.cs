namespace TestIListSource2
{
    using DS;
    using System;
    using System.Collections;
    using System.ComponentModel;

    public class TestSource : Component, IListSource
    {
        private OrderList _Orders = new OrderList();
        private OrderList _Orders2 = new OrderList();

        IList IListSource.GetList()
        {
            DSTypeDescriptorCollection descriptors = new DSTypeDescriptorCollection();
            descriptors.Add(this, "Orders", typeof(Order));
            descriptors.Add(this, "Orders2", typeof(Order));
            return descriptors;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public OrderList Orders
        {
            get
            {
                return this._Orders;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public OrderList Orders2
        {
            get
            {
                return this._Orders2;
            }
        }

        bool IListSource.ContainsListCollection
        {
            get
            {
                return false;
            }
        }
    }
}

