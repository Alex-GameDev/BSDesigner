using System;
using System.Collections.Generic;
using System.Linq;

namespace BSDesigner.Core.Metadata
{
    public abstract class APIMetadata
    {
        private Dictionary<Type, TypeHierarchyNode> _typeMap = new Dictionary<Type, TypeHierarchyNode>();

        private Dictionary<Type, TypeRelation> _relatedTypes = new Dictionary<Type, TypeRelation>();

        private Dictionary<TypeRelation, Type> _concreteRelatedTypes = new Dictionary<TypeRelation, Type>();

        protected APIMetadata()
        {
            Initialize();
            IEnumerable<Type> types = GetTargetAssemblies();
            var requiredRoots = GetRequiredRootTypes();

            foreach (Type type in types)
            {
                if (IsValidType(type) && requiredRoots.Any(t => t.IsAssignableFrom(type)))
                {
                    ProcessRelationAttributes(type);
                    ProcessType(type);
                }
            }
        }

        private void ProcessRelationAttributes(Type type)
        {
            foreach (var attribute in type.GetCustomAttributes(true))
            {
                if (_relatedTypes.TryGetValue(attribute.GetType(), out var relatedTypes))
                {
                    var typedAttribute = (TypeRelationAttribute)attribute;
                    if (relatedTypes.targetType.IsAssignableFrom(typedAttribute.Type))
                    {
                        var typeRelation = new TypeRelation(typedAttribute.Type, relatedTypes.SourceType);
                        _concreteRelatedTypes.Add(typeRelation, type);
                    }
                }
            }
        }

        protected abstract void Initialize();

        public TypeHierarchyNode GetTypeNode(Type type) => _typeMap.GetValueOrDefault(type);

        protected abstract IEnumerable<Type> GetTargetAssemblies();

        protected abstract HashSet<Type> GetRequiredRootTypes();

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

        /// <summary>
        /// Add a type relation attribute to the metadata generation. The source and target types must be
        /// included in required root types.
        /// </summary>
        /// <typeparam name="T">The type of relation attribute</typeparam>
        /// <param name="targetType">The type that the attribute shold have targeted as a parameter.<param>
        /// <param name="sourceType">The type of the annotated class.<param>
        protected void RegisterTargetType<T>(Type sourceType,  Type targetType) where T : TypeRelationAttribute
        {
            _relatedTypes.Add(typeof(T), new TypeRelation(sourceType, targetType));
        }

        public Type? GetRelatedTypeof<T>(Type type)
        {
            Type? relatedType = null;
            while(type != null && relatedType == null)
            {
                if(!this._concreteRelatedTypes.TryGetValue(new TypeRelation(type, typeof(T)), out relatedType))
                {
                    type = type.BaseType;
                }
            }
            return relatedType;
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

        private struct TypeRelation
        {
            public Type SourceType { get; }
            public Type targetType { get; }

            public TypeRelation(Type sourceType, Type targetType)
            {
                this.SourceType = sourceType;
                this.targetType = targetType;
            }

            public override int GetHashCode()
            {
                return this.SourceType.GetHashCode() ^ this.targetType.GetHashCode();
            }
        }
    }
}
