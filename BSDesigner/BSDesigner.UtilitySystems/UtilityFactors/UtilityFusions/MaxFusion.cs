using System.Collections.Generic;
using System.Linq;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Fusion factor that returns the higher utility from its children.
    /// </summary>
    public class MaxFusion : UtilityFusion
    {
        /// <summary>
        /// Returns the higher value from <paramref name="utilities"/>
        /// </summary>
        /// <param name="utilities">The child utilities.</param>
        /// <returns>The higher utility of the children.</returns>
        protected override float Evaluate(IEnumerable<float> utilities) => utilities.Max();
    }
}