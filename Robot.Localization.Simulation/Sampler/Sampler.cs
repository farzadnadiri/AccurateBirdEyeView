using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Localization.Sampler
{
    public static class Sampler
    {
        public static readonly Random Random = new Random();

        internal static double GetRandomInRange(double b)
        {
            return Random.NextDouble() * 2 * b - b;
        }

        private static ISampler _sSampler;

        public static void Initialize(ISampler sampler)
        {
            _sSampler = sampler;
        }

        public static double Sample(double variance)
        {
            return _sSampler.Sample(variance);
        }
    }
}
