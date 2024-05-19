
using BSDesigner.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BSDesigner.Reflection
{
    /// <summary>
    /// Defines an element of the domain type hierarchy
    /// </summary>
    public class TypeHierarchyNode
    {
        /// <summary>
        /// The name of the node
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The assigned type of the node
        /// </summary>
        public Type? Type { get; }

        /// <summary>
        /// List of nested type nodes.
        /// </summary>
        public IReadOnlyList<TypeHierarchyNode> SubTypes => subTypes;

        private List<TypeHierarchyNode> subTypes = new List<TypeHierarchyNode>();
        public TypeHierarchyNode(Type type)
        {
            Name = type.GetCustomAttribute<AliasAttribute>()?.Name ?? type.Name;
            Type = type;
        }

        public TypeHierarchyNode(string name)
        {
            Name = name;
        }

        public void AddChild(TypeHierarchyNode node)
        {
            this.subTypes.Add(node);
        }

        public override string ToString()
        {
            return $"{Name}\n{string.Join(", ", this.subTypes.Select(s => s.ToString()))}";
        }
    }
}
