using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPlague.Services;
using RevitPlague.Services.Contracts;
using RevitPlague.ViewModels;
using RevitPlague.Views;
using RevitPlague.Views.Pages;
using Wpf.Ui;

namespace RevitPlague.Commands;

[Transaction(TransactionMode.Manual)]

public class EntryCommand : IExternalCommand
{
    public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        // var mainWindow = Host.GetService<MainWindow>() as Window;
        // var mainWindow = Host.GetService<IWindow>() as Window;
        
        // mainWindow.ShowDialog();
        // mainWindow.Show();
        
        Host.GetService<IPlagueService>().Show<DashboardPage>();

        return Result.Succeeded;
    }
}