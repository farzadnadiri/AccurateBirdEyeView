using System;
using System.Globalization;

namespace Robot.Localization.Model
{
    public class Angle
    {
        private const double SRadToDegree = 180.0/Math.PI;
        private const double SDegreeToRad = Math.PI/180.0;

        public static Angle FromRads(double rad)
        {
            return new Angle(rad);
        }

        public static Angle FromDegrees(double degree)
        {
            return new Angle(degree * SDegreeToRad);
        }

        public static Angle operator +(Angle a1, Angle a2)
        {
            return new Angle(a1.Rads + a2.Rads);
        }

        public static Angle operator -(Angle a1, Angle a2)
        {
            return new Angle(a1.Rads - a2.Rads);
        }

        public Angle(double rad)
        {
            Rads = rad;
        }

        public double Degrees
        {
            get { return Rads * SRadToDegree; }
        }

        public double Rads;

        public override string ToString()
        {
            return Degrees.ToString(CultureInfo.InvariantCulture) + " deg";
        }
    }
}
