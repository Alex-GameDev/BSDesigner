namespace BSDesigner.Core
{
    public interface IRandom
    {
        /// <summary>
        /// Generate a value between <see cref="minIncludedValue"/> and <paramref name="maxExcludedValue"/>.
        /// </summary>
        /// <param name="minIncludedValue">The minimum inclusive value.</param>
        /// <param name="maxExcludedValue">The maximum exclusive value.</param>
        /// <returns>The generated value.</returns>
        public int NextInt(int minIncludedValue, int maxExcludedValue);

        /// <summary>
        /// Generate a double value greater or equal than 0.0 and lower than 1.0.
        /// </summary>
        /// <returns>The generated value</returns>
        public double NextDouble();
    }
}