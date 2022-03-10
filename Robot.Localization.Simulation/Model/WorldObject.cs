using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Localization.Model
{
    public class WorldObject
    {
        public enum ObjectType
        {
            Circle,      //!< Center circle
            Goal,        //!< Goal (center position)
            GoalPost,    //!< A single goal post
            XMarker,     //!< One of the two penalty markers
            FieldLine,   //!< Field line
            LineXingT,   //!< T-crossing of two lines
            LineXingX,   //!< X-crossing of two lines
            LineXingL,   //!< L-crossing of two lines
            MagneticHeading, //!< Magnetic heading towards X+
            NumTypes
        };


        public enum MirrorFlag
        {
            NoMirror = 0,
            MirrorX = (1 << 0),
            MirrorY = (1 << 1),
            MirrorAll = MirrorX | MirrorY
        };


        public WorldObject MirrorX()
        {

            var deg = Math.PI - Pose.Heading.Degrees;
            var pose = new Pose(-1 * Pose.X, Pose.Y, Angle.FromDegrees(deg));

            var points = new List<Point>();
            for (int i = 0; i < Points.Count; ++i)
            {
                points.Add(new Point(-1 * Points[i].X, Points[i].Y));
            }

            return new WorldObject(Type, pose, points);
        }


        public WorldObject MirrorY()
        {

          
            var deg = -1 * Pose.Heading.Degrees;
            var pose = new Pose(Pose.X, -1 * Pose.Y, Angle.FromDegrees(deg));
        

            var points = new List<Point>();
            for (int i = 0; i < Points.Count; ++i)
            {
                points.Add(new Point(Points[i].X, -1 * Points[i].Y));
            }

            return new WorldObject(Type, pose, points);
        }

        //! Object type
        public ObjectType Type { get; set; }
        //! Object pose (x, y, theta)
        public Pose Pose { get; set; }
        //! Points belonging to the object (e.g. line start and end)
        public List<Point> Points { get; set; }


        public WorldObject(ObjectType type, Pose pose, IEnumerable<Point> points)
        {
            Type = type;
            Pose = pose;
            Points = new List<Point>(points);

        }
        public WorldObject(ObjectType type, IEnumerable<Point> points)
        {
            Type = type;
            Pose=new Pose(0,0,Angle.FromDegrees(0));
            Points = new List<Point>(points);

        }

        public WorldObject(ObjectType type, Pose pose)
        {
            Type = type;
            Pose = pose;
            Points = new List<Point>();

        }

    }
}
