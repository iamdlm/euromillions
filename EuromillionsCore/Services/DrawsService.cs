using EuroMillionsAI.DTOs;
using EuroMillionsAI.Models;
using EuromillionsCore.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace EuromillionsCore.Services
{
    public class DrawsService : IDrawsService
    {
        public Draw Generate()
        {
            while (true)
            {
                Draw draw = new Draw();

                IsDrawValid(draw);

                return draw;
            }
        }

        public bool IsDrawValid(Draw draw)
        {
            // Remove all draws outside of range [min, max]

            if (!IsSumInRange(draw.Numbers, 95, 160))
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
