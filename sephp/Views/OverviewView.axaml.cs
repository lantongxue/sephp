using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using ReactiveUI;
using sephp.Monitor.System;
using sephp.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Joins;
using System.Reactive.Linq;

namespace sephp.Views;


public partial class OverviewView : ReactiveUserControl<OverviewViewModel>
{
    private CpuMonitor cpu = new CpuMonitor();
    private MemoryMonitor memory = new MemoryMonitor();
    private NetworkMonitor network = new NetworkMonitor();
    public OverviewView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            // Ã¿Ãë²ÉÑù
            Observable.Interval(TimeSpan.FromSeconds(1))
                .SelectMany(async _ => {
                    var cpuTimes = await cpu.GetCpuUsageAsync();
                    var (memUsage, memTotal) = await memory.GetMemoryUsageAsync();
                    var (upload, download) = network.GetNetworkUsage();
                    return (cpuTimes, memUsage, memTotal, upload, download);
                })
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(result =>
                {
                    if(ViewModel!.CpuTimes.Count >= 10)
                    {
                        ViewModel.CpuTimes.RemoveAt(0);
                    }   
                    ViewModel.CpuTimes.Add(new ObservableValue { Value = result.cpuTimes });

                    if(ViewModel.MemoryTotal.Count >= 10)
                    {
                        ViewModel.MemoryTotal.RemoveAt(0);
                    }
                    ViewModel.MemoryTotal.Add(new ObservableValue { Value = result.memTotal });

                    if(ViewModel.MemoryUsgae.Count >= 10)
                    {
                        ViewModel.MemoryUsgae.RemoveAt(0);
                    }
                    ViewModel.MemoryUsgae.Add(new ObservableValue { Value = result.memUsage });
                    if(ViewModel.NetworkDownloads.Count >= 10)
                    {
                        ViewModel.NetworkDownloads.RemoveAt(0);
                    }
                    ViewModel.NetworkDownloads.Add(new ObservableValue { Value = result.download });
                    if(ViewModel.NetworkUploads.Count >= 10)
                    {
                        ViewModel.NetworkUploads.RemoveAt(0);
                    }
                    ViewModel.NetworkUploads.Add(new ObservableValue { Value = result.upload });
                })
                .DisposeWith(disposables);

        });

    }
}