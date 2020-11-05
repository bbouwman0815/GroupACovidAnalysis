using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
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

        public const string FilenameXmlSerialization = "covidStat.xml";

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
        public string DataInCsvForm { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileSaver" /> class.
        /// </summary>
        /// <param name="data">The Data.</param>
        public FileSaver(IEnumerable<DailyCovidStat> data)
        {
            this.DataInCsvForm = string.Empty;
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

            savePicker.FileTypeChoices.Add("CSV", new List<string> {".csv"});
            savePicker.FileTypeChoices.Add("XML", new List<string> { ".xml" });

            savePicker.SuggestedFileName = "New Document";
            var file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                if (file.FileType == ".xml")
                {
                    var outstream = await file.OpenStreamForWriteAsync();
                    var writer =
                        new XmlSerializer(typeof(DailyCovidStat));

                    foreach (var currentDay in this.Data)
                    {
                        writer.Serialize(outstream, currentDay);
                    }
                    outstream.Close();
                }
                else
                {
                    CachedFileManager.DeferUpdates(file);

                    await FileIO.WriteTextAsync(file, this.DataInCsvForm);

                    var status =
                        await CachedFileManager.CompleteUpdatesAsync(file);
                    if (status == FileUpdateStatus.Complete)
                    {
                        this.FileSaved = true;
                    }
                }
            }
        }

        private void formatCollectionForSave()
        {
            foreach (var currentDay in this.Data)
            {
                this.DataInCsvForm += currentDay.Date.ToString("yyyy/MM/dd");
                this.DataInCsvForm += Comma;

                this.DataInCsvForm += currentDay.Region;
                this.DataInCsvForm += Comma;

                this.DataInCsvForm += currentDay.PositiveIncrease;
                this.DataInCsvForm += Comma;

                this.DataInCsvForm += currentDay.NegativeIncrease;
                this.DataInCsvForm += Comma;

                this.DataInCsvForm += currentDay.HospitalizedCurrently;
                this.DataInCsvForm += Comma;

                this.DataInCsvForm += currentDay.HospitalizedIncrease;
                this.DataInCsvForm += Comma;

                this.DataInCsvForm += currentDay.DeathIncrease;

                this.DataInCsvForm += Environment.NewLine;
            }
        }

        #endregion
    }
}