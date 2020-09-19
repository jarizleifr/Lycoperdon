using System;

namespace Lycoperdon
{
    public static class RNG
    {
        private static Random rng = new Random();

        public static int Int(int max) => rng.Next(max);
        public static int Int(int min, int max) => rng.Next(min, max);

        public static Drum PickRandom(params Drum[] drums) =>
            drums[Int(drums.Length)];
    }
}