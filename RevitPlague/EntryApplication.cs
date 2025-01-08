using Nice3point.Revit.Extensions;
using Nice3point.Revit.Toolkit.External;
using RevitPlague.Commands;

namespace RevitPlague;

// public class EntryApplication : IExternalApplication
// {
//     public Result OnStartup(UIControlledApplication application)
//     {
//         Host.Start();
//         
//         #region Ribbon Panel
//         
//         RibbonPanel ribbonPanel = application.CreateRibbonPanel("Revit Plague");
//         string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;
//         PushButtonData buttonData = new PushButtonData("cmdHelloWorld",
//             "RevitPlague", thisAssemblyPath, "RevitPlague.Commands.EntryCommand");
//         PushButton pushButton = ribbonPanel.AddItem(buttonData) as PushButton;
//         pushButton.ToolTip = "Say hello to the entire world.";
//         Uri uriImage = new Uri("pack://application:,,,/RevitPlague;component/Resources/icons/RibbonIcon32.png");
//         BitmapImage largeImage = new BitmapImage(uriImage);
//         pushButton.LargeImage = largeImage;
//         
//         #endregion
//         
//         return Result.Succeeded;
//     }
//     
//     public Result OnShutdown(UIControlledApplication application)
//     {
//         return Result.Succeeded;
//     }
// }
public class EntryApplication : ExternalApplication
{
    public override void OnStartup()
    {
        Host.Start();
        CreateRibbon();
    }

    private void CreateRibbon()
    {
        var panel = Application.CreatePanel("Commands", "RP");

        panel.AddPushButton<EntryCommand>("Execute")
            .SetImage("/RevitPlague;component/Resources/Icons/RibbonIcon16.png")
            .SetLargeImage("/RevitPlague;component/Resources/Icons/RibbonIcon32.png");
    }
}