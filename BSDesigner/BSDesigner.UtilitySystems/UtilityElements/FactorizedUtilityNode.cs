using BSDesigner.Core.Exceptions;
using System;

namespace BSDesigner.UtilitySystems.UtilityElements
{
    /// <summary>
    /// Utility node that gets its utility from a factor node.
    /// </summary>
    public abstract class FactorizedUtilityNode : SelectableUtilityNode
    {
        public override int MaxOutputConnections => 1;

        public override Type ChildType => typeof(UtilityFactor);

        protected UtilityFactor Factor
        {
            get
            {
                if (_cachedFactor == null)
                {
                    if (Children.Count == 0) throw new MissingConnectionException("Can't find the child node if the children list is empty");
                    _cachedFactor = (UtilityFactor)Children[0];
                }
                return _cachedFactor;
            }
        }

        private UtilityFactor? _cachedFactor;

        public override bool HasPriority => false;

        /// <summary>
        /// <inheritdoc/>
        /// Updates the utility of the factor and gets it.
        /// </summary>
        /// <returns>The utility of the factor</returns>
        protected override float GetUtility()
        {
            Factor.UpdateUtility();
            return Factor.Utility;
        }
    }
}
