using BSDesigner.Core;
using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Window
{
    public class GenericEngineView : IEngineView
    {
        public event Action DataChanged;

        private IEngineView currentView;

        private IEngineView graphView;

        private VisualElement content;

        public void CreateUI(VisualElement parent)
        {
            var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ToolSettings.instance.LayoutPath + "/bsengineview.uxml");
            asset.CloneTree(parent);
            this.content = parent.Q("bw-engineview-content");
            this.graphView = new GraphEngineView();
            this.graphView.CreateUI(this.content);

            this.graphView.Hide();
        }

        /// <summary>
        /// Changes the engine that is loaded in the view
        /// </summary>
        /// <param name="engine">The new rendered engine</param>
        public void Update(BehaviourEngine engine)
        {
            this.currentView?.Clear();
            var selectedView = GetViewByEngine(engine);
            if (selectedView != this.currentView)
            {
                this.currentView?.Hide();
                this.currentView = selectedView;
                this.currentView?.Show();
            }
            this.currentView?.Update(engine);
        }


        public void Clear()
        {
            this.currentView.Clear();
            this.currentView = null;
        }

        private IEngineView GetViewByEngine(BehaviourEngine engine)
        {
            return engine is BehaviourGraph ? graphView : null;
        }

        public void Hide() => content.style.display = DisplayStyle.None;

        public void Show() => content.style.display = DisplayStyle.None;
    }
}
