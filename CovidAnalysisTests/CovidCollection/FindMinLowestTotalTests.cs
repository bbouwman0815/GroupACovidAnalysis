using System;
using System.Linq;
using Covid19Analysis.CollectionQueries;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisTests
{
    /// Input ({total tests} of the item in the collection of covid stat) |Expected value (Lowest amount of total test in the collection
    /// Input: {}                                                         |OutPut: ArgumentOutOfRangeException
    /// Input: {0, 222, 200, 300}                                         |OutPut: 0
    /// Input: {555, 555, 200, 0}                                         |OutPut: 0
    /// Input: {33, 2, 0, 300, 400}                                       |OutPut: 0
    [TestClass]
    public class FindMinLowestTotalTests
    {
        #region Methods

        [TestMethod]
        public void TestWithEmptyCollection()
        {
            var data = new TotalCovidStats();

            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => FindMinMax.FindLowestTotalTests(data.ToList())
            );
        }


        [TestMethod]
        public void TestWithMinAtStartOfCollection()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 0, 0, 125, 0, 0);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 222, 0, 77, 0, 0);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 100, 100, 55, 100, 100);
            var day4 = new DailyCovidStat(new DateTime(2020, 8, 18), "GA", 200, 100, 111, 100, 100);

            data.Add(day1);
            data.Add(day2);
            data.Add(day3);
            data.Add(day4);

            var lowestTotalTests = FindMinMax.FindLowestTotalTests(data.ToList());

            Assert.AreEqual(0, lowestTotalTests.TotalTests);
        }

        [TestMethod]
        public void TestWithMaxAtEndOfCollection()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 333, 222, 125, 0, 0);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 111, 444, 77, 0, 0);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 100, 100, 55, 100, 100);
            var day4 = new DailyCovidStat(new DateTime(2020, 8, 18), "GA", 0, 0, 111, 100, 100);

            data.Add(day1);
            data.Add(day2);
            data.Add(day3);
            data.Add(day4);

            var lowestTotalTests = FindMinMax.FindLowestTotalTests(data.ToList());

            Assert.AreEqual(0, lowestTotalTests.TotalTests);
        }

        [TestMethod]
        public void TestWithMaxInMiddleOfCollection()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 12), "GA", 33, 0, 0, 0, 0);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 2, 0, 0, 0, 0);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 0, 0, 123, 100, 100);
            var day4 = new DailyCovidStat(new DateTime(2020, 8, 18), "GA", 200, 100, 44, 100, 100);
            var day5 = new DailyCovidStat(new DateTime(2020, 8, 19), "GA", 300, 100, 208, 100, 100);
            
            data.Add(day1);
            data.Add(day2);
            data.Add(day3);
            data.Add(day4);
            data.Add(day5);
            
            var lowestTotalTests = FindMinMax.FindLowestTotalTests(data.ToList());

            Assert.AreEqual(0, lowestTotalTests.TotalTests);
        }



        #endregion
    }
}