using System;
using System.Linq;
using Covid19Analysis.CollectionQueries;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisTests
{
    /// Input: {}                               OutPut: ArgumentOutOfRangeException
    /// Input: {333, 222, 100, 200}             OutPut: 333
    /// Input: {0, 0, 100, 200}                 OutPut: 200
    /// Input: {33, 2, 555, 200, 300}           OutPut: 555
    [TestClass]
    public class FindMaxPositiveTests
    {
        #region Methods

        [TestMethod]
        public void TestWithEmptyCollection()
        {
            var data = new TotalCovidStats();

            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => FindMinMax.FindHighestPositiveCases(data.ToList())
            );
        }


        [TestMethod]
        public void TestWithMaxAtStartOfCollection()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 333, 0, 125, 0, 0);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 222, 0, 77, 0, 0);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 100, 100, 55, 100, 100);
            var day4 = new DailyCovidStat(new DateTime(2020, 8, 18), "GA", 200, 100, 111, 100, 100);

            data.Add(day1);
            data.Add(day2);
            data.Add(day3);
            data.Add(day4);

            var highestPosTest = FindMinMax.FindHighestPositiveCases(data.ToList());

            Assert.AreEqual(333, highestPosTest.PositiveIncrease);
        }

        [TestMethod]
        public void TestWithMaxAtEndOfCollection()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 0, 0, 125, 0, 0);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 0, 0, 77, 0, 0);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 100, 100, 55, 100, 100);
            var day4 = new DailyCovidStat(new DateTime(2020, 8, 18), "GA", 200, 100, 111, 100, 100);

            data.Add(day1);
            data.Add(day2);
            data.Add(day3);
            data.Add(day4);

            var highestPosTest = FindMinMax.FindHighestPositiveCases(data.ToList());

            Assert.AreEqual(200, highestPosTest.PositiveIncrease);
        }

        [TestMethod]
        public void TestWithMaxInMiddleOfCollection()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 12), "GA", 33, 0, 0, 0, 0);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 2, 0, 0, 0, 0);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 555, 100, 123, 100, 100);
            var day4 = new DailyCovidStat(new DateTime(2020, 8, 18), "GA", 200, 100, 44, 100, 100);
            var day5 = new DailyCovidStat(new DateTime(2020, 8, 19), "GA", 300, 100, 208, 100, 100);
            
            data.Add(day1);
            data.Add(day2);
            data.Add(day3);
            data.Add(day4);
            data.Add(day5);
            
            var highestPosTest = FindMinMax.FindHighestPositiveCases(data.ToList());

            Assert.AreEqual(555, highestPosTest.PositiveIncrease);
        }



        #endregion
    }
}