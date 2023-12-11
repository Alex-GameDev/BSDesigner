using System;
using BSDesigner.Core;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Base node for utility systems.
    /// </summary>
    public abstract class UtilityNode : Node
    {
        public override Type GraphType => typeof(UtilitySystem);

        /// <summary>
        /// If true, utility system update will recalculate <see cref="Utility"/>. If false, <see cref="Utility"/> can only be recalculate calling <see cref="UpdateUtility"/> manually.
        /// </summary>
        public bool PullingEnabled = true;

        /// <summary>
        /// The utility Value of the node.
        /// </summary>
        public float Utility { get; protected set; }

        bool _needToRecalculateUtility;

        public void UpdateUtility(bool forceRecalculate = false)
        {
            if (_needToRecalculateUtility || forceRecalculate)
            {
                Utility = GetUtility();
                _needToRecalculateUtility = false;
            }
        }

        /// <summary>
        /// If this node has <see cref="PullingEnabled"/> flag to true, indicates that the <see cref="Utility"/> has to be recalculated.
        /// </summary>
        public void MarkUtilityAsDirty()
        {
            if (PullingEnabled) _needToRecalculateUtility = true;
        }

        /// <summary>
        /// Compute the utility of this node.
        /// </summary>
        /// <returns>The updated utility Value of this node.</returns>
        protected abstract float GetUtility();
    }
}