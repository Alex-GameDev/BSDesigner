using BSDesigner.Core;
using BSDesigner.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BSDesigner.Reflection
{
    public abstract class APIMetadata
    {
        // Hierarchies:
        // - Node
        // - BehaviourEngine
        // - Task

        //Associations
        // - GraphRenderer
        // - NodeRenderer

        private Dictionary<Type, TypeHierarchyNode> _typeMap = new Dictionary<Type, TypeHierarchyNode>();

        protected APIMetadata()
        {
            IEnumerable<Type> types = GetTargetAssemblies();


            foreach (Type type in types)
            {
                if (IsValidType(type) && typeof(Node).IsAssignableFrom(type))
                {
                    ProcessType(type);
                }
            }
        }

        public TypeHierarchyNode GetTypeNode(Type type) => _typeMap.GetValueOrDefault(type);

        protected abstract IEnumerable<Type> GetTargetAssemblies();

        private void ProcessType(Type type)
        {
            TypeHierarchyNode node = new TypeHierarchyNode(type);
            this._typeMap.Add(type, node);

            TypeHierarchyNode? parentNode = null;
            type = type.BaseType;

            while (type != null && !this._typeMap.TryGetValue(type, out parentNode))
            {
                parentNode = new TypeHierarchyNode(type);
                this._typeMap.Add(type, parentNode);
                parentNode.AddChild(node);

                type = type.BaseType;
                node = parentNode;
            }

            parentNode?.AddChild(node);

        }

        private bool IsValidType(Type type)
        {
            if (type.IsAbstract || type.IsGenericType) return false;

            foreach (var constructor in type.GetConstructors())
            {
                if (constructor.GetParameters().Length == 0)
                    return true;
            }

            return false;
        }
    }
}
