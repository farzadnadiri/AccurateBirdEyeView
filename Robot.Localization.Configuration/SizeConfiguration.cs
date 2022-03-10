using System;
using System.Drawing;

namespace Robot.Localization.Configuration
{
    public class SizeConfiguration
    {
        private static LocalizationSettings _configs = new LocalizationSettings();
        public Size SimulatedSize;
        public Size RealSize = new Size(_configs.Field_A+2*_configs.Field_I,_configs.Field_B+(2*_configs.Field_I));
        public double RealToSimulatedTransform;
        public double SimulatedToRealTransform;
        public SizeConfiguration(Size simulatedSize)
        {

            SimulatedSize = simulatedSize;
            RealToSimulatedTransform = (double) SimulatedSize.Width/RealSize.Width;
            SimulatedToRealTransform = (double) RealSize.Width/SimulatedSize.Width;
        }


        public int ConvertRealSizeToSimulatedSize(int realSize)
        {
            return Convert.ToInt32(realSize * RealToSimulatedTransform);
        }

        public int ConvertSimulatedSizeToRealSize (int simulatedSize)
        {
            return Convert.ToInt32(simulatedSize * SimulatedToRealTransform);
        }

      

    }
}
