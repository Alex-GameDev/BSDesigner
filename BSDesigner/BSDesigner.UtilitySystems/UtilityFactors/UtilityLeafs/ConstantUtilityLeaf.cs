using BSDesigner.Core;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Leaf factor which have a constant utility value
    /// </summary>
    public class ConstantUtilityLeaf : UtilityLeaf
    {
        /// <summary>
        /// The utility Value.
        /// </summary>
        public Parameter<float> Value = 0f;

        /// <summary>
        /// Returns the constant Value.
        /// </summary>
        /// <returns><see cref="Value"/></returns>
        protected override float ComputeUtility()
        {
            return Value;
        }
    }
}