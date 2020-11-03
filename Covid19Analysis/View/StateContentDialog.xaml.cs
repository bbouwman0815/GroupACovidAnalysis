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
            this.stateComboBox.Items.Add("AL");
            this.stateComboBox.Items.Add("AK");
            this.stateComboBox.Items.Add("AZ"); 
            this.stateComboBox.Items.Add("AR"); 
            this.stateComboBox.Items.Add("CA"); 
            this.stateComboBox.Items.Add("CO"); 
            this.stateComboBox.Items.Add("CT"); 
            this.stateComboBox.Items.Add("DE"); 
            this.stateComboBox.Items.Add("DC"); 
            this.stateComboBox.Items.Add("FL"); 
            this.stateComboBox.Items.Add("GA"); 
            this.stateComboBox.Items.Add("HI"); 
            this.stateComboBox.Items.Add("ID"); 
            this.stateComboBox.Items.Add("IL"); 
            this.stateComboBox.Items.Add("IN"); 
            this.stateComboBox.Items.Add("IA"); 
            this.stateComboBox.Items.Add("KS"); 
            this.stateComboBox.Items.Add("KY"); 
            this.stateComboBox.Items.Add("LA"); 
            this.stateComboBox.Items.Add("ME"); 
            this.stateComboBox.Items.Add("MD"); 
            this.stateComboBox.Items.Add("MA"); 
            this.stateComboBox.Items.Add("MI"); 
            this.stateComboBox.Items.Add("MN"); 
            this.stateComboBox.Items.Add("MS"); 
            this.stateComboBox.Items.Add("MO"); 
            this.stateComboBox.Items.Add("MT"); 
            this.stateComboBox.Items.Add("NE");
            this.stateComboBox.Items.Add("NV"); 
            this.stateComboBox.Items.Add("NH");
            this.stateComboBox.Items.Add("NJ"); 
            this.stateComboBox.Items.Add("NM"); 
            this.stateComboBox.Items.Add("NY"); 
            this.stateComboBox.Items.Add("NC"); 
            this.stateComboBox.Items.Add("ND"); 
            this.stateComboBox.Items.Add("OH"); 
            this.stateComboBox.Items.Add("OK"); 
            this.stateComboBox.Items.Add("PA"); 
            this.stateComboBox.Items.Add("RI"); 
            this.stateComboBox.Items.Add("SC"); 
            this.stateComboBox.Items.Add("SC"); 
            this.stateComboBox.Items.Add("TN"); 
            this.stateComboBox.Items.Add("TX");
            this.stateComboBox.Items.Add("UT"); 
            this.stateComboBox.Items.Add("VT"); 
            this.stateComboBox.Items.Add("VA"); 
            this.stateComboBox.Items.Add("WA"); 
            this.stateComboBox.Items.Add("WV"); 
            this.stateComboBox.Items.Add("WI");
            this.stateComboBox.Items.Add("PR");
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
