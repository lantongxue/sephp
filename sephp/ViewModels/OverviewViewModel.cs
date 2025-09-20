using Avalonia;
using Avalonia.Controls;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using sephp.I18n;
using System;
using System.Collections.ObjectModel;


namespace sephp.ViewModels
{
    public partial class OverviewViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = Resource.Overview;

        public IScreen HostScreen { get; }

        public ObservableCollection<ObservableValue> NetworkDownloads { get; set; } = [];

        public ObservableCollection<ObservableValue> NetworkUploads { get; set; } = [];

        public ObservableCollection<ObservableValue> MemoryTotal { get; set; } = [];

        public ObservableCollection<ObservableValue> MemoryUsgae { get; set; } = [];

        public ObservableCollection<ObservableValue> CpuTimes { get; set; } = [];
        public ISeries[] CpuSeries =>
        [
            new LineSeries<ObservableValue>
            {
                Values = CpuTimes,
                GeometrySize = 0,
                LineSmoothness = 10,
                Name = "Usage",
                YToolTipLabelFormatter = point => $"{point.Coordinate.PrimaryValue} %"
            }
        ];

        public ISeries[] MemorySeries =>
        [
            new LineSeries<ObservableValue>
            {
                Values = MemoryTotal,
                GeometrySize = 0,
                LineSmoothness = 0,
                Name = "Total",
                YToolTipLabelFormatter = point => $"{Math.Round(point.Coordinate.PrimaryValue / 1024)} GB"
            },
            new LineSeries<ObservableValue>
            {
                Values = MemoryUsgae,
                GeometrySize = 0,
                LineSmoothness = 0,
                Name = "Usage",
                YToolTipLabelFormatter = point => $"{Math.Round(point.Coordinate.PrimaryValue / 1024)} GB"
            }
        ];

        public ISeries[] NetworkSeries =>
        [
            new LineSeries<ObservableValue>
            {
                Values = NetworkUploads,
                GeometrySize = 0,
                LineSmoothness = 10,
                Name = "Upload",
                YToolTipLabelFormatter = _networkYTooltipFormatter
            },
            new LineSeries<ObservableValue>
            {
                Values = NetworkDownloads,
                GeometrySize = 0,
                LineSmoothness = 10,
                Name = "Download",
                YToolTipLabelFormatter = _networkYTooltipFormatter
            }
        ];

        private Func<ChartPoint, string> _networkYTooltipFormatter = point => {
            if (point.Coordinate.PrimaryValue < 1024)
            {
                return $"{point.Coordinate.PrimaryValue} Kb";
            }
            else
            {
                return $"{Math.Round(point.Coordinate.PrimaryValue / 1024, 2)} Mb";
            }
        };

        public OverviewViewModel(IScreen screen)
        {
            HostScreen = screen;

            for(int i = 0; i < 10; i++)
            {
                CpuTimes.Add(new ObservableValue { Value = 0 });
                MemoryTotal.Add(new ObservableValue { Value = 0 });
                MemoryUsgae.Add(new ObservableValue { Value = 0 });
                NetworkDownloads.Add(new ObservableValue { Value = 0 });
                NetworkUploads.Add(new ObservableValue { Value = 0 });
            }
        }
    }
}
