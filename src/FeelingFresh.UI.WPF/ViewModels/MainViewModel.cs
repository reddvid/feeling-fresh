using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FeelingFresh.Library;
using FeelingFresh.Library.Logging;
using FeelingFresh.Library.Models;
using FeelingFresh.Library.Repositories;

namespace FeelingFresh.UI.WPF.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ILoggerAdapter<MainViewModel> _logger;
    private readonly IAppRepository _repository;

    [ObservableProperty] private ObservableCollection<Win32App> _apps;
    
    public MainViewModel(ILoggerAdapter<MainViewModel> logger, IAppRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [RelayCommand]
    private async Task GetData()
    {
       Apps = new ObservableCollection<Win32App>(await _repository.GetAllAsync());
       
       OnPropertyChanged(nameof(Apps));
    }
}