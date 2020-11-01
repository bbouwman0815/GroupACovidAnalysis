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

        public StateContentDialog()
        {
            this.InitializeComponent();
            StateComboBox.Items.Add("AL");
            StateComboBox.Items.Add("AK");
            StateComboBox.Items.Add("AZ"); 
            StateComboBox.Items.Add("AR"); 
            StateComboBox.Items.Add("CA"); 
            StateComboBox.Items.Add("CO"); 
            StateComboBox.Items.Add("CT"); 
            StateComboBox.Items.Add("DE"); 
            StateComboBox.Items.Add("DC"); 
            StateComboBox.Items.Add("FL"); 
            StateComboBox.Items.Add("GA"); 
            StateComboBox.Items.Add("HI"); 
            StateComboBox.Items.Add("ID"); 
            StateComboBox.Items.Add("IL"); 
            StateComboBox.Items.Add("IN"); 
            StateComboBox.Items.Add("IA"); 
            StateComboBox.Items.Add("KS"); 
            StateComboBox.Items.Add("KY"); 
            StateComboBox.Items.Add("LA"); 
            StateComboBox.Items.Add("ME"); 
            StateComboBox.Items.Add("MD"); 
            StateComboBox.Items.Add("MA"); 
            StateComboBox.Items.Add("MI"); 
            StateComboBox.Items.Add("MN"); 
            StateComboBox.Items.Add("MS"); 
            StateComboBox.Items.Add("MO"); 
            StateComboBox.Items.Add("MT"); 
            StateComboBox.Items.Add("NE");
            StateComboBox.Items.Add("NV"); 
            StateComboBox.Items.Add("NH");
            StateComboBox.Items.Add("NJ"); 
            StateComboBox.Items.Add("NM"); 
            StateComboBox.Items.Add("NY"); 
            StateComboBox.Items.Add("NC"); 
            StateComboBox.Items.Add("ND"); 
            StateComboBox.Items.Add("OH"); 
            StateComboBox.Items.Add("OK"); 
            StateComboBox.Items.Add("PA"); 
            StateComboBox.Items.Add("RI"); 
            StateComboBox.Items.Add("SC"); 
            StateComboBox.Items.Add("SC"); 
            StateComboBox.Items.Add("TN"); 
            StateComboBox.Items.Add("TX");
            StateComboBox.Items.Add("UT"); 
            StateComboBox.Items.Add("VT"); 
            StateComboBox.Items.Add("VA"); 
            StateComboBox.Items.Add("WA"); 
            StateComboBox.Items.Add("WV"); 
            StateComboBox.Items.Add("WI");
            StateComboBox.Items.Add("PR");
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (StateComboBox.SelectedItem != null) this.Region = StateComboBox.SelectedItem.ToString();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
