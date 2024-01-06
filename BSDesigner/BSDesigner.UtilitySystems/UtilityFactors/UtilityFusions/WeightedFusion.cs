using BSDesigner.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Fusion factor that returns the weighted average from its children utility.
    /// </summary>
    public class WeightedFusion : UtilityFusion
    {
        /// <summary>
        /// The weights applied to each utility.
        /// </summary>
        public Parameter<List<float>> Weights = default!;

        /// <summary>
        /// Returns the weighted average of the utilities in <paramref name="utilities"/>
        /// </summary>
        /// <param name="utilities">The child utilities.</param>
        /// <returns>The weighted average of the children utility.</returns>
        protected override float Evaluate(IEnumerable<float> utilities)
        {
            return utilities.Zip(Weights.Value, (utility, weight) => utility * weight).Sum();
        }
    }
}