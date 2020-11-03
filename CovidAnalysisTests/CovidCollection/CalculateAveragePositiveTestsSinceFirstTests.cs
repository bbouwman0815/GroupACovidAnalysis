using System;
using System.Linq;
using Covid19Analysis.CollectionQueries;
using Covid19Analysis.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CovidAnalysisTests
{
    /// Input (A collection of covid stats) |Expected value : The correct average for the given set of days
    /// Input: EmptyCollection              |OutPut: ArgumentOutOfRangeException
    /// Input: No Positive Tests at start   |OutPut: 250
    /// Input: No Positive Tests in middle  |Output: 200
    /// Input: No Positive Tests at end     |Output: 180
    /// Input: Gap of days between Data     |Output: 200
    [TestClass]
    public class FindAveragePositiveTestsSinceFirstTests
    {
        #region Methods

        [TestMethod]
        public void TestWithEmptyCollection()
        {
            var data = new TotalCovidStats();

            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => CalculateAverages.CalculateAvgPosTestsSinceFirstPosTest(data.ToList())
            );
        }

        [TestMethod]
        public void TestWithNoPositiveTestsAtStartDays()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 0, 0, 0, 0, 0);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 0, 0, 0, 0, 0);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 100, 100, 100, 100, 100);
            var day4 = new DailyCovidStat(new DateTime(2020, 8, 18), "GA", 200, 100, 100, 100, 100);
            var day5 = new DailyCovidStat(new DateTime(2020, 8, 19), "GA", 300, 100, 100, 100, 100);
            var day6 = new DailyCovidStat(new DateTime(2020, 8, 20), "GA", 400, 100, 100, 100, 100);

            data.Add(day1);
            data.Add(day2);
            data.Add(day3);
            data.Add(day4);
            data.Add(day5);
            data.Add(day6);

            var averagePosTests = CalculateAverages.CalculateAvgPosTestsSinceFirstPosTest(data.ToList());

            Assert.AreEqual(250, averagePosTests);
        }

        [TestMethod]
        public void TestWithNoPositiveTestsInMiddleOfDays()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 100, 0, 100,0, 0);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 500, 0, 100, 0, 0);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 0, 100, 100, 100, 100);
            var day4 = new DailyCovidStat(new DateTime(2020, 8, 18), "GA", 0, 100, 100, 100, 100);
            var day5 = new DailyCovidStat(new DateTime(2020, 8, 19), "GA", 400, 100, 100, 100, 100);

            data.Add(day1);
            data.Add(day2);
            data.Add(day3);
            data.Add(day4);
            data.Add(day5);

            var averagePosTests = CalculateAverages.CalculateAvgPosTestsSinceFirstPosTest(data.ToList());

            Assert.AreEqual(200, averagePosTests);
        }

        [TestMethod]
        public void TestWithNoPositiveTestsAtEnd()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 100, 0, 0, 0, 0);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 500, 0, 0, 0, 0);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 17), "GA", 300, 0, 100, 100, 100);
            var day4 = new DailyCovidStat(new DateTime(2020, 8, 18), "GA", 0, 0, 100, 100, 100);
            var day5 = new DailyCovidStat(new DateTime(2020, 8, 19), "GA", 0, 0, 100, 100, 100);

            data.Add(day1);
            data.Add(day2);
            data.Add(day3);
            data.Add(day4);
            data.Add(day5);

            var averagePosTests = CalculateAverages.CalculateAvgPosTestsSinceFirstPosTest(data.ToList());

            Assert.AreEqual(180, averagePosTests);
        }

        [TestMethod]
        public void TestWithDateGap()
        {
            var data = new TotalCovidStats();

            var day1 = new DailyCovidStat(new DateTime(2020, 8, 15), "GA", 600, 0, 0, 0, 0);
            var day2 = new DailyCovidStat(new DateTime(2020, 8, 16), "GA", 500, 0, 0, 0, 0);
            var day3 = new DailyCovidStat(new DateTime(2020, 8, 19), "GA", 300, 100, 0, 100, 100);
            var day4 = new DailyCovidStat(new DateTime(2020, 8, 20), "GA", 0, 100, 0, 100, 100);
            var day5 = new DailyCovidStat(new DateTime(2020, 8, 21), "GA", 0, 100, 0, 100, 100);

            data.Add(day1);
            data.Add(day2);
            data.Add(day3);
            data.Add(day4);
            data.Add(day5);

            var averagePosTests = CalculateAverages.CalculateAvgPosTestsSinceFirstPosTest(data.ToList());

            Assert.AreEqual(200, averagePosTests);
        }

        #endregion
    }
}