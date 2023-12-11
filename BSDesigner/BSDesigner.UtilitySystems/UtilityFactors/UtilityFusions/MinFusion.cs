using System.Collections.Generic;
using System.Linq;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Fusion factor that returns the lower utility from its children.
    /// </summary>
    public class MinFusion : UtilityFusion
    {
        /// <summary>
        /// Returns the lower value from <paramref name="utilities"/>
        /// </summary>
        /// <param name="utilities">The child utilities.</param>
        /// <returns>The lower utility of the children.</returns>
        protected override float Evaluate(IEnumerable<float> utilities) => utilities.Min();
    }
}