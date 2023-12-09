using System;

namespace BSDesigner.Core.Utils
{
    public class DefaultRandom : IRandom
    {
        private readonly Random _random = new Random();
        public int NextInt(int minIncludedValue, int maxExcludedValue) => _random.Next(minIncludedValue, maxExcludedValue);

        public double NextDouble() => _random.NextDouble();
    }
}