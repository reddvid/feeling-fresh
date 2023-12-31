using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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

    [ObservableProperty] private Win32App _selectedItem;

    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private bool _isSorted;
    
    public MainViewModel(ILoggerAdapter<MainViewModel> logger, IAppRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [RelayCommand]
    private async Task GetData()
    {
        IsLoading = true;
        
        Apps = new ObservableCollection<Win32App>(await _repository.GetAllAsync());

        IsLoading = false;
    }

    [RelayCommand]
    private void SortData()
    {
        if (Apps.Count == 0) return;

        if (IsSorted)
        {
          Apps = new ObservableCollection<Win32App>(Apps?.OrderBy(x => x.AppName));
        }
        else
        {
          Apps = new ObservableCollection<Win32App>(Apps?.OrderBy(x => x.Id));
        }
        
        OnPropertyChanged(nameof(Apps));
    }
}