using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor.Graphs
{
    /// <summary>
    /// Generic Edge connector listener class.
    /// </summary>
    /// <typeparam name="TEdge">The type of the edges that t</typeparam>
    internal class CustomEdgeConnectorListener<TEdge> : IEdgeConnectorListener where TEdge : Edge
    {
        private readonly Action<TEdge> m_callback = null;
        private readonly Action<TEdge, Vector2> m_callbackOutsidePort = null;

        public CustomEdgeConnectorListener(System.Action<TEdge> callback, System.Action<TEdge, Vector2> callbackOutsidePort = null)
        {
            m_callback = callback;
            m_callbackOutsidePort = callbackOutsidePort;
        }

        public void OnDrop(UnityEditor.Experimental.GraphView.GraphView _, Edge edge) => m_callback?.Invoke(edge as TEdge);

        public void OnDropOutsidePort(Edge edge, Vector2 position) => m_callbackOutsidePort?.Invoke(edge as TEdge, position);
    }
}
