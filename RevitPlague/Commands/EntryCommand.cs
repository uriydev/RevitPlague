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
        // var view = Host.GetService<RevitPlagueView>();
        var view = Host.GetService<IWindow>();
        view.Show();
        
        return Result.Succeeded;
    }
    
    public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
    {
        return true;
    }
}