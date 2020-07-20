namespace P371.ASDML.Types
{
    public class Number : Object<double>
    {
        public string RawValue { get; }

        internal Number(double value) : this(value: value, rawValue: value.ToString()) { }

        internal Number(double value, string rawValue) : base(value: value) => RawValue = rawValue;

        public static implicit operator Number(string rawValue)
            => double.TryParse(s: rawValue, out double result)
                ? new Number(value: result, rawValue: rawValue)
                : new Number(value: double.NaN, rawValue: rawValue);

        public static implicit operator string(Number number) => number.RawValue;

        public static implicit operator Number(double value) => new Number(value: value);

        public static implicit operator double(Number number) => number.Value;
    }
}
