namespace P371.ASDML.Types
{
    public class Logical : Object<bool>
    {
        internal Logical(bool value) : base(value: value) { }

        public static implicit operator Logical(bool value) => new Logical(value: value);

        public static implicit operator bool(Logical logical) => logical.Value;
    }
}
