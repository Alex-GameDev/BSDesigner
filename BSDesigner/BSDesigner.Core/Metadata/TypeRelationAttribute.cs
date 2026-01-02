using System;

namespace BSDesigner.Core.Metadata
{
    /// <summary>
    /// Defines an attribute that relates a type to another.
    /// </summary>
    public abstract class TypeRelationAttribute : Attribute
    {
        public Type Type { get; set; }

        public TypeRelationAttribute(Type type)
        {
            this.Type = type;
        }
    }
}
