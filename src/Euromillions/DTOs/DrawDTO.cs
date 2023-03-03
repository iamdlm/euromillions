using System;
using System.Linq;

namespace Euromillions.DTOs
{
    public class DrawDTO
    {
        public DateTime Date { get; set; }

        public string Ball_1 { get; set; }
        public string Ball_2 { get; set; }
        public string Ball_3 { get; set; }
        public string Ball_4 { get; set; }
        public string Ball_5 { get; set; }

        public string Star_1 { get; set; }
        public string Star_2 { get; set; }
    }
}
