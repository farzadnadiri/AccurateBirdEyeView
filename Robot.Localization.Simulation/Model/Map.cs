using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Robot.Localization.Configuration;

namespace Robot.Localization.Model
{
    public class Map
    {
        //! Field width (inside the lines)
        public int FieldWidth { get; set; }
        //! Field length (inside the lines)
        public int FieldLength { get; set; }
        //! Field boundary (amount of green outside the field boundary)
        public int Boundary { get; set; }
        //! Goal width
        public int GoalWidth { get; set; }
        //! Width of the penalty area before each goal
        public int GoalAreaWidth { get; set; }
        //! Depth of the penalty area before each goal
        public int GoalAreaDepth { get; set; }
        public int CenterCircleDiameter { get; set; }
        //! Distance from the goal line to the penalty marker
        public int PenaltyMarkerDist { get; set; }
        //! Diameter of the ball
        public int BallDiameter { get; set; }
        //! Border for the top(positive) part of the field
        public int BorderTop { get; set; }
        //! Border for the bottom(negative) part of the field
        public int BorderBottom { get; set; }
        //! Border for the left(when looking to positive goal) part of the field
        public int BorderLeft { get; set; }
        //! Border for the right(when looking to positive goal) part of the field
        public int BorderRight { get; set; }

        public List<WorldObject> WorldObjects { get; set; }
        public Point[] Rectangle { get; private set; }
        public double XMin { get; private set; }
        public double XMax { get; private set; }
        public double YMin { get; private set; }
        public double YMax { get; private set; }

        public double Width
        {
            get { return XMax - XMin; }
        }

        public double Height
        {
            get { return YMax - YMin; }
        }

        // field width half
        private int hw;
        // field length half
        private int hl;

        double picut(double a)
        {
            while (a > Math.PI)
                a -= 2.0 * Math.PI;
            while (a < -Math.PI)
                a += 2.0 * Math.PI;
            return a;
        }
        Angle picut(Angle a)
        {
            while (a.Degrees > Math.PI)
                a = Angle.FromDegrees(a.Degrees - 2.0 * Math.PI);
            while (a.Degrees < -Math.PI)
                a = Angle.FromDegrees(a.Degrees + 2.0 * Math.PI);
            return a;
        }




        private LocalizationSettings _configs = new LocalizationSettings();
        public Map()
        {
            var points = new Point[4];
            var p1 = new Point(_configs.Field_I, _configs.Field_I);
            var p2 = new Point(_configs.Field_I + _configs.Field_A, _configs.Field_I);
            var p3 = new Point(_configs.Field_I + _configs.Field_A, _configs.Field_I + _configs.Field_B);
            var p4 = new Point(_configs.Field_I, _configs.Field_I + _configs.Field_B);
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
            points[3] = p4;
            Rectangle = points;

            InitializeMinsAndMaxs();
            WorldObjects = new List<WorldObject>();
            PrepareMap();
        }

        private void InitializeMinsAndMaxs()
        {
            XMin = double.MaxValue;
            XMax = double.MinValue;
            YMin = double.MaxValue;
            YMax = double.MinValue;

            foreach (Point point in Rectangle)
            {
                if (point.X < XMin)
                {
                    XMin = point.X;
                }
                if (point.X > XMax)
                {
                    XMax = point.X;
                }
                if (point.Y < YMin)
                {
                    YMin = point.Y;
                }
                if (point.Y > YMax)
                {
                    YMax = point.Y;
                }
            }

            FieldLength = _configs.Field_A;
            FieldWidth = _configs.Field_B;
            GoalWidth = _configs.Field_D;
            GoalAreaDepth = _configs.Field_E;
            GoalAreaWidth = _configs.Field_F;
            CenterCircleDiameter = _configs.Field_H;
            PenaltyMarkerDist = _configs.Field_G;
            BorderTop = _configs.Field_I;
            BorderBottom = _configs.Field_I;
            BorderLeft = _configs.Field_I;
            BorderRight = _configs.Field_I;
            Boundary = _configs.Field_I;
            hw = FieldWidth / 2;
            hl = FieldLength / 2;
        }



        public bool IsInside(Point point)
        {
            return IsInside(point.X, point.Y);
        }


        public bool IsInside(double x, double y)
        {
            bool isIn = false;
            for (int i = 0; i < Rectangle.Length; i++)
            {
                int j = (i + Rectangle.Length - 1) % Rectangle.Length;
                if (
                    ((Rectangle[i].Y > y) != (Rectangle[j].Y > y)) &&
                    (x < (Rectangle[j].X - Rectangle[i].X) * (y - Rectangle[i].Y) / (Rectangle[j].Y - Rectangle[i].Y) + Rectangle[i].X)
                   )
                {
                    isIn = !isIn;
                }
            }
            return isIn;
        }

