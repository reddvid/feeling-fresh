using System;
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
using FeelingFresh.UI.WPF.Helpers;

namespace FeelingFresh.UI.WPF.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ILoggerAdapter<MainViewModel> _logger;
    private readonly IAppRepository _repository;

    [ObservableProperty] private ObservableCollection<Win32App?> _apps;

    [ObservableProperty] private Win32App? _selectedItem;

    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private bool _isSorted;

    [ObservableProperty] private string _queryText;

    [ObservableProperty] private string _searchResult;

    partial void OnQueryTextChanging(string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            OnPropertyChanging(nameof(HasQueryText));
        }
    }

    public bool HasQueryText => !string.IsNullOrWhiteSpace(QueryText);

    public MainViewModel(ILoggerAdapter<MainViewModel> logger, IAppRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [RelayCommand]
    private async Task GetApps()
    {
        IsLoading = true;

        Apps = new ObservableCollection<Win32App?>(await _repository.GetAllAsync());

        IsLoading = false;
    }

    [RelayCommand]
    private void SortApps()
    {
        if (Apps.Count == 0) return;

        if (IsSorted)
        {
            Apps = new ObservableCollection<Win32App?>(Apps?.OrderBy(x => x.AppName));
        }
        else
        {
            Apps = new ObservableCollection<Win32App?>(Apps?.OrderBy(x => x.Id));
        }

        OnPropertyChanged(nameof(Apps));
    }

    [RelayCommand]
    private void SearchApp()
    {
        if (string.IsNullOrWhiteSpace(QueryText) || Apps.Count == 0)
            return;

        SelectedItem = Apps
            .FirstOrDefault(x => x.AppName.ToLower().StartsWith(QueryText.ToLower()));
    }

    [RelayCommand]
    private void CheckApp()
    {
        if (string.IsNullOrWhiteSpace(QueryText))
            return;

        SelectedItem = Apps
            .FirstOrDefault(x => x!.AppName!.ToLower().Equals(QueryText.ToLower()));
    }

    [RelayCommand]
    private async Task AddApp()
    {
        IsLoading = true;

        await _repository.AddAppAsync(QueryText);
        
        IsLoading = false;
    }
}