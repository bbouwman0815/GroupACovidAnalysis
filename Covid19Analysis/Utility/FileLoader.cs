using System;
using System.Collections.Generic;
using System.Linq;
using Covid19Analysis.CollectionQueries;
using Covid19Analysis.Model;

namespace Covid19Analysis.Utility
{
    /// <summary>
    ///     The file loader class allows the user to choose a CSV file to load
    /// </summary>
    public class FileLoader
    {
        #region Data members

        /// <summary>
        /// Creates new line.
        /// </summary>
        private const string NewLine = "\n";

        /// <summary>
        /// The comma
        /// </summary>
        private const string Comma = ",";

        /// <summary>
        /// The zero
        /// </summary>
        private const string Zero = "0";

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the errors.
        /// </summary>
        /// <value>
        ///     The errors.
        /// </value>
        public string Errors { get; set; }

        /// <summary>
        ///     Gets or sets the duplicates.
        /// </summary>
        /// <value>
        ///     The duplicates.
        /// </value>
        public TotalCovidStats DuplicateData { get; }

        /// <summary>
        ///     Gets the loaded covid stats.
        /// </summary>
        /// <value>
        ///     The loaded covid stats.
        /// </value>
        public TotalCovidStats LoadedCovidStats { get; }

        /// <summary>
        ///     Gets or sets the added daily covid stat.
        /// </summary>
        /// <value>
        ///     the added daily covid stat.
        /// </value>
        public DailyCovidStat AddedStat { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileLoader" /> class.
        /// </summary>
        public FileLoader()
        {
            this.LoadedCovidStats = new TotalCovidStats();
            this.DuplicateData = new TotalCovidStats();
            this.Errors = string.Empty;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Loads the file.
        /// </summary>
        /// <param name="fileContent">Content of the file.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void LoadFile(string fileContent)
        {
            if (fileContent == null)
            {
                throw new ArgumentNullException();
            }

            if (fileContent.Equals(string.Empty))
            {
                throw new ArgumentException();
            }

            var covidStats = fileContent.Split(NewLine);

            var lineNumber = 0;
            foreach (var stat in covidStats)
            {
                lineNumber++;
                var covidStatField = stat.Split(Comma);
                if (lineNumber != 1)
                {
                    try
                    {
                        this.addDay(covidStatField);
                    }
                    catch (Exception)
                    {
                        this.Errors += stat + " Line number: " + lineNumber + Environment.NewLine;
                    }
                }
            }
        }

        private void addDay(IReadOnlyList<string> stats)
        {
            var statistic = CreateDayData(stats);

            if (CheckMultipleMinMax.CheckSpecificDuplicate(statistic, this.LoadedCovidStats.ToList()))
            {
                this.DuplicateData.Add(statistic);
            }

            if (!CheckMultipleMinMax.CheckSpecificDuplicate(statistic, this.DuplicateData.ToList()))
            {
                this.LoadedCovidStats.Add(statistic);
            }
        }

        /// <summary>
        ///     Creates the day data.
        /// </summary>
        /// <param name="stats">The stats.</param>
        /// <returns>
        ///     dayData the day of covid data
        /// </returns>
        public static DailyCovidStat CreateDayData(IReadOnlyList<string> stats)
        {
            var firstClean = handleFieldsWithEmptyStrings(stats).ToList();
            var secondClean = handleFieldsWithNegativeValues(firstClean).ToList();

            var dateTime = SummaryFormatTools.CreateDateTime(secondClean[0]);
            var state = secondClean[1];
            var posIncrease = int.Parse(secondClean[2]);
            var negIncrease = int.Parse(secondClean[3]);
            var hospitalizedCurrently = int.Parse(secondClean[4]);
            var hospitalizedIncrease = int.Parse(secondClean[5]);
            var deathIncrease = int.Parse(secondClean[6]);

            var dayData = new DailyCovidStat(dateTime, state, posIncrease, negIncrease, hospitalizedCurrently, hospitalizedIncrease,
                deathIncrease);

            return dayData;
        }

        private static IEnumerable<string> handleFieldsWithEmptyStrings(IReadOnlyList<string> stats)
        {
            var data = stats.ToList();

            for (var i = 2; i < stats.Count; i++)
            {
                if (stats[i].Equals(string.Empty))
                {
                    data[i] = Zero;
                }
            }
            return data;
        }

        private static IEnumerable<string> handleFieldsWithNegativeValues(IReadOnlyList<string> stats)
        {
            var data = stats.ToList();

            for (var i = 2; i < stats.Count; i++)
            {
                var parsedValue = int.Parse(data[i]);
                if (parsedValue < 0)
                {
                    data[i] = Zero;
                }
            }
            return data;
        }

        #endregion
    }
}