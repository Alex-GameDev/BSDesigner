using System;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Create a curve factor with an sigmoid function
    /// </summary>
    public class SigmoidCurve : UtilityCurve
    {
        /// <summary>
        /// The grown rate of the function.
        /// </summary>
        public float GrownRate = 1f;

        /// <summary>
        /// The mid point of the function.
        /// </summary>
        public float Midpoint = 0.5f;

        /// <summary>
        /// Compute the utility using a exponential function [y = (1 / (1 + e^(-grownRate * (x - midPoint)))]
        /// </summary>
        /// <param name="x">The child utility. </param>
        /// <returns>The result of apply the function to <paramref name="x"/>.</returns>
        protected override float Evaluate(float x) => (float)(1f / (1f + Math.Pow(Math.E, -GrownRate * (x - Midpoint))));
    }
}