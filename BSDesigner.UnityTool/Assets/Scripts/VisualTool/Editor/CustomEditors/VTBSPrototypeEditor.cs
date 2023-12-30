using System.Collections;
using System.Collections.Generic;
using BSDesigner.Unity.Runtime;
using BSDesigner.Unity.VisualTool.Runtime;
using UnityEditor;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor
{
    [CustomEditor(typeof(VTBSPrototype))]
    public class VTBSPrototypeEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open"))
            {
                var bsPrototype = (VTBSPrototype)target;
                BSEditorWindow.Open(bsPrototype, bsPrototype.Data);
            }
        }
    }

}