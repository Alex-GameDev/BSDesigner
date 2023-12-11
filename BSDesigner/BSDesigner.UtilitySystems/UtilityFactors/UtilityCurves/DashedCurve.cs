using System.Collections.Generic;
using System.Numerics;

namespace BSDesigner.UtilitySystems
{
    /// <summary>
    /// Create a curve factor with an with a linear function defined with points.
    /// </summary>
    public class DashedCurve : UtilityCurve
    {
        /// <summary>
        /// The points used to define the function. Must be ordered in its x component to avoid errors.
        /// </summary>
        public List<Vector2> Points = new List<Vector2>();

        /// <summary>
        /// Compute the utility using a linear function defined with points.
        /// <para>If x is lower than the first point x component, the value will be its y component.</para>
        /// <para>If x is higher than the last point x component, the value will be its y component.</para>
        /// </summary>
        /// <param name="x">The child utility. </param>
        /// <returns>The result of apply the function to <paramref name="x"/>.</returns>
        protected override float Evaluate(float x)
        {
            if (Points.Count == 0) return 0;

            var id = FindClosestLowerId(x);

            if (id == -1)
            {
                return Points[0].Y;
            }

            else if (id == Points.Count - 1)
                return Points[^1].Y;
            else
            {
                var delta = (x - Points[id].X) / (Points[id + 1].X - Points[id].X);
                return Points[id].Y * (1 - delta) + Points[id + 1].Y * delta;
            }
        }

        private int FindClosestLowerId(float x)
        {
            var id = 0;
            while (id < Points.Count && Points[id].X <= x)
            {
                id++;
            }
            return id - 1;
        }
    }
}