using System.Windows;
using System.Windows.Threading;
using Visibility = System.Windows.Visibility;

namespace RevitPlague.Services.Contracts;

public interface IWindow
{
    bool IsLoaded { get; }
    Visibility Visibility { get; set; }
    Dispatcher Dispatcher { get; }
    
    void EnableSizeTracking();
    void DisableSizeTracking();
    
    event RoutedEventHandler Loaded;
}