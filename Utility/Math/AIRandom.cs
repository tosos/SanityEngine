using System;
using System.Collections.Generic;
using System.Text;

namespace AIEngine.Utility.Math
{
    class AIRandom
    {
        static Random rng = new Random();

        public static float Uniform()
        {
            return (float)rng.NextDouble();
        }

        public static float Uniform(float min, float max)
        {
            return Uniform() * (max - min) + min;
        }

        public static int UniformInt(int min, int max)
        {
            return (int)System.Math.Round(Uniform() * (max - min) + min);
        }
    }
}
