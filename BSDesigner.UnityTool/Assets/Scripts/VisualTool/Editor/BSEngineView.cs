using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor
{
    /// <summary>
    /// Represents a behaviour engine structure in the editor view.
    /// </summary>
    public class BSEngineView : VisualElement
    {
        private BSGraphView m_GraphView;

        private VisualElement m_SidePanel;

        /// <summary>
        /// Event called when any property is changed.
        /// </summary>
        public event Action DataChanged;

        public BSEngineView(EditorWindow window)
        {
            this.AddToClassList("row-rev");
            CreateGraphView(window);
            CreateSideInspector();
        }

        /// <summary>
        /// Load an engine in the view. If its a graph, display it in the graph view.
        /// </summary>
        /// <param name="engine">The engine provided.</param>
        public void LoadEngine(BehaviourEngine engine)
        {
            //Display engine in editor inspector
            var graph = engine as BehaviourGraph;
            m_GraphView.LoadGraph(graph);
            m_GraphView.style.display = graph != null ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void CreateGraphView(EditorWindow window)
        {
            m_GraphView = new BSGraphView(window);
            m_GraphView.StretchToParentSize();
            this.Add(m_GraphView);
            m_GraphView.style.display = DisplayStyle.None;
            m_GraphView.DataChanged += GraphView_OnDataChanged;
            m_GraphView.NodeSelectionChanged += GraphView_OnNodeSelectionChanged;
        }

        private void CreateSideInspector()
        {
            m_SidePanel = new VisualElement();
            m_SidePanel.AddToClassList("side-panel");
            this.Add(m_SidePanel);
            m_SidePanel.style.display = DisplayStyle.None;
        }

        private void GraphView_OnDataChanged()
        {
            DataChanged?.Invoke();
            //Update inspector.
        }

        private void GraphView_OnNodeSelectionChanged(IEnumerable<Node> selectedNodes)
        {
            if (selectedNodes != null && selectedNodes.Count() == 1)
            {
                m_SidePanel.style.display = DisplayStyle.Flex;
            }
            else
            {
                m_SidePanel.style.display = DisplayStyle.None;
            }
        }
    }
}
