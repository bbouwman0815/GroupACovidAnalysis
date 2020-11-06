using System;
using System.Linq;
using Covid19Analysis.CollectionQueries;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisTests.CovidCollection
{
    /// Input ({hospitalized} in  statistics list| Expected Output Th average of hospitalized values
    /// Input: {}                                | OutPut: ArgumentOutOfRangeException
    /// Input: {125, 77, 55, 111, 144, 222}      | OutPut: 122.33 repeating
    /// Input: {0, 0, 123, 44, 208}              | OutPut: 46.87
    [TestClass]
    public class CalculateAverageCurrentlyHospitalized
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

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 0, 0, 125, 0, 0);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 0, 0, 77, 0, 0);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 100, 100, 55, 100, 100);
            var day4 = new DailyCovidStat(new DateTime(2020, 8, 18), "GA", 200, 100, 111, 100, 100);
            var day5 = new DailyCovidStat(new DateTime(2020, 8, 19), "GA", 300, 100, 144, 100, 100);
            var day6 = new DailyCovidStat(new DateTime(2020, 8, 20), "GA", 400, 100, 222, 100, 100);

            data.Add(day1);
            data.Add(day2);
            data.Add(day3);
            data.Add(day4);
            data.Add(day5);
            data.Add(day6);

            var averagePosTests = CalculateAverages.CalculateAverageCurrentlyHospitalized(data.ToList());

            Assert.AreEqual(122.33, averagePosTests, .01);
        }

        [TestMethod]
        public void TestWithMultipleDaysAndDateGap()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 12), "GA", 0, 0, 0, 0, 0);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 0, 0, 0, 0, 0);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 100, 100, 123, 100, 100);
            var day4 = new DailyCovidStat(new DateTime(2020, 8, 18), "GA", 200, 100, 44, 100, 100);
            var day5 = new DailyCovidStat(new DateTime(2020, 8, 19), "GA", 300, 100, 208, 100, 100);
            
            data.Add(day1);
            data.Add(day2);
            data.Add(day3);
            data.Add(day4);
            data.Add(day5);
            
            var averagePosTests = CalculateAverages.CalculateAverageCurrentlyHospitalized(data.ToList());

            Assert.AreEqual(46.87, averagePosTests, .01);
        }



        #endregion
    }
}