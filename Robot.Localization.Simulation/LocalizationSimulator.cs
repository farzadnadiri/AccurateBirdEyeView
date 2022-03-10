using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Windows.Forms;
using Emgu.CV.Structure;
using Robot.Localization.Configuration;
using Robot.Localization.Model;

namespace Robot.Localization.Simulation
{
    public partial class LocalizationSimulator : UserControl
    {

        private Bitmap _bitmap;
        private Graphics _graphics;
        private Pen _robotPen = new Pen(Color.Red, 2);
        private Pen _gazePen = new Pen(Color.Red, 1);

        private Pen _particlePen = new Pen(Color.Black, 1);
        private Pen _linesPen = new Pen(Color.White, 3);

        private Pen _compassPen = new Pen(Color.Yellow, 6);
        private Pen _detectedLines = new Pen(Color.LightBlue, 2);
        private SizeConfiguration _sizeConfiguration;
        static LocalizationSettings _configs = new LocalizationSettings();
        private Size _validSize;
        public Pose[] Particles { get; set; }
        public Pose Robot { get; set; }
        public double Compass { get; set; }
        public bool IsRightSideGoal { get; set; }
        public System.Windows.Point[] GazePoints { get; set; }
        public double HeadDirection { get; set; }
        public LineSegment2D[] Lines { get; set; }

        public LocalizationSimulator()
        {
            InitializeComponent();

            Compass = double.MinValue;
            PrepareControll();
        }

        private Size CalculateSize(Size controllSize)
        {
            var realFieldWidth = _configs.Field_A + (2 * _configs.Field_I);
            var realFieldHeight = _configs.Field_B + (2 * _configs.Field_I);

            var realTransform = (double)realFieldWidth / realFieldHeight;
            var controllTransform = (double)controllSize.Width / controllSize.Height;

            if (controllTransform > realTransform)
            {
                var width = Convert.ToInt32(controllSize.Height * realTransform);
                var newSize = new Size(width, controllSize.Height);

                return newSize;
            }
            else
            {

                var height = Convert.ToInt32(controllSize.Width / realTransform);
                var newSize = new Size(controllSize.Width, height);

                return newSize;
            }
        }

