using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Graphs
{
    public class PortView : Port
    {
        public PortView(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type, IEdgeConnectorListener connector) :
                   base(portOrientation, portDirection, portCapacity, type)
        {
            this.Q("type").style.display = DisplayStyle.None;
            m_EdgeConnector = new EdgeConnector<EdgeView>(connector);
            this.AddManipulator(m_EdgeConnector);
            portName = string.Empty;
        }

        public Vector2 ConnectionDirection { get; set; }
    }
}
