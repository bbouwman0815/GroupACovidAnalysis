using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Covid19Analysis.View
{
    /// <summary>
    ///  Brings up a content dialog allowing the user to manually select a stat
    /// </summary>
    public sealed partial class StateContentDialog : ContentDialog
    {
        /// <summary>
        /// The region
        /// </summary>
        public string Region;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateContentDialog"/> class.
        /// </summary>
        public StateContentDialog()
        {
            this.InitializeComponent();
            var itemCollection = this.stateComboBox.Items;
            if (itemCollection != null)
            {
                itemCollection.Add("AL");
                itemCollection.Add("AK");
                itemCollection.Add("AZ");
                itemCollection.Add("AR");
                itemCollection.Add("CA");
                itemCollection.Add("CO");
                itemCollection.Add("CT");
                itemCollection.Add("DE");
                itemCollection.Add("DC");
                itemCollection.Add("FL");
                itemCollection.Add("GA");
                itemCollection.Add("HI");
                itemCollection.Add("ID");
                itemCollection.Add("IL");
                itemCollection.Add("IN");
                itemCollection.Add("IA");
                itemCollection.Add("KS");
                itemCollection.Add("KY");
                itemCollection.Add("LA");
                itemCollection.Add("ME");
                itemCollection.Add("MD");
                itemCollection.Add("MA");
                itemCollection.Add("MI");
                itemCollection.Add("MN");
                itemCollection.Add("MS");
                itemCollection.Add("MO");
                itemCollection.Add("MT");
                itemCollection.Add("NE");
                itemCollection.Add("NV");
                itemCollection.Add("NH");
                itemCollection.Add("NJ");
                itemCollection.Add("NM");
                itemCollection.Add("NY");
                itemCollection.Add("NC");
                itemCollection.Add("ND");
                itemCollection.Add("OH");
                itemCollection.Add("OK");
                itemCollection.Add("PA");
                itemCollection.Add("RI");
                itemCollection.Add("SC");
                itemCollection.Add("SC");
                itemCollection.Add("TN");
                itemCollection.Add("TX");
                itemCollection.Add("UT");
                itemCollection.Add("VT");
                itemCollection.Add("VA");
                itemCollection.Add("WA");
                itemCollection.Add("WV");
                itemCollection.Add("WI");
                itemCollection.Add("PR");
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
