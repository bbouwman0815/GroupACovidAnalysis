using System;
using Covid19Analysis.CollectionQueries;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisTests
{
    /// Input: EmptyCollection                    Output: ArgumentOutOfRangeException
    /// Input: boundary test (1 in bounds)        Output: PositiveTestsGreaterThanX = 1
    /// Input: test of size 1                     Output: PositiveTestsGreaterThanX = 1
    [TestClass]
    public class PositiveTestsGreaterThanXTests
    {
        #region Methods

        [TestMethod]
        public void TestGreaterThan500WithCollectionOfSizeThreeOnBoundaries()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 499, 0, 100, 100, 100);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 500, 0, 100, 100, 100);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 501, 0, 100, 100, 100);

            data.Add(day1);
            data.Add(day2);
            data.Add(day3);

            var total = CalculateAverages.CalculatePositiveTestsGreaterThanX(data, 500);

            Assert.AreEqual(1, total);
        }

        [TestMethod]
        public void TestGreaterThan500WithCollectionOfSizeOne()
        {
            var data = new TotalCovidStats();

            var day = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 501, 0, 100, 100, 100);
            data.Add(day);

            var total = CalculateAverages.CalculatePositiveTestsGreaterThanX(data, 500);

            Assert.AreEqual(1, total);
        }

        [TestMethod]
        public void TestGreaterThan500WithEmptyCollection()
        {
            var data = new TotalCovidStats();

            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => CalculateAverages.CalculatePositiveTestsGreaterThanX(data, 500)
            );
        }

        #endregion
    }
}