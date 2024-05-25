using BSDesigner.Core;
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


        private IEngineView engineView;
        private IEngineListView engineListView;

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

        private void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            if (!m_VisualTreeAsset)
            {
                Debug.LogWarning($"Window layout path was not found. Check the path in BehaviourAPISettings section");
                return;
            }

            m_VisualTreeAsset.CloneTree(root);

            var main = root.Q("bw-main");

            var dialogDisplay = root.Q("bw-dialog");
            ///
            engineListView = new EngineListController();
            engineListView.CreateUI(main, dialogDisplay);
            engineListView.EngineSelected += EngineListView_EngineSelected;

            engineView = new GenericEngineView();
            engineView.CreateUI(main);
            ///

            if (m_StyleSheet != null)
            {
                root.styleSheets.Add(m_StyleSheet);
            }

            ToolMetadata metadata = ToolMetadata.Instance;
            Debug.Log("Loaded metadata");
        }

        #region UI Events

        private void EngineListView_EngineSelected(BehaviourEngine engine)
        {
            this.engineView.Update(engine);
        }

        #endregion

        #region Data events

        private void Load(Object obj, BSData data)
        {
            Object = obj;
            Data = data;

            //1.    Carga la lista de engines
            this.engineListView.Update(data.Engines);
            //1.1.  Muestra el blackboard

            //2.    Muestra el engine seleccionado (First or default)
            this.engineView.Update(data.Engines.FirstOrDefault());
        }


        private void RegisterUserAction(string actionName)
        {
            if (this.Object == null) return;

            Undo.RegisterCompleteObjectUndo(this.Object, actionName);
        }

        private void SaveChanges()
        {
            EditorUtility.SetDirty(this.Object);
        } 

        #endregion
    }
}