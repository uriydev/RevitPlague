using System.Windows;
using System.Windows.Threading;

namespace RevitPlague.Views;

public interface IWindow
{
    bool IsLoaded { get; }
    Visibility Visibility { get; set; }
    Dispatcher Dispatcher { get; }
    
    void EnableSizeTracking();
    void DisableSizeTracking();
    
    event RoutedEventHandler Loaded;
}