using System;

namespace BSDesigner.Core.Attributes
{
    /// <summary>
    /// Specify a group for an element
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GroupAttribute
    {
        /// <summary>
        /// Name of the group
        /// </summary>
        public string Name { get; set; }

        public GroupAttribute(string name)
        {
            Name = name;
        }
    }
}