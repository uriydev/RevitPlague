using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using RevitPlague.Models;
using Wpf.Ui.Controls;

namespace RevitPlague.ViewModels;

public partial class DataViewModel : ObservableObject, INavigationAware
{
    private bool _isInitialized = false;

    [ObservableProperty]
    private List<DataColor> _colors = [];

    public void OnNavigatedTo()
    {
        if (!_isInitialized)
        {
            InitializeViewModel();
        }
    }

    public void OnNavigatedFrom() { }

    private void InitializeViewModel()
    {
        var random = new Random();
        Colors.Clear();

        for (int i = 0; i < 8192; i++)
        {
            Colors.Add(
                new DataColor
                {
                    Color = new SolidColorBrush(
                        Color.FromArgb(
                            (byte)200,
                            (byte)random.Next(0, 250),
                            (byte)random.Next(0, 250),
                            (byte)random.Next(0, 250)
                        )
                    )
                }
            );
        }

        _isInitialized = true;
    }
}
