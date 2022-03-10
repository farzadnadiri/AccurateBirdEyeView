using System;

namespace Robot.Localization.Sampler
{
    public class SamplerTriangular : ISampler
    {
        private static readonly double SNormalizationFactor = Math.Sqrt(6.0) / 2.0;
 

        public double Sample(double variance)
        {
            var b = Math.Sqrt(variance);
            return SNormalizationFactor * (Sampler.GetRandomInRange(b) + Sampler.GetRandomInRange(b));
        }

    }
}
