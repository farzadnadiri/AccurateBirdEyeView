using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.Structure;

namespace BirdEyeView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int _alpha, _beta, _gamma, _dist, _f;
        private Image<Bgr, byte> _image;
        private Image<Bgr, byte> _imageSource;
        private int _height = 50;
        private double _k = 1;
        private void Form1_Load(object sender, EventArgs e)
        {



            _alpha = _beta = _gamma = 90;
            _dist = _f = 500;

        }

        public void UpdateImage()
        {

            _image = new Image<Bgr, byte>(new Bitmap("D:/1.jpg"));
            _imageSource = new Image<Bgr, byte>(new Bitmap("D:/1.jpg"));



            _alpha = trackBar1.Value;
            _beta = trackBar2.Value;
            _gamma = trackBar3.Value;
            _f = trackBar4.Value;
            _dist = trackBar5.Value;
            _height = trackBar6.Value;
            _k = Convert.ToDouble(numericUpDown1.Value);
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

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            UpdateImage();
            label1.Text = trackBar1.Value.ToString();
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            UpdateImage();
            label6.Text = trackBar6.Value.ToString();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            UpdateImage();
            label2.Text = trackBar2.Value.ToString();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            UpdateImage();
            label3.Text = trackBar3.Value.ToString();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            UpdateImage();
            label4.Text = trackBar4.Value.ToString();

        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            UpdateImage();
            label5.Text = trackBar5.Value.ToString();
        }
    }
}
