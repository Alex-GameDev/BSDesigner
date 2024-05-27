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

            try
            {
                _engines = JsonUtilities.Deserialize(jsonData);
            }
            catch
            {
                Debug.LogError("Error deserializing data: " + this.jsonData);
                //TODO: Serialize data
            }

        }

        public void OnBeforeSerialize()
        {
            if (!m_DirtyFlag) return;

            try
            {
                this.jsonData = JsonUtilities.Serialize(_engines);
            }
            catch 
            {
                Debug.LogError("Error serializing data");
            }
            finally
            {
                m_DirtyFlag = false;
            }
        }
    }
}
