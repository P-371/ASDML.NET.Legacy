using System;
using System.Collections.Generic;

namespace P371.ASDML.Types
{
    public class Group : Object<Group>
    {
        private string name;

        public string Name
        {
            get => name;
            set
            {
                name = Text.IsSimple(text: value)
                    ? value
                    : throw new ArgumentException(message: "The value is not a simple text literal", paramName: nameof(value));
            }
        }

        public Dictionary<string, Object> Properties { get; } = new Dictionary<string, Object>();

        public Dictionary<string, Object> NestedContent { get; } = new Dictionary<string, Object>();

        public override Group Value => this;

        public Group(string name) : base(null)
        {
            Name = name;
        }
    }
}
