using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPlague.Views;

namespace RevitPlague.Commands;

[Transaction(TransactionMode.Manual)]
public class PlagueViewCommand : IExternalCommand
{
    public Result Execute(
        ExternalCommandData commandData,
        ref string message,
        ElementSet elements)
    {
        var view = Host.GetService<RevitPlagueView>();
        view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        view.Show(commandData.Application.MainWindowHandle);

        return Result.Succeeded;
    }
}