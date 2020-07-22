namespace P371.ASDML.Types
{
    /// <summary>
    /// A Logical value in ASDML
    /// </summary>
    public sealed class Logical : Object<bool>
    {
        internal Logical(bool value) : base(value) { }

        /// <summary>
        /// Creates an ASDML Logical value from a <see cref="bool" />
        /// </summary>
        /// <param name="value">The <see cref="bool" /> to convert</param>
        public static implicit operator Logical(bool value) => new Logical(value);

        /// <summary>
        /// Converts an ASDML logical to a <see cref="bool" />
        /// </summary>
        /// <param name="logical">The <see cref="Logical" /> to convert</param>
        public static implicit operator bool(Logical logical) => logical.Value;
    }
}
