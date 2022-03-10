using Robot.Localization.Configuration;
using Robot.Localization.Model;
using Point = System.Windows.Point;

namespace Robot.Localization
{
    public class MonteCarloLocalization
    {
        public int ParticleCount { get; set; }
        public Pose Robot { get; set; }
        public Pose[] Particles { get; set; }
        public Angle DeltaTheta { get; set; }
        public Map Map { get; set; }
        private LocalizationSettings _configs = new LocalizationSettings();
        
        public MonteCarloLocalization()
        {
          

            Map = new Map();
            ParticleCount = _configs.ParticleCount;
            DeltaTheta = Angle.FromDegrees(5.0);
            Particles = new Pose[ParticleCount];
            Robot=new Pose(_configs.Field_I+_configs.Field_A/2,_configs.Field_I+_configs.Field_B/2,new Angle(0));
            InitializeParticles();
        }

        private void InitializeParticles()
        {
            for (var i = 0; i < ParticleCount; i++)
            {
                Particles[i] = CreateRandomPose();
            }
        }

        private Pose CreateRandomPose()
        {
            var angleBucketCount = (int)(360 / DeltaTheta.Degrees);
            var heading = Angle.FromDegrees(Sampler.Sampler.Random.Next(angleBucketCount) * DeltaTheta.Degrees);

            double x, y;
            do
            {
                x = Map.XMin + Sampler.Sampler.Random.NextDouble() * (Map.XMax - Map.XMin);
                y = Map.YMin + Sampler.Sampler.Random.NextDouble() * (Map.YMax - Map.YMin);
            }
            while (!Map.IsInside(x, y));

            return new Pose(x, y, heading);
        }


    }
}
