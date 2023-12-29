using BSDesigner.Core;
using UnityEngine;

namespace BSDesigner.Unity.Runtime
{
    /// <summary>
    /// Base class to implement reusable asset-stored behaviour systems.
    /// </summary>
    public abstract class BSPrototype : ScriptableObject
    {
        /// <summary>
        /// Create a behaviour system.
        /// </summary>
        /// <returns>The main engine of the system created.</returns>
        public abstract BehaviourEngine CreateBehaviourSystem();
    }
}
