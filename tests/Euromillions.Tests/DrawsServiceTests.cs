using Euromillions.Entities;
using Euromillions.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace Euromillions.Tests
{
    [TestClass]
    public class DrawsServiceTests
    {
        IConfiguration config;

        [TestInitialize]
        public void Init()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            this.config = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                           .AddJsonFile($"appsettings.{environment}.json", optional: true)
                           .AddEnvironmentVariables()
                           .Build();
        }

        [TestMethod]
        public void IsDrawValid_PastDraws_True()
        {
            var drawsService = new DrawsService(config);
            var dataService = new DataService(config);

            Draw draw = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 10, 11, 20, 31, 45 },
                Stars = new int[] { 2, 3 }
            };

            List<Draw> pastDraws = dataService.ReadFile(Entities.Type.Drawn);

            bool result = drawsService.IsDrawValid(draw, pastDraws);

            Assert.IsTrue(result, "Draw was not previously drawn.");
        }

        [TestMethod]
        public void IsDrawValid_PastDraws_False()
        {
            var drawsService = new DrawsService(config);
            var dataService = new DataService(config);

            Draw draw = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 18, 20, 35, 38, 48 },
                Stars = new int[] { 9, 12 }
            };

            List<Draw> pastDraws = dataService.ReadFile(Entities.Type.Drawn);

            bool result = drawsService.IsDrawValid(draw, pastDraws);

            Assert.IsFalse(result, "Draw was previously drawn.");
        }

        [TestMethod]
        public void IsDrawValid_SumInRange_True()
        {
            var drawsService = new DrawsService(config);

            Draw draw = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 4, 10, 20, 33, 45 },
                Stars = new int[] { 2, 3 }
            };

            bool result = drawsService.IsDrawValid(draw);

            Assert.IsTrue(result, "Draw sum is in range.");
        }

        [TestMethod]
        public void IsDrawValid_SumInRange_False()
        {
            var drawsService = new DrawsService(config);

            Draw drawBellowRange = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 1, 2, 10, 12, 25 },
                Stars = new int[] { 4, 5 }
            };

            bool resultBellowRange = drawsService.IsDrawValid(drawBellowRange);

            Draw drawAboveRange = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 23, 30, 35, 40, 41 },
                Stars = new int[] { 1, 7 }
            };

            bool resultAboveRange = drawsService.IsDrawValid(drawAboveRange);

            Assert.IsFalse(resultBellowRange, "Draw bellow range.");
            Assert.IsFalse(resultAboveRange, "Draw above range.");
        }

        [TestMethod]
        public void IsDrawValid_IsEvenNumbersCountInRange_True()
        {
            var drawsService = new DrawsService(config);

            Draw drawEvenOverOdd = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 2, 12, 24, 27, 35 },

                Stars = new int[] { 1, 7 }
            };

            bool resultEvenOverOdd = drawsService.IsDrawValid(drawEvenOverOdd);

            Draw drawOddOverEven = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 1, 13, 25, 26, 32 },
                Stars = new int[] { 1, 7 }
            };

            bool resultOddOverEven = drawsService.IsDrawValid(drawOddOverEven);

            Assert.IsTrue(resultEvenOverOdd, "Draw wih 3 even numbers.");
            Assert.IsTrue(resultOddOverEven, "Draw with 2 even numbers.");
        }

        [TestMethod]
        public void IsDrawValid_IsEvenNumbersCountInRange_False()
        {
            var drawsService = new DrawsService(config);

            Draw drawAllEven = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 2, 10, 24, 36, 42 },
                Stars = new int[] { 1, 7 }
            };

            bool resultAllEven = drawsService.IsDrawValid(drawAllEven);

            Draw drawAllOdd = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 1, 13, 25, 33, 39 },
                Stars = new int[] { 1, 7 }
            };

            bool resultAllOdd = drawsService.IsDrawValid(drawAllOdd);

            Assert.IsFalse(resultAllEven, "Draw can't be all even numbers.");
            Assert.IsFalse(resultAllOdd, "Draw can't be all odd numbers.");
        }

        [TestMethod]
        public void IsDrawValid_IsSequentialNumber_True()
        {
            var drawsService = new DrawsService(config);

            Draw draw = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 4, 10, 20, 33, 45 },
                Stars = new int[] { 2, 3 }
            };

            bool result = drawsService.IsDrawValid(draw);

            Assert.IsTrue(result, "Draw is not sequential number.");
        }

        [TestMethod]
        public void IsDrawValid_IsSequentialNumber_False()
        {
            var drawsService = new DrawsService(config);

            Draw draw = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 23, 24, 25, 26, 27 },
                Stars = new int[] { 2, 3 }
            };

            bool result = drawsService.IsDrawValid(draw);

            Assert.IsFalse(result, "Draw can't be sequential number.");
        }

        [TestMethod]
        public void EvaluatePrize_HasPrize_True()
        {
            var drawsService = new DrawsService(config);

            Draw drawnKey = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 21, 22, 23, 24, 25 },
                Stars = new int[] { 2, 3 }
            };

            Draw firstPrize = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 21, 22, 23, 24, 25 },
                Stars = new int[] { 2, 3 }
            };

            Draw secondPrize = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 21, 22, 23, 24, 25 },
                Stars = new int[] { 2, 4 }
            };

            Draw thirdPrize = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 21, 22, 23, 24, 25 },
                Stars = new int[] { 1, 4 }
            };

            Draw fourthPrize = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 20, 22, 23, 24, 25 },
                Stars = new int[] { 2, 3 }
            };

            Draw fifthPrize = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 20, 22, 23, 24, 25 },
                Stars = new int[] { 2, 4 }
            };

            Draw sixthPrize = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 21, 22, 23, 26, 27 },
                Stars = new int[] { 2, 3 }
            };

            Draw seventhPrize = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 21, 22, 23, 24, 27 },
                Stars = new int[] { 1, 4 }
            };

            Draw eighthPrize = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 21, 22, 27, 28, 29 },
                Stars = new int[] { 2, 3 }
            };

            Draw ninthPrize = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 21, 22, 23, 28, 29 },
                Stars = new int[] { 2, 4 }
            };

            Draw tenthPrize = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 21, 22, 23, 28, 29 },
                Stars = new int[] { 1, 4 }
            };

            Draw eleventhPrize = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 21, 28, 29, 30, 31 },
                Stars = new int[] { 2, 3 }
            };

            Draw twelfthPrize = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 21, 22, 27, 28, 29 },
                Stars = new int[] { 2, 4 }
            };

            Draw thirteenthPrize = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 21, 22, 27, 28, 29 },
                Stars = new int[] { 1, 4 }
            };



            Assert.AreEqual(1, drawsService.EvaluatePrize(drawnKey, firstPrize), "1st prize.");
            Assert.AreEqual(2, drawsService.EvaluatePrize(drawnKey, secondPrize), "2nd prize.");
            Assert.AreEqual(3, drawsService.EvaluatePrize(drawnKey, thirdPrize), "3rd prize.");
            Assert.AreEqual(4, drawsService.EvaluatePrize(drawnKey, fourthPrize), "4th prize.");
            Assert.AreEqual(5, drawsService.EvaluatePrize(drawnKey, fifthPrize), "5th prize.");
            Assert.AreEqual(6, drawsService.EvaluatePrize(drawnKey, sixthPrize), "6th prize.");
            Assert.AreEqual(7, drawsService.EvaluatePrize(drawnKey, seventhPrize), "7th prize.");
            Assert.AreEqual(8, drawsService.EvaluatePrize(drawnKey, eighthPrize), "8th prize.");
            Assert.AreEqual(9, drawsService.EvaluatePrize(drawnKey, ninthPrize), "9th prize.");
            Assert.AreEqual(10, drawsService.EvaluatePrize(drawnKey, tenthPrize), "10th prize.");
            Assert.AreEqual(11, drawsService.EvaluatePrize(drawnKey, eleventhPrize), "11th prize.");
            Assert.AreEqual(12, drawsService.EvaluatePrize(drawnKey, twelfthPrize), "12th prize.");
            Assert.AreEqual(13, drawsService.EvaluatePrize(drawnKey, thirteenthPrize), "13th prize.");
        }

        [TestMethod]
        public void EvaluatePrize_HasPrize_False()
        {
            var drawsService = new DrawsService(config);

            Draw drawnKey = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 21, 22, 23, 24, 25 },
                Stars = new int[] { 2, 3 }
            };

            Draw noMatches = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 11, 12, 17, 18, 19 },
                Stars = new int[] { 1, 4 }
            };

            Draw oneNumberOneStar = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 21, 12, 17, 18, 19 },
                Stars = new int[] { 2, 4 }
            };

            Draw oneNumber = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 21, 12, 17, 18, 19 },
                Stars = new int[] { 1, 4 }
            };

            Draw oneStar = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 20, 12, 17, 18, 19 },
                Stars = new int[] { 2, 4 }
            };


            Assert.AreEqual(0, drawsService.EvaluatePrize(drawnKey, noMatches), "No prize.");
            Assert.AreEqual(0, drawsService.EvaluatePrize(drawnKey, oneNumberOneStar), "No prize.");
            Assert.AreEqual(0, drawsService.EvaluatePrize(drawnKey, oneNumber), "No prize.");
            Assert.AreEqual(0, drawsService.EvaluatePrize(drawnKey, oneStar), "No prize.");
        }
    }
}
