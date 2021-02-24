using EuromillionsCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EuromillionsCore.Extensions
{
    public static class MathExtensions
    {
        public static int Average(List<Draw> draws)
        {
            double sum = draws.Sum(s => s.Numbers.Sum());

            double average = sum / draws.Count();

            return Convert.ToInt32(average);
        }

        public static int StandardDeviation(List<Draw> draws, int average)
        {
            double stdDev = Math.Sqrt(draws.Sum(d => (d.Numbers.Sum() - average) * (d.Numbers.Sum() - average)) / draws.Count());

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
