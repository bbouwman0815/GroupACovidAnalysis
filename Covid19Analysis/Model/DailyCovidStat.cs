using System;

namespace Covid19Analysis.Model
{
    /// <summary>
    ///     Holds covid statistics for a specific Region on a specific day
    /// </summary>
    public class DailyCovidStat : IComparable<DailyCovidStat>
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the Date.
        /// </summary>
        /// <value>
        ///     The Date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Gets or sets the Region.
        /// </summary>
        /// <value>
        ///     The Region.
        /// </value>
        public string Region { get; set; }

        /// <summary>
        ///     Gets or sets the positive increase.
        /// </summary>
        /// <value>
        ///     The positive increase.
        /// </value>
        public int PositiveIncrease { get; set; }

        /// <summary>
        ///     Gets or sets the negative increase.
        /// </summary>
        /// <value>
        ///     The negative increase.
        /// </value>
        public int NegativeIncrease { get; set; }

        /// <summary>
        ///     Gets or sets the Death increase.
        /// </summary>
        /// <value>
        ///     The Death increase.
        /// </value>
        public int DeathIncrease { get; set; }

        /// <summary>
        ///     Gets or sets the hospitalized increase.
        /// </summary>
        /// <value>
        ///     The hospitalized increase.
        /// </value>
        public int HospitalizedIncrease { get; set; }

        /// <summary>
        /// Gets or sets the hospitalized currently.
        /// </summary>
        /// <value>
        /// The hospitalized currently.
        /// </value>
        public int HospitalizedCurrently { get; set; }

        /// <summary>
        ///     Gets the total tests.
        /// </summary>
        /// <value>
        ///     The total tests.
        /// </value>
        public int TotalTests => this.PositiveIncrease + this.NegativeIncrease;

        /// <summary>
        ///     Gets the positivity.
        /// </summary>
        /// <value>
        ///     The positivity.
        /// </value>
        public double Positivity => double.Parse(this.PositiveIncrease.ToString())
                                    / double.Parse(this.TotalTests.ToString());

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has positivity.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has positivity; otherwise, <c>false</c>.
        /// </value>
        public bool HasPositiveCases { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance has negative cases.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has negative cases; otherwise, <c>false</c>.
        /// </value>
        public bool HasNegativeCases { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DailyCovidStat" /> class.
        /// </summary>
        /// <param name="date">The Date.</param>
        /// <param name="region">The Region.</param>
        /// <param name="positiveIncrease">The positive increase.</param>
        /// <param name="negativeIncrease">The negative increase.</param>
        /// <param name="hospitalizedCurrently">The hospitalized currently.</param>
        /// <param name="hospitalizedIncrease">The hospitalized increase.</param>
        /// <param name="deathIncrease">The Death increase.</param>
        /// <exception cref="ArgumentNullException">Region</exception>
        public DailyCovidStat(DateTime date, string region, int positiveIncrease, int negativeIncrease,
            int hospitalizedCurrently,
            int hospitalizedIncrease,
            int deathIncrease)
        {
            this.HasPositiveCases = false;
            this.HasNegativeCases = false;
            this.Date = date;
            this.Region = region ?? throw new ArgumentNullException(nameof(region));
            this.PositiveIncrease = positiveIncrease;
            this.NegativeIncrease = negativeIncrease;
            this.DeathIncrease = deathIncrease;
            this.HospitalizedCurrently = hospitalizedCurrently;
            this.HospitalizedIncrease = hospitalizedIncrease;

            if (positiveIncrease > 0)
            {
                this.HasPositiveCases = true;
            }

            if (negativeIncrease > 0)
            {
                this.HasNegativeCases = true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Compares days of CovidData.
        /// </summary>
        /// <param name="statistic">The statistic.</param>
        /// <returns>
        ///     +, - or 0 depending on value
        /// </returns>
        public int CompareTo(DailyCovidStat statistic)
        {
            return this.Date.CompareTo(statistic.Date);
        }

        /// <summary>
        ///     Converts to string to return the Date and PosTests
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var statisticDetails = string.Empty;

            statisticDetails += $"Date: {this.Date.ToShortDateString()}";
            statisticDetails += $" Positive increase: {this.PositiveIncrease}";
            return statisticDetails;
        }

        /// <summary>
        /// Gets the formated string.
        /// </summary>
        /// <returns>
        /// The full formatted string for the statistic
        /// </returns>
        public String GetsFullFormattedString() 
        {

            var statisticDetails = string.Empty;

            statisticDetails += $"Date: {this.Date.ToShortDateString()}";
            statisticDetails += $" Region: {this.Region}";
            statisticDetails += Environment.NewLine;
            statisticDetails += $"Positive increase: {this.PositiveIncrease}";
            statisticDetails += Environment.NewLine;
            statisticDetails += $"Negative increase: {this.NegativeIncrease}";
            statisticDetails += Environment.NewLine;
            statisticDetails += $"Death Increase: {this.DeathIncrease}";
            statisticDetails += Environment.NewLine;
            statisticDetails += $"Hospitalized increase: {this.HospitalizedIncrease}";
            statisticDetails += Environment.NewLine;
            statisticDetails += $"Overall Positivity: {this.Positivity}";
            statisticDetails += Environment.NewLine;
            statisticDetails += $"Total tests: {this.TotalTests}";
            statisticDetails += Environment.NewLine;
            statisticDetails += $"Currently hospitalized: {this.HospitalizedCurrently}";
            statisticDetails += Environment.NewLine;
            statisticDetails += Environment.NewLine;

            return statisticDetails;
        }

        #endregion
    }
}