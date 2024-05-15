using BSDesigner.Unity.VisualTool;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BSEditorWindow : EditorWindow
{
    public Object Object { get; set; }
    public BSData Data { get; set; }

    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
    [SerializeField] private StyleSheet m_StyleSheet = default;

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

        if (m_StyleSheet != null)
        {
            root.styleSheets.Add(m_StyleSheet);
        }

        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }

    private void Load(Object obj, BSData data)
    {
        Object = obj;
        Data = data;
    }
}
