using System.Globalization;
using System.IO;
using Emgu.CV;

namespace Robot.Vision.CameraCalibrator
{
    public class HomographyMatrixSupport
    {
        public static void Save(HomographyMatrix homographyMatrix, string filePath)
        {
            using (TextWriter writer = new StreamWriter(filePath))
            {
                for (var x = 0; x < 3; x++)
                {
                    for (var y = 0; y < 3; y++)
                    {
                        writer.WriteLine(homographyMatrix[x, y].ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
        }

        public static HomographyMatrix Load(string filePath)
        {
            var homographyMatrix = new HomographyMatrix();
            using (TextReader reader = new StreamReader(filePath))
            {
                for (var x = 0; x < 3; x++)
                {
                    for (var y = 0; y < 3; y++)
                    {
                        homographyMatrix[x, y] = GetNextValue(reader);
                    }
                }
            }

            return homographyMatrix;
        }

        private static double GetNextValue(TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.Length == 0 || line.StartsWith("#"))
                {
                    continue;
                }

                var value = double.Parse(line, CultureInfo.InvariantCulture);
                return value;
            }

            throw new EndOfStreamException("Unexpected end of file");
        }
    }
}

