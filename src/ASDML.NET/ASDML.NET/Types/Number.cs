using System;

namespace P371.ASDML.Types
{
    public sealed class Number : Object<double>
    {
        public string RawValue { get; }

        internal Number(double value) : this(value, value.ToString()) { }

        internal Number(double value, string rawValue) : base(value) => RawValue = rawValue;

        // https://stackoverflow.com/questions/1130698/checking-if-an-object-is-a-number-in-c-sharp
        private static bool IsNumeric(object obj)
            => obj is sbyte
            || obj is byte
            || obj is short
            || obj is ushort
            || obj is int
            || obj is uint
            || obj is long
            || obj is ulong
            || obj is float
            || obj is double
            || obj is decimal;

        public override bool Equals(object obj)
            => IsNumeric(obj) ? Convert.ToDouble(obj) == Value : base.Equals(obj);

        public static implicit operator Number(string rawValue)
            => double.TryParse(rawValue, out double result)
                ? new Number(result, rawValue)
                : new Number(double.NaN, rawValue);

        public static implicit operator string(Number number) => number.RawValue;

        public static implicit operator Number(double value) => new Number(value);

        public static implicit operator double(Number number) => number.Value;

        public static bool operator <(Number left, Number right) => left.Value < right.Value;
        public static bool operator <(Number left, double right) => left.Value < right;
        public static bool operator <(double left, Number right) => left < right.Value;

        public static bool operator >(Number left, Number right) => left.Value > right.Value;
        public static bool operator >(Number left, double right) => left.Value > right;
        public static bool operator >(double left, Number right) => left > right.Value;

        public static bool operator <=(Number left, Number right) => left.Value <= right.Value;
        public static bool operator <=(Number left, double right) => left.Value <= right;
        public static bool operator <=(double left, Number right) => left <= right.Value;

        public static bool operator >=(Number left, Number right) => left.Value >= right.Value;
        public static bool operator >=(Number left, double right) => left.Value >= right;
        public static bool operator >=(double left, Number right) => left >= right.Value;
    }
}
