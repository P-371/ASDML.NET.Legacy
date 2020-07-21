using System;
using System.Collections.Generic;

namespace P371.ASDML.Types
{
    /// <summary>
    /// A Group in ASDML
    /// </summary>
    public sealed class Group : Object<Group>
    {
        private string name;

        internal GroupConstructionStep ConstructionStep = GroupConstructionStep.NameDone;

        /// <summary>
        /// The ASDML Group #ID
        /// </summary>
        public string ID { get; internal set; }

        /// <summary>
        /// The ASDML Group Name
        /// </summary>
        /// <exception cref="ArgumentException">If the given name is not a Simple Text Literal</exception>
        public string Name
        {
            get => name;
            set
            {
                name = Text.IsSimple(value)
                    ? value
                    : throw new ArgumentException("The value is not a simple text literal", nameof(value));
            }
        }

        /// <summary>
        /// The ASDML Group Constructor parameters
        /// </summary>
        public List<Object> ConstructorParameters { get; } = new List<Object>();

        /// <summary>
        /// The ASDML Group Properties as key-value pairs
        /// </summary>
        public Dictionary<string, Object> Properties { get; } = new Dictionary<string, Object>();

        /// <summary>
        /// The list of ASDML Group nested objects
        /// </summary>
        public List<Object> NestedContent { get; } = new List<Object>();

        /// <summary>
        /// Returns the group itself
        /// </summary>
        public override Group Value => this;

        private Group() : base(null) { }

        /// <summary>
        /// Create a group with the given <paramref name="name"/>
        /// </summary>
        /// <param name="name">The group name. It must be a Simple Text Literal</param>
        /// <exception cref="ArgumentException">If the given name is not a Simple Text Literal</exception>
        /// <returns>A new group instance</returns>
        public Group(string name) : base(null) => Name = name;

        internal static Group CreateRoot() => new Group { ConstructionStep = GroupConstructionStep.Done };

        /// <summary>
        /// Default implementation
        /// </summary>
        /// <returns>Returns the string representation of the group</returns>
        public override string ToString() => GetType().ToString();
    }
}
