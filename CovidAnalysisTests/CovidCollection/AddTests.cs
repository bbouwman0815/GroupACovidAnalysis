using System;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisTests
{
    /// <summary>
    ///     Test Cases
    ///     Input: Null         OutPut: NullReferenceException
    ///     Input: One Day      OutPut: Count == 1
    ///     Input: Three Days   OutPut: Count == 3
    /// </summary>
    [TestClass]
    public class AddTests
    {
        #region Methods

        [TestMethod]
        public void TestAddNullDailyCovidStat()
        {
            var data = new TotalCovidStats();

            DailyCovidStat day = null;

            Assert.ThrowsException<NullReferenceException>(
                () => data.Add(day)
            );
        }

        [TestMethod]
        public void TestAddOneDailyCovidStat()
        {
            var data = new TotalCovidStats();

            var day = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 100, 100, 100, 100, 100);

            data.Add(day);

            Assert.AreEqual(1, data.Count);
        }

        [TestMethod]
        public void TestAddThreeDailyCovidStats()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 100, 100, 100, 100, 100);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 100, 100, 100, 100, 100);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 100, 100, 100, 100, 100);

            data.Add(day1);
            data.Add(day2);
            data.Add(day3);

            Assert.AreEqual(3, data.Count);
        }

        #endregion
    }
}