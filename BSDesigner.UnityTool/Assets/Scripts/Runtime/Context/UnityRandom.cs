using BSDesigner.Core;

namespace BSDesigner.UnityTool.Runtime.Random
{
    /// <summary>
    /// Unity random provider
    /// </summary>
    public class UnityRandom : IRandomProvider, IRandom
    {
        public int NextInt(int minIncludedValue, int maxExcludedValue) => UnityEngine.Random.Range(minIncludedValue, maxExcludedValue);

        public double NextDouble() => UnityEngine.Random.Range(0f, 1f);

        public IRandom CreateRandom() => this;
    }
}