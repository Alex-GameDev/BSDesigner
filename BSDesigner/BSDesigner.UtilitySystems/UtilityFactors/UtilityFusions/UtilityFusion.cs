using System.Collections.Generic;
using System.Linq;
using BSDesigner.Core.Exceptions;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Factor that combines the utility of its children factors.
    /// </summary>
    public abstract class UtilityFusion : UtilityFactor
    {
        public override int MaxOutputConnections => -1;

        /// <summary>
        /// The child factors of this fusion node.
        /// </summary>
        protected IEnumerable<UtilityFactor> ChildFactors
        {
            get
            {
                _cachedChildFactors ??= Children.Cast<UtilityFactor>().ToList();
                return _cachedChildFactors;
            }
        }

        private List<UtilityFactor>? _cachedChildFactors;

        /// <summary>
        /// Update the children utilities and compute its utility.
        /// </summary>
        /// <returns>The computed utility.</returns>
        protected override float ComputeUtility()
        {
            if (!ChildFactors.Any())
                throw new MissingConnectionException("This utility fusion has not child factors");

            foreach (var utilityFactor in ChildFactors)
            {
                utilityFactor.UpdateUtility();
            }
            return Evaluate(ChildFactors.Select(child => child.Utility));
        }

        /// <summary>
        /// Override this method to compute the fusion factor utility.
        /// </summary>
        /// <param name="utilities">The children utilities.</param>
        /// <returns>The computed utility.</returns>
        protected abstract float Evaluate(IEnumerable<float> utilities);
    }
}