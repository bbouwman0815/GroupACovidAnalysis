using Windows.UI.Xaml.Controls;
using Covid19Analysis.Model;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Covid19Analysis.View
{
    /// <summary>
    ///  Brings up a content dialog allowing the user to manually select a stat
    /// </summary>
    public sealed partial class StateContentDialog
    {
        /// <summary>
        /// The region
        /// </summary>
        public string Region;

        /// <summary>
        /// Gets the regions.
        /// </summary>
        /// <value>
        /// The regions.
        /// </value>
        public TotalCovidStats Regions { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateContentDialog"/> class.
        /// </summary>
        public StateContentDialog(TotalCovidStats data)
        {
            this.InitializeComponent();
            this.Regions = data;
            var itemCollection = this.stateComboBox.Items;
            if (itemCollection != null)
            {
                var availableRegions = TotalCovidStats.FindRegions(this.Regions);
                foreach (var region in availableRegions)
                {
                    itemCollection.Add(region);
                }
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (this.stateComboBox.SelectedItem != null) this.Region = this.stateComboBox.SelectedItem.ToString();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
