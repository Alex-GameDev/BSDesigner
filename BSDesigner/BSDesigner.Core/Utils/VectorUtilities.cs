using System;
using System.Numerics;

namespace BSDesigner.Core.Utils
{
    public static class VectorUtilities
    {
        public static float Magnitude(this Vector2 vector)
        {
            return MathF.Sqrt(MathF.Pow(vector.X, 2) +  MathF.Pow(vector.Y, 2));
        }

        public static Vector2 Normalize(this Vector2 vector)
        {
            var inverseMagnitude = 1f / vector.Magnitude();
            return inverseMagnitude * vector;
        }
    }
}
