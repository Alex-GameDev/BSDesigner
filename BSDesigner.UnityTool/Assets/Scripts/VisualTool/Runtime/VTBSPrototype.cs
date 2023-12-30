using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core;
using BSDesigner.Unity.Runtime;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool.Runtime
{
    /// <summary>
    /// Behaviour system prototype that can be created using the visual tool.
    /// </summary>
    [CreateAssetMenu(menuName = "BSDesigner/VisualTool/Prototype", fileName = "NewBSPrototype")]
    public class VTBSPrototype : BSPrototype
    {
        [SerializeField] private BSData data;

        public BSData Data => data;

        public override BehaviourEngine CreateBehaviourSystem()
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