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
        /// If true, utility system update will recalculate <see cref="Utility"/> otherwise <see cref="Utility"/> can only be recalculate calling <see cref="UpdateUtility"/> manually.
        /// </summary>
        public bool EnableUtilityUpdating = true;

        /// <summary>
        /// The utility Value of the node.
        /// </summary>
        public float Utility { get; protected set; }

        bool _needToRecalculateUtility;

        /// <summary>
        /// Update the utility value for this element. When the utility is updated, a flag is raised to avoid it from being recalculated
        /// more than once in the same frame. This flag is reset calling MarkUtilityAsDirty.
        /// </summary>
        /// <param name="forceRecalculate">If the utility will be recalculated event if pulling is disabled and utility was already computed this frame</param>
        public void UpdateUtility(bool forceRecalculate = false)
        {
            if (_needToRecalculateUtility || forceRecalculate)
            {
                Utility = GetUtility();
                _needToRecalculateUtility = false;
            }
        }

        /// <summary>
        /// If this node has <see cref="EnableUtilityUpdating"/> flag to true, indicates that the <see cref="Utility"/> has to be recalculated.
        /// </summary>
        public void MarkUtilityAsDirty()
        {
            if (EnableUtilityUpdating) _needToRecalculateUtility = true;
        }

        /// <summary>
        /// Compute the utility of this node.
        /// </summary>
        /// <returns>The updated utility Value of this node.</returns>
        protected abstract float GetUtility();
    }
}