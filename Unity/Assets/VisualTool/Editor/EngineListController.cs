using BSDesigner.Core;
using BSDesigner.Unity.VisualTool.Editor.Assets.VisualTool.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private ScrollView engineListView;

        private List<BehaviourEngine> engineList = new List<BehaviourEngine>();

        private BehaviourEngine selectedEngine;

        public void Clear()
        {
            this.engineList = null;
            this.selectedEngine = null;
            this.engineList.Clear();
        }

        public void Update(List<BehaviourEngine> engines)
        {
            this.engineList = engines;
            this.RefreshList();

            this.selectedEngine = engineList.FirstOrDefault();
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
            element.Q<Button>("bw-enginelist-delete-btn").clicked += HandleDeleteBtnClick;

            this.engineListView = element.Q<ScrollView>("bw-enginelist-content");

        }
        private void HandleSelectedEngine(BehaviourEngine engine)
        {
            if(engine != null)
            {
                this.EngineSelected?.Invoke(engine);
                this.selectedEngine = engine;
            }
        }


        private void HandleAddBtnClick()
        {
            this.creationView.Show();
        }


        private void HandleDeleteBtnClick()
        {
            if(this.selectedEngine != null && EditorUtility.DisplayDialog("Delete engine","¿Are you sure to delete the selected engine?", "OK"))
            {
                this.engineList.Remove(this.selectedEngine);
                this.EngineRemoved?.Invoke(selectedEngine);
                this.RefreshList();

                this.selectedEngine = engineList.FirstOrDefault();
                this.EngineSelected?.Invoke(this.selectedEngine);
            }
        }

        private void HandleCreateBehaviourEngine(Type engineType, string name)
        {
            if(engineType != null && !string.IsNullOrEmpty(name))
            {
                //1. Create engine
                var engine = (BehaviourEngine)Activator.CreateInstance(engineType);
                if(engine != null)
                {
                    engine.Name = name;
                    //2. Add engine to the system
                    this.engineList.Add(engine);
                    this.EngineAdded?.Invoke(engine);
                    this.RefreshList();

                    this.selectedEngine = engine;
                    this.EngineSelected?.Invoke(this.selectedEngine);
                }
            }
        }

        private void RefreshList()
        {
            this.engineListView.Clear();
            foreach (var engine in engineList)
            {
                var item = new BSEngineIconView();
                item.SetEngine(engine);
                item.OnClick += HandleSelectedEngine;
                this.engineListView.Add(item);
            }
        }


        private class BSEngineIconView : VisualElement
        {
            private readonly Button btnIcon;

            public event Action<BehaviourEngine> OnClick;

            private BehaviourEngine engine;
            public BSEngineIconView()
            {
                var treeView = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ToolSettings.instance.LayoutPath + "/bsengineicon.uxml");
                treeView.CloneTree(this);

                this.btnIcon = this.Q<Button>("bs-engineicon-btn");
                this.btnIcon.clicked +=  () => OnClick?.Invoke(engine);
            }

            public void SetEngine(BehaviourEngine engine)
            {
                this.engine = engine;
                string iconName = $"/{engine.GetType().Name.ToLower()}";
                var iconImg = AssetDatabase.LoadAssetAtPath<Texture2D>(ToolSettings.instance.IconPath + iconName + ".png");
                btnIcon.style.backgroundImage = iconImg;
            }
        }
    }
}
