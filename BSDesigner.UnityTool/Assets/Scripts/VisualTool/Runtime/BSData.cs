using System.Collections.Generic;
using BSDesigner.Core;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Runtime
{
    /// <summary>
    /// Data class that stores a serialized behaviour system.
    /// </summary>
    [System.Serializable]
    public struct BSData : ISerializationCallbackReceiver
    {
        [SerializeField] private string jsonData;

        [SerializeField] private List<UnityEngine.Object> referencedObjects;

        private List<BehaviourEngine> _engines;

        /// <summary>
        /// The list of behaviour engines in the system
        /// </summary>
        public IReadOnlyList<BehaviourEngine> Engines => _engines;

        private bool dirtyFlag;

        public void OnBeforeSerialize()
        {
            if(!dirtyFlag) return;
        }

        public void OnAfterDeserialize()
        {
            _engines?.Clear();
        }
    }
}
