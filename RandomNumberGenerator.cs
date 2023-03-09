using System;

namespace SnakeGame
{
    public static class RandomNumberGenerator
    {
        static Random rand;

        /// <summary>
        /// Initializes the random number generator
        /// </summary>
        public static void Initialize()
        {
            rand = new Random();
        }

        public static int Next(int maxValue)
        {
            return rand.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return rand.Next(minValue, maxValue);
        }

        public static float NextFloat(float maxValue)
        {
            return (float)rand.NextDouble() * maxValue;
        }

        public static float NextFloat(float minValue, float maxValue)
        {
            return minValue + (float)rand.NextDouble() * (maxValue - minValue);
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0
        /// </summary>
        /// <returns>the random number</returns>
        public static double NextDouble()
        {
            return rand.NextDouble();
        }
    }
}
