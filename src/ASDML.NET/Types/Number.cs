namespace P371.ASDML.Types
{
    public class Number : Object<double>
    {
        public Number(double value) : base(value) { }

        public static implicit operator Number(double value) => new Number(value: value);

        public static implicit operator double(Number number) => number.Value;
    }
}
