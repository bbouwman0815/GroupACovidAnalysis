using System;
using System.Collections.Generic;
using System.Linq;
using Covid19Analysis.CollectionQueries;
using Covid19Analysis.Model;
using Covid19Analysis.Utility;

namespace Covid19Analysis.Report
{
    /// <summary>
    ///     Creates a monthly report for a Region
    /// </summary>
    public class MonthlyReport
    {
        private const string Separator = "---------------------------------------------------------------------------";
        #region Methods

        /// <summary>
        ///     Generates the monthly Data report for a Region.
        /// </summary>
        /// <returns>
        ///     the monthly Data report for a Region
        /// </returns>
        public static string GenerateMonthlyDataReport(Dictionary<string, List<DailyCovidStat>> regionData, Dictionary<string, List<DailyCovidStat>> monthData,
            string region)
        {
            var summary = string.Empty;
            summary += generateMonthlyTitle();

            var data = regionData[region];

            var minDay = FindMinMax.FindOldestPositiveCase(data);
            var min = minDay.Date.Month;

            var maxDay = FindMinMax.FindNewestPositiveCase(data);
            var max = maxDay.Date.Month;

            for (var i = min; i <= max; i++)
            {
                summary += generateDataForMonth(i, monthData);
                summary += Environment.NewLine;
            }

            return summary;
        }

        private static string generateMonthlyTitle()
        {
            var summary = string.Empty;
            summary += Separator;
            summary += Environment.NewLine;
            summary += "Monthly Covid-19 Breakdown";
            summary += Environment.NewLine;
            summary += Separator;
            summary += Environment.NewLine;
            summary += Environment.NewLine;

            return summary;
        }

        private static string generateDataForMonth(int month, Dictionary<string, List<DailyCovidStat>> data)
        {
            var monthData = data[month.ToString()];
            var summary = string.Empty;

            if (monthData.Count > 0)
            {
                summary += generateMonthlyHeader(month, monthData);
                summary += Environment.NewLine;

                summary += generateMonthlyMinMax(monthData);
                summary += generateAverages(monthData);
                summary += Environment.NewLine;
                summary += Separator;
                summary += Environment.NewLine;
            }
            else
            {
                summary += generateMonthlyHeader(month, monthData);
                summary += Environment.NewLine;
                summary += Separator;
                summary += Environment.NewLine;
                summary += Environment.NewLine;
            }

            return summary;
        }

        private static string generateMonthlyHeader(int month, List<DailyCovidStat> data)
        {
            var summary = string.Empty;

            if (data.Count > 0)
            {
                var year = data[0].Date.Year;
                summary += SummaryFormatTools.CreateMonth(month);
                summary += $" {year} ({data.Count} days of Data)";
                summary += Environment.NewLine;
                return summary;
            }

            summary += SummaryFormatTools.CreateMonth(month);
            summary += $" ({data.Count} days of Data)";
            summary += Environment.NewLine;

            return summary;
        }

        private static string generateMonthlyMinMax(List<DailyCovidStat> data)
        {
            var summary = string.Empty;
            summary += generateHighestPositiveTests(data);
            summary += Environment.NewLine;
            summary += generateLowestPositiveTests(data);
            summary += Environment.NewLine;
            summary += Environment.NewLine;
            summary += generateHighestCurrentlyHospitalized(data);
            summary += Environment.NewLine;
            summary += generateLowestCurrentlyHospitalized(data);
            summary += Environment.NewLine;
            summary += Environment.NewLine;
            summary += generateHighestTotalTests(data);
            summary += Environment.NewLine;
            summary += generateLowestTotalTests(data);
            summary += Environment.NewLine;
            summary += Environment.NewLine;

            return summary;
        }

        private static string generateAverages(List<DailyCovidStat> data)
        {
            var averagePos = CalculateAverages.CalculateAvgPosTestsSinceFirstTest(data);
            var averageTotal = CalculateAverages.CalculateAverageTotalTests(data);

            var summary = string.Empty;
            summary += $"Average # positive tests per Day : {SummaryFormatTools.FormatDecimalNumber(averagePos)}";
            summary += Environment.NewLine;
            summary += $"Average # total tests per Day : {SummaryFormatTools.FormatDecimalNumber(averageTotal)}";
            summary += Environment.NewLine;

            return summary;
        }

        private static string generateHighestPositiveTests(List<DailyCovidStat> data)
        {
            var summary = string.Empty;
            var dates = string.Empty;
            var highestPos = FindMinMax.FindHighestPositiveCases(data);

            if (CheckMultipleMinMax.CheckSimilarPositiveHigh(data))
            {
                var days = CheckMultipleMinMax.FindDaysSamePositiveHigh(data);
                dates = days.Aggregate(dates, (current, currentDay) => current +
                                                                       $"{SummaryFormatTools.CreateDayWithSuffix(currentDay)} ");
                summary += $"Highest # positive tests: {SummaryFormatTools.FormatNumber(highestPos.PositiveIncrease)} ";
                summary += $"happened on the :: {dates}";

                return summary;
            }

            summary += $"Highest # positive tests: {SummaryFormatTools.FormatNumber(highestPos.PositiveIncrease)} " +
                       $"happened on the : {SummaryFormatTools.CreateDayWithSuffix(highestPos)}";

            return summary;
        }

