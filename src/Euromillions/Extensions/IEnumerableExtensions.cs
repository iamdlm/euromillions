using Euromillions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Euromillions.Extensions
{
    public static class IEnumerableExtensions
    {
        public static double StandardDeviation<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            var data = source.Select(selector);
            
            int n = data.Count();
            
            double mean = data.Average();
            
            double sum = data.Sum(x => Math.Pow(x - mean, 2));
            
            double stdDev = Math.Sqrt(sum / (n - 1));
            
            return stdDev;
        }

        public static double StandardDeviation<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector, double mean)
        {
            var data = source.Select(selector);

            int n = data.Count();

            double sum = data.Sum(x => Math.Pow(x - mean, 2));

            double stdDev = Math.Sqrt(sum / (n - 1));

            return stdDev;
        }
    }
}
