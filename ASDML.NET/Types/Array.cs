using System.Linq;
using System.Collections.Generic;
using P371.ASDML.Types.Helpers;

namespace P371.ASDML.Types
{
    /// <summary>
    /// An Array in ASDML
    /// </summary>
    public sealed class Array : Object<Object[]>, IObjectCollection
    {
        internal Array() : base(null) => NestedObjects = new List<Object>();

        internal Array(Object[] value) : base(null) => NestedObjects = value.ToList();

        /// <summary>
        /// The items of the ASDML Array
        /// </summary>
        public List<Object> NestedObjects { get; } = new List<Object>();

        /// <summary>
        /// Returns a new array from <see cref="IObjectCollection.NestedObjects" />
        /// </summary>
        public override Object[] Value => NestedObjects.ToArray();

        /// <summary>
        /// Creates an ASDML Array from an <see cref="T:Object[]" />
        /// </summary>
        /// <param name="value">The <see cref="Object" /> to convert</param>
        public static implicit operator Array(Object[] value) => new Array(value);

        /// <summary>
        /// Converts an ASDML Array to an <see cref="T:Object[]" />
        /// </summary>
        /// <param name="array">The <see cref="Object" /> to convert</param>
        public static implicit operator Object[](Array array) => array.Value;
    }
}
