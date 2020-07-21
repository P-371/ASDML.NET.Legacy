namespace P371.ASDML.Types
{
    public class Number : Object<double>
    {
        public string RawValue { get; }

        internal Number(double value) : this(value, value.ToString()) { }

        internal Number(double value, string rawValue) : base(value) => RawValue = rawValue;

        public static implicit operator Number(string rawValue)
            => double.TryParse(rawValue, out double result)
                ? new Number(result, rawValue)
                : new Number(double.NaN, rawValue);

        public static implicit operator string(Number number) => number.RawValue;

        public static implicit operator Number(double value) => new Number(value);

        public static implicit operator double(Number number) => number.Value;
    }
}
