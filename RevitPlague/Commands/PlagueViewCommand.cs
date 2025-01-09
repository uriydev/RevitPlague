using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using RevitPlague.Contracts;
using RevitPlague.Models;
using RevitPlague.ViewModels;
using RevitPlague.Views;
using SimpleInjector;

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
        view.Show();

        return Result.Succeeded;
    }
}
// public class PlagueViewCommand : ExternalCommand
// {
//     public override void Execute()
//     {
//         var view = Host.GetService<RevitPlagueView>();
//         view.Show();
//     }
// }
// public class PlagueViewCommand : IExternalCommand
// {
//     public Result Execute(
//         ExternalCommandData commandData,
//         ref string message,
//         ElementSet elements)
//     {
//         var uiDocument = commandData
//             .Application
//             .ActiveUIDocument;
//
//         var app = commandData
//             .Application
//             .Application;
//
//         var container = ConfigureUI();
//
//         container.RegisterInstance(app);
//         container.RegisterInstance(uiDocument);
//
//         container.Register<IGetEntities, GetElementsService>();
//         container.Register<IPickEntities, PickElementsService>();
//         container.Register<IZoomEntity, ZoomElementService>();
//         container.Register<IWatchDocument, WatchDocumentService>();
//         container.Register<IDeleteEnitity, DeleteElementService>();
//         container.Register<ElementToDTOConverter>();
//
//         var window = container
//             .GetInstance<MainWindow>();
//
//         window.Show();
//
//         return Result.Succeeded;
//     }
//
//     public static Container ConfigureUI()
//     {
//         var container = new Container();
//         container.Register<GetEntitiesCommand>();
//         container.Register<PickEntitiesCommand>();
//         container.Register<ZoomEntityCommand>();
//         container.Register<MainViewModel>();
//         container.Register<MainWindow>();
//         return container;
//     }
// }