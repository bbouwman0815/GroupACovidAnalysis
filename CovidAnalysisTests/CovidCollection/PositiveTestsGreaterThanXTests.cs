using System;
using Covid19Analysis.CollectionQueries;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisTests.CovidCollection
{
    /// Input ({Positive tests} in a collection of covid stats |Expected Value (the number of positive tests below the threshold)
    /// Input: {}                                              |Output: ArgumentOutOfRangeException
    /// Input: {1, 499, 500, 501}                              |Output: 2
    /// Input: {501, 1000, 500, 9999}                          |Output: 0
    /// Input: {499}                                           |Output: 1
    [TestClass]
    public class PositiveTestsLessThanXTests
    {
        #region Methods

        [TestMethod]
        public void TestLessThan500WithEmptyCollection()
        {
            var data = new TotalCovidStats();

            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => CalculateAverages.CalculatePositiveTestsLessThanX(data, 500)
            );
        }

        [TestMethod]
        public void TestLessThan500WithCollectionOfSizeThreeOnBoundaries()
        {
            var data = new TotalCovidStats();

            var day0 = new DailyCovidStat(new DateTime(2020, 8, 14), "GA", 1, 0, 100, 100, 100);
            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 499, 0, 100, 100, 100);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 500, 0, 100, 100, 100);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 501, 0, 100, 100, 100);

            data.Add(day0);
            data.Add(day1);
            data.Add(day2);
            data.Add(day3);

            var total = CalculateAverages.CalculatePositiveTestsLessThanX(data, 500);

            Assert.AreEqual(2, total);
        }

        [TestMethod]
        public void TestLessThan500WithZeroResultingDays()
        {
            var data = new TotalCovidStats();

            var day0 = new DailyCovidStat(new DateTime(2020, 8, 14), "GA", 501, 0, 100, 100, 100);
            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 1000, 0, 100, 100, 100);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 500, 0, 100, 100, 100);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 9999, 0, 100, 100, 100);

            data.Add(day0);
            data.Add(day1);
            data.Add(day2);
            data.Add(day3);

            var total = CalculateAverages.CalculatePositiveTestsLessThanX(data, 500);

            Assert.AreEqual(0, total);
        }

        [TestMethod]
        public void TestLessThan500WithCollectionOfSizeOne()
        {
            var data = new TotalCovidStats();

            var day = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 499, 0, 100, 100, 100);
            data.Add(day);

            var total = CalculateAverages.CalculatePositiveTestsLessThanX(data, 500);

            Assert.AreEqual(1, total);
        }

        #endregion
    }
}