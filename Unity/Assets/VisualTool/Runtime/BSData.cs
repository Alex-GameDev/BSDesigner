using BehaviourDesigner.JsonSerialization;
using BSDesigner.Core;
using System.Collections.Generic;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool
{
    [System.Serializable]
    /// <summary>
    /// Data class that stores a serialized behaviour system.
    /// </summary>
    public class BSData : ISerializationCallbackReceiver
    {
        /// <summary>
        /// The list of unity object referenced in the system as parameters.
        /// </summary>
        [SerializeField] private List<Object> referencedObjects;

        /// <summary>
        /// The behaviour system data serialized in json format.
        /// </summary>
        [SerializeField] private string jsonData;

        /// <summary>
        /// The list of behaviour engines in the system
        /// </summary>
        public List<BehaviourEngine> Engines => _engines;
        List<BehaviourEngine> _engines = new List<BehaviourEngine>();


        bool m_DirtyFlag;

        /// <summary>
        /// Enable the serialization after a change.
        /// </summary>
        public void SetDirty() => m_DirtyFlag = true;

        public void OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(jsonData)) return;

            Debug.Log("Deserialize data: " + this.jsonData);
            //TODO: Serialize data

            _engines = JsonUtilities.Deserialize(jsonData); 
        }

        public void OnBeforeSerialize()
        {
            if (!m_DirtyFlag) return;

            Debug.Log("Serialize data: " + this.jsonData);
            this.jsonData = JsonUtilities.Serialize(_engines);
            m_DirtyFlag = false;
        }
    }
}
