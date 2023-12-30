using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FeelingFresh.Library;

namespace FeelingFresh.UI.WPF.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IDataAccess _data;
    
    public MainViewModel(IDataAccess data)
    {
        _data = data;
        
        
    }

    [RelayCommand]
    private async Task GetData()
    {
        
    }
}