using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Test
{
    public static class Util
    {
        public static List<double> pose_global(List<double> pRelative, List<double> pose)
        {
            var ca = Math.Cos(pose[3]);
            var sa = Math.Sin(pose[3]);
            return new List<double>
            {
                pose[1] + ca*pRelative[1] - sa*pRelative[2],
                pose[2] + sa*pRelative[1] + ca*pRelative[2],
                pose[3] + pRelative[3]
            };
        }



        public static List<double> pose_relative(List<double> pGlobal, List<double> pose)
        {
            var ca = Math.Cos(pose[3]);
            var sa = Math.Sin(pose[3]);
            var px = pGlobal[1] - pose[1];
            var py = pGlobal[2] - pose[2];
            var pa = pGlobal[3] - pose[3];
            return
            new List<double> { ca * px + sa * py, -sa * px + ca * py, mod_angle(pa) };
        }

        private static double mod_angle(double a)
        {

            a = a % (2 * Math.PI);
            if (a >= Math.PI)
            {
                a = a - 2 * Math.PI;
            }
            return a;

        }


        public static double procFunc(double a, double deadband, double maxvalue)
        {
            double b = 0;
            if (a > 0)
            {
                b = Math.Min(Math.Max(0, Math.Abs(a) - deadband), maxvalue);
            }
            else
            {
                b = -Math.Min(Math.Max(0, Math.Abs(a) - deadband), maxvalue);
            }
            return b;
        }

        public static List<double> se2_interpolate(double t, List<double> u1, List<double> u2)
        {
            return new List<double>
            
            {
                u1[1] + t*(u2[1] - u1[1]),
                u1[2] + t*(u2[2] - u1[2]),
                u1[3] + t*mod_angle(u2[3] - u1[3])
            };
        }
    }
}
