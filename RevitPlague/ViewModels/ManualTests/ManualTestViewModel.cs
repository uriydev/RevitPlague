using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RevitPlague.ViewModels.ManualTests;

/// <summary>
///  Using ObservableObject automatically takes care of INotifyPropertyChanged
/// </summary>
public partial class ManualTestViewModel : ObservableObject
{
    private string? input;

    public string? Input
    {
        get => input;
        set => SetProperty(ref input, value);
    }

    [RelayCommand]
    private void Submit()
    {
        // Показать введённое значение
        MessageBox.Show($"Введённое значение: {Input}");

        // Изменить значение и отобразить
        Input += "_изменено";
        MessageBox.Show($"Изменённое значение: {Input}");
    }
}