using System.Windows;
using System.Windows.Threading;

namespace RevitPlague.Services.Contracts;

public interface IWindow
{
    bool IsLoaded { get; }
    Visibility Visibility { get; set; }
    Dispatcher Dispatcher { get; }
    event RoutedEventHandler Loaded;

    // ON TESTING
    void Close();
    void Show();
    bool? ShowDialog();
}