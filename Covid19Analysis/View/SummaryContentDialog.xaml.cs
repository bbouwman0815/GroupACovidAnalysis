using System;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Covid19Analysis.View
{
    /// <summary>
    ///     Brings up a content dialog Displaying the summary of the covid statistic in thr collection
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class SummaryContentDialog
    {
        #region Data members        
        
        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddDailyStatContentDialog" /> class.
        /// </summary>
        public SummaryContentDialog(String displayText)
        {
            this.InitializeComponent();
            this.summaryTextBox.Text = displayText;
        }

        #endregion

        #region Methods

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        #endregion
    }
}