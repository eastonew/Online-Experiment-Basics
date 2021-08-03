using System;
using System.Collections.Generic;
using System.Text;

namespace MainEnvironment.Core.Helpers
{
    public static class RandomHelper
    {
        private static Random Rand = new Random();

        public static int GetNext(int minVal, int maxVal)
        {
            return Rand.Next(0, maxVal);
        }
    }
}
