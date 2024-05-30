using BSDesigner.Reflection;
using System;

namespace BSDesigner.Unity.VisualTool.Editor.Graphs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeRendererOfTypeAttribute : TypeRelationAttribute
    {
        public NodeRendererOfTypeAttribute(Type type) : base(type)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class GraphRendererOfTypeAttribute : TypeRelationAttribute
    {
        public GraphRendererOfTypeAttribute(Type type) : base(type)
        {
        }
    }
}