        void AddObject(WorldObject.ObjectType type, double x, double y, double t, WorldObject.MirrorFlag flags)
        {
         

           var obj=new WorldObject(type, new Pose(x,y,Angle.FromDegrees(t)));
           WorldObjects.Add(obj);

          

            if (flags==WorldObject.MirrorFlag.MirrorX)
               WorldObjects.Add(obj.MirrorX());

            if (flags == WorldObject.MirrorFlag.MirrorY)
                WorldObjects.Add(obj.MirrorX());

            if (flags == WorldObject.MirrorFlag.MirrorAll)
                WorldObjects.Add(obj.MirrorX().MirrorY());

        
        }

        void AddLine(List<Point> points, WorldObject.MirrorFlag flags)
        {
            var obj = new WorldObject(WorldObject.ObjectType.FieldLine, points);
            WorldObjects.Add(obj);
            
            if (flags == WorldObject.MirrorFlag.MirrorX)
                WorldObjects.Add(obj.MirrorX());

            if (flags == WorldObject.MirrorFlag.MirrorY)
                WorldObjects.Add(obj.MirrorY());

            if (flags == WorldObject.MirrorFlag.MirrorAll)
                WorldObjects.Add(obj.MirrorX().MirrorY());
        }


        void PrepareMap()
        {

            // (0,0) is center of field , negative and positive area for x and y 
            //   _____________________________   
            //  |              |              |
            //  |     (X-,Y-)  |     (X+,Y-)  |
            //  |              |              |
            //  |--------------|--------------|
            //  |              |              |
            //  |     (X-,Y+)  |     (X+,Y+)  |
            //  |______________|______________|  

            var points =new List<Point>();

            // Goals & posts
            AddObject(WorldObject.ObjectType.Goal, -hl, 0.0, 0.0, WorldObject.MirrorFlag.MirrorX);
            AddObject(WorldObject.ObjectType.GoalPost, -hl, GoalWidth / 2.0, 0.0, WorldObject.MirrorFlag.MirrorAll);

            // Center circle
            AddObject(WorldObject.ObjectType.Circle, 0.0, 0.0, 0.0, WorldObject.MirrorFlag.NoMirror);

            // Penalty markers
            AddObject(WorldObject.ObjectType.XMarker, -hl + PenaltyMarkerDist, 0.0, 0.0, WorldObject.MirrorFlag.MirrorX);

            // Center line
            points.Clear();
            points.Add(new Point(0, hw));
            points.Add(new Point(0, -hw));
            AddLine(points, WorldObject.MirrorFlag.NoMirror);

            // Side lines
            points.Clear();
            points.Add(new Point(-hl, hw));
            points.Add(new Point(hl, hw));
            AddLine(points, WorldObject.MirrorFlag.MirrorY);

            // Goal lines
            points.Clear();
            points.Add(new Point(-hl, hw));
            points.Add(new Point(-hl, -hw));
            AddLine(points, WorldObject.MirrorFlag.MirrorX);

            // Goal area lines (long line)
            points.Clear();
            points.Add(new Point(-hl + GoalAreaDepth, (int)(GoalAreaWidth / 2.0)));
            points.Add(new Point(-hl + GoalAreaDepth, (int)(-1*GoalAreaWidth / 2.0)));
            AddLine(points, WorldObject.MirrorFlag.MirrorX);

            // Goal area lines (small line)
            points.Clear();
            points.Add(new Point(-hl, (int)(GoalAreaWidth / 2.0)));
            points.Add(new Point(-hl + GoalAreaDepth, (int)(GoalAreaWidth / 2.0)));
            AddLine(points, WorldObject.MirrorFlag.MirrorAll);

            // Corner L Xings (theta is the half angle between the two lines)
            AddObject(WorldObject.ObjectType.LineXingL, -hl, hw, -Math.PI / 4.0, WorldObject.MirrorFlag.MirrorAll);

            // Goal area L Xings (theta is the half angle between the two lines)
            AddObject(WorldObject.ObjectType.LineXingL, -hl + GoalAreaDepth, GoalAreaWidth / 2.0, -3.0 * Math.PI / 4.0, WorldObject.MirrorFlag.MirrorAll);

            // Goal area T Xings (theta points along the central line)
            AddObject(WorldObject.ObjectType.LineXingT, -hl, GoalAreaWidth / 2.0, 0.0, WorldObject.MirrorFlag.MirrorAll);
            AddObject(WorldObject.ObjectType.LineXingT, 0, -hw,0, WorldObject.MirrorFlag.MirrorY);

            // Central X Xings
            AddObject(WorldObject.ObjectType.LineXingX, 0.0, CenterCircleDiameter / 2.0, 0.0, WorldObject.MirrorFlag.MirrorY);

            // Magnetic orientation
            AddObject(WorldObject.ObjectType.MagneticHeading, 0, 0, 0.0, WorldObject.MirrorFlag.NoMirror);
        }

    }
}
