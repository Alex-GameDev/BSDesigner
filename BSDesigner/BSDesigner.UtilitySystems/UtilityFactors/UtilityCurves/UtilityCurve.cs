using BSDesigner.Core.Exceptions;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Factor that modifies its child value with a function.
    /// </summary>
    public abstract class UtilityCurve : UtilityFactor
    {
        public override int MaxOutputConnections => 1;

        /// <summary>
        /// The child factor of the curve.
        /// </summary>
        protected UtilityFactor ChildFactor
        {
            get
            {
                if (_cachedChildFactor == null)
                {
                    if (Children.Count == 0) throw new MissingConnectionException("Can't find the child node if the children list is empty");
                    _cachedChildFactor = (UtilityFactor)Children[0];
                }

                return _cachedChildFactor;
            }
        }

        private UtilityFactor? _cachedChildFactor;

        /// <summary>
        /// Compute its utility applying a function to the utility of its child factor.
        /// </summary>
        /// <returns>The result of apply the evaluate function to the child, or 0 if the child is null.</returns>
        protected override float ComputeUtility()
        {
            ChildFactor.UpdateUtility();
            return Evaluate(ChildFactor.Utility);
        }

        /// <summary>
        /// Modify the child utility with a function.
        /// </summary>
        /// <param name="childUtility">The child utility.</param>
        /// <returns>The child utility modified.</returns>
        protected abstract float Evaluate(float childUtility);
    }
}