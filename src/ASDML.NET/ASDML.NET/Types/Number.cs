using System;

namespace P371.ASDML.Types
{
    /// <summary>
    /// A Number in ASDML
    /// </summary>
    public sealed class Number : Object<double>
    {
        /// <summary>
        /// The raw string value that works even if the double value overflows
        /// </summary>
        /// <value></value>
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

        /// <summary>
        /// Gets the hash code of the current Number
        /// </summary>
        /// <returns>The hash code of the value</returns>
        public override int GetHashCode() => Value.GetHashCode();

        /// <summary>
        /// Checks if the current object is equal to <paramref name="obj" />.
        /// this method handles numbers if <paramref name="obj" /> is number or calls the base implementation
        /// </summary>
        /// <param name="obj">The other object to check equality</param>
        /// <returns>The current object and <paramref name="obj" /> are equal or not</returns>
        public override bool Equals(object obj)
            => IsNumeric(obj) ? Convert.ToDouble(obj) == Value : base.Equals(obj);

        /// <summary>
        /// Return the string representation of the current Number
        /// </summary>
        /// <returns>The raw value</returns>
        public override string ToString() => RawValue;

        /// <summary>
        /// Creates an ASDML Number from a raw <see cref="string" />.
        /// This method can be used if the raw value would overflow
        /// </summary>
        /// <param name="rawValue">The raw <see cref="string" /> to convert</param>
        public static implicit operator Number(string rawValue)
            => double.TryParse(rawValue, out double result)
                ? new Number(result, rawValue)
                : new Number(double.NaN, rawValue);

        /// <summary>
        /// Converts an ASDML Number to a <see cref="string" />
        /// </summary>
        /// <param name="number">The <see cref="Number" /> to convert</param>
        public static implicit operator string(Number number) => number.RawValue;

        /// <summary>
        /// Creates an ASDML Number from a <see cref="double" />.
        /// </summary>
        /// <param name="value">The <see cref="string" /> to convert</param>
        public static implicit operator Number(double value) => new Number(value);

        /// <summary>
        /// Converts an ASDML Number to a <see cref="double" />
        /// </summary>
        /// <param name="number">The <see cref="Text" /> to convert</param>
        public static implicit operator double(Number number) => number.Value;

        /// <summary>
        /// Compares <paramref name="left" /> to <paramref name="right" />
        /// </summary>
        /// <param name="left">The left Number of the operator</param>
        /// <param name="right">The right Number of the operator</param>
        /// <returns>The left number is less than the right number</returns>
        public static bool operator <(Number left, Number right) => left.Value < right.Value;

        /// <summary>
        /// Compares <paramref name="left" /> to <paramref name="right" />
        /// </summary>
        /// <param name="left">The left Number of the operator</param>
        /// <param name="right">The right Number of the operator</param>
        /// <returns>The left number is less than the right number</returns>
        public static bool operator <(Number left, double right) => left.Value < right;

        /// <summary>
        /// Compares <paramref name="left" /> to <paramref name="right" />
        /// </summary>
        /// <param name="left">The left Number of the operator</param>
        /// <param name="right">The right Number of the operator</param>
        /// <returns>The left number is less than the right number</returns>
        public static bool operator <(double left, Number right) => left < right.Value;

        /// <summary>
        /// Compares <paramref name="left" /> to <paramref name="right" />
        /// </summary>
        /// <param name="left">The left Number of the operator</param>
        /// <param name="right">The right Number of the operator</param>
        /// <returns>The left number is more than the right number</returns>
        public static bool operator >(Number left, Number right) => left.Value > right.Value;

        /// <summary>
        /// Compares <paramref name="left" /> to <paramref name="right" />
        /// </summary>
        /// <param name="left">The left Number of the operator</param>
        /// <param name="right">The right Number of the operator</param>
        /// <returns>The left number is more than the right number</returns>
        public static bool operator >(Number left, double right) => left.Value > right;

        /// <summary>
        /// Compares <paramref name="left" /> to <paramref name="right" />
        /// </summary>
        /// <param name="left">The left Number of the operator</param>
        /// <param name="right">The right Number of the operator</param>
        /// <returns>The left number is more than the right number</returns>
        public static bool operator >(double left, Number right) => left > right.Value;

        /// <summary>
        /// Compares <paramref name="left" /> to <paramref name="right" />
        /// </summary>
        /// <param name="left">The left Number of the operator</param>
        /// <param name="right">The right Number of the operator</param>
        /// <returns>The left number is less than or equal to the right number</returns>
        public static bool operator <=(Number left, Number right) => left.Value <= right.Value;

        /// <summary>
        /// Compares <paramref name="left" /> to <paramref name="right" />
        /// </summary>
        /// <param name="left">The left Number of the operator</param>
        /// <param name="right">The right Number of the operator</param>
        /// <returns>The left number is less than or equal to the right number</returns>
        public static bool operator <=(Number left, double right) => left.Value <= right;

        /// <summary>
        /// Compares <paramref name="left" /> to <paramref name="right" />
        /// </summary>
        /// <param name="left">The left Number of the operator</param>
        /// <param name="right">The right Number of the operator</param>
        /// <returns>The left number is less than or equal to the right number</returns>
        public static bool operator <=(double left, Number right) => left <= right.Value;

        /// <summary>
        /// Compares <paramref name="left" /> to <paramref name="right" />
        /// </summary>
        /// <param name="left">The left Number of the operator</param>
        /// <param name="right">The right Number of the operator</param>
        /// <returns>The left number is more than or equal to the right number</returns>
        public static bool operator >=(Number left, Number right) => left.Value >= right.Value;

        /// <summary>
        /// Compares <paramref name="left" /> to <paramref name="right" />
        /// </summary>
        /// <param name="left">The left Number of the operator</param>
        /// <param name="right">The right Number of the operator</param>
        /// <returns>The left number is more than or equal to the right number</returns>
        public static bool operator >=(Number left, double right) => left.Value >= right;

        /// <summary>
        /// Compares <paramref name="left" /> to <paramref name="right" />
        /// </summary>
        /// <param name="left">The left Number of the operator</param>
        /// <param name="right">The right Number of the operator</param>
        /// <returns>The left number is more than or equal to the right number</returns>
        public static bool operator >=(double left, Number right) => left >= right.Value;
    }
}
