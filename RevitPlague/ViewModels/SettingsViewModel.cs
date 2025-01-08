using System.Windows.Input;
using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External.Handlers;

namespace RevitPlague.ViewModels;

// public class SettingsViewModel
// {
//     private RevitTask _revitTask = new RevitTask();
//     
//     public SettingsViewModel()
//     {
//         RunLongRevit = new UiCommand(PlaceInstances);
//     }
//     
//     public ICommand RunLongRevit { get; set; }
//     
//     private async void PlaceInstances()
//     {
//         try
//         {
//             await _revitTask.Run<int>(app => app.ActiveUIDocument
//                 .Selection
//                 .PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element)
//                 .Count);
//         }
//         catch(Exception e)
//         {
//             TaskDialog.Show("Debug", e.Message);
//         }
//     }
// }
public class SettingsViewModel
{
    // Используем AsyncEventHandler для выполнения асинхронной операции с возвратом значения
    private AsyncEventHandler<int> _asyncEventHandler;

    public SettingsViewModel()
    {
        // Инициализируем обработчик асинхронного события
        _asyncEventHandler = new AsyncEventHandler<int>();

        RunLongRevit = new UiCommand(PlaceInstances);
    }

    public ICommand RunLongRevit { get; set; }

    private async void PlaceInstances()
    {
        try
        {
            // Запуск асинхронного внешнего события
            var selectedCount = await _asyncEventHandler.RaiseAsync(application =>
            {
                var selection = application.ActiveUIDocument.Selection;

                // Запрос выбора объектов в Revit
                var pickedObjects = selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element);

                // Возвращаем количество выбранных объектов
                return pickedObjects.Count;
            });

            // Показываем результат пользователю
            TaskDialog.Show("Selection Result", $"Selected objects count: {selectedCount}");
        }
        catch (Exception e)
        {
            // Обработка ошибок
            TaskDialog.Show("Error", e.Message);
        }
    }
}
