using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Vision.CameraCalibrator
{
    public class InversePerspectiveMapping
    {

 
        private bool InitIpm(float diagonalAngleView)
        {
            double w = 320;
            double h = 240;

            double w_2 = w / 2.0;
            double h_2 = h / 2.0;

            double d = 1.0;

            double gama = ConvertToRadians(diagonalAngleView) / 2.0;
            var tg = Math.Tan(gama);

            double nr = tg;

            double nx = nr / Math.Sqrt(w_2 * w_2 + h_2 * h_2);
            double nw = nx * w_2;
            double nh = nx * h_2;

            double alfa = Math.Atan(nh / d);
            double beta = Math.Atan(nw / d);

            var ta = Math.Tan(alfa);
            var tb = Math.Tan(beta);

            return true;
        }
        public double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

    }
}
