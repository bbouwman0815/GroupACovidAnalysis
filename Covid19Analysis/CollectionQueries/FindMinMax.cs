using System;
using System.Collections.Generic;
using System.Linq;
using Covid19Analysis.Comparers;
using Covid19Analysis.Model;

namespace Covid19Analysis.CollectionQueries
{
    /// <summary>
    ///     Finds DayData according to specification
    /// </summary>
    public class FindMinMax
    {
        #region Methods

        /// <summary>
        ///     Finds the Day of the oldest positive case.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <returns>
        ///     the Day of the oldest positive case
        /// </returns>
        public static DailyCovidStat FindOldestPositiveCase(ICollection<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var filteredData = data.Where(currentDay => currentDay.HasPositiveCases).ToList();

            var comparer = new LambdaComparer<DailyCovidStat>((x, y) => x.Date.DayOfYear - y.Date.DayOfYear);
            filteredData.Sort(comparer);

            return filteredData[0];
        }

        /// <summary>
        ///     Finds the Day of the oldest test case (positive or negative).
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <returns>
        ///     the Day of the oldest test case
        /// </returns>
        public static DailyCovidStat FindOldestTestCase(ICollection<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var filteredData = data.Where(currentDay => currentDay.HasPositiveCases || currentDay.HasNegativeCases)
                                   .ToList();

            var comparer = new LambdaComparer<DailyCovidStat>((x, y) => x.Date.DayOfYear - y.Date.DayOfYear);
            filteredData.Sort(comparer);

            return filteredData[0];
        }

        /// <summary>
        ///     Finds the newest positive case.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <returns>
        ///     the Day with the newest positive case
        /// </returns>
        public static DailyCovidStat FindNewestPositiveCase(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var comparer = new LambdaComparer<DailyCovidStat>((x, y) => y.Date.DayOfYear - x.Date.DayOfYear);
            data.Sort(comparer);

            return data[0];
        }

        /// <summary>
        ///     Finds the Day with the highest positive cases.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <returns>
        ///     the Day with the highest positive cases.
        /// </returns>
        public static DailyCovidStat FindHighestPositiveCases(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var comparer = new LambdaComparer<DailyCovidStat>((x, y) => y.PositiveIncrease - x.PositiveIncrease);
            data.Sort(comparer);

            return data[0];
        }

        /// <summary>
        ///     Finds the Day with the lowest positive cases.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <returns>
        ///     the Day with the lowest positive cases.
        /// </returns>
        public static DailyCovidStat FindLowestPositiveCases(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var comparer = new LambdaComparer<DailyCovidStat>((x, y) => x.PositiveIncrease - y.PositiveIncrease);
            data.Sort(comparer);

            return data[0];
        }

        /// <summary>
        ///     Finds the Day with the lowest # of people currently hospitalized.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <returns>
        ///     the Day with the lowest # of people currently hospitalized.
        /// </returns>
        public static DailyCovidStat FindLowestCurrentlyHospitalized(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var comparer = new LambdaComparer<DailyCovidStat>((x, y) => x.HospitalizedCurrently - y.HospitalizedCurrently);
            data.Sort(comparer);

            return data[0];
        }

        /// <summary>
        ///     Finds the Day with the highest negative cases.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <returns>
        ///     the Day with the highest negative cases.
        /// </returns>
        public static DailyCovidStat FindHighestNegativeCases(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var comparer = new LambdaComparer<DailyCovidStat>((x, y) => y.NegativeIncrease - x.NegativeIncrease);
            data.Sort(comparer);

            return data[0];
        }

        /// <summary>
        ///     Finds the Day with the highest # of total tests.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <returns>
        ///     the Day with the highest # of total tests.
        /// </returns>
        public static DailyCovidStat FindHighestTotalTests(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var comparer = new LambdaComparer<DailyCovidStat>((x, y) => y.TotalTests - x.TotalTests);
            data.Sort(comparer);

            return data[0];
        }

        /// <summary>
        ///     Finds the Day with the lowest # of total tests.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     the Day with the lowest total # tests
        /// </returns>
        public static DailyCovidStat FindLowestTotalTests(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var comparer = new LambdaComparer<DailyCovidStat>((x, y) => x.TotalTests - y.TotalTests);
            data.Sort(comparer);

            return data[0];
        }

        /// <summary>
        ///     Finds the Day with the highest deaths.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <returns>
        ///     the Day with the highest deaths
        /// </returns>
        public static DailyCovidStat FindHighestDeaths(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var comparer = new LambdaComparer<DailyCovidStat>((x, y) => y.DeathIncrease - x.DeathIncrease);
            data.Sort(comparer);

            return data[0];
        }

        /// <summary>
        ///     Finds the highest currently hospitalized.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <returns>
        ///     the Day with the highest currently hospitalized count
        /// </returns>
        public static DailyCovidStat FindHighestCurrentlyHospitalizations(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var comparer =
                new LambdaComparer<DailyCovidStat>((x, y) => y.HospitalizedCurrently - x.HospitalizedCurrently);
            data.Sort(comparer);

            return data[0];
        }

        /// <summary>
        ///     Finds the highest hospitalizations.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <returns>
        ///     the Day with the highest hospitalization count
        /// </returns>
        public static DailyCovidStat FindHighestHospitalizations(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var comparer =
                new LambdaComparer<DailyCovidStat>((x, y) => y.HospitalizedIncrease - x.HospitalizedIncrease);
            data.Sort(comparer);

            return data[0];
        }

        /// <summary>
        ///     Finds the highest positive percent.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The CovidData.</param>
        /// <returns>
        ///     the Day with the highest positivity
        /// </returns>
        public static DailyCovidStat FindHighestPositivePercent(ICollection<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var highestPercent = double.MinValue;
            DailyCovidStat highestDay = null;

            foreach (var currentDay in data)
            {
                if (!currentDay.HasNegativeCases || !currentDay.HasPositiveCases)
                {
                    continue;
                }

                var posPercent = currentDay.Positivity;

                if (!(posPercent > highestPercent))
                {
                    continue;
                }

                highestDay = currentDay;
                highestPercent = posPercent;
            }

            return highestDay;
        }

        private static double findPositivePercent(DailyCovidStat day)
        {
            var posTests = (double) day.PositiveIncrease;
            var totalTests = (double) day.TotalTests;
            var posPercent = posTests / totalTests;

            return posPercent;
        }

        /// <summary>
        ///     Finds the count of days with positive cases between a lower and upper bound
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The Data.</param>
        /// <param name="lower">The lower.</param>
        /// <param name="upper">The upper.</param>
        /// <returns>
        ///     the count of days with positive cases between lower and upper bound
        /// </returns>
        public static int FindUpperLowerBoundCount(ICollection<DailyCovidStat> data, int lower, int upper)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            return data.Select(currentDay => currentDay.PositiveIncrease)
                       .Count(numberCheck => numberCheck >= lower && numberCheck <= upper);
        }

        #endregion
    }
}