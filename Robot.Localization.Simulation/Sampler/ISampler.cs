using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Localization.Sampler
{
    public interface ISampler
    {
        double Sample(double variance);
    }
}
