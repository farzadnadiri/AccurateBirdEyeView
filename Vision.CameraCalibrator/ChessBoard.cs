using System.Collections.Generic;
using System.Drawing;
using Emgu.CV.Structure;

namespace Robot.Vision.CameraCalibrator
{
    public class ChessBoard
    {
        public int XCount { get; private set; }
        public int YCount { get; private set; }
        public int CornerCount { get; private set; }
        public Size PatternSize { get; private set; }
        public MCvPoint3D32f[] CornerPoints { get; private set; }

        public ChessBoard(int xCount, int yCount)
        {
            XCount = xCount;
            YCount = yCount;

            PatternSize = new Size(this.XCount - 1, this.YCount - 1);
            CornerCount = this.PatternSize.Width * this.PatternSize.Height;

            var cornerPoints = new List<MCvPoint3D32f>(this.CornerCount);
            for (var x = 0; x < this.XCount - 1; x++)
            {
                for (var y = 0; y < this.YCount - 1; y++)
                {
                    var cornerPoint = new MCvPoint3D32f(x * 25, y * 25, 0);
                    cornerPoints.Add(cornerPoint);
                }
            }
            CornerPoints = cornerPoints.ToArray();
        }
    }
}
