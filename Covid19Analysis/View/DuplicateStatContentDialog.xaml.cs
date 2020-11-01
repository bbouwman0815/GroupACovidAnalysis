using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Covid19Analysis.Model;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Covid19Analysis.View
{
    /// <summary>
    ///     Actions to take
    /// </summary>
    public enum Result
    {
        /// <summary>
        ///     Discard the covid statistic
        /// </summary>
        Discard,

        /// <summary>
        ///     Discard all of the duplicate covid statistics
        /// </summary>
        DiscardAll,

        /// <summary>
        ///     Replace the current covid statistic with the duplicate
        /// </summary>
        Replace,

        /// <summary>
        ///     Replace all current statistics with the duplicates
        /// </summary>
        ReplaceAll,

        /// <summary>
        ///     The nothing
        /// </summary>
        Nothing
    }

    /// <summary>
    ///     Creates a content dialog for duplicate days of Covid Data
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class DuplicateStatContentDialog : ContentDialog
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the result.
        /// </summary>
        /// <value>
        ///     The result.
        /// </value>
        public Result Result { get; set; }

        /// <summary>
        ///     Gets or sets the Day.
        /// </summary>
        /// <value>
        ///     The Day.
        /// </value>
        public DailyCovidStat Day { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DuplicateStatContentDialog" /> class.
        /// </summary>
        public DuplicateStatContentDialog(DailyCovidStat day)
        {
            this.InitializeComponent();
            this.Result = Result.Nothing;
            this.Day = day;
            Title = $"Duplicate found for : {day.Region} {day.Date.ToShortDateString()}";
        }

        #endregion

        #region Methods

        private void discard_Click(object sender, RoutedEventArgs e)
        {
            this.Result = Result.Discard;
            this.dialog.Hide();
        }

        private void replace_Click(object sender, RoutedEventArgs e)
        {
            this.Result = Result.Replace;
            this.dialog.Hide();
        }

        private void replaceAll_Click(object sender, RoutedEventArgs e)
        {
            this.Result = Result.ReplaceAll;
            this.dialog.Hide();
        }

        #endregion

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            this.Result = Result.DiscardAll;
            this.dialog.Hide();
        }
    }
}