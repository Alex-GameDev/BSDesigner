using System;

namespace BSDesigner.Core.Attributes
{
    /// <summary>
    /// Attribute that specifies a desciption for a class type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; }

        public DescriptionAttribute(string description)
        {
            this.Description = description;
        }
    }
}
