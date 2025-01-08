using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitPlague.Views;

namespace RevitPlague.Commands;

// [Transaction(TransactionMode.Manual)]
// public class EntryCommand : IExternalCommand
// {
//     public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
//     {
//         var view = new MainWindow();
//         view.Show();
//         
//         return Result.Succeeded;
//     }
// }
[Transaction(TransactionMode.Manual)]
public class EntryCommand : ExternalCommand
{
    public override void Execute()
    {
        var view = Host.GetService<MainWindow>();
        view.Show();

        // var view = new MainWindow();
        // view.Show();
    }
}