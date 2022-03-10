using System;
using System.Windows;

namespace Robot.Localization.Model
{
    public class Pose
    {
        private const double STwoPi = 2*Math.PI;

        /// <summary>
        /// Nomalizes the provided angle (degree) so that it falls in the range
        /// (-180, 180]
        /// </summary>
        public static Angle NormalizeAngle(Angle angle)
        {
            var factor = Math.Floor(angle.Rads / STwoPi);
            var normalizedAngleRad = angle.Rads - factor * STwoPi;
            if (normalizedAngleRad < 0)
            {
                normalizedAngleRad = angle.Rads + STwoPi;
            }

            if (normalizedAngleRad > Math.PI)
            {
                normalizedAngleRad = normalizedAngleRad - STwoPi;
            }
            return Angle.FromRads(normalizedAngleRad);
        }

        public double X
        {
            get { return Location.X; }
            set { Location = new Point(value, Y); }
        }

        public double Y
        {
            get { return Location.Y; }
            set { Location = new Point(X, value); }
        }

        private Angle _mHeading;
        public Angle Heading
        {
            get { return _mHeading; }
            set { _mHeading = NormalizeAngle(value); }
        }

        public Pose(Point location, Angle heading)
        {
            Location = location;
            Heading = heading;
        }

        public Pose(double x, double y, Angle heading)
        {
            Location = new Point(x, y);
            Heading = heading;
        }

        public Point Location { get; set; }

        public override string ToString()
        {
            return String.Format("x={0}, y={1}, heading={2}", X, Y, Heading.Degrees);
        }
    }
}
