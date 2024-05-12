using UnityEditor;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor
{
    [CustomEditor(typeof(BSVisualPrototype))]
    public class VTBSPrototypeEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open"))
            {
                var bsPrototype = (BSVisualPrototype)target;
                BSEditorWindow.Open(bsPrototype, bsPrototype.Data);
            }
        }
    }
}
