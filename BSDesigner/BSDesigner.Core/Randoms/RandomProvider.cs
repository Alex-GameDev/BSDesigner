namespace BSDesigner.Core
{
    /// <summary>
    /// Random provider that creates randoms of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Tye type of the random.</typeparam>
    public class RandomProvider<T> : IRandomProvider where T : IRandom, new()
    {
        /// <summary>
        /// Create a new random of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>The random created.</returns>
        public IRandom CreateRandom()
        {
            var random = new T();
            return random;
        }
    }
}