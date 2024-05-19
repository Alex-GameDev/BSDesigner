using System;

namespace BSDesigner.Core.Attributes
{
    /// <summary>
    /// Attribute that identifies a class as part of a custom group.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class GroupAttribute : Attribute
    {
        public string Name { get; }

        public GroupAttribute(string name)
        {
            this.Name = name;
        }
    }

    /// <summary>
    /// Attribute that gives an alias to a class type name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AliasAttribute : Attribute
    {
        public string Name { get; }

        public AliasAttribute(string name)
        {
            this.Name = name;
        }
    }

    /// <summary>
    /// Attribute that indicates how the type hierarchy should be interpretated below this class type
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HierarchyAttribute : Attribute
    {
        public bool Flat { get; }

        public HierarchyAttribute(bool flat)
        {
            this.Flat = flat;
        }
    }
}
