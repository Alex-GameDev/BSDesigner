namespace BSDesigner.Core
{
    /// <summary>
    /// Interface for random providers.
    /// </summary>
    public interface IRandomProvider
    {
        /// <summary>
        /// Create a new random.
        /// </summary>
        /// <returns>The random generated.</returns>
        IRandom CreateRandom();
    }
}