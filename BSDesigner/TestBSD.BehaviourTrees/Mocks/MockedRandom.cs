using BSDesigner.Core;

namespace TestBSD.BehaviourTrees.Mocks
{
    public class MockedRandom : IRandomProvider, IRandom
    {
        public int IntValue;

        public double DoubleValue;

        public IRandom CreateRandom() => this;

        public int NextInt(int minIncludedValue, int maxExcludedValue) => Math.Clamp(IntValue, minIncludedValue, maxExcludedValue);

        public double NextDouble() => Math.Clamp(DoubleValue, 0f, 1f);
    }
}