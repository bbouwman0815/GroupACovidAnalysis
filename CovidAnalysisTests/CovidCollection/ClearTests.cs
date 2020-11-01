using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisTests
{
    /// <summary>
    ///     Test Cases
    ///     Input: Empty        OutPut: Count == 0
    ///     Input: One Day      OutPut: Count == 0
    ///     Input: Three Days   OutPut: Count == 0
    /// </summary>
    [TestClass]
    public class ClearTests
    {
        #region Methods

        [TestMethod]
        public void TestClearEmptyCollection()
        {
            var data = new TotalCovidStats();

            data.Clear();

            Assert.AreEqual(0, data.Count);
        }

        [TestMethod]
        public void TestClearCollectionSizeOne()
        {
            var data = new TotalCovidStats();

            var day = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 100, 100, 100, 100);
            data.Add(day);

            data.Clear();

            Assert.AreEqual(0, data.Count);
        }

        [TestMethod]
        public void TestClearCollectionSizeThree()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 100, 100, 100, 100);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 100, 100, 100, 100);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 100, 100, 100, 100);

            data.Add(day1);
            data.Add(day2);
            data.Add(day3);

            data.Clear();

            Assert.AreEqual(0, data.Count);
        }

        #endregion
    }
}