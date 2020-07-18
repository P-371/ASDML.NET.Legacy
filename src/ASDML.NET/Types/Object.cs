namespace P371.ASDML.Types
{
    public class Object
    {
        public static readonly Object<Object> Null = new Object<Object>(value: null);
    }

    public class Object<TValue> : Object
    {
        public virtual TValue Value { get; }

        internal Object(TValue value)
        {
            Value = value;
        }

        public override string ToString() => Value.ToString();
    }
}
