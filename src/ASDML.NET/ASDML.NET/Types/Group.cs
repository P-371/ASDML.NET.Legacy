using System;
using System.Collections.Generic;

namespace P371.ASDML.Types
{
    public class Group : Object<Group>
    {
        private string name;

        internal GroupConstructionStep ConstructionStep = GroupConstructionStep.NameDone;

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

        public List<Object> ConstructorParameters { get; } = new List<Object>();

        public Dictionary<string, Object> Properties { get; } = new Dictionary<string, Object>();

        public List<Object> NestedContent { get; } = new List<Object>();

        public override Group Value => this;

        public Group(string name) : base(null) => Name = name;

        public override string ToString() => GetType().ToString();
    }
}
