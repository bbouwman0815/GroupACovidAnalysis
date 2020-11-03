using System;
using System.Globalization;
using Covid19Analysis.Model;

namespace Covid19Analysis.Utility
{
    /// <summary>
    ///     CreationTools
    /// </summary>
    public class SummaryFormatTools
    {
        #region Methods

        /// <summary>
        ///     Formats the number using #,# notation.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>
        ///     the formatted number using #,# notation
        /// </returns>
        public static string FormatNumber(int number)
        {
            var formattedNumber = number.ToString("#,##0", CultureInfo.InvariantCulture);

            return formattedNumber;
        }

        /// <summary>
        ///     Formats the decimal number to the hundredth place.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>
        ///     the formatted number to the hundredth place
        /// </returns>
        public static string FormatDecimalNumber(double number)
        {
            var formattedNumber = number.ToString("#,##0.00");

            return formattedNumber;
        }

        /// <summary>
        ///     Creates the name of a month from a given int (1-12).
        /// </summary>
        /// <param name="month">The month.</param>
        /// <returns>
        ///     the name of a month
        /// </returns>
        public static string CreateMonth(int month)
        {
            var summary = string.Empty;
            var dateTime = new DateTime(1, month, 1);
            var formattedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);

            summary += formattedMonth;

            return summary;
        }

        /// <summary>
        ///     Creates a Day with a suffix.
        ///     Ex) 1st, 2nd, 3rd, 4th
        /// </summary>
        /// <param name="dayData">The Day Data.</param>
        /// <returns>
        ///     a Day with a suffix
        /// </returns>
        public static string CreateDayWithSuffix(DailyCovidStat dayData)
        {
            var day = dayData.Date.Day;
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return day + "st";
                case 2:
                case 22:
                    return day + "nd";
                case 3:
                case 23:
                    return day + "rd";
                default:
                    return day + "th";
            }
        }

        /// <summary>
        ///     Creates the Date time.
        /// </summary>
        /// <param name="date">The Date.</param>
        /// <returns>
        ///     a dateTime
        /// </returns>
        public static DateTime CreateDateTime(string date)
        {
            var dateTime = Convert.ToDateTime(date);

            return dateTime;
        }

        #endregion
    }
}