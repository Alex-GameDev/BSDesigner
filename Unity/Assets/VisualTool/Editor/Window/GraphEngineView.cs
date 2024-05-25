using BSDesigner.Core;
using BSDesigner.Unity.VisualTool.Editor.Graphs;
using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Window
{
    /// <summary>
    /// View controller that displays an inspector and a graphview for a given behaviour graph.
    /// </summary>
    public class GraphEngineView : IEngineView
    {
        private GraphView graphView;
        
        public event Action DataChanged;

        public void Clear()
        {
            graphView.Clear();
        }

        public void CreateUI(VisualElement parent)
        {
            this.graphView = new GraphView();
            parent.Add(this.graphView);
            this.graphView.StretchToParentSize();
        }

        public void Hide()
        {
            this.graphView.style.display = DisplayStyle.None;
        }

        public void Show()
        {
            this.graphView.style.display = DisplayStyle.Flex;
        }

        public void Update(BehaviourEngine engine)
        {
            this.graphView?.Update(engine);
        }
    }
}
