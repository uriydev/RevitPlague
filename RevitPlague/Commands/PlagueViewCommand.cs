using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitPlague.Views;

namespace RevitPlague.Commands;

[Transaction(TransactionMode.Manual)]
public class PlagueViewCommand : ExternalCommand
{
    public override void Execute()
    {
        var view = Host.GetService<RevitPlagueView>();
        view.Show();
    }
}