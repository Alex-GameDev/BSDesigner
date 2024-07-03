using BSDesigner.Core;
using BSDesigner.Unity.VisualTool.Editor.Graphs;
using BSDesigner.Unity.VisualTool.Editor.Inspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Window
{
    /// <summary>
    /// View controller that displays an inspector and a graphview for a given behaviour graph.
    /// </summary>
    public class GraphEngineView : IEngineView
    {
        private readonly ISearchMenuProvider searchMenuProvider;

        private GraphView graphView;
        private CustomInspector inspector;
        public event Action DataChanged;

        public GraphEngineView(ISearchMenuProvider searchMenuProvider)
        {
            this.searchMenuProvider = searchMenuProvider;
        }

        public void Clear()
        {
            graphView.ClearGraph();
        }

        public void CreateUI(VisualElement parent)
        {
            this.graphView = new GraphView(this.searchMenuProvider);
            this.graphView.DataChanged += this.DataChanged;
            this.graphView.NodeSelectionChanged += GraphView_SelectionChanged;
            parent.Add(this.graphView);
            this.graphView.StretchToParentSize();

            this.inspector = new CustomInspector(searchMenuProvider);
            this.inspector.style.top = 0;
            this.inspector.style.left = 0;
            this.inspector.style.position = Position.Absolute;
            this.inspector.DataChanged += this.DataChanged;
            parent.Add(this.inspector);
        }

        public void Hide()
        {
            this.graphView.style.display = DisplayStyle.None;
            this.inspector.style.display = DisplayStyle.None;
        }

        public void Show()
        {
            this.graphView.style.display = DisplayStyle.Flex;
            this.inspector.style.display = DisplayStyle.Flex;
        }

        public void Update(BehaviourEngine engine)
        {
            this.graphView?.Update(engine);
        }

        private void GraphView_SelectionChanged(IEnumerable<Node> enumerable)
        {
            this.inspector.Update(enumerable.FirstOrDefault());
        }

    }
}
