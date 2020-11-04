using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Covid19Analysis.Annotations;
using Covid19Analysis.Extensions;
using Covid19Analysis.Model;
using Covid19Analysis.Report;
using Covid19Analysis.Utility;
using Covid19Analysis.View;

namespace Covid19Analysis.CovidAnalysisViewModel
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        #region Data members

        /// <summary>
        ///     The Region
        /// </summary>
        public const string DefaultRegion = "GA";

        /// <summary>
        ///     The default upper bound
        /// </summary>
        public const int DefaultUpperBound = 2500;

        /// <summary>
        ///     The default lower bound
        /// </summary>
        public const int DefaultLowerBound = 1000;

        /// <summary>
        ///     The default histogram bin size
        /// </summary>
        public const int DefaultHistogramBinSize = 500;

        public TotalCovidStats AllStatistics;

        private DailyCovidStat selectedStat;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the summary report.
        /// </summary>
        /// <value>
        ///     The summary report.
        /// </value>
        public SummaryReport SummaryReport { get; set; }

        /// <summary>
        ///     Gets or sets the lower bound.
        /// </summary>
        /// <value>
        ///     The lower bound.
        /// </value>
        public int LowerBound { get; set; }

        /// <summary>
        ///     Gets or sets the upper bound.
        /// </summary>
        /// <value>
        ///     The upper bound.
        /// </value>
        public int UpperBound { get; set; }

        /// <summary>
        ///     Gets or sets the size of the histogram bin.
        /// </summary>
        /// <value>
        ///     The size of the histogram bin.
        /// </value>
        public int HistogramBinSize { get; set; }

        /// <summary>
        ///     Gets or sets the region.
        /// </summary>
        /// <value>
        ///     The region.
        /// </value>
        public string Region { get; set; }

        /// <summary>
        ///     Gets or sets the selected daily covid stat.
        /// </summary>
        /// <value>
        ///     the selected daily covid stat.
        /// </value>
        public DailyCovidStat SelectedStat
        {
            get => this.selectedStat;
            set
            {
                this.selectedStat = value;
                this.OnPropertyChanged();
                this.DeleteCommand.OnCanExecuteChanged();
            }
        }

        /// <summary>
        ///     Gets the file loader.
        /// </summary>
        /// <value>
        ///     The file loader.
        /// </value>
        public FileLoader FileLoader { get; }

        public ObservableCollection<DailyCovidStat> Statistics { get; set; }

        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand EditCommand { get; set; }

        public RelayCommand DisplayDetailsCommand { get; set; }

        public RelayCommand LoadFileCommand { get; set; }

        #endregion

        #region Constructors

        public MainPageViewModel()
        {
            this.HistogramBinSize = DefaultHistogramBinSize;
            this.Region = DefaultRegion;
            this.LowerBound = DefaultLowerBound;
            this.UpperBound = DefaultUpperBound;
            this.FileLoader = new FileLoader();
            this.SummaryReport = new SummaryReport(DefaultRegion, this.FileLoader.LoadedCovidStats, DefaultLowerBound,
                DefaultUpperBound,
                DefaultHistogramBinSize);
            this.AllStatistics = new TotalCovidStats();
            this.Statistics = this.AllStatistics.ToObservableCollection();
            this.loadCommands();
        }

        #endregion

        #region Methods

        public event PropertyChangedEventHandler PropertyChanged;

        private void loadCommands()
        {
            this.DeleteCommand = new RelayCommand(this.DeleteStatistic, this.CanDeleteStatistic);
            this.EditCommand = new RelayCommand(this.EditStatistic, this.CanEditStatistic);
            this.DisplayDetailsCommand =
                new RelayCommand(this.DisplayStatisticDetails, this.CanDisplayStatisticDetails);
            this.LoadFileCommand = new RelayCommand(this.LoadFile, this.CanLoadFile);
        }

        private bool CanLoadFile(object obj)
        {
            return true;
        }

        private bool CanDisplayStatisticDetails(object obj)
        {
            return this.SelectedStat != null;
        }

        private async void DisplayStatisticDetails(object obj)
        {
            var displayStatDetails = new DisplayStatDetailsContentDialog(this.SelectedStat.GetsFullFormattedString());

            await displayStatDetails.ShowAsync();
        }

        private bool CanEditStatistic(object obj)
        {
            return this.SelectedStat != null;
        }

        private async void EditStatistic(object obj)
        {
            var editContentDialog = new EditDailyStatContentDialog(this.SelectedStat);

            await editContentDialog.ShowAsync();
        }

        private bool CanDeleteStatistic(object obj)
        {
            return this.SelectedStat != null;
        }

        private void DeleteStatistic(object obj)
        {
            this.AllStatistics.Remove(this.SelectedStat);
            this.Statistics = this.AllStatistics.ToObservableCollection();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadFile(object obj)
        {
            this.FileLoader.DuplicateData.Clear();
            if (this.FileLoader.LoadedCovidStats.ContainsData())
            {
                this.displayExistingFileDialog();
            }

            if (!this.FileLoader.LoadedCovidStats.ContainsData())
            {
                this.loadAndReadFile();
            }
        }

        private void loadAndReadFile()
        {
            var fileOpenPicker = this.setFileOpenPicker();
            this.openFile(fileOpenPicker);
        }

        private FileOpenPicker setFileOpenPicker()
        {
            var fileOpener = new FileOpenPicker {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            fileOpener.FileTypeFilter.Add(".csv");
            fileOpener.FileTypeFilter.Add(".txt");

            return fileOpener;
        }

        private async void openFile(FileOpenPicker picker)
        {
            var selectedFile = await picker.PickSingleFileAsync();

            if (selectedFile != null)
            {
                this.processFile(selectedFile);
            }
        }

        private async void processFile(StorageFile selectedFile)
        {
            string fileContent;
            var fileToRead = await selectedFile.OpenAsync(FileAccessMode.Read);
            _ = new FileInfo(selectedFile.Path);

            using (var streamReader = new StreamReader(fileToRead.AsStream()))
            {
                fileContent = await streamReader.ReadToEndAsync();
            }

            fileContent = fileContent.Replace("\r", string.Empty);

            this.FileLoader.LoadFile(fileContent);
            this.handleDuplicateDays();
             this.updateSummary();
        }

        private void handleDuplicateDays()
        {
            if (this.FileLoader.DuplicateData.Count > 0)
            {
                this.displayExistingRecordsDialog();
            }
        }

        private async void displayExistingFileDialog()
        {
            var existingFile = new ExistingFileContentDialog();

            var result = await existingFile.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                this.FileLoader.LoadedCovidStats.Clear();
                this.loadAndReadFile();
            }

            if (result == ContentDialogResult.Secondary)
            {
                this.loadAndReadFile();
            }
        }

        private async void displayExistingRecordsDialog()
        {
            var hasReplaceAllSelected = false;
            foreach (var currentDay in this.FileLoader.DuplicateData.ToList())
            {
                var existingCovidDay = new DuplicateStatContentDialog(currentDay);
                try
                {
                    if (!hasReplaceAllSelected)
                    {
                        await existingCovidDay.ShowAsync();
                    }

                    if (existingCovidDay.Result == Result.Replace)
                    {
                        this.FileLoader.LoadedCovidStats.ReplaceDuplicate(currentDay);
                        this.FileLoader.DuplicateData.Remove(currentDay);
                    }

                    if (existingCovidDay.Result == Result.ReplaceAll)
                    {
                        this.handleReplaceAllDuplicates();
                        hasReplaceAllSelected = true;
                    }

                    if (existingCovidDay.Result == Result.DiscardAll)
                    {
                        this.FileLoader.DuplicateData.Clear();
                        this.handleReplaceAllDuplicates();
                        hasReplaceAllSelected = true;
                    }
                }
                catch
                {
                    throw new ArgumentException();
                }

                this.updateSummary();
            }
        }

        private void handleReplaceAllDuplicates()
        {
            foreach (var currentDuplicate in this.FileLoader.DuplicateData.ToList())
            {
                this.FileLoader.LoadedCovidStats.ReplaceDuplicate(currentDuplicate);
                this.FileLoader.DuplicateData.Remove(currentDuplicate);
            }
        }

        private async void displayLineErrorDialog()
        {
            var errorDialog = new ContentDialog {
                Title = "Logged Errors",
                Content = this.FileLoader.Errors,
                CloseButtonText = "Close"
            };

            await errorDialog.ShowAsync();
        }

        private void updateSummary()
        {
            this.SummaryReport =
                new SummaryReport(this.Region, this.FileLoader.LoadedCovidStats, this.LowerBound, this.UpperBound,
                    this.HistogramBinSize);
            this.AllStatistics = this.FileLoader.LoadedCovidStats;
            this.Statistics = this.AllStatistics.ToObservableCollection();
        }


        

        #endregion
    }
}