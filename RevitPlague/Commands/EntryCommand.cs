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
    ApplicationHostService appHostService;
    
    public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        try
        {
            // var mainViewModel = new MainWindowViewModel();
            // var win = new MainWindow(mainViewModel);
            // win.ShowDialog();
            var mainWindow = Host.GetService<MainWindow>() as Window;
            mainWindow.Show();
            // Получаем окно из контейнера зависимостей
            // var view = Host.GetService<ILookupService>();
            
            // if (view == null)
            // {
            //     message = "Не удалось получить окно из контейнера зависимостей.";
            //     return Result.Failed;
            // }

            // Показываем окно
            // view.ShowDialog();

            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            message = $"Произошла ошибка: {ex.Message}";
            return Result.Failed;
        }
    }
}