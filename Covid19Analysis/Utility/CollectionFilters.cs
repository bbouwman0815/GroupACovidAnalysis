using System.Collections.Generic;
using System.Linq;
using Covid19Analysis.Model;

namespace Covid19Analysis.Utility
{
    /// <summary>
    ///     Filters for collections of DayData
    /// </summary>
    public class CollectionFilters
    {
        #region Methods

        /// <summary>
        ///     Filters the Region's Data from a collection
        /// </summary>
        /// <param name="data">The CovidData.</param>
        /// <param name="region">The Region.</param>
        /// <returns>
        ///     the filtered Region's CovidData
        /// </returns>
        public static List<DailyCovidStat> FilterByRegion(List<DailyCovidStat> data, string region)
        {
            return data.Where(currentDay => currentDay.Region.Equals(region)).ToList();
        }

        /// <summary>
        ///     Filters the CovidData by month.
        /// </summary>
        /// <param name="data">The CovidData.</param>
        /// <param name="month">The month.</param>
        /// <returns>
        ///     Data for a specific month
        /// </returns>
        public static List<DailyCovidStat> FilterDataByMonth(List<DailyCovidStat> data, int month)
        {
            return data.Where(currentDay => currentDay.Date.Month == month).ToList();
        }

        #endregion
    }
}