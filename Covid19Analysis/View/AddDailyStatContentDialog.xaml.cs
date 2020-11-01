using System;
using Windows.UI.Xaml.Controls;
using Covid19Analysis.Model;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Covid19Analysis.View
{
    /// <summary>
    ///     Brings up a content dialog allowing the user to manually enter a daily covid statistic
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class AddDailyStatContentDialog : ContentDialog
    {
        #region Data members

        /// <summary>
        ///     The Date
        /// </summary>
        public DateTime Date;

        /// <summary>
        ///     The region
        /// </summary>
        public string Region;

        /// <summary>
        ///     The positive test count
        /// </summary>
        public int PositiveTestCount;

        /// <summary>
        ///     The negative test count
        /// </summary>
        public int NegativeTestCount;

        /// <summary>
        /// The hospitalized currently
        /// </summary>
        public int HospitalizedCurrently;

        /// <summary>
        ///     The hospitalization count
        /// </summary>
        public int HospitalizationCount;

        /// <summary>
        ///     The death
        /// </summary>
        public int Death;

        /// <summary>
        ///     The added daily covid stat
        /// </summary>
        public DailyCovidStat AddedDailyCovidStat;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddDailyStatContentDialog" /> class.
        /// </summary>
        public AddDailyStatContentDialog()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var date = this.datePicker.SelectedDate;

            if (date != null)
            {
                this.Date = date.Value.Date;
            }

            this.Death = int.Parse(this.deathCountTextBox.Text);
            this.HospitalizedCurrently = int.Parse(this.hospitalizedCurrentlyTextBox.Text);
            this.HospitalizationCount = int.Parse(this.hospitalizationCountTextBox.Text);
            this.NegativeTestCount = int.Parse(this.negativeTestsTextBox.Text);
            this.PositiveTestCount = int.Parse(this.positiveTestsTextBox.Text);
            this.Region = this.regionTextBox.Text;

            try
            {
                this.AddedDailyCovidStat = new DailyCovidStat(this.Date, this.Region, this.PositiveTestCount,
                    this.NegativeTestCount, this.HospitalizedCurrently, this.HospitalizationCount, this.Death);
            }
            catch
            {
                throw new ArgumentException();
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        #endregion
    }
}