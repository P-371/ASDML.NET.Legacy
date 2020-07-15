namespace P371.ASDML.Types
{
    public class Logical : Object<bool>
    {
        public Logical(bool value) : base(value) { }

        public static implicit operator Logical(bool value) => new Logical(value: value);

        public static implicit operator bool(Logical logical) => logical.Value;
    }
}
