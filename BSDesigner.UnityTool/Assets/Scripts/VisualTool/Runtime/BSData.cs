using System.Collections.Generic;
using BSDesigner.Core;
using BSDesigner.JsonSerialization;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Runtime
{
    /// <summary>
    /// Data class that stores a serialized behaviour system.
    /// </summary>
    [System.Serializable]
    public class BSData : ISerializationCallbackReceiver
    {
        /// <summary>
        /// The behaviour system data serialized in json format.
        /// </summary>
        [SerializeField] private string jsonData;

        /// <summary>
        /// The list of unity object referenced in the system as parameters.
        /// </summary>
        [SerializeField] private List<UnityEngine.Object> referencedObjects;

        /// <summary>
        /// The list of engines in the system
        /// </summary>
        private List<BehaviourEngine> _engines = new List<BehaviourEngine>();

        /// <summary>
        /// The list of behaviour engines in the system
        /// </summary>
        public List<BehaviourEngine> Engines => _engines;

        private bool m_DirtyFlag;

        /// <summary>
        /// Enable the serialization after a change.
        /// </summary>
        public void SetDirty() => m_DirtyFlag = true;

        public void OnBeforeSerialize()
        {
            if(!m_DirtyFlag) return;

            Debug.Log("Serialize");

            var settings = new JsonSettings();
            settings.AddReferenceConverter(ref referencedObjects);
            jsonData = JsonUtilities.Serialize(_engines, settings);

            m_DirtyFlag = false;
        }

        public void OnAfterDeserialize()
        {
            if(string.IsNullOrEmpty(jsonData)) return;

            Debug.Log("Deserialize: " + jsonData);

            var settings = new JsonSettings();
            settings.AddReferenceConverter<Object>(ref referencedObjects);
            _engines = JsonUtilities.Deserialize(jsonData, settings);
        }
    }
}
