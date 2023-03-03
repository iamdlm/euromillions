using EuromillionsCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EuromillionsCore.Extensions
{
    public static class MathExtensions
    {
        public static int StandardDeviation(List<Draw> draws, int average)
        {
            double stdDev = Math.Sqrt(draws.Sum(d => (d.Numbers.Sum() - average) * (d.Numbers.Sum() - average)) / draws.Count());

            return Convert.ToInt32(stdDev);
        }

        public static int StandardDeviation(List<int> list, int average)
        {
            double stdDev = Math.Sqrt(list.Average(v => Math.Pow(v - average, 2)));

            return Convert.ToInt32(stdDev);
        }

        public static int LowerLimit(int average, int stdDev)
        {
            return average - stdDev;
        }

        public static int UpperLimit(int average, int stdDev)
        {
            return average + stdDev;
        }
    }
}
