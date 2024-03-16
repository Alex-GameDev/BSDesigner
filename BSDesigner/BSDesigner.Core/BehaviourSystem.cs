using System.Collections.Generic;
using System.Linq;

namespace BSDesigner.Core
{
    /// <summary>
    /// Behaviour system.
    /// </summary>
    public class BehaviourSystem
    {
        /// <summary>
        /// List of behaviour engines in the system.
        /// </summary>
        public List<BehaviourEngine> engines;

        /// <summary>
        /// Global blackboard of the system
        /// </summary>
        public Blackboard? blackboard;

        /// <summary>
        /// Get the main behaviour engine of the system.
        /// </summary>
        public BehaviourEngine? MainEngine => engines.FirstOrDefault();

        public BehaviourSystem()
        {
            this.engines = new List<BehaviourEngine>();
        }

        /// <summary>
        /// Create a new behaviour system.
        /// </summary>
        /// <param name="engines">List of behaviour engines in the system</param>
        /// <param name="blackboard">Global blackboard of the system</param>
        public BehaviourSystem(List<BehaviourEngine> engines, Blackboard? blackboard)
        {
            this.engines = engines;
            this.blackboard = blackboard;
        }

        /// <summary>
        /// Create a new behaviour system with a single behaviour engine.
        /// </summary>
        /// <param name="engine">The single engine of the system.</param>
        public BehaviourSystem(BehaviourEngine engine)
        {
            this.engines = new List<BehaviourEngine>{ engine };
        }
    }
}