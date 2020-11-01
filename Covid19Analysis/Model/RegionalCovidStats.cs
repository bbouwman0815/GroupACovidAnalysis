using System.Collections.Generic;

namespace Covid19Analysis.Model
{
    /// <summary>
    ///     Holds CovidData for all of the different regions
    /// </summary>
    public class RegionalCovidStats
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the Region's Data.
        /// </summary>
        /// <value>
        ///     The state Data.
        /// </value>
        public Dictionary<string, List<DailyCovidStat>> Data { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegionalCovidStats" /> class.
        /// </summary>
        public RegionalCovidStats()
        {
            this.Data = new Dictionary<string, List<DailyCovidStat>>();
        }

        #endregion
    }
}