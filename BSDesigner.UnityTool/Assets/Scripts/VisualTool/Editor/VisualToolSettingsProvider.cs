using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor
{
    public class VisualToolSettingsProvider : SettingsProvider
    {
        SerializedObject m_SerializedObject;

        public VisualToolSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            VisualToolSettings.instance.Save();
            m_SerializedObject = new SerializedObject(VisualToolSettings.instance);
        }

        public override void OnGUI(string searchContext)
        {
            using (CreateSettingWindowGUIScope())
            {
                EditorGUILayout.LabelField("General", EditorStyles.boldLabel);
            }

        }

        [SettingsProvider]
        public static SettingsProvider CreateMySingletonProvider()
        {
            var provider = new VisualToolSettingsProvider("Project/BSDesigner visual tool settings", SettingsScope.Project, GetSearchKeywordsFromGUIContentProperties<Styles>());
            return provider;
        }

        private IDisposable CreateSettingWindowGUIScope()
        {
            var unityEditorAssembly = Assembly.GetAssembly(typeof(EditorWindow));
            var type = unityEditorAssembly.GetType("UnityEditor.SettingsWindow+GUIScope");
            return Activator.CreateInstance(type) as IDisposable;
        }
    }
}
