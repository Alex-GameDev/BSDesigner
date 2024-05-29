using System;
using System.Collections.Generic;
using System.Text;

namespace BSDesigner.Reflection
{
    internal struct TypeRelation
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
