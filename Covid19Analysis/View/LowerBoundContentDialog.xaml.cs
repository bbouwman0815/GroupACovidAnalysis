using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Covid19Analysis.View
{
    /// <summary>
    ///     Allows the user to set a lower boundary for checking Data
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class LowerBoundContentDialog : ContentDialog
    {
        #region Data members

        /// <summary>
        ///     The default bound
        /// </summary>
        public const int DefaultBound = 1000;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the bound.
        /// </summary>
        /// <value>
        ///     The bound.
        /// </value>
        public int Bound { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LowerBoundContentDialog" /> class.
        /// </summary>
        public LowerBoundContentDialog()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Bound = int.Parse(this.lowerBoundTextBox.Text);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Bound = DefaultBound;
        }

        #endregion
    }
}