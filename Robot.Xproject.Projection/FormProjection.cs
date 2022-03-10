using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ProjectionUsingIMU
{
    public partial class FormProjection : Form
    {
        private VideoCapture _capture = null;
        private bool _captureInProgress;
        private Mat _frame;
        public FormProjection()
        {
            InitializeComponent();
            CvInvoke.UseOpenCL = false;
            try
            {
                _capture = new VideoCapture();
                _capture.SetCaptureProperty(CapProp.FrameWidth, 320);
                _capture.SetCaptureProperty(CapProp.FrameHeight,240);
                _capture.ImageGrabbed += ProcessFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
            _frame = new Mat();
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame, 0);
              
                UpdateImage(_frame.ToImage<Bgr,byte>());
            }
        }
        
        private int _alpha, _beta, _gamma, _dist, _f;
        private Image<Bgr, byte> _image;
        private Image<Bgr, byte> _imageSource;
        public double Pitch, Yaw, Roll;
        private void FormProjection_Load(object sender, EventArgs e)
        {

            _alpha = _beta = _gamma = 90;
            _dist = _f = 500;
        }

        private int _height = 50;
        private delegate void SetTextDeleg(string text);
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string data = serialPort1.ReadLine();
            BeginInvoke(new SetTextDeleg(si_DataReceived), data);
        }

        private void si_DataReceived(string data)
        {
            try
            {
                var splitted = data.Split('*');

                Roll = Convert.ToDouble(splitted[0]);
                Pitch = Convert.ToDouble(splitted[1]);
                Yaw = Convert.ToDouble(splitted[2]);

                textBox_roll.Text = splitted[0];
                textBox_pitch.Text = splitted[1];
                textBox_yaw.Text = splitted[2];
            }
            catch
            {


            }

        }

        private double _k = 1;

        public void UpdateImage(Image<Bgr, byte> input)
        {
            _image = input;
            _imageSource = input.Clone();

            _alpha = 38-Convert.ToInt32(1.0*Convert.ToInt32(Pitch));
            _beta = 90 ;
            _gamma = 90 + Convert.ToInt32(1.4 * Convert.ToInt32(Roll));
            _f = 80;
            _dist = 500;
            _height = 180;
            _k = Convert.ToDouble(1);
            double focalLength, dist, alpha, beta, gamma;

            alpha = ((double)_alpha - 90) * Math.PI / 180;
            beta = ((double)_beta - 90) * Math.PI / 180;
            gamma = ((double)_gamma - 90) * Math.PI / 180;
            focalLength = (double)_f;
            dist = (double)_dist;
            double w = 320, h = 240;

            // Projecion matrix 2D -> 3D
            Matrix<double> A1 = new Matrix<double>(4, 3)
            {
                [0, 0] = 1,
                [0, 1] = 0,
                [0, 2] = -w / 2,
                [1, 0] = 0,
                [1, 1] = 1,
                [1, 2] = -h / 2,
                [2, 0] = 0,
                [2, 1] = 0,
                [2, 2] = 0,
                [3, 0] = 0,
                [3, 1] = 0,
                [3, 2] = 1
            };

            // Rotation matrices Rx, Ry, Rz
            Matrix<double> RX = new Matrix<double>(4, 4)
            {
                [0, 0] = 1,
                [0, 1] = 0,
                [0, 2] = 0,
                [0, 3] = 0,
                [1, 0] = 0,
                [1, 1] = Math.Cos(alpha),
                [1, 2] = -Math.Sin(alpha),
                [1, 3] = 0,
                [2, 0] = 0,
                [2, 1] = Math.Sin(alpha),
                [2, 2] = Math.Cos(alpha),
                [2, 3] = 0,
                [3, 0] = 0,
                [3, 1] = 0,
                [3, 2] = 0,
                [3, 3] = 1,
            };


            Matrix<double> RY = new Matrix<double>(4, 4)
            {
                [0, 0] = Math.Cos(beta),
                [0, 1] = 0,
                [0, 2] = -Math.Sin(beta),
                [0, 3] = 0,
                [1, 0] = 0,
                [1, 1] = 1,
                [1, 2] = 0,
                [1, 3] = 0,
                [2, 0] = Math.Sin(beta),
                [2, 1] = 0,
                [2, 2] = Math.Cos(beta),
                [2, 3] = 0,
                [3, 0] = 0,
                [3, 1] = 0,
                [3, 2] = 0,
                [3, 3] = 1,
            };

            Matrix<double> RZ = new Matrix<double>(4, 4)
            {
                [0, 0] = Math.Cos(gamma),
                [0, 1] = -Math.Sin(gamma),
                [0, 2] = 0,
                [0, 3] = 0,
                [1, 0] = Math.Sin(gamma),
                [1, 1] = Math.Cos(gamma),
                [1, 2] = 0,
                [1, 3] = 0,
                [2, 0] = 0,
                [2, 1] = 0,
                [2, 2] = 1,
                [2, 3] = 0,
                [3, 0] = 0,
                [3, 1] = 0,
                [3, 2] = 0,
                [3, 3] = 1,
            };


            // Rotation matrices Rx, Ry, Rz
            var R = RX * RY * RZ;

            var sin = Math.Sin(alpha);
            // T - translation matrix
            Matrix<double> T = new Matrix<double>(4, 4)
            {
                [0, 0] = 1,
                [0, 1] = 0,
                [0, 2] = 0,
                [0, 3] = 0,
                [1, 0] = 0,
                [1, 1] = 1,
                [1, 2] = 0,
                [1, 3] = 0,
                [2, 0] = 0,
                [2, 1] = 0,
                [2, 2] = 1,
                [2, 3] = _k * (-_height / sin),
                [3, 0] = 0,
                [3, 1] = 0,
                [3, 2] = 0,
                [3, 3] = 1,
            };
            // K - intrinsic matrix
            Matrix<double> K = new Matrix<double>(3, 4)
            {
                [0, 0] = focalLength * 4,
                [0, 1] = 0,
                [0, 2] = w / 2,
                [0, 3] = 0,
                [1, 0] = 0,
                [1, 1] = focalLength * 3,
                [1, 2] = h / 2,
                [1, 3] = 0,
                [2, 0] = 0,
                [2, 1] = 0,
                [2, 2] = 1,
                [2, 3] = 0,

            };

            var transformationMat = K * (T * (R * A1));
            var newImage = _image.WarpPerspective(transformationMat, Emgu.CV.CvEnum.Inter.Cubic, Emgu.CV.CvEnum.Warp.InverseMap, Emgu.CV.CvEnum.BorderType.Transparent, new Bgr(0, 0, 0));

            imageBox1.Image = newImage;
            imageBox2.Image = _imageSource;
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start")
            {
                serialPort1.Open();
                button1.Text = "Stop";
                _capture.Start();
            }
            else
            {
                serialPort1.Close();
                button1.Text = "Start";
                _capture.Stop();
            }
        }
    }
}
