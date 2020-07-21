namespace P371.ASDML.Types
{
    public sealed class Logical : Object<bool>
    {
        internal Logical(bool value) : base(value) { }

        public static implicit operator Logical(bool value) => new Logical(value);

        public static implicit operator bool(Logical logical) => logical.Value;
    }
}
