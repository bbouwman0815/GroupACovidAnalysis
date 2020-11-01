using System;
using System.Linq;
using Covid19Analysis.CollectionQueries;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisTests
{
    /// Input: EmptyCollection        OutPut: ArgumentOutOfRangeException
    /// Input: two positive highs     OutPut: Count == 2
    /// Input: one positive high      Output: Count == 0
    [TestClass]
    public class FindDaysSamePositiveHighTests
    {
        #region Methods

        [TestMethod]
        public void TestWithEmptyCollection()
        {
            var data = new TotalCovidStats();

            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => CheckMultipleMinMax.FindDaysSamePositiveHigh(data.ToList())
            );
        }

        [TestMethod]
        public void TestWithTwoPositiveHighs()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 499, 100, 100, 100);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 10000, 100, 100, 100);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 500, 100, 100, 100);
            var day4 = new DailyCovidStat(new DateTime(2020, 8, 18), "GA", 499, 100, 100, 100);
            var day5 = new DailyCovidStat(new DateTime(2020, 8, 19), "GA", 10000, 100, 100, 100);
            var day6 = new DailyCovidStat(new DateTime(2020, 8, 20), "GA", 500, 100, 100, 100);

            data.Add(day1);
            data.Add(day2);
            data.Add(day3);
            data.Add(day4);
            data.Add(day5);
            data.Add(day6);

            var collectionOfSimilarHighs = CheckMultipleMinMax.FindDaysSamePositiveHigh(data.ToList());

            Assert.AreEqual(2, collectionOfSimilarHighs.Count);
        }

        [TestMethod]
        public void TestWithOnePositiveHigh()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 499, 100, 100, 100);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 10000, 100, 100, 100);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 501, 100, 100, 100);

            data.Add(day1);
            data.Add(day2);
            data.Add(day3);

            var collectionOfSimilarHighs = CheckMultipleMinMax.FindDaysSamePositiveHigh(data.ToList());

            Assert.AreEqual(0, collectionOfSimilarHighs.Count);
        }

        #endregion
    }
}