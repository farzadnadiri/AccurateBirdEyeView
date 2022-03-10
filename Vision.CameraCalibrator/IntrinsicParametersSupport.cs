using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Emgu.CV;

namespace Robot.Vision.CameraCalibrator
{
    public static class IntrinsicParametersSupport
    {
        public static void Save(IntrinsicCameraParameters cameraParameters, string filePath)
        {
            using (TextWriter writer = new StreamWriter(filePath))
            {
                WriteIntrinsics(cameraParameters.IntrinsicMatrix, writer);
                writer.WriteLine();
                WriteDistortionCoeffs(cameraParameters.DistortionCoeffs, writer);
            }
        }

        private static void WriteIntrinsics(Matrix<double> intrinsicMatrix, TextWriter writer)
        {
            writer.WriteLine("# Intrinsic parameters:");
            Write("fx:", intrinsicMatrix[0, 0], writer);
            Write("fy:", intrinsicMatrix[1, 1], writer);
            Write("cx:", intrinsicMatrix[0, 2], writer);
            Write("cy:", intrinsicMatrix[1, 2], writer);
        }

        private static void WriteDistortionCoeffs(Matrix<double> distortionCoeffs, TextWriter writer)
        {
            writer.WriteLine("# Distortion parameters:");
            Write("k1:", distortionCoeffs[0, 0], writer);
            Write("k2:", distortionCoeffs[1, 0], writer);
            Write("k3:", distortionCoeffs[4, 0], writer);
            Write("p1:", distortionCoeffs[2, 0], writer);
            Write("p2:", distortionCoeffs[3, 0], writer);
        }

        private static void Write(string name, double value, TextWriter writer)
        {
            writer.WriteLine(name + "\t" + value.ToString(CultureInfo.InvariantCulture));
        }

        public static IntrinsicCameraParameters Load(string filePath)
        {
            var valuesByName = new Dictionary<string, double>(StringComparer.InvariantCultureIgnoreCase);
            using (TextReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.Length == 0 || line.StartsWith("#"))
                    {
                        continue;
                    }

                    var lineParts = line.Split('\t');
                    var name = lineParts[0];
                    var value = double.Parse(lineParts[1], CultureInfo.InvariantCulture);
                    valuesByName.Add(name, value);
                }
            }

            var cameraParameters = new IntrinsicCameraParameters();

            // Intrinsic parameters
            cameraParameters.IntrinsicMatrix[0, 0] = valuesByName["fx:"];
            cameraParameters.IntrinsicMatrix[1, 1] = valuesByName["fy:"];
            cameraParameters.IntrinsicMatrix[0, 2] = valuesByName["cx:"];
            cameraParameters.IntrinsicMatrix[1, 2] = valuesByName["cy:"];
            cameraParameters.IntrinsicMatrix[2, 2] = 1.0;

            // Distortion coefficients
            cameraParameters.DistortionCoeffs[0, 0] = valuesByName["k1:"];
            cameraParameters.DistortionCoeffs[1, 0] = valuesByName["k2:"];
            cameraParameters.DistortionCoeffs[2, 0] = valuesByName["p1:"];
            cameraParameters.DistortionCoeffs[3, 0] = valuesByName["p2:"];
            cameraParameters.DistortionCoeffs[4, 0] = valuesByName["k3:"];

            return cameraParameters;
        }
    }
}
