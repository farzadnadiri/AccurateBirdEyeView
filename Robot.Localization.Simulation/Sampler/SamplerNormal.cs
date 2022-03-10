using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Localization.Sampler
{
    public class SamplerNormal : ISampler
    {
        private const int CAggregationCount = 12;



        public double Sample(double variance)
        {
            double b = Math.Sqrt(variance);
            double result = 0.0;

            for (int i = 0; i < CAggregationCount; i++)
            {
                result += Sampler.GetRandomInRange(b);
            }
            return 0.5 * result;
        }

  
    }
}
