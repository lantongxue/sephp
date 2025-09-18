using Avalonia;
using Avalonia.Threading;
using LiveChartsCore.Defaults;
using ReactiveUI;
using sephp.I18n;
using sephp.Monitor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace sephp.ViewModels
{
    public class OverviewViewModel : ViewModelBase, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = Resource.Overview;

        public IScreen HostScreen { get; }

        public ObservableCollection<ObservableValue> CpuTimes { get; set; } = [];

        public ObservableCollection<ObservableValue> NetworkDownloads { get; set; } = [];

        public ObservableCollection<ObservableValue> NetworkUploads { get; set; } = [];

        public ObservableCollection<ObservableValue> MemoryTotal { get; set; } = [];

        public ObservableCollection<ObservableValue> MemoryUsgae { get; set; } = [];

        public OverviewViewModel(IScreen screen)
        {
            HostScreen = screen;

            DispatcherTimer timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private Cpu cpu = new Cpu();
        private Network network = new Network();

        private Memory memory = new Memory();

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            if(CpuTimes.Count >= 10)
            {
                CpuTimes.RemoveAt(0);
            }
            CpuTimes.Add(new ObservableValue
            {
                Value = await cpu.GetCpuUsageAsync()
            });

            var (download, upload) = network.GetNetworkUsage();

            if (NetworkDownloads.Count >= 10)
            {
                NetworkDownloads.RemoveAt(0);
            }
            NetworkDownloads.Add(new ObservableValue
            {
                Value = download
            });

            if (NetworkUploads.Count >= 10)
            {
                NetworkUploads.RemoveAt(0);
            }
            NetworkUploads.Add(new ObservableValue
            {
                Value = upload
            });

            var (usage, total) = await memory.GetMemoryUsageAsync();

            if (MemoryUsgae.Count >= 10)
            {
                MemoryUsgae.RemoveAt(0);
            }
            MemoryUsgae.Add(new ObservableValue
            {
                Value = usage
            });

            if (MemoryTotal.Count >= 10)
            {
                MemoryTotal.RemoveAt(0);
            }
            MemoryTotal.Add(new ObservableValue
            {
                Value = total
            });
        }
    }

    

}
