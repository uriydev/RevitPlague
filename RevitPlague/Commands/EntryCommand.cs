using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPlague.Services;
using RevitPlague.ViewModels;
using RevitPlague.Views;
using Wpf.Ui;

namespace RevitPlague.Commands;

[Transaction(TransactionMode.Manual)]
public class EntryCommand : IExternalCommand
{
    public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        try
        {
            var mainWindow = Host.GetService<MainWindow>() as Window;
            
            if (mainWindow == null)
            {
                message = "Не удалось получить окно из контейнера зависимостей.";
                return Result.Failed;
            }

            // mainWindow.ShowDialog();
            mainWindow.Show();

            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            message = $"Произошла ошибка: {ex.Message}";
            return Result.Failed;
        }
    }
}