        public void PrepareControll()
        {

            _validSize = CalculateSize(Size);
            _sizeConfiguration = new SizeConfiguration(_validSize);

            if (_sizeConfiguration != null && _sizeConfiguration.SimulatedSize.Width != 0 &&
                _sizeConfiguration.SimulatedSize.Height != 0)
            {


                _bitmap = new Bitmap(_sizeConfiguration.SimulatedSize.Width, _sizeConfiguration.SimulatedSize.Height);
                _graphics = Graphics.FromImage(_bitmap);
                _graphics.Clear(Color.ForestGreen);
                _graphics.DrawRectangle(_linesPen,
                    new Rectangle(_sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I),
                        _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I),
                        _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_A),
                        _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_B)));
                _graphics.DrawRectangle(_linesPen,
                    new Rectangle(_sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I),
                        _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_B / 2 -
                                                                          _configs.Field_F / 2),
                        _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_E),
                        _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_F)));
                _graphics.DrawRectangle(_linesPen,
                    new Rectangle(
                        _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_A -
                                                                          _configs.Field_E),
                        _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_B / 2 -
                                                                          _configs.Field_F / 2),
                        _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_E),
                        _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_F)));
                _graphics.DrawLine(_linesPen,
                    new Point(_sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_A / 2),
                        _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I)),
                    new Point(_sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_A / 2),
                        _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_B)));

                _graphics.DrawEllipse(_linesPen,
                    _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_A / 2 -
                                                                      _configs.Field_H / 2),
                    _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_B / 2 -
                                                                      _configs.Field_H / 2),
                    _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_H),
                    _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_H));


                const int penaltymarkersWidth = 10;


                _graphics.FillEllipse(Brushes.White,
                  _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_A / 2 -
                                                                    penaltymarkersWidth / 2),
                  _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_B / 2 -
                                                                    penaltymarkersWidth / 2),
                  _sizeConfiguration.ConvertRealSizeToSimulatedSize(penaltymarkersWidth),
                  _sizeConfiguration.ConvertRealSizeToSimulatedSize(penaltymarkersWidth));


                _graphics.FillEllipse(Brushes.White,
                  _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_G -
                                                                    penaltymarkersWidth / 2),
                  _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_B / 2 -
                                                                    penaltymarkersWidth / 2),
                  _sizeConfiguration.ConvertRealSizeToSimulatedSize(penaltymarkersWidth),
                  _sizeConfiguration.ConvertRealSizeToSimulatedSize(penaltymarkersWidth));

                _graphics.FillEllipse(Brushes.White,
             _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_A - _configs.Field_G -
                                                               penaltymarkersWidth / 2),
             _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_B / 2 -
                                                               penaltymarkersWidth / 2),
             _sizeConfiguration.ConvertRealSizeToSimulatedSize(penaltymarkersWidth),
             _sizeConfiguration.ConvertRealSizeToSimulatedSize(penaltymarkersWidth));

                var height = _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I / 4);
                _graphics.FillRectangle(Brushes.Yellow, new Rectangle(_sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I / 4), _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_B / 2 - _configs.Field_D / 2) - height / 2, _sizeConfiguration.ConvertRealSizeToSimulatedSize(6 * _configs.Field_I / 8), height));
                _graphics.FillRectangle(Brushes.Yellow, new Rectangle(_sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I / 4), _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_B / 2 + _configs.Field_D / 2) - height / 2, _sizeConfiguration.ConvertRealSizeToSimulatedSize(6 * _configs.Field_I / 8), height));
                _graphics.FillRectangle(Brushes.Yellow, new Rectangle(_sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_A + 2), _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_B / 2 - _configs.Field_D / 2) - height / 2, _sizeConfiguration.ConvertRealSizeToSimulatedSize(6 * _configs.Field_I / 8), height));
                _graphics.FillRectangle(Brushes.Yellow, new Rectangle(_sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_A + 2), _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_B / 2 + _configs.Field_D / 2) - height / 2, _sizeConfiguration.ConvertRealSizeToSimulatedSize(6 * _configs.Field_I / 8), height));




                if (Compass <= 180 && Compass >= -180)
                {
                    DrawCompass();
                }




                if (Robot != null)
                {
                    DrawRobot(Robot);

                    if (GazePoints != null)
                    {
                        DrawGaze(Robot);
                    }
                }
                if (Particles != null)
                {
                    foreach (var particle in Particles)
                    {
                        DrawParticle(particle);
                    }
                }
                ViewerControll.Image = _bitmap;
            }
        }

        private void DrawGaze(Pose realPose)
        {
            var radius = _sizeConfiguration.ConvertRealSizeToSimulatedSize(Convert.ToInt32(_configs.Field_I / 2));
            var pose = new Pose(_sizeConfiguration.ConvertRealSizeToSimulatedSize(Convert.ToInt32(realPose.X)), _sizeConfiguration.ConvertRealSizeToSimulatedSize(Convert.ToInt32(realPose.Y)), realPose.Heading);
            var center = new Point(Convert.ToInt32(pose.X), Convert.ToInt32(pose.Y));
            var dest = new Point(Convert.ToInt32(pose.X + 1.5 * radius * Math.Cos(pose.Heading.Rads)), Convert.ToInt32(pose.Y + radius * Math.Sin(pose.Heading.Rads)));
            var p1 = new Point(dest.X, dest.Y - radius);
            var p2 = new Point(dest.X, dest.Y + radius);
            var p3 = new Point(dest.X + 20 * radius, dest.Y + 20 * radius);
            var p4 = new Point(dest.X + 20 * radius, dest.Y - 20 * radius);
            var points = new Point[4];
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
            points[3] = p4;


            _graphics.TranslateTransform(center.X, center.Y);
            _graphics.RotateTransform((float)HeadDirection);
            _graphics.TranslateTransform(-1 * center.X, -1 * center.Y);
            _graphics.DrawPolygon(_gazePen, points);
            _graphics.ResetTransform();



        }


        private void DrawCompass()
        {
            var font = new Font("Tahoma", _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I / 2));
            var drawFormat = new StringFormat(StringFormatFlags.DirectionVertical);
            Point p;
            if (IsRightSideGoal)
            {
                p = new Point(_sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I / 6),
                   _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I / 4 + _configs.Field_B / 2));
            }
            else
            {
                p = new Point(_sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_A),
                _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I / 4 + _configs.Field_B / 2));
            }
            _graphics.DrawString("Goal", font, Brushes.Crimson, p, drawFormat);

            var center =
                new Point(
                    _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_A / 2),
                    _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_B / 2));
            var difrence = _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I / 2);
            var points = new Point[3];

            points[0] = new Point(center.X + difrence, center.Y);
            points[1] = new Point(center.X - difrence, center.Y - difrence / 2);
            points[2] = new Point(center.X - difrence, center.Y + difrence / 2);

            _graphics.TranslateTransform(center.X, center.Y);
            _graphics.RotateTransform((float)Compass);
            _graphics.TranslateTransform(-1 * center.X, -1 * center.Y);
            _graphics.DrawPolygon(_compassPen, points);
            _graphics.FillPolygon(Brushes.Blue, points);
            _graphics.ResetTransform();

        }

        private void DrawRobot(Pose realPose)
        {
            var radius = _sizeConfiguration.ConvertRealSizeToSimulatedSize(Convert.ToInt32(_configs.Field_I / 3));
            var pose = new Pose(_sizeConfiguration.ConvertRealSizeToSimulatedSize(Convert.ToInt32(realPose.X)), _sizeConfiguration.ConvertRealSizeToSimulatedSize(Convert.ToInt32(realPose.Y)), realPose.Heading);

            _graphics.DrawEllipse(_robotPen, Convert.ToInt32(pose.X - radius), Convert.ToInt32(pose.Y - radius), 2 * radius, 2 * radius);
            _graphics.DrawLine(_robotPen, Convert.ToInt32(pose.X), Convert.ToInt32(pose.Y), Convert.ToInt32(pose.X + radius * Math.Cos(pose.Heading.Rads)), Convert.ToInt32(pose.Y + radius * Math.Sin(pose.Heading.Rads)));
        }

        private void DrawParticle(Pose realPose)
        {
            var radius = _sizeConfiguration.ConvertRealSizeToSimulatedSize(Convert.ToInt32(_configs.Field_I / 10));
            var pose = new Pose(_sizeConfiguration.ConvertRealSizeToSimulatedSize(Convert.ToInt32(realPose.X)), _sizeConfiguration.ConvertRealSizeToSimulatedSize(Convert.ToInt32(realPose.Y)), realPose.Heading);


            _graphics.DrawEllipse(_particlePen, Convert.ToInt32(pose.X - radius), Convert.ToInt32(pose.Y - radius), 2 * radius, 2 * radius);
            _graphics.DrawLine(_particlePen, Convert.ToInt32(pose.X), Convert.ToInt32(pose.Y), Convert.ToInt32(pose.X + radius * 3 * Math.Cos(pose.Heading.Rads)), Convert.ToInt32(pose.Y + radius * 3 * Math.Sin(pose.Heading.Rads)));

        }



        private void ViewerControll_SizeChanged(object sender, EventArgs e)
        {
            PrepareControll();
        }

        private void ViewerControll_MouseClick(object sender, MouseEventArgs e)
        {
            var xDif = Size.Width - _validSize.Width;
            var yDif = Size.Height - _validSize.Height;
            var finalX = (e.X - xDif/2);
            var finalY = (e.Y - yDif/2);
            if (finalX >= _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I) &&
                finalX <= _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_A) &&
                finalY >= _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I) &&
                finalY <= _sizeConfiguration.ConvertRealSizeToSimulatedSize(_configs.Field_I + _configs.Field_B))
            {
              //  Robot = new Pose(new System.Windows.Point(_sizeConfiguration.ConvertSimulatedSizeToRealSize(finalX), _sizeConfiguration.ConvertSimulatedSizeToRealSize(finalY)),new Angle(0));
                MessageBox.Show("SimulatedPos(X=" + finalX + ",Y=" + finalY+") RealPos(X="+_sizeConfiguration.ConvertSimulatedSizeToRealSize(finalX)+",Y="+_sizeConfiguration.ConvertSimulatedSizeToRealSize(finalY)+")");
            }

        }








    }
}
