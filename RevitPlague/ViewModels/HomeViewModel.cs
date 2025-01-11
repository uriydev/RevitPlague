using System.Windows.Input;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Input;
using Autodesk.Revit.UI;
using RevitPlague.Core.Services;

namespace RevitPlague.ViewModels;

public class HomeViewModel
{
    private readonly RevitApiTaskHandler _actionEventHandler;

    public HomeViewModel(RevitApiTaskHandler actionEventHandler)
    {
        _actionEventHandler = actionEventHandler;
        HomeVMCommand = new RelayCommand(DeleteInstances);
    }
    
    public ICommand HomeVMCommand { get; }

    private void DeleteInstances()
    {
        _actionEventHandler.Run(application =>
        {
            var selection = application.ActiveUIDocument.Selection;

            try
            {
                var document = application.ActiveUIDocument.Document;

                var pickedObjects = selection.PickObjects(ObjectType.Element);
                
                using var transaction = new Transaction(document, $"Delete elements");
                transaction.Start();
                
                foreach (var pickedObject in pickedObjects)
                {
                    var elementId = pickedObject.ElementId;
                    var element = document.GetElement(elementId);
                    document.Delete(elementId);
                }
                
                transaction.Commit();
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ошибка", ex.Message);
            }
        });
    }
}