namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Create a curve factor with a linear function
    /// </summary>
    public class LinearCurve : UtilityCurve
    {
        /// <summary>
        /// The slope of the function.
        /// </summary>
        public float Slope = 1f;

        /// <summary>
        /// The y intercept of the function.
        /// </summary>
        public float YIntercept = 0f;

        /// <summary>
        /// Compute the utility using a linear function [y = slope * x + yIntercept]
        /// </summary>
        /// <param name="x">The child utility. </param>
        /// <returns>The result of apply the function to <paramref name="x"/>.</returns>
        protected override float Evaluate(float x) => Slope * x + YIntercept;
    }
}