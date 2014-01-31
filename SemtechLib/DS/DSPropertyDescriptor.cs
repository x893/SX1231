namespace DS
{
    using System;
    using System.ComponentModel;

    public class DSPropertyDescriptor : PropertyDescriptor
    {
        private PropertyDescriptor _Desc;
        private Type _ElementType;
        private object _Target;

        public DSPropertyDescriptor(object Target, string PropertyName, Type ElementType) : base(PropertyName, null)
        {
            this._Target = Target;
            this._ElementType = ElementType;
            Type propertyType = Target.GetType().GetProperty(PropertyName).PropertyType;
            this._Desc = TypeDescriptor.CreateProperty(Target.GetType(), PropertyName, propertyType, new Attribute[0]);
        }

        public override bool CanResetValue(object Component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return this._Desc.GetValue(this._Target);
        }

        public override void ResetValue(object Component)
        {
        }

        public override void SetValue(object component, object value)
        {
        }

        public override bool ShouldSerializeValue(object Component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get
            {
                return this._ElementType;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this._Desc.PropertyType;
            }
        }
    }
}

