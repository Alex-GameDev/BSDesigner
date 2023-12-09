using System;

namespace BSDesigner.Core.Utils
{
    public class MockedRandom : IRandom
    {
        public int IntValue = 0;

        public double DoubleValue = 0f;

        public int NextInt(int minIncludedValue, int maxExcludedValue) => Math.Clamp(IntValue, minIncludedValue, maxExcludedValue);

        public double NextDouble() => Math.Clamp(DoubleValue, 0f, 1f);
    }
}