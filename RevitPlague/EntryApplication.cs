using Nice3point.Revit.Extensions;
using Nice3point.Revit.Toolkit.External;
using RevitPlague.Commands;

namespace RevitPlague;

public class EntryApplication : ExternalApplication
{
    public override void OnStartup()
    {
        Host.Start();
        CreateRibbon();
    }

    private void CreateRibbon()
    {
        var panel = Application.CreatePanel("Commands", "RP Tab");

        panel.AddPushButton<PlagueViewCommand>("Execute")
            .SetImage("/RevitPlague;component/Resources/Icons/RibbonIcon16.png")
            .SetLargeImage("/RevitPlague;component/Resources/Icons/RibbonIcon32.png");
    }
}