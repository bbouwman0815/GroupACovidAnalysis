﻿using System;
using Windows.UI.Xaml.Controls;
using Covid19Analysis.Model;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Covid19Analysis.View
{
    /// <summary>
    ///     Brings up a content dialog displaying the details of the selected stats
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class DisplayStatDetailsContentDialog : ContentDialog
    {
        #region Data members        
        
        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddDailyStatContentDialog" /> class.
        /// </summary>
        public DisplayStatDetailsContentDialog(String displayText)
        {
            this.InitializeComponent();
            this.detailsTextBox.Text = displayText;
        }

        #endregion

        #region Methods

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        #endregion
    }
}