using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using RevitPlague.Commands;

namespace RevitPlague;

// public class EntryApplication : ExternalApplication
// {
//     public override void OnStartup()
//     {
//         Host.Start();
//         CreateRibbon();
//     }
//
//     private void CreateRibbon()
//     {
//         var panel = Application.CreatePanel("Commands", "RP Tab");
//
//         panel.AddPushButton<PlagueViewCommand>("Execute")
//             .SetImage("/RevitPlague;component/Resources/Icons/RibbonIcon16.png")
//             .SetLargeImage("/RevitPlague;component/Resources/Icons/RibbonIcon32.png");
//     }
// }
public class EntryApplication : IExternalApplication
{
    public Result OnStartup(UIControlledApplication application)
    {
        Host.Start();
        
        #region Ribbon Panel
        
        RibbonPanel ribbonPanel = application.CreateRibbonPanel("Revit Plague");
        string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
        PushButtonData buttonData = new PushButtonData("cmdHelloWorld",
            "RevitPlague", thisAssemblyPath, "RevitPlague.Commands.PlagueViewCommand");
        PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;
        pushButton.ToolTip = "Say hello to the entire world.";
        Uri uriImage = new Uri("pack://application:,,,/RevitPlague;component/Resources/icons/RibbonIcon32.png");
        BitmapImage largeImage = new BitmapImage(uriImage);
        pushButton.LargeImage = largeImage;
        
        #endregion
        
        return Result.Succeeded;
    }
    
    public Result OnShutdown(UIControlledApplication application)
    {
        return Result.Succeeded;
    }
}