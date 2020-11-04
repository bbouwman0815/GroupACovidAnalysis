using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Covid19Analysis.CollectionQueries;
using Covid19Analysis.Model;
using Covid19Analysis.Utility;

namespace Covid19Analysis.Report
{
    /// <summary>
    ///     Generates a report about a Region's Covid-19 Data
    /// </summary>
    public class SummaryReport
    {
        #region Data members

        /// <summary>
        ///     The default upper bound
        /// </summary>
        public const int DefaultUpperBound = 2500;

        /// <summary>
        ///     The default lower bound
        /// </summary>
        public const int DefaultLowerBound = 1000;

        /// <summary>
        /// The month separator
        /// </summary>
        private const string Separator = "-------------------------------------------------";

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the CovidData.
        /// </summary>
        /// <value>
        ///     The CovidData.
        /// </value>
        public TotalCovidStats CovidData { get; set; }

        /// <summary>
        ///     Gets or sets the Region.
        /// </summary>
        /// <value>
        ///     The Region.
        /// </value>
        public string Region { get; set; }

        public Dictionary<string, List<DailyCovidStat>> RegionData { get; set; }

        public Dictionary<string, List<DailyCovidStat>> MonthlyData { get; set; }

        /// <summary>
        ///     Gets or sets the number formatter.
        /// </summary>
        /// <value>
        ///     The number formatter.
        /// </value>
        public NumberFormatInfo NumberFormatter { get; set; }

        /// <summary>
        ///     Gets or sets the lower bound.
        /// </summary>
        /// <value>
        ///     The lower bound.
        /// </value>
        public int LowerBound { get; set; }

        /// <summary>
        ///     Gets or sets the upper bound.
        /// </summary>
        /// <value>
        ///     The upper bound.
        /// </value>
        public int UpperBound { get; set; }

