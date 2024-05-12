using BSDesigner.Core;
using System.Collections.Generic;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool
{
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
            //TODO: Deserialize data
        }

        public void OnBeforeSerialize()
        {
            if (!m_DirtyFlag) return;

            //TODO: Serialize data

            m_DirtyFlag = false;
        }
    }
}
