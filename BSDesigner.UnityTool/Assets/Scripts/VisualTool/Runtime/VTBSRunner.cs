using BSDesigner.Core;
using BSDesigner.Unity.Runtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Runtime
{
    /// <summary>
    /// Behaviour system runner that creates the behaviour system using the visual tool.
    /// </summary>
    public class VTBSRunner : BSRunner
    {
        [SerializeField] private BSData data;

        protected override BehaviourEngine CreateBehaviourSystem()
        {
            var engines = data.Engines;
            ModifySystem(engines);
            return engines.FirstOrDefault();
        }

        /// <summary>
        /// Override this method to modify the system created in the editor.
        /// </summary>
        /// <param name="engines">The list of engines in the system.</param>
        protected virtual void ModifySystem(IReadOnlyList<BehaviourEngine> engines)
        {
        }
    }
}

