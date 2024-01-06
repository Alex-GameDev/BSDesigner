using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEditorInternal;
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
            using (var scope = CreateSettingWindowGUIScope())
            {
                m_SerializedObject.Update();
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.LabelField("General", EditorStyles.boldLabel);

                var prop = m_SerializedObject.FindProperty("assemblies");

                for (var i = 0; i < prop.arraySize; i++)
                {
                    var element = prop.GetArrayElementAtIndex(i);
                    element.objectReferenceValue = EditorGUILayout.ObjectField(element.objectReferenceValue, typeof(AssemblyDefinitionAsset), false);
                }

                if (GUILayout.Button("Add assembly"))
                {
                    prop.InsertArrayElementAtIndex(prop.arraySize);
                }

                if (GUILayout.Button("Remove assembly"))
                {
                    prop.DeleteArrayElementAtIndex(prop.arraySize - 1);
                }

                EditorGUILayout.Space(10f);
                EditorGUILayout.LabelField("Debug UI", EditorStyles.boldLabel);
                var debugUIProp = m_SerializedObject.FindProperty("_debugUI");
                debugUIProp.boolValue = EditorGUILayout.Toggle("Enable debug", debugUIProp.boolValue);

                if (EditorGUI.EndChangeCheck())
                {
                    m_SerializedObject.ApplyModifiedProperties();
                    VisualToolSettings.instance.Save();
                }
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
