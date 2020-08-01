using System.Collections.Generic;

namespace P371.ASDML.Types.Helpers
{
    /// <summary>
    /// An ASDML object that can have nested content
    /// </summary>
    public interface IObjectCollection
    {
        /// <summary>
        /// The list of the nested objects
        /// </summary>
        List<Object> NestedObjects { get; }
    }
}
