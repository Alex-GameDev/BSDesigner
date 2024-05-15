using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine.UIElements;

namespace BSDesigner.Unity.VisualTool.Editor.Assets.VisualTool.Editor.Window
{
    [FilePath("ProjectSettings/BSDesignerSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class ToolSettings: ScriptableSingleton<ToolSettings>
    {
        private static readonly string k_RootPath = "Assets/Scripts/BSDesigner";

        internal void Save() => this.Save(true);
    }

    public class ToolSettingsProvider : SettingsProvider
    {
        SerializedObject m_SerializedObject;

        public ToolSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) :
            base(path, scopes, keywords)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            ToolSettings.instance.Save();
            m_SerializedObject = new SerializedObject(ToolSettings.instance);
        }

        public override void OnGUI(string searchContext)
        {
            using (var scope = CreateSettingWindowGUIScope())
            {
                m_SerializedObject.Update();
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
                if (EditorGUI.EndChangeCheck())
                {
                    m_SerializedObject.ApplyModifiedProperties();
                    ToolSettings.instance.Save();
                }
            }

        }

        [SettingsProvider]
        public static SettingsProvider CreateMySingletonProvider()
        {
            var provider = new ToolSettingsProvider("Project/BSDesigner", SettingsScope.Project, GetSearchKeywordsFromGUIContentProperties<Styles>());
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