        private static string generateLowestPositiveTests(List<DailyCovidStat> data)
        {
            var summary = string.Empty;
            var dates = string.Empty;
            var lowestPos = FindMinMax.FindLowestPositiveCases(data);

            if (CheckMultipleMinMax.CheckSimilarPositiveLow(data))
            {
                var days = CheckMultipleMinMax.FindDaysSamePositiveLow(data);
                dates = days.Aggregate(dates, (current, currentDay) => current +
                                                                       $"{SummaryFormatTools.CreateDayWithSuffix(currentDay)} ");
                summary +=
                    $"Lowest # positive tests: {SummaryFormatTools.FormatNumber(lowestPos.PositiveIncrease)} happened on the :: {dates}";

                return summary;
            }

            summary += $"Lowest # positive tests: {SummaryFormatTools.FormatNumber(lowestPos.PositiveIncrease)} " +
                       $"happened on the : {SummaryFormatTools.CreateDayWithSuffix(lowestPos)}";

            return summary;
        }

        private static string generateHighestCurrentlyHospitalized(List<DailyCovidStat> data)
        {
            var summary = string.Empty;
            var dates = string.Empty;
            var highestCurrentlyHospitalized = FindMinMax.FindHighestCurrentlyHospitalizations(data);

            if (CheckMultipleMinMax.CheckSimilarCurrentlyHospitalizedHigh(data))
            {
                var days = CheckMultipleMinMax.FindDaysSameCurrentlyHospitalizedHigh(data);
                dates = days.Aggregate(dates, (current, currentDay) => current +
                                                                       $"{SummaryFormatTools.CreateDayWithSuffix(currentDay)} ");
                summary += $"Highest # of people currently hospitalized: {SummaryFormatTools.FormatNumber(highestCurrentlyHospitalized.HospitalizedCurrently)} ";
                summary += $"happened on the :: {dates}";

                return summary;
            }

            summary += $"Highest # of people currently hospitalized: {SummaryFormatTools.FormatNumber(highestCurrentlyHospitalized.HospitalizedCurrently)} " +
                       $"happened on the : {SummaryFormatTools.CreateDayWithSuffix(highestCurrentlyHospitalized)}";

            return summary;
        }

        private static string generateLowestCurrentlyHospitalized(List<DailyCovidStat> data)
        {
            var summary = string.Empty;
            var dates = string.Empty;
            var lowestHospitalized = FindMinMax.FindLowestCurrentlyHospitalized(data);

            if (CheckMultipleMinMax.CheckSimilarCurrentlyHospitalizedLow(data))
            {
                var days = CheckMultipleMinMax.FindDaysSameCurrentlyHospitalizedLow(data);
                dates = days.Aggregate(dates, (current, currentDay) => current +
                                                                       $"{SummaryFormatTools.CreateDayWithSuffix(currentDay)} ");
                summary +=
                    $"Lowest # of people currently hospitalized: {SummaryFormatTools.FormatNumber(lowestHospitalized.HospitalizedCurrently)} happened on the :: {dates}";

                return summary;
            }

            summary += $"Lowest # of people currently hospitalized: {SummaryFormatTools.FormatNumber(lowestHospitalized.HospitalizedCurrently)} " +
                       $"happened on the : {SummaryFormatTools.CreateDayWithSuffix(lowestHospitalized)}";

            return summary;
        }

        private static string generateLowestTotalTests(List<DailyCovidStat> data)
        {
            var summary = string.Empty;
            var dates = string.Empty;
            var lowestTotal = FindMinMax.FindLowestTotalTests(data);

            if (CheckMultipleMinMax.CheckSimilarTotalLow(data))
            {
                var days = CheckMultipleMinMax.FindDaysSameTotalLow(data);
                dates = days.Aggregate(dates, (current, currentDay) => current +
                                                                       $"{SummaryFormatTools.CreateDayWithSuffix(currentDay)} ");
                summary +=
                    $"Lowest # total tests: {SummaryFormatTools.FormatNumber(lowestTotal.TotalTests)} happened on the :: {dates}";

                return summary;
            }

            summary += $"Lowest # total tests: {SummaryFormatTools.FormatNumber(lowestTotal.TotalTests)} " +
                       $"happened on the : {SummaryFormatTools.CreateDayWithSuffix(lowestTotal)}";

            return summary;
        }

        private static string generateHighestTotalTests(List<DailyCovidStat> data)
        {
            var summary = string.Empty;
            var dates = string.Empty;
            var highestTotal = FindMinMax.FindHighestTotalTests(data);

            if (CheckMultipleMinMax.CheckSimilarTotalHigh(data))
            {
                var days = CheckMultipleMinMax.FindDaysSameTotalHigh(data);
                dates = days.Aggregate(dates, (current, currentDay) => current +
                                                                       $"{SummaryFormatTools.CreateDayWithSuffix(currentDay)} ");
                summary +=
                    $"Highest # total tests: {SummaryFormatTools.FormatNumber(highestTotal.TotalTests)} happened on the :: {dates}";

                return summary;
            }

            summary += $"Highest # total tests: {SummaryFormatTools.FormatNumber(highestTotal.TotalTests)} " +
                       $"happened on the : {SummaryFormatTools.CreateDayWithSuffix(highestTotal)}";

            return summary;
        }

        #endregion
    }
}