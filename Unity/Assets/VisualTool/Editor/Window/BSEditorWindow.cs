using BSDesigner.Unity.VisualTool.Editor.Graphs;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Window
{

    public class BSEditorWindow : EditorWindow
    {
        public Object Object { get; set; }
        public BSData Data { get; set; }

        [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
        [SerializeField] private StyleSheet m_StyleSheet = default;

        private GraphView graphView;

        /// <summary>
        /// Open an editor window with the specified data. If a window with <paramref name="obj"/> 
        /// already exists, focus the window. Otherwise, creates a new window.
        /// </summary>
        /// <param name="obj">The object reference of the element edited.</param>
        /// <param name="data">The data edited.</param>
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

        /// <summary>
        /// Update the system rendered in the window
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="data"></param>
        public void UpdateSystem(Object obj, BSData data)
        {
            Object = obj;
            Data = data;

            if(data != null)
            {
                this.graphView.Update(data.Engines.FirstOrDefault());
            }
        }

        private void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            if (!m_VisualTreeAsset)
            {
                Debug.LogWarning($"Window layout path was not found. Check the path in BehaviourAPISettings section");
                return;
            }

            m_VisualTreeAsset.CloneTree(root);

            var graphDataView = new Graphs.GraphView();
            var graphContainer = rootVisualElement.Q("bw-main");
            graphDataView.StretchToParentSize();
            graphContainer.Insert(0, graphDataView);

            if (m_StyleSheet != null)
            {
                root.styleSheets.Add(m_StyleSheet);
            }
        }

        private void Load(Object obj, BSData data)
        {
            Object = obj;
            Data = data;
        }
    }

}