using BSDesigner.Core;
using BSDesigner.Unity.VisualTool.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public class BSEditorWindow : EditorWindow
    {
        public Object Object { get; set; }
        public BSData Data { get; set; }

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

        private void Load(Object obj, BSData data)
        {
            Object = obj;
            Data = data;
            DrawGraph();
        }

        private void DrawGraph()
        {
            m_GraphView = new BSGraphView();
            m_GraphView.StretchToParentSize();
            rootVisualElement.Add(m_GraphView);
        }
    }
}