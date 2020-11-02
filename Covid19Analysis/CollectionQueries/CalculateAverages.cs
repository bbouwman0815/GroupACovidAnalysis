using System;
using System.Collections.Generic;
using System.Linq;
using Covid19Analysis.Model;

namespace Covid19Analysis.CollectionQueries
{
    /// <summary>
    ///     Calculation methods for DayData lists
    /// </summary>
    public class CalculateAverages
    {
        #region Methods

        /// <summary>
        ///     Calculates the average number of positive
        ///     tests since the first positive test.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <returns>
        ///     the average number of positive tests
        ///     since the first positive test
        /// </returns>
        public static double CalculateAvgPosTestsSinceFirstPosTest(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var timeDifference = calculateTimeDifferenceSinceFirstPosTest(data);
            var firstDay = FindMinMax.FindOldestPositiveCase(data);
            var totalTests = data.Where(currentDay => currentDay.Date.Date >= firstDay.Date.Date)
                                 .Sum(currentDay => currentDay.PositiveIncrease);

            var average = (double) totalTests / timeDifference;

            return average;
        }

        /// <summary>
        ///     Calculates the average number of positive
        ///     tests since the first pos/neg test.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <returns>
        ///     the average number of positive tests
        ///     since the first pos/neg test.
        /// </returns>
        public static double CalculateAvgPosTestsSinceFirstTest(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var timeDifference = calculateTimeDifferenceSinceFirstTest(data);
            var firstDay = FindMinMax.FindOldestTestCase(data);
            var totalTests = data.Where(currentDay => currentDay.Date.Date >= firstDay.Date.Date)
                                 .Sum(currentDay => currentDay.PositiveIncrease);

            var average = (double) totalTests / timeDifference;

            return average;
        }

        private static int calculateTimeDifferenceSinceFirstPosTest(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var oldestDay = FindMinMax.FindOldestPositiveCase(data);
            var newestDay = FindMinMax.FindNewestPositiveCase(data);

            var timeDifference = (newestDay.Date - oldestDay.Date).Days;
            timeDifference++;

            return timeDifference;
        }

        private static int calculateTimeDifferenceSinceFirstTest(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var oldestDay = FindMinMax.FindOldestTestCase(data);
            var newestDay = FindMinMax.FindNewestPositiveCase(data);

            var timeDifference = (newestDay.Date - oldestDay.Date).Days;
            timeDifference++;

            return timeDifference;
        }

        /// <summary>
        ///     Calculates the average # of total tests
        ///     since the first positive or negative case
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data"></param>
        /// <returns>
        ///     the average # of total tests
        /// </returns>
        public static double CalculateAverageTotalTests(ICollection<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var filteredList = data.Where(currentDay => currentDay.HasNegativeCases || currentDay.HasPositiveCases)
                                   .ToList();
            var numberDays = calculateTimeDifferenceSinceFirstTest(filteredList);
            var totalTests = filteredList.Sum(currentDay => currentDay.TotalTests);
            var average = (double) totalTests / calculateTimeDifference(filteredList);

            return average;
        }

        /// <summary>
        ///     Calculates the average # of people that are currently hospitalized
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data"></param>
        /// <returns>
        ///     the average # of total tests
        /// </returns>
        public static double CalculateAverageCurrentlyHospitalized(ICollection<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var numberDays = calculateTimeDifferenceSinceFirstTest(data.ToList());
            var totalCurrentHosp = data.Sum(currentDay => currentDay.HospitalizedCurrently);
            var average = (double)totalCurrentHosp / calculateTimeDifference(data);

            return average;
        }

        private static int calculateTimeDifference(ICollection<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var min = data.Min(x => x.Date);
            var max = data.Max(x => x.Date);

            var timeDifference = (max.Date - min.Date).Days;
            timeDifference++;

            return timeDifference;
        }

        /// <summary>
        ///     Calculates the overall positivity.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <returns>
        ///     the overall positivity of the tests
        /// </returns>
        public static double CalculateOverallPositivity(ICollection<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            double totalTests = 0;
            double positiveTests = 0;

            foreach (var currentDay in data)
            {
                totalTests += currentDay.TotalTests;
                positiveTests += currentDay.PositiveIncrease;
            }

            var overallPositivity = positiveTests / totalTests;

            return overallPositivity;
        }

        /// <summary>
        ///     Calculates the number of days of positive tests greater than x.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <param name="threshold">The threshold.</param>
        /// <returns>
        ///     the number of days with positive tests greater than x
        /// </returns>
        public static int CalculatePositiveTestsGreaterThanX(ICollection<DailyCovidStat> data, int threshold)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var firstCase = FindMinMax.FindOldestPositiveCase(data);

            return data.Count(
                currentDay => currentDay.PositiveIncrease > threshold && currentDay.Date >= firstCase.Date);
        }

        /// <summary>
        ///     Calculates the number of days with positive tests less than x.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <param name="threshold">The threshold.</param>
        /// <returns>
        ///     the number of days with positive tests less than x
        /// </returns>
        public static int CalculatePositiveTestsLessThanX(ICollection<DailyCovidStat> data, int threshold)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var firstCase = FindMinMax.FindOldestPositiveCase(data);

            return data.Count(
                currentDay => currentDay.PositiveIncrease < threshold && currentDay.Date >= firstCase.Date);
        }

        #endregion
    }
}