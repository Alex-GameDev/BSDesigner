using UnityEditor;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Editor
{
    /// <summary>
    /// Configuration for all the elements in the behaviour api package
    /// </summary>
    [FilePath("ProjectSettings/BehaviourAPISettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class VisualToolSettings : ScriptableSingleton<VisualToolSettings>
    {
        private static readonly string k_RootPath = "Assets/Scripts/VisualTool";

        [SerializeField] string RootPath = k_RootPath;

        public string EditorLayoutsPath => RootPath + "/Editor/uxml/";

        public string EditorStylesPath => RootPath + "/Editor/uss/";
    }
}
