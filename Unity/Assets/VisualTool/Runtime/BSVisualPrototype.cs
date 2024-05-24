using BSDesigner.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BSDesigner.Unity.VisualTool
{
    /// <summary>
    /// Behaviour system prototype that can be created using the visual tool.
    /// </summary>
    [CreateAssetMenu(menuName = "BSDesigner/VisualTool/Prototype", fileName = "NewBSPrototype")]
    public class BSVisualPrototype : BSPrototype
    {
        [SerializeField] private BSData data = new BSData();

        /// <summary>
        /// The behaviour system data of the asset (for the editor)
        /// </summary>
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

        #region Test


#if UNITY_EDITOR

        [ContextMenu("Add BT")]
        public void CreateBT()
        {
            var bt = new BehaviourTrees.BehaviourTree();
            data.Engines.Add(bt);
            data.SetDirty();
        }

        [ContextMenu("Debug")]
        public void DebugInfo()
        {
            var sb = new StringBuilder();
            foreach (var engine in data.Engines)
            {
                sb.AppendLine($"ENGINE: {engine.Name} ({engine.GetType().Name})");
            }
            Debug.Log(sb.ToString());
        }
#endif

        #endregion
    }
}
