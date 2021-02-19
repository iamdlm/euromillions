using System;
using System.Linq;

namespace EuromillionsCore.Models
{
    public class Draw
    {
        public DateTime Date { get; set; }

        public int[] Numbers { get; set; }

        public int[] Stars { get; set; }

        public Draw()
        {
            this.Numbers = GetSortedArray(50, 5);
            this.Stars = GetSortedArray(12, 2);
            this.Date = DateTime.Now;
        }

        private static int[] GetSortedArray(int length, int numbers)
        {
            int[] arr = PopulateArrayConsecutively(length);

            int[] res = FisherYatesShuffle(arr).Take(numbers).OrderBy(x => x).ToArray();

            return res;
        }

        private static int[] PopulateArrayConsecutively(int length)
        {
            int[] res = new int[length];

            for (int i = 0; i < length; i++)
            {
                res[i] = i + 1;
            }

            return res;
        }

        private static int[] FisherYatesShuffle(int[] data)
        {
            int[] retVal = new int[data.Length];
            int[] ind = new int[data.Length];
            int index;
            Random rand = new Random();

            for (int i = 0; i < data.Length; ++i)
            {
                ind[i] = 0;
            }

            for (int i = 0; i < data.Length; ++i)
            {
                do
                {
                    index = rand.Next(data.Length);
                } while (ind[index] != 0);

                ind[index] = 1;
                retVal[i] = data[index];
            }

            return retVal;
        }
    }
}
