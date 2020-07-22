namespace P371.ASDML.Types
{
    /// <summary>
    /// The non-generic common ASDML base Object
    /// </summary>
    public class Object { }

    /// <summary>
    /// The base Object of ASDML types
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class Object<TValue> : Object
    {
        /// <summary>
        /// The C# representation of the ASDML Object
        /// </summary>
        public virtual TValue Value { get; }

        internal Object(TValue value) => Value = value;

        /// <summary>
        /// Gets the hash code of the current Number
        /// </summary>
        /// <returns>The hash code of <see cref="Value" /></returns>
        public override int GetHashCode() => Value == null ? 0 : Value.GetHashCode();

        /// <summary>
        /// Checks if the current object is equal to <paramref name="obj" />
        /// </summary>
        /// <param name="obj">The other object to check equality</param>
        /// <returns>The current object and <paramref name="obj" /> are equal or not</returns>
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

        /// <summary>
        /// Return the string representation of <see cref="Value" />
        /// </summary>
        /// <returns>The value as <see cref="string" /></returns>
        public override string ToString() => Value?.ToString();

        /// <summary>
        /// Checks if two Objects are equal
        /// </summary>
        /// <param name="left">The left Object of the operator</param>
        /// <param name="right">The right Object of the operator</param>
        /// <returns>The two Objects are equal or not</returns>
        public static bool operator ==(Object<TValue> left, Object<TValue> right)
            => left is null && right is null ? true : left.Equals(right);

        /// <summary>
        /// Checks if two Objects are equal
        /// </summary>
        /// <param name="left">The left Object of the operator</param>
        /// <param name="right">The right Object of the operator</param>
        /// <returns>The two Objects are equal or not</returns>
        public static bool operator !=(Object<TValue> left, Object<TValue> right)
            => left is null && right is null ? false : !left.Equals(right);

        /// <summary>
        /// Checks if two Objects are equal
        /// </summary>
        /// <param name="left">The left Object of the operator</param>
        /// <param name="right">The right Object of the operator</param>
        /// <returns>The two Objects are equal or not</returns>
        public static bool operator ==(Object<TValue> left, object right)
            => left is null && right is null ? true : left.Equals(right);

        /// <summary>
        /// Checks if two Objects are equal
        /// </summary>
        /// <param name="left">The left Object of the operator</param>
        /// <param name="right">The right Object of the operator</param>
        /// <returns>The two Objects are equal or not</returns>
        public static bool operator !=(Object<TValue> left, object right)
            => left is null && right is null ? false : !left.Equals(right);

        /// <summary>
        /// Checks if two Objects are equal
        /// </summary>
        /// <param name="left">The left Object of the operator</param>
        /// <param name="right">The right Object of the operator</param>
        /// <returns>The two Objects are equal or not</returns>
        public static bool operator ==(object left, Object<TValue> right) => right == left;

        /// <summary>
        /// Checks if two Objects are equal
        /// </summary>
        /// <param name="left">The left Object of the operator</param>
        /// <param name="right">The right Object of the operator</param>
        /// <returns>The two Objects are equal or not</returns>
        public static bool operator !=(object left, Object<TValue> right) => right != left;
    }
}
