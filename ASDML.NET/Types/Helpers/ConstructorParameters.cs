using System.Collections.Generic;

namespace P371.ASDML.Types.Helpers
{
    /// <summary>
    /// Holds the list of constructor parameters
    /// </summary>
    public class ConstructorParameters : IObjectCollection
    {
        /// <summary>
        /// The list of constructor parameters
        /// </summary>
        public List<Object> NestedObjects { get; } = new List<Object>();
    }
}
