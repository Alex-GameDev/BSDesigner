using System.Reflection;
using BSDesigner.Core;
using BSDesigner.Unity.VisualTool.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UTILS = BSDesigner.Unity.VisualTool.Editor.EditorUtilities;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public class BSEditorWindow : EditorWindow
    {
        public Object Object { get; set; }
        public BSData Data { get; set; }

        private BSEngineListView m_EngineListView;

        private BSGraphView m_GraphView;


        public static void Open(Object obj, BSData data)
        {
            var windows = Resources.FindObjectsOfTypeAll<BSEditorWindow>();
            foreach (var w in windows)
            {
                if (w.Object == obj)
                {
                    w.Focus();
                    return;
                }
            }

            var window = CreateWindow<BSEditorWindow>(typeof(BSEditorWindow), typeof(SceneView));
            window.titleContent = new GUIContent($"{obj.name}");
            window.Load(obj, data);
        }

        void CreateGUI()
        {
            var stylePath = VisualToolSettings.instance.EditorStylesPath + "BSEditorWindow.uss";
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(stylePath);
            rootVisualElement.styleSheets.Add(styleSheet);

            CreateEngineDetailsPanel();
            CreateGraphView();
            CreateToolbar();
            CreateSidePanel();
        }

        private void Load(Object obj, BSData data)
        {
            Object = obj;
            Data = data;
            UTILS.LOG("BW - Load data.");
            m_EngineListView.Populate(Data);
            ChangeSelectedEngineInSystem(data.Engines.Count > 0 ? 0 : -1);
        }

        private void CreateToolbar()
        {
            var toolbar = new Toolbar();
            rootVisualElement.Add(toolbar);
        }

        private void CreateSidePanel()
        {
            var element = new VisualElement();
            element.name = "side-panel";
            rootVisualElement.Add(element);

            m_EngineListView = new BSEngineListView();
            m_EngineListView.name = "engine-list";
            m_EngineListView.EngineAdded += AddEngineToSystem;
            m_EngineListView.EngineRemoved += RemoveEngineToSystem;
            m_EngineListView.SystemClean += ClearAllEnginesInSystem;

            element.Add(m_EngineListView);
        }

        private void CreateEngineDetailsPanel()
        {
            var element = new VisualElement();
            element.StretchToParentSize();
            element.AddToClassList("bg-panel");

            var label = new Label("No representation");
            element.Add(label);
            label.AddToClassList("bg-panel-text");

            rootVisualElement.Add(element);
        }

        private void CreateGraphView()
        {
            m_GraphView = new BSGraphView(this);
            m_GraphView.StretchToParentSize();
            rootVisualElement.Add(m_GraphView);
            m_GraphView.style.display = DisplayStyle.None;
            m_GraphView.DataChanged += GraphView_OnDataChanged;
        }

        #region Change Data

        private void AddEngineToSystem(BehaviourEngine engine)
        {
            Data.Engines.Add(engine);
            int newIndex = Data.Engines.Count - 1;
            UTILS.LOG($"BW - Add new engine {engine.GetType().Name}");

            ChangeSelectedEngineInSystem(newIndex);
            SaveDataChanges();
        }

        private void RemoveEngineToSystem(int index)
        {
            Data.Engines.RemoveAt(index);

            int newIndex = Data.Engines.Count > 0 ? Mathf.Max(index - 1, 0) : -1;
            UTILS.LOG($"BW - Remove engine at position {index}");

            ChangeSelectedEngineInSystem(newIndex);
            SaveDataChanges();
        }

        private void ClearAllEnginesInSystem()
        {
            Data.Engines.Clear();

            UTILS.LOG($"BW - All engines removed");

            ChangeSelectedEngineInSystem(-1);
            SaveDataChanges();
        }

        private void GraphView_OnDataChanged()
        {
            UTILS.LOG($"BW - Data changed");
            SaveDataChanges();
        }

        #endregion

        private void ChangeSelectedEngineInSystem(int engineIndex)
        {
            UTILS.LOG($"BW - Change selected engine to element at index {engineIndex}");
            var engine = (Data != null && engineIndex >= 0) ? Data.Engines[engineIndex] : null;
            LoadEngineView(engine);
        }

        private void LoadEngineView(BehaviourEngine engine)
        {
            //Display engine in editor inspector
            var graph = engine as BehaviourGraph;
            m_GraphView.LoadGraph(graph);
            m_GraphView.style.display = graph != null ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void SaveDataChanges()
        {
            Data.SetDirty();
            EditorUtility.SetDirty(Object);
        }
    }
}