        /// <summary>
        ///     Gets or sets the size of the histogram bin.
        /// </summary>
        /// <value>
        ///     The size of the histogram bin.
        /// </value>
        public int HistogramBinSize { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SummaryReport" /> class.
        /// </summary>
        /// <param name="region">The Region.</param>
        /// <param name="data">The Data.</param>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <param name="binSize">Size of the bin.</param>
        public SummaryReport(string region, TotalCovidStats data, int lowerBound, int upperBound, int binSize)
        {
            this.Region = region;
            this.CovidData = data;
            this.RegionData = new Dictionary<string, List<DailyCovidStat>>();
            this.MonthlyData = new Dictionary<string, List<DailyCovidStat>>();
            this.NumberFormatter = new NumberFormatInfo();
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
            this.HistogramBinSize = binSize;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Generates the CovidData for a Region.
        /// </summary>
        /// <returns>
        ///     a summary of the overall and monthly CovidData for a Region
        /// </returns>
        public string GenerateDataForRegion()
        {
            var summary = string.Empty;

            if (this.CovidData.ContainsTestData())
            {
                this.RegionData = this.CovidData.CreateRegionalDictionary(this.CovidData.ToList());
                this.MonthlyData = this.CovidData.CreateMonthlyDictionary(this.RegionData[this.Region]);

                var monthlySummary =
                    MonthlyReport.GenerateMonthlyDataReport(this.RegionData, this.MonthlyData, this.Region);

                summary += this.generateOverallDataReport();

                summary += Environment.NewLine;
                summary += Environment.NewLine;

                summary += monthlySummary;

                return summary;
            }

            summary += "No Positive or Negative tests in the Data";
            return summary;
        }

        private string generateOverallDataReport()
        {
            var summary = string.Empty;

            summary += Separator;
            summary += Environment.NewLine;
            summary += $"Covid-19 Summary Report for : {this.Region}";
            summary += Environment.NewLine;
            summary += Separator;
            summary += Environment.NewLine;
            summary += Environment.NewLine;

            summary += this.generateHighestDataReport();

            summary += Environment.NewLine;
            summary += Environment.NewLine;

            summary += this.generatePositivitySummary();

            summary += Environment.NewLine;

            summary += this.generateThresholdSummary(this.LowerBound, this.UpperBound);

            summary += Environment.NewLine;
            summary += Environment.NewLine;

            summary += this.generatePositiveCaseHistogram(this.RegionData[this.Region]);

            return summary;
        }

        /// <summary>
        ///     Generates a summary of a regions highest covid statistics
        /// </summary>
        /// <returns>
        ///     a summary of a regions highest covid statistics
        /// </returns>
        private string generateHighestDataReport()
        {
            var data = this.RegionData[this.Region];

            var summary = string.Empty;

            var highestPositivity = FindMinMax.FindHighestPositivePercent(data);
            var firstPositiveTest = FindMinMax.FindOldestPositiveCase(data);

            if (highestPositivity != null)
            {
                summary += generateFirstInstanceOfPositiveTest(firstPositiveTest);

                summary += Environment.NewLine;
                summary += Environment.NewLine;

                summary += generateHighestBaseStats(data);

                summary += Environment.NewLine;
                summary += Environment.NewLine;

                summary += generateHighestPositivityRate(highestPositivity);
            }

            return summary;
        }

        private static string generateFirstInstanceOfPositiveTest(DailyCovidStat firstPositiveTest)
        {
            var summary = string.Empty;

            summary += "First positive case appeared on: ";
            summary += firstPositiveTest.Date.ToShortDateString();

            return summary;
        }

        private static string generateHighestBaseStats(List<DailyCovidStat> data)
        {
            var summary = string.Empty;

            var highestNeg = FindMinMax.FindHighestNegativeCases(data);
            var highestTotal = FindMinMax.FindHighestTotalTests(data);
            var highestDeaths = FindMinMax.FindHighestDeaths(data);
            var highestCurrentlyHosp = FindMinMax.FindHighestCurrentlyHospitalizations(data);
            var highestHosp = FindMinMax.FindHighestHospitalizations(data);
            var highestPos = FindMinMax.FindHighestPositiveCases(data);

            summary += generateHighestTemplate("positive tests", highestPos, highestPos.PositiveIncrease);
            summary += Environment.NewLine;
            summary += generateHighestTemplate("negative tests", highestNeg, highestNeg.NegativeIncrease);
            summary += Environment.NewLine;
            summary += generateHighestTemplate("total tests", highestTotal, highestTotal.TotalTests);
            summary += Environment.NewLine;
            summary += generateHighestTemplate("deaths", highestDeaths, highestDeaths.DeathIncrease);
            summary += Environment.NewLine;
            summary += generateHighestTemplate("current hospitalizations", highestCurrentlyHosp, highestCurrentlyHosp.HospitalizedCurrently);
            summary += Environment.NewLine;
            summary += generateHighestTemplate("hospitalization increases", highestHosp, highestHosp.HospitalizedIncrease);

            return summary;
        }

        private static string generateHighestTemplate(string type, DailyCovidStat day, int number)
        {
            var summary = string.Empty;
            summary += $"Highest # of {type}";
            summary += $" happened on : {day.Date.ToShortDateString()}";
            summary += $" :: {SummaryFormatTools.FormatNumber(number)}";

            return summary;
        }

        private static string generateHighestPositivityRate(DailyCovidStat highestPositivity)
        {
            if (highestPositivity == null)
            {
                throw new NullReferenceException();
            }

            var summary = string.Empty;

            summary += "The highest percentage of positive tests happened on: ";
            summary += highestPositivity.Date.ToShortDateString();
            summary += $" :: {SummaryFormatTools.FormatDecimalNumber(highestPositivity.Positivity)}";
            summary += Environment.NewLine;
            summary += $"Positive tests : {SummaryFormatTools.FormatNumber(highestPositivity.PositiveIncrease)}";
            summary += Environment.NewLine;
            summary += $"Negative tests : {SummaryFormatTools.FormatNumber(highestPositivity.NegativeIncrease)}";

            return summary;
        }

        private string generatePositivitySummary()
        {
            var data = this.RegionData[this.Region];

            var summary = string.Empty;
            var averagePosTests = CalculateAverages.CalculateAvgPosTestsSinceFirstPosTest(data);
            var averageCurrentlyHospitalized = CalculateAverages.CalculateAverageCurrentlyHospitalized(data);
            var overallPositivity = CalculateAverages.CalculateOverallPositivity(data);

            summary += "Average # of positive tests since first test: ";
            summary += SummaryFormatTools.FormatDecimalNumber(averagePosTests);
            summary += Environment.NewLine;
            summary += "Average # of people currently hospitalized per day: ";
            summary += SummaryFormatTools.FormatDecimalNumber(averageCurrentlyHospitalized);
            summary += Environment.NewLine;
            summary += Environment.NewLine;
            summary += "The overall positivity rate of the tests is : ";
            summary += SummaryFormatTools.FormatDecimalNumber(overallPositivity);
            summary += Environment.NewLine;

            return summary;
        }

        private string generateThresholdSummary(int lessThan, int moreThan)
        {
            if (lessThan == 0 || moreThan == 0)
            {
                lessThan = DefaultLowerBound;
                moreThan = DefaultUpperBound;
            }

            var data = this.RegionData[this.Region];

            var summary = string.Empty;

            var less = CalculateAverages.CalculatePositiveTestsLessThanX(data, lessThan);
            var more = CalculateAverages.CalculatePositiveTestsGreaterThanX(data, moreThan);

            summary += $"Number of days with positive tests greater than {moreThan}";
            summary += $" : {more}";
            summary += Environment.NewLine;
            summary += $"Number of days with positive tests less than {lessThan}";
            summary += $" : {less}";

            return summary;
        }

        private string generatePositiveCaseHistogram(List<DailyCovidStat> data)
        {
            var segment = this.HistogramBinSize;
            var summary = string.Empty;
            summary += "   Histogram Data";
            summary += Environment.NewLine;
            summary += Environment.NewLine;

            var upperLimit = FindMinMax.FindHighestPositiveCases(data).PositiveIncrease;
            var lowerBound = 0;
            var upperBound = segment;
            var segments = Math.Ceiling((double) upperLimit / segment);

            for (var i = segments; i > 0; i--)
            {
                summary +=
                    $"{lowerBound,6} - {upperBound,-6} : {FindMinMax.FindUpperLowerBoundCount(data, lowerBound, upperBound)}";
                summary += Environment.NewLine;

                lowerBound = upperBound + 1;
                upperBound += segment;
            }

            return summary;
        }

        #endregion
    }
}