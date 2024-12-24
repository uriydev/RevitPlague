using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPlague.Services.Contracts;
using RevitPlague.Views;
using RevitPlague.Views.Pages;

namespace RevitPlague.Commands;

[Transaction(TransactionMode.Manual)]

public class EntryCommand : IExternalCommand
{
    public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        Host.GetService<IPlagueService>().Show<DashboardPage>();
        // var view = Host.GetService<MainWindow>();
        // view.Show();

        return Result.Succeeded;
    }
}