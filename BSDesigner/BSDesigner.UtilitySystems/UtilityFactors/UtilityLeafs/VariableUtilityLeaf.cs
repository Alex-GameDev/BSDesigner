using BSDesigner.Core;
using System;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Leaf factor that computes its utility using a function.
    /// </summary>
    public class VariableUtilityLeaf : UtilityLeaf
    {
        /// <summary>
        /// The function used to get the utility
        /// </summary>
        public Func<float>? ValueFunction;

        /// <summary>
        /// The minimum value that <see cref="ValueFunction"/> returns.
        /// </summary>
        public Parameter<float> Min = 0f;

        /// <summary>
        /// The maximum variable that <see cref="ValueFunction"/> returns.
        /// </summary>
        public Parameter<float> Max = 1f;

        /// <summary>
        /// Calculates the utility by normalizing the result of <see cref="ValueFunction"/> 
        /// using <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        /// <returns>The result of the function normalized.</returns>
        protected override float ComputeUtility()
        {
            Utility = ValueFunction?.Invoke() ?? Min;
            Utility = (Utility - Min) / (Max - Min);
            return Utility;
        }
    }
}