using System;
using System.Collections.Generic;
using System.Linq;
using Covid19Analysis.Model;

namespace Covid19Analysis.CollectionQueries
{
    /// <summary>
    ///     Checks for multiple highs or lows
    /// </summary>
    public class CheckMultipleMinMax
    {
        #region Methods

        /// <summary>
        ///     Checks the collection for duplicate Positive values.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     true if more than 1 Day with same #, false otherwise
        /// </returns>
        public static bool CheckSimilarPositiveHigh(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var maxDay = FindMinMax.FindHighestPositiveCases(data).PositiveIncrease;
            var hasDupe = false;
            var duplicateCounter = data.Count(currentDay => currentDay.PositiveIncrease == maxDay);

            if (duplicateCounter > 1)
            {
                hasDupe = true;
            }

            return hasDupe;
        }

        /// <summary>
        ///     Checks the collection for duplicate currently hospitalized values.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     true if more than 1 Day with same #, false otherwise
        /// </returns>
        public static bool CheckSimilarCurrentlyHospitalizedHigh(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var maxDay = FindMinMax.FindHighestCurrentlyHospitalizations(data).HospitalizedCurrently;
            var hasDupe = false;
            var duplicateCounter = data.Count(currentDay => currentDay.HospitalizedCurrently == maxDay);

            if (duplicateCounter > 1)
            {
                hasDupe = true;
            }

            return hasDupe;
        }

        /// <summary>
        ///     Finds the days same currently hospitalized high.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     the days with the same positive highs
        /// </returns>
        public static IList<DailyCovidStat> FindDaysSameCurrentlyHospitalizedHigh(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var maxDay = FindMinMax.FindHighestCurrentlyHospitalizations(data).HospitalizedCurrently;
            var sameData = new List<DailyCovidStat>();

            if (!CheckSimilarCurrentlyHospitalizedHigh(data))
            {
                return sameData;
            }

            sameData.AddRange(data.Where(currentDay => currentDay.HospitalizedCurrently == maxDay));

            return sameData;
        }

        /// <summary>
        ///     Finds the days same positive high.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     the days with the same positive highs
        /// </returns>
        public static IList<DailyCovidStat> FindDaysSamePositiveHigh(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var maxDay = FindMinMax.FindHighestPositiveCases(data).PositiveIncrease;
            var sameData = new List<DailyCovidStat>();

            if (!CheckSimilarPositiveHigh(data))
            {
                return sameData;
            }

            sameData.AddRange(data.Where(currentDay => currentDay.PositiveIncrease == maxDay));

            return sameData;
        }

        /// <summary>
        ///     Checks the duplicate positive low.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     true if more than 1 Day with same #, false otherwise
        /// </returns>
        public static bool CheckSimilarPositiveLow(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var maxDay = FindMinMax.FindLowestPositiveCases(data).PositiveIncrease;
            var hasDupe = false;
            var duplicateCounter = data.Count(currentDay => currentDay.PositiveIncrease == maxDay);

            if (duplicateCounter > 1)
            {
                hasDupe = true;
            }

            return hasDupe;
        }

        /// <summary>
        ///     Checks the duplicate currently hospitalized low.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     true if more than 1 Day with same #, false otherwise
        /// </returns>
        public static bool CheckSimilarCurrentlyHospitalizedLow(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var maxDay = FindMinMax.FindLowestCurrentlyHospitalized(data).HospitalizedCurrently;
            var hasDupe = false;
            var duplicateCounter = data.Count(currentDay => currentDay.HospitalizedCurrently == maxDay);

            if (duplicateCounter > 1)
            {
                hasDupe = true;
            }

            return hasDupe;
        }

        /// <summary>
        ///     Finds the days with the same currently hospitalized low.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     the days with the same currently hospitalized low
        /// </returns>
        public static IList<DailyCovidStat> FindDaysSameCurrentlyHospitalizedLow(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var maxDay = FindMinMax.FindLowestCurrentlyHospitalized(data).HospitalizedCurrently;
            var sameData = new List<DailyCovidStat>();

            if (!CheckSimilarCurrentlyHospitalizedLow(data))
            {
                return sameData;
            }

            sameData.AddRange(data.Where(currentDay => currentDay.HospitalizedCurrently == maxDay));

            return sameData;
        }

        /// <summary>
        ///     Finds the days with the same positive low.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     the days with the same positive low
        /// </returns>
        public static IList<DailyCovidStat> FindDaysSamePositiveLow(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var maxDay = FindMinMax.FindLowestPositiveCases(data).PositiveIncrease;
            var sameData = new List<DailyCovidStat>();

            if (!CheckSimilarPositiveLow(data))
            {
                return sameData;
            }

            sameData.AddRange(data.Where(currentDay => currentDay.PositiveIncrease == maxDay));

            return sameData;
        }

        /// <summary>
        ///     Checks for multiple days with record low.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     true if multiple lows, false otherwise
        /// </returns>
        public static bool CheckSimilarTotalLow(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var maxDay = FindMinMax.FindLowestTotalTests(data).TotalTests;
            var hasDupe = false;
            var duplicateCounter = data.Count(currentDay => currentDay.TotalTests == maxDay);

            if (duplicateCounter > 1)
            {
                hasDupe = true;
            }

            return hasDupe;
        }

        /// <summary>
        ///     Finds the days with the same total low.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     the days with same record lows
        /// </returns>
        public static IList<DailyCovidStat> FindDaysSameTotalLow(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var maxDay = FindMinMax.FindLowestTotalTests(data).TotalTests;
            var sameData = new List<DailyCovidStat>();

            if (!CheckSimilarTotalLow(data))
            {
                return sameData;
            }

            sameData.AddRange(data.Where(currentDay => currentDay.TotalTests == maxDay));

            return sameData;
        }

        /// <summary>
        ///     Checks for similar total highs.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     true if similar total highs, false otherwise
        /// </returns>
        public static bool CheckSimilarTotalHigh(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var maxDay = FindMinMax.FindHighestTotalTests(data).TotalTests;
            var hasDupe = false;
            var duplicateCounter = data.Count(currentDay => currentDay.TotalTests == maxDay);

            if (duplicateCounter > 1)
            {
                hasDupe = true;
            }

            return hasDupe;
        }

        /// <summary>
        ///     Finds the days same total high.
        /// </summary>
        /// <Precondition>Data != null AND Data.Count > 0</Precondition>
        /// <Postcondition>none</Postcondition>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     the days with the same total highs
        /// </returns>
        public static IList<DailyCovidStat> FindDaysSameTotalHigh(List<DailyCovidStat> data)
        {
            if (data == null)
            {
                throw new NullReferenceException("Collection cannot be null.");
            }

            if (data.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), "Collection cannot be empty.");
            }

            var maxDay = FindMinMax.FindHighestTotalTests(data).TotalTests;
            var sameData = new List<DailyCovidStat>();

            if (!CheckSimilarTotalHigh(data))
            {
                return sameData;
            }

            sameData.AddRange(data.Where(currentDay => currentDay.TotalTests == maxDay));

            return sameData;
        }

        /// <summary>
        ///     Checks the set of Data for a specific duplicate.
        /// </summary>
        /// <param name="day">The Day.</param>
        /// <param name="data">The Data.</param>
        /// <returns>
        ///     true if duplicate, false otherwise
        /// </returns>
        public static bool CheckSpecificDuplicate(DailyCovidStat day, List<DailyCovidStat> data)
        {
            return data.Any(currentDay => currentDay.Date == day.Date && currentDay.Region == day.Region);
        }

        #endregion
    }
}