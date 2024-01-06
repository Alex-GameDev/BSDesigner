using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
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

        #region Serialized fields

        [SerializeField] private string _rootPath = k_RootPath;

        [SerializeField] private List<AssemblyDefinitionAsset> assemblies;

        [SerializeField] private bool _debugUI;

        #endregion

        #region Public properties

        /// <summary>
        /// The path to the folder that contains the layout definition files (.uxml)
        /// </summary>
        public string EditorLayoutsPath => _rootPath + "/Editor/uxml/";

        /// <summary>
        /// The path to the folder that contains the style definition files (.uss)
        /// </summary>
        public string EditorStylesPath => _rootPath + "/Editor/uss/";

        /// <summary>
        /// Is the UI log messages enabled?
        /// </summary>
        public bool DebugUI => _debugUI;

        #endregion

        public void Save() => Save(true);

        public Assembly[] GetIncludedAssemblies()
        {
            var assemblyNameSet = assemblies.Select(a => a.name).ToHashSet();

            var userAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => assemblyNameSet.Contains(a.GetName().Name));
           
            return userAssemblies.ToArray();
        }
    }
}
