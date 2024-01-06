using BSDesigner.Core;
using System;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Create a curve factor with an exponential function
    /// </summary>
    public class ExponentialCurve : UtilityCurve
    {
        /// <summary>
        /// The exponent of the function
        /// </summary>
        public Parameter<float> Exponent = 1f;

        /// <summary>
        /// The x displacement of the function.
        /// </summary>
        public Parameter<float> DespX = 0f;

        /// <summary>
        /// The y displacement of the function.
        /// </summary>
        public Parameter<float> DespY = 0f;

        /// <summary>
        /// Compute the utility using a exponential function [y = (x - dx)^exp + dy]
        /// </summary>
        /// <param name="x">The child utility. </param>
        /// <returns>The result of apply the function to <paramref name="x"/>.</returns>
        protected override float Evaluate(float x) => (float)Math.Pow(x - DespX, Exponent) + DespY;
    }
}