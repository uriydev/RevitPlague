using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPlague.Services.Contracts;
using RevitPlague.Views;
using RevitPlague.Views.Pages;

namespace RevitPlague.Commands;

[Transaction(TransactionMode.Manual)]

public class EntryCommand : IExternalCommand, IExternalCommandAvailability
{
    public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        // Host.GetService<IPlagueService>().Show<DashboardPage>(); //(1)
        // var view = Host.GetService<MainWindow>(); //(2)
        // view.Show(); //(2)
        
        UIApplication uiapp = commandData.Application;
        new MainView().Show();

        return Result.Succeeded;
    }
    
    public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
    {
        return true;
    }
}