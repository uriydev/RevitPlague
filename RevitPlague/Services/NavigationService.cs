using System.Windows.Controls;

namespace RevitPlague.Services;

public class NavigationService
{
    private readonly Frame _mainFrame;

    public NavigationService(Frame mainFrame)
    {
        _mainFrame = mainFrame;
    }

    public void Navigate(Page page)
    {
        _mainFrame.Navigate(page);
    }
}