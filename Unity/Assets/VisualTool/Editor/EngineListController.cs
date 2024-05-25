using BSDesigner.Core;
using BSDesigner.Unity.VisualTool.Editor.Assets.VisualTool.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public class EngineListController : IEngineListView
    {
        public event Action DataChanged;

        public event Action<BehaviourEngine> EngineSelected;
        public event Action<BehaviourEngine> EngineAdded;
        public event Action<BehaviourEngine> EngineRemoved;

        private EngineCreationView creationView;

        public void Clear()
        {
           
        }

        public void Update(List<BehaviourEngine> engine)
        {
           
        }

        public void CreateUI(VisualElement parent, VisualElement dialogDisplay)
        {
            VisualTreeAsset asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{ToolSettings.instance.LayoutPath}/bsenginelist.uxml");
            var element = asset.Instantiate();
            parent.Add(element);

            this.creationView = new EngineCreationView(HandleCreateBehaviourEngine);
            this.creationView.StretchToParentSize();
            this.creationView.Hide();
            dialogDisplay.Add(creationView);

            element.Q<Button>("bw-enginelist-add-btn").clicked += HandleAddBtnClick;
        }

        private void HandleAddBtnClick()
        {
            this.creationView.Show();
        }

        private void HandleCreateBehaviourEngine(Type engineType, string name)
        {
            if(engineType != null && !string.IsNullOrEmpty(name))
            {
                //1. Create engine
                var engine = (BehaviourEngine)Activator.CreateInstance(engineType);
                engine.Name = name;

                //2. Add engine to the system
                this.EngineAdded?.Invoke(engine);
            }
            else
            {

            }

        }

    }
}
