using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Covid19Analysis.Annotations;
using Covid19Analysis.CollectionQueries;
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

        /// <summary>
        /// Gets or sets all statistics.
        /// </summary>
        /// <value>
        /// All statistics.
        /// </value>
        public TotalCovidStats AllStatistics;

        private DailyCovidStat selectedStat;
        private ObservableCollection<DailyCovidStat> statistic;


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
                this.EditCommand.OnCanExecuteChanged();
                this.DisplayDetailsCommand.OnCanExecuteChanged();
            }
        }

        /// <summary>
        ///     Gets the file loader.
        /// </summary>
        /// <value>
        ///     The file loader.
        /// </value>
        public FileLoader FileLoader { get; }

        private TotalCovidStats Data => this.FileLoader.LoadedCovidStats;

        /// <summary>
        /// Gets or sets the statistics.
        /// </summary>
        /// <value>
        /// The statistics.
        /// </value>
        public ObservableCollection<DailyCovidStat> Statistics
        {
            get { return statistic; }
            set
            {
                this.statistic = value;

                this.OnPropertyChanged();
                this.ClearAllDataCommand.OnCanExecuteChanged();
                this.DisplaySummaryCommand.OnCanExecuteChanged();
                this.DisplayErrorsCommand.OnCanExecuteChanged();
                this.DisplayDetailsCommand.OnCanExecuteChanged();
                this.SetUpperBoundCommand.OnCanExecuteChanged();
                this.SetLowerBoundCommand.OnCanExecuteChanged();
                this.SetHistogramCommand.OnCanExecuteChanged();
                this.SaveFileCommand.OnCanExecuteChanged();
                this.SetStateCommand.OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets the delete command.
        /// </summary>
        /// <value>
        /// The delete command.
        /// </value>
        public RelayCommand DeleteCommand { get; set; }

        /// <summary>
        /// Gets or sets the edit command.
        /// </summary>
        /// <value>
        /// The edit command.
        /// </value>
        public RelayCommand EditCommand { get; set; }

        /// <summary>
        /// Gets or sets the display details command.
        /// </summary>
        /// <value>
        /// The display details command.
        /// </value>
        public RelayCommand DisplayDetailsCommand { get; set; }

        /// <summary>
        /// Gets or sets the load file command.
        /// </summary>
        /// <value>
        /// The load file command.
        /// </value>
        public RelayCommand LoadFileCommand { get; set; }

        /// <summary>
        /// Gets or sets the clear all data command.
        /// </summary>
        /// <value>
        /// The clear all data command.
        /// </value>
        public RelayCommand ClearAllDataCommand { get; set; }

        /// <summary>
        /// Gets or sets the set state command.
        /// </summary>
        /// <value>
        /// The set state command.
        /// </value>
        public RelayCommand SetStateCommand { get; set; }

        /// <summary>
        /// Gets or sets the set upper bound command.
        /// </summary>
        /// <value>
        /// The set upper bound command.
        /// </value>
        public RelayCommand SetUpperBoundCommand { get; set; }

        /// <summary>
        /// Gets or sets the set lower bound command.
        /// </summary>
        /// <value>
        /// The set lower bound command.
        /// </value>
        public RelayCommand SetLowerBoundCommand { get; set; }

        /// <summary>
        /// Gets or sets the display errors command.
        /// </summary>
        /// <value>
        /// The display errors command.
        /// </value>
        public RelayCommand DisplayErrorsCommand { get; set; }

        /// <summary>
        /// Gets or sets the display summary command.
        /// </summary>
        /// <value>
        /// The display summary command.
        /// </value>
        public RelayCommand DisplaySummaryCommand { get; set; }

        /// <summary>
        /// Gets or sets the set histogram command.
        /// </summary>
        /// <value>
        /// The set histogram command.
        /// </value>
        public RelayCommand SetHistogramCommand { get; set; }

        /// <summary>
        /// Gets or sets the save file command.
        /// </summary>
        /// <value>
        /// The save file command.
        /// </value>
        public RelayCommand SaveFileCommand { get; set; }

        /// <summary>
        /// Gets or sets the add statistic command.
        /// </summary>
        /// <value>
        /// The add statistic command.
        /// </value>
        public RelayCommand AddStatisticCommand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        public MainPageViewModel()
        {
            this.initializeDefaultValues();
            this.loadCommands();
            this.FileLoader = new FileLoader();
            this.SummaryReport = new SummaryReport(this.Region, this.Data, DefaultLowerBound,
                DefaultUpperBound,
                DefaultHistogramBinSize);
            this.AllStatistics = new TotalCovidStats();
            this.Statistics = this.AllStatistics.ToObservableCollection();

        }

        private void initializeDefaultValues()
        {
            this.HistogramBinSize = DefaultHistogramBinSize;
            this.Region = String.Empty;
            this.LowerBound = DefaultLowerBound;
            this.UpperBound = DefaultUpperBound;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <returns></returns>
        public event PropertyChangedEventHandler PropertyChanged;

        private void loadCommands()
        {
            this.DeleteCommand = new RelayCommand(this.DeleteStatistic, this.CanDeleteStatistic);
            this.EditCommand = new RelayCommand(this.EditStatistic, this.CanEditStatistic);
            this.DisplayDetailsCommand =
                new RelayCommand(this.DisplayStatisticDetails, this.CanDisplayStatisticDetails);
            this.LoadFileCommand = new RelayCommand(this.LoadFile, this.CanLoadFile);
            this.ClearAllDataCommand = new RelayCommand(this.ClearAllStatistics, this.CanClearAllStatistics);
            this.DisplayErrorsCommand = new RelayCommand(DisplayErrors, CanDisplayErrors);
            this.DisplaySummaryCommand = new RelayCommand(DisplaySummary, CanDisplaySummary);
            this.SetHistogramCommand = new RelayCommand(SetHistogram, CanSetHistogram);
            this.SetStateCommand = new RelayCommand(SetState, CanSetState);
            this.SetLowerBoundCommand = new RelayCommand(SetLowerBound, CanSetLowerBound);
            this.SetUpperBoundCommand = new RelayCommand(SetUpperBound, CanSetUpperBound);
            this.SaveFileCommand = new RelayCommand(SaveFile, CanSaveFile);
            this.AddStatisticCommand = new RelayCommand(AddStatistic, CanAddStatistic);
        }

        private bool CanAddStatistic(object obj)
        {
            return true;
        }

        private async void AddStatistic(object obj)
        {
            var addNewDailyCovidStat = new AddDailyStatContentDialog();

            var result = await addNewDailyCovidStat.ShowAsync();
            DailyCovidStat addedStat = null;
            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            if (addNewDailyCovidStat.Death < 0 || addNewDailyCovidStat.PositiveTestCount < 0 ||
                addNewDailyCovidStat.NegativeTestCount < 0 || addNewDailyCovidStat.HospitalizationCount < 0)
            {
                this.displayAddError();
                return;
            }

            addedStat = addNewDailyCovidStat.AddedDailyCovidStat;
            if (CheckMultipleMinMax.CheckSpecificDuplicate(addedStat, this.Data))
            {
                this.handleAddWithDuplicates(addedStat);
            }

            if (!CheckMultipleMinMax.CheckSpecificDuplicate(addedStat, this.Data))
            {
                this.Data.Add(addedStat);
                this.AllStatistics = this.Data;
                this.updateSummary();
            }

        }

        private bool CanSaveFile(object obj)
        {
            return this.AllStatistics.Count > 0 && this.Region != string.Empty;
        }

        private void SaveFile(object obj)
        {
            var collectionToSave = CollectionFilters.FilterByRegion(this.Data, Region);
            var fileSaver = new FileSaver(collectionToSave);
            fileSaver.Initialize();
        }

        private async void SetState(object obj)
        {
            var getRegion = new StateContentDialog(this.AllStatistics);

            var result = await getRegion.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var regionalData =
                    this.Data.CreateRegionalDictionary(this.Data
                        .ToList());
                if (!regionalData.ContainsKey(getRegion.Region))
                {
                    this.displayStateError();
                    this.Region = String.Empty;
                }
                else
                {
                    this.Region = getRegion.Region;
                    this.updateSummary();
                }
            }
        }

        private async void SetHistogram(object obj)
        {
            var getBinSize = new HistogramBinContentDialog();

            var result = await getBinSize.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (getBinSize.BinSize > 0)
                {
                    this.HistogramBinSize = getBinSize.BinSize;
                    this.updateSummary();
                }
                else
                {
                    this.displayHistogramError();
                }
            }
        }

        private async void displayHistogramError()
        {
            var errorDialog = new ContentDialog {
                Title = "Set Histogram Bin Error",
                Content = "Unable to set. Make sure the number is positive",
                CloseButtonText = "Close"
            };
            await errorDialog.ShowAsync();
        }

        private async void SetLowerBound(object obj)
        {
            var getLowerBound = new LowerBoundContentDialog();

            var result = await getLowerBound.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                this.LowerBound = getLowerBound.Bound;
                this.updateSummary();
            }
        }

        private async void SetUpperBound(object obj)
        {
            var getUpperBound = new UpperBoundContentDialog();

            var result = await getUpperBound.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                this.UpperBound = getUpperBound.Bound;
                this.updateSummary();
            }
        }

        private async void DisplaySummary(object obj)
        {
            var displaySummary = new SummaryContentDialog(this.SummaryReport.GenerateDataForRegion());

            await displaySummary.ShowAsync();
        }

        private bool CanSetUpperBound(object obj)
        {
            return this.AllStatistics.Count > 0;
        }

        private bool CanSetLowerBound(object obj)
        {
            return this.AllStatistics.Count > 0;
        }

        private bool CanSetState(object obj)
        {
            return this.AllStatistics.Count > 0;
        }

        private bool CanSetHistogram(object obj)
        {
            return this.AllStatistics.Count > 0;
        }

        private bool CanDisplaySummary(object obj)
        {
            return this.AllStatistics.Count > 0;
        }

        private bool CanDisplayErrors(object obj)
        {
            return this.AllStatistics.Count > 0;
        }

        private void DisplayErrors(object obj)
        {
            this.displayLineErrorDialog();
        }

        private bool CanClearAllStatistics(object obj)
        {
            return this.AllStatistics.Count > 0;
        }

        private void ClearAllStatistics(object obj)
        {
            this.AllStatistics.Clear();
            this.Region = string.Empty;
            this.FileLoader.Errors = string.Empty;
            this.Statistics = this.AllStatistics.ToObservableCollection();
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
            this.AllStatistics.Remove(this.SelectedStat);
            this.SelectedStat = editContentDialog.EditedDailyCovidStat;
            this.AllStatistics.Add(SelectedStat);
            this.updateSummary();
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
            if (this.AllStatistics.ContainsData())
            {
                this.displayExistingFileDialog();
            }

            if (!this.AllStatistics.ContainsData())
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
            this.AllStatistics = this.Data;
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
                this.Data.Clear();
                this.AllStatistics.Clear();
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
                        this.AllStatistics.ReplaceDuplicate(currentDay);
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
                this.AllStatistics.ReplaceDuplicate(currentDuplicate);
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
                new SummaryReport(this.Region, this.AllStatistics, this.LowerBound, this.UpperBound,
                    this.HistogramBinSize);
            var regionalData =
                this.AllStatistics.CreateRegionalDictionary(this.AllStatistics.ToList());
            if (this.Region == String.Empty)
            {
                this.Statistics = this.AllStatistics.ToObservableCollection();
            }
            else
            {
                regionalData[this.Region].Sort();
                this.Statistics = regionalData[this.Region].ToObservableCollection();
            }
    }


private async void displayStateError()
        {
            var stateDialog = new ContentDialog
            {
                Title = "Set State Error",
                Content = "Unable to set. There isn't any data with selected state",
                CloseButtonText = "Close"
            };
            await stateDialog.ShowAsync();
        }
        private async void displayAddError()
        {
            var errorDialog = new ContentDialog
            {
                Title = "Add Error",
                Content = "Unable to add. Make sure all fields are correct.",
                CloseButtonText = "Close"
            };
            await errorDialog.ShowAsync();
        }

        private async void handleAddWithDuplicates(DailyCovidStat addedStat)
        {
            var duplicateStatContentDialog = new DuplicateStatContentDialog(addedStat);

            await duplicateStatContentDialog.ShowAsync();

            if (duplicateStatContentDialog.Result == Result.Replace)
            {
                this.AllStatistics.ReplaceDuplicate(addedStat);
                this.updateSummary();
            }
        }

        #endregion
    }
}
