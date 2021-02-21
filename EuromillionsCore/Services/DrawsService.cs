using EuromillionsCore.Models;
using EuromillionsCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using EuromillionsCore.Extensions;
using Microsoft.Extensions.Configuration;

namespace EuromillionsCore.Services
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

        public List<Draw> Generate(List<Draw> previousDraws)
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

                IsDrawValid(draw, previousDraws);

                draws.Add(draw);
            }

            return draws;
        }        

        public bool IsDrawValid(Draw draw)
        {
            return IsDrawValid(draw, null);
        }

        public bool IsDrawValid(Draw draw, List<Draw> previousDraws)
        {
            if (previousDraws != null)
            {
                // Remove draws already drawn

                if (!IsNotEqualPastDraws(draw, previousDraws))
                {
                    return false;
                }

                int average = MathExtensions.Average(previousDraws);
                int stdDev = MathExtensions.StandardDeviation(previousDraws, average);

                return IsDrawValid(draw, MathExtensions.LowerLimit(average, stdDev), MathExtensions.UpperLimit(average, stdDev));
            }

            return IsDrawValid(draw, NUMBERS_LOWER_LIMIT, NUMBERS_UPPER_LIMIT);
        }

        private bool IsDrawValid(Draw draw, int min, int max)
        {
            // Remove all draws outside of range [min, max]

            if (!IsSumInRange(draw.Numbers, min, max))
            {
                return false;
            }

            // Remove all odd or all even numbers

            if (!IsEvenNumbersCountInRange(draw.Numbers, 2, 3))
            {
                return false;
            }

            // Removed sequential numbers

            if (CountSequentialNumbers(draw.Numbers) == draw.Numbers.Length - 1)
            {
                return false;
            }

            return true;
        }


        private bool IsNotEqualPastDraws(Draw draw, List<Draw> previousDraws)
        {
            if (previousDraws.FirstOrDefault(d => d.Numbers.SequenceEqual(draw.Numbers) && d.Stars.SequenceEqual(draw.Stars)) != null)
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
    }
}
