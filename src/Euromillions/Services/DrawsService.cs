using Euromillions.Entities;
using Euromillions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Euromillions.Extensions;
using Microsoft.Extensions.Configuration;

namespace Euromillions.Services
{
    public class DrawsService : IDrawsService
    {
        private readonly int NUMBERS_LOWER_LIMIT = 95;
        private readonly int NUMBERS_UPPER_LIMIT = 160;

        IConfiguration config;

        public DrawsService(IConfiguration _config)
        {
            this.config = _config;
        }

        public List<Draw> Generate()
        {
            return Generate(null);
        }

        public List<Draw> Generate(List<Draw> pastDraws)
        {
            int keys = 1;

            try
            {
                keys = Convert.ToInt32(config.GetSection("NumberOfKeys").Value);
            }
            catch (Exception)
            {

            }

            List<Draw> draws = new List<Draw>();

            for (int i = 0; i < keys; i++)
            {
                Draw draw = new Draw();

                while (!IsDrawValid(draw, pastDraws))
                {
                    draw = new Draw();
                }

                draws.Add(draw);
            }

            Console.WriteLine("New keys generated.");

            return draws;
        }

        public bool IsDrawValid(Draw draw)
        {
            return IsDrawValid(draw, null);
        }

        public bool IsDrawValid(Draw draw, List<Draw> pastDraws)
        {
            if (pastDraws != null)
            {
                // Ignore draws with past big prizes

                if (!IsPointsInRange(draw, pastDraws))
                {
                    return false;
                }
                    
                // Ignore draws already drawn

                if (!IsNotEqualPastDraws(draw, pastDraws))
                {
                    return false;
                }

                int average = (int)Math.Round(pastDraws.Average(x => x.Numbers.Sum()));

                int stdDev = MathExtensions.StandardDeviation(pastDraws, average);

                return IsDrawValid(draw, MathExtensions.LowerLimit(average, stdDev), MathExtensions.UpperLimit(average, stdDev));
            }

            return IsDrawValid(draw, NUMBERS_LOWER_LIMIT, NUMBERS_UPPER_LIMIT);
        }

        public int EvaluatePrize(Draw draw, Draw prize)
        {
            int result = 0;

            int numbers = draw.Numbers.Intersect(prize.Numbers).Count();
            int stars = draw.Stars.Intersect(prize.Stars).Count();

            if (numbers == 5)
            {
                if (stars == 2)
                {
                    return 1;
                }

                if (stars == 1)
                {
                    return 2;
                }

                if (stars == 0)
                {
                    return 3;
                }
            }

            if (numbers == 4)
            {
                if (stars == 2)
                {
                    return 4;
                }

                if (stars == 1)
                {
                    return 5;
                }
            }

            if (numbers == 3)
            {
                if (stars == 2)
                {
                    return 6;
                }
            }

            if (numbers == 4)
            {
                if (stars == 0)
                {
                    return 7;
                }
            }

            if (numbers == 2)
            {
                if (stars == 2)
                {
                    return 8;
                }
            }

            if (numbers == 3)
            {
                if (stars == 1)
                {
                    return 9;
                }

                if (stars == 0)
                {
                    return 10;
                }
            }

            if (numbers == 1)
            {
                if (stars == 2)
                {
                    return 11;
                }
            }

            if (numbers == 2)
            {
                if (stars == 1)
                {
                    return 12;
                }
            }

            if (numbers == 2)
            {
                if (stars == 0)
                {
                    return 13;
                }
            }

            return result;
        }

        public static int EvaluatePoints(Draw draw, Draw prize)
        {
            int result = 0;

            int numbers = draw.Numbers.Intersect(prize.Numbers).Count();
            int stars = draw.Stars.Intersect(prize.Stars).Count();

            if (numbers == 5)
            {
                if (stars == 2)
                {
                    return 139838160;
                }

                if (stars == 1)
                {
                    return 6991908;
                }

                if (stars == 0)
                {
                    return 3107515;
                }
            }

            if (numbers == 4)
            {
                if (stars == 2)
                {
                    return 621503;
                }

                if (stars == 1)
                {
                    return 31076;
                }
            }

            if (numbers == 3)
            {
                if (stars == 2)
                {
                    return 14126;
                }
            }

            if (numbers == 4)
            {
                if (stars == 0)
                {
                    return 13821;
                }
            }

            if (numbers == 2)
            {
                if (stars == 2)
                {
                    return 986;
                }
            }

            if (numbers == 3)
            {
                if (stars == 1)
                {
                    return 707;
                }

                if (stars == 0)
                {
                    return 314;
                }
            }

            if (numbers == 1)
            {
                if (stars == 2)
                {
                    return 188;
                }
            }

            if (numbers == 2)
            {
                if (stars == 1)
                {
                    return 50;
                }
            }

            if (numbers == 2)
            {
                if (stars == 0)
                {
                    return 22;
                }
            }

            return result;
        }

        private bool IsDrawValid(Draw draw, int min, int max)
        {
            // Ignore all draws outside of range [min, max]

            if (!IsSumInRange(draw.Numbers, min, max))
            {
                return false;
            }

            // Ignore all patterns different from 3-odd-2-even or 3-even-2-odd

            if (!IsEvenNumbersCountInRange(draw.Numbers, 2, 3))
            {
                return false;
            }

            // Ignore all patterns different from 3-low-2-high or 2-low-3-high

            if (!IsLowNumbersCountInRange(draw.Numbers, 2, 3))
            {
                return false;
            }

            // Ignore sequential keys

            if (CountSequentialNumbers(draw.Numbers) == draw.Numbers.Length - 1)
            {
                return false;
            }

            return true;
        }

        private bool IsPointsInRange(Draw draw, List<Draw> pastDraws)
        {
            List<int> pastDrawsPoints = CalculatePastDrawsPoints(pastDraws);

            int pastDrawsPointsAvg = Convert.ToInt32(pastDrawsPoints.Average());
            int pastDrawsPointsStd = MathExtensions.StandardDeviation(pastDrawsPoints, pastDrawsPointsAvg);

            int drawPointsAvg = CalculateDrawPoints(draw, pastDraws);

            return drawPointsAvg >= MathExtensions.LowerLimit(pastDrawsPointsAvg, pastDrawsPointsStd) && 
                drawPointsAvg <= MathExtensions.UpperLimit(pastDrawsPointsAvg, pastDrawsPointsStd);
        }

        private static List<int> CalculatePastDrawsPoints(List<Draw> pastDraws)
        {
            List<int> points = new List<int>();

            for (int i = 0; i < pastDraws.Count; i++)
            {
                if (i == 0) { continue; }

                List<Draw> drawsBefore = pastDraws.Where(w => w.Date < pastDraws[i].Date).ToList();

                points.Add(CalculateDrawPoints(pastDraws[i], drawsBefore));
            }

            return points;
        }

        private static int CalculateDrawPoints(Draw draw, List<Draw> pastDraws)
        {
            int points = 0;

            foreach (Draw pastDraw in pastDraws)
            {
                points += EvaluatePoints(draw, pastDraw);
            }

            return points;
        }

        private bool IsNotEqualPastDraws(Draw draw, List<Draw> pastDraws)
        {
            if (pastDraws.FirstOrDefault(d => d.Numbers.SequenceEqual(draw.Numbers) && d.Stars.SequenceEqual(draw.Stars)) != null)
            {
                return false;
            }

            return true;
        }

        private static bool IsSumInRange(int[] arr, int min, int max)
        {
            return arr.Sum() >= min && arr.Sum() <= max;
        }

        private static bool IsEvenNumbersCountInRange(int[] arr, int min, int max)
        {
            int evenCount = CountEvenNumbers(arr);

            return evenCount >= min && evenCount <= max;
        }

        private static int CountEvenNumbers(int[] arr)
        {
            int evenCount = 0;

            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i] % 2 == 0)
                {
                    evenCount++;
                }
            }

            return evenCount;
        }

        private static int CountSequentialNumbers(int[] arr)
        {
            int seq = 0;

            for (var i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i] + 1 == arr[i + 1])
                {
                    seq++;
                }
            }

            return seq;
        }

        private static bool IsLowNumbersCountInRange(int[] arr, int min, int max)
        {
            int lowCount = CountLowNumbers(arr);

            return lowCount >= min && lowCount <= max;
        }

        private static int CountLowNumbers(int[] arr)
        {
            int lowCount = 0;

            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i] <= 25)
                {
                    lowCount++;
                }
            }

            return lowCount;
        }
    }
}
