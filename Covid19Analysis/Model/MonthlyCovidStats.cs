using System;
using System.Collections.Generic;

namespace Covid19Analysis.Model
{
    /// <summary>
    ///     Holds the monthly Covid Data for a Region
    /// </summary>
    public class MonthlyCovidStats
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the monthly Covid Data.
        /// </summary>
        /// <value>
        ///     The monthly Data.
        /// </value>
        public Dictionary<string, List<DailyCovidStat>> Data { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MonthlyCovidStats" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">monthlyData</exception>
        public MonthlyCovidStats()
        {
            this.Data = new Dictionary<string, List<DailyCovidStat>>();
        }

        #endregion
    }
}