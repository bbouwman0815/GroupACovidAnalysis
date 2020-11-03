using System;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Covid19Analysis.CollectionQueries;
using Covid19Analysis.Model;
using Covid19Analysis.Report;
using Covid19Analysis.Utility;
using Covid19Analysis.View;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Covid19Analysis
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Data members

        /// <summary>
        ///     The application height
        /// </summary>
        public const int ApplicationHeight = 400;

        /// <summary>
        ///     The application width
        /// </summary>
        public const int ApplicationWidth = 625;

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
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public string Region { get; set; }

        /// <summary>
        ///     Gets or sets the added daily covid stat.
        /// </summary>
        /// <value>
        ///     the added daily covid stat.
        /// </value>
        public DailyCovidStat AddedStat { get; set; }

        /// <summary>
        ///     Gets the file loader.
        /// </summary>
        /// <value>
        ///     The file loader.
        /// </value>
        public FileLoader FileLoader { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.HistogramBinSize = DefaultHistogramBinSize;
            this.Region = DefaultRegion;
            this.FileLoader = new FileLoader();
            this.SummaryReport = new SummaryReport(DefaultRegion, this.FileLoader.LoadedCovidStats, DefaultLowerBound,
                DefaultUpperBound,
                DefaultHistogramBinSize);
            this.dataListView.ItemsSource = this.FileLoader.LoadedCovidStats;

            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));
        }

        #endregion

        #region Methods

        private void loadFile_Click(object sender, RoutedEventArgs e)
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
            var fileOpener = new FileOpenPicker
            {
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
            this.dataListView.ItemsSource = this.FileLoader.LoadedCovidStats.ToList();
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
            var errorDialog = new ContentDialog
            {
                Title = "Logged Errors",
                Content = this.FileLoader.Errors,
                CloseButtonText = "Close"
            };
            await errorDialog.ShowAsync();
        }


        private async void getLowerBoundButton_Click(object sender, RoutedEventArgs e)
        {
            var getLowerBound = new LowerBoundContentDialog();

            var result = await getLowerBound.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                this.LowerBound = getLowerBound.Bound;
                this.updateSummary();
            }
        }

        private async void getUpperBoundButton_Click(object sender, RoutedEventArgs e)
        {
            var getUpperBound = new UpperBoundContentDialog();

            var result = await getUpperBound.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                this.UpperBound = getUpperBound.Bound;
                this.updateSummary();
            }
        }

        private void updateSummary()
        {
            this.SummaryReport =
                new SummaryReport(this.Region, this.FileLoader.LoadedCovidStats, this.LowerBound, this.UpperBound,
                    this.HistogramBinSize);
            this.dataListView.ItemsSource = this.FileLoader.LoadedCovidStats.ToList();
        }

        private void clearSummary()
        {
            this.FileLoader.LoadedCovidStats.Clear();
            this.dataListView.ItemsSource = this.FileLoader.LoadedCovidStats.ToList();
        }

        private void displayErrorsButton_Click(object sender, RoutedEventArgs e)
        {
            this.displayLineErrorDialog();
        }

        private void clearDataButton_Click(object sender, RoutedEventArgs e)
        {
            this.clearSummary();
            this.FileLoader.Errors = string.Empty;
        }

        private async void addDataButton_Click(object sender, RoutedEventArgs e)
        {
            var addNewDailyCovidStat = new AddDailyStatContentDialog();

            var result = await addNewDailyCovidStat.ShowAsync();

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

            this.AddedStat = addNewDailyCovidStat.AddedDailyCovidStat;
            if (CheckMultipleMinMax.CheckSpecificDuplicate(this.AddedStat, this.FileLoader.LoadedCovidStats.ToList()))
            {
                this.handleAddWithDuplicates();
            }

            if (!CheckMultipleMinMax.CheckSpecificDuplicate(this.AddedStat, this.FileLoader.LoadedCovidStats.ToList()))
            {
                this.FileLoader.LoadedCovidStats.Add(this.AddedStat);
                this.updateSummary();
            }
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

        private async void handleAddWithDuplicates()
        {
            var duplicateStatContentDialog = new DuplicateStatContentDialog(this.AddedStat);

            await duplicateStatContentDialog.ShowAsync();

            if (duplicateStatContentDialog.Result == Result.Replace)
            {
                this.FileLoader.LoadedCovidStats.ReplaceDuplicate(this.AddedStat);
                this.updateSummary();
            }
        }

        private async void getHistogramButton_Click(object sender, RoutedEventArgs e)
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
            var errorDialog = new ContentDialog
            {
                Title = "Set Histogram Bin Error",
                Content = "Unable to set. Make sure the number is positive",
                CloseButtonText = "Close"
            };
            await errorDialog.ShowAsync();
        }

        private void saveData_Click(object sender, RoutedEventArgs e)
        {
            var collectionToSave = CollectionFilters.FilterByRegion(this.FileLoader.LoadedCovidStats.ToList(), Region);
            var fileSaver = new FileSaver(collectionToSave);
            fileSaver.Initialize();
        }

        #endregion

        private async void SelectStateButton_OnClick(object sender, RoutedEventArgs e)
        {
            var getRegion = new StateContentDialog();

            var result = await getRegion.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                if (!this.SummaryReport.RegionData.ContainsKey(getRegion.Region))
                {
                    this.displayStateError();
                    this.Region = DefaultRegion;
                }
                else
                {
                    this.Region = getRegion.Region;
                    this.updateSummary();
                }
            }
        }

        private async void DisplaySummaryButton_OnClick(object sender, RoutedEventArgs e)
        {
            var displaySummary = new SummaryContentDialog(this.SummaryReport.GenerateDataForRegion());

            await displaySummary.ShowAsync();
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

        private async void DisplayDailyStatDetailsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = this.dataListView.SelectedItem;
            if (selectedItem == null || selectedItem.GetType() != typeof(DailyCovidStat))
            {
                return;
            }

            DailyCovidStat selectedStat = selectedItem as DailyCovidStat;
            if (selectedStat == null)
            {
                return;
            }

            var displayStatDetails = new DisplayStatDetailsContentDialog(selectedStat.GetsFullFormattedString());

            await displayStatDetails.ShowAsync();

        }

        private void DataListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.displayDailyStatDetailsButton.IsEnabled = true;
            this.editSelectedDayButton.IsEnabled = true;
            this.deleteSelectedDayButton.IsEnabled = true;
        }

        private void deleteSelectedDayButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = this.dataListView.SelectedItem;

            if (selectedItem == null || selectedItem.GetType() != typeof(DailyCovidStat))
            {
                return;
            }

            DailyCovidStat selectedStat = selectedItem as DailyCovidStat;
            this.FileLoader.LoadedCovidStats.Remove(selectedStat);
            this.dataListView.ItemsSource = this.FileLoader.LoadedCovidStats.ToList();
        }

        private async void EditSelectedDayButton_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = this.dataListView.SelectedItem;
            if (selectedItem.GetType().Equals(typeof(DailyCovidStat)))
            {
                DailyCovidStat selectedStat = (DailyCovidStat) selectedItem;
                var editContentDialog = new EditDailyStatContentDialog(selectedStat);
                
                await editContentDialog.ShowAsync();
                
            }
        }
    }
}