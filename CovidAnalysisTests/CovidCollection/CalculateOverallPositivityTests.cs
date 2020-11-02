using System;
using System.Linq;
using Covid19Analysis.CollectionQueries;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisTests
{
    /// Input ({Positive tests / total Tests} int Daily covid Stat |Expected Output (Positivity Rate)
    /// Input: {}                                                  |OutPut: ArgumentOutOfRangeException
    /// Input: {55/160, 222/900, 100/500}                          |OutPut: 0.24
    /// Input: {44/232}                                            |OutPut: 0.18
    [TestClass]
    public class CalculateOverallPositivityTests
    {
        #region Methods

        [TestMethod]
        public void TestWithEmptyCollection()
        {
            var data = new TotalCovidStats();

            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => CalculateAverages.CalculateAverageCurrentlyHospitalized(data.ToList())
            );
        }

        [TestMethod]
        public void TestWithMultipleDays()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 55, 105, 125, 0, 0);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 222, 678, 77, 0, 0);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 100, 400, 55, 100, 100);

            data.Add(day1);
            data.Add(day2);
            data.Add(day3);

            var overallPos = CalculateAverages.CalculateOverallPositivity(data.ToList());

            Assert.AreEqual(0.24, overallPos, .01);
        }

        [TestMethod]
        public void TestWithOneDay()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 12), "GA", 44, 188, 0, 0, 0);

            data.Add(day1);

            var overallPos = CalculateAverages.CalculateOverallPositivity(data.ToList());

            Assert.AreEqual(0.18, overallPos, .01);
        }



        #endregion
    }
}