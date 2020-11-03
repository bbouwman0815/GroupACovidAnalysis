using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Covid19Analysis.Annotations;
using Covid19Analysis.Extensions;
using Covid19Analysis.Model;
using Covid19Analysis.Report;
using Covid19Analysis.Utility;

namespace Covid19Analysis.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
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
        ///     Gets or sets the errors.
        /// </summary>
        /// <value>
        ///     The errors.
        /// </value>
        public string Errors { get; set; }

        /// <summary>
        ///     Gets or sets the added daily covid stat.
        /// </summary>
        /// <value>
        ///     the added daily covid stat.
        /// </value>
        public DailyCovidStat SelectedStat { get; set; }

        /// <summary>
        ///     Gets the file loader.
        /// </summary>
        /// <value>
        ///     The file loader.
        /// </value>
        public FileLoader FileLoader { get; }

        public ObservableCollection<DailyCovidStat> Statistics { get; set; }

        public MainPageViewModel()
        {
            this.HistogramBinSize = DefaultHistogramBinSize;
            this.Region = DefaultRegion;
            this.FileLoader = new FileLoader();
            this.SummaryReport = new SummaryReport(DefaultRegion, this.FileLoader.LoadedCovidStats, DefaultLowerBound,
                DefaultUpperBound,
                DefaultHistogramBinSize);
            this.Statistics = ListExtensions.ToObservableCollection(this.SummaryReport.RegionData[this.Region]);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
