using BSDesigner.Unity.VisualTool.Editor.Assets.VisualTool.Editor.Graphs;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Graphs
{
    internal class PortView : Port
    {
        public PortView(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type, IEdgeConnectorListener connector) :
                   base(portOrientation, portDirection, portCapacity, type)
        {
            m_EdgeConnector = new EdgeConnector<EdgeView>(connector);
            this.AddManipulator(m_EdgeConnector);
            portName = string.Empty;
        }

        public Vector2 ConnectionDirection { get; set; }
    }
}
