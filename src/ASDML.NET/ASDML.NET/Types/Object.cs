namespace P371.ASDML.Types
{
    public class Object { }

    public class Object<TValue> : Object
    {
        public virtual TValue Value { get; }

        internal Object(TValue value) => Value = value;

        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();

        public override bool Equals(object obj)
            => Value is null && obj is null
                ? true
                : obj is Object<TValue> o
                    ? Value is null && o.Value is null
                        ? true
                        : Value?.Equals(o.Value) == true
                    : obj is TValue t
                        ? Value is null && t is null
                            ? true
                            : Value?.Equals(t) == true
                        : false;

        public override string ToString() => Value?.ToString();

        public static bool operator ==(Object<TValue> left, Object<TValue> right)
            => left is null && right is null ? true : left.Equals(right);
        public static bool operator !=(Object<TValue> left, Object<TValue> right)
            => left is null && right is null ? false : !left.Equals(right);
        public static bool operator ==(Object<TValue> left, object right)
            => left is null && right is null ? true : left.Equals(right);

        public static bool operator !=(Object<TValue> left, object right)
            => left is null && right is null ? false : !left.Equals(right);
        public static bool operator ==(object left, Object<TValue> right) => right == left;
        public static bool operator !=(object left, Object<TValue> right) => right != left;
    }
}
