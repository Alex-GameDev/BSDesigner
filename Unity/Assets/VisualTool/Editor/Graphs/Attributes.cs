using System;

namespace BSDesigner.Unity.VisualTool.Editor.Assets.VisualTool.Editor.GraphView
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeRendererAttribute : Attribute
    {
        public Type NodeType { get; set; }

        public NodeRendererAttribute(Type nodeType)
        {
            NodeType = nodeType;
        }
    }
}
