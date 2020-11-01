using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Covid19Analysis.Model;

namespace Covid19Analysis.Utility
{
    /// <summary>
    ///     Allows user to choose a file to save Data to
    /// </summary>
    public class FileSaver
    {
        #region Data members

        /// <summary>
        ///     The comma
        /// </summary>
        public const string Comma = ",";

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the Data.
        /// </summary>
        /// <value>
        ///     The Data.
        /// </value>
        public IEnumerable<DailyCovidStat> Data { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [file saved].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [file saved]; otherwise, <c>false</c>.
        /// </value>
        public bool FileSaved { get; set; }

        /// <summary>
        ///     Gets or sets the Data in CSV form.
        /// </summary>
        /// <value>
        ///     The Data in CSV form.
        /// </value>
        public string DataInCSVForm { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileSaver" /> class.
        /// </summary>
        /// <param name="data">The Data.</param>
        public FileSaver(IEnumerable<DailyCovidStat> data)
        {
            this.Data = data;
            this.FileSaved = false;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Initializes the filesaver. Allows the user to choose a file to write the covid Data to.
        /// </summary>
        public async void Initialize()
        {
            this.formatCollectionForSave();
            var savePicker = new FileSavePicker {SuggestedStartLocation = PickerLocationId.DocumentsLibrary};

            savePicker.FileTypeChoices.Add("Text", new List<string> {".txt"});
            savePicker.FileTypeChoices.Add("CSV", new List<string> {".csv"});

            savePicker.SuggestedFileName = "New Document";
            var file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                CachedFileManager.DeferUpdates(file);

                await FileIO.WriteTextAsync(file, this.DataInCSVForm);

                var status =
                    await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == FileUpdateStatus.Complete)
                {
                    this.FileSaved = true;
                }
            }
        }

        private string formatCollectionForSave()
        {
            foreach (var currentDay in this.Data)
            {
                this.DataInCSVForm += currentDay.Date.ToString("yyyyMMdd");
                this.DataInCSVForm += Comma;

                this.DataInCSVForm += currentDay.Region;
                this.DataInCSVForm += Comma;

                this.DataInCSVForm += currentDay.PositiveIncrease;
                this.DataInCSVForm += Comma;

                this.DataInCSVForm += currentDay.NegativeIncrease;
                this.DataInCSVForm += Comma;

                this.DataInCSVForm += currentDay.DeathIncrease;
                this.DataInCSVForm += Comma;

                this.DataInCSVForm += currentDay.HospitalizedIncrease;
                this.DataInCSVForm += Environment.NewLine;
            }

            return this.DataInCSVForm;
        }

        #endregion
    }
}