using System;
using System.Collections.Generic;
using System.Text;

namespace BSDesigner.UtilitySystems
{
    public abstract class Factor : UtilityNode
    {
        public override Type ChildType => typeof(Factor);
        public override int MaxInputConnections => -1;

        /// <summary>
        /// <inheritdoc/>
        /// Clamp the utility computed.
        /// </summary>
        /// <returns>The utility clamped between 0 and 1.</returns>
        protected sealed override float GetUtility()
        {
            var value = ComputeUtility();
            if (0 <= value && value <= 1) return value;
            if (value < 0) return 0;
            return 1;
        }

        /// <summary>
        /// Method used to calculate the utility.
        /// </summary>
        /// <returns>The utility of the factor.</returns>
        protected abstract float ComputeUtility();
    }
}
