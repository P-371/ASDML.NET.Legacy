namespace P371.ASDML.Types.Helpers
{
    /// <summary>
    /// Holds the ID of the referenced group
    /// </summary>
    public class GroupReference : Object<GroupReference>
    {
        /// <summary>
        /// The group ID to reference
        /// </summary>
        public SimpleText GroupID { get; internal set; }

        /// <summary>
        /// Returns the group reference itself
        /// </summary>
        public override GroupReference Value => this;

        /// <summary>
        /// Creates a reference to a group
        /// </summary>
        /// <param name="id">The ID of the referenced group</param>
        public GroupReference(SimpleText id) : base(null) => GroupID = id;

        /// <summary>
        /// Returns the referenced group ID
        /// </summary>
        /// <returns>Returns the ID of the referenced group prefixed with #</returns>
        public override string ToString() => "#" + GroupID;
    }
}
