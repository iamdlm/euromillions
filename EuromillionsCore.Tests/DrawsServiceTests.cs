using EuromillionsCore.Models;
using EuromillionsCore.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace EuromillionsCore.Tests
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
        public void IsDrawValid_SumInRange_True()
        {
            var drawsService = new DrawsService(config);

            Draw draw = new Draw
            {
                Date = DateTime.Now,
                Numbers = new int[] { 4, 10, 20, 23, 45 },
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
                Numbers = new int[] { 4, 10, 20, 23, 45 },
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
                Numbers = new int[] { 21, 22, 23, 24, 25 },
                Stars = new int[] { 2, 3 }
            };

            bool result = drawsService.IsDrawValid(draw);

            Assert.IsFalse(result, "Draw can't be sequential number.");
        }
    }
}
