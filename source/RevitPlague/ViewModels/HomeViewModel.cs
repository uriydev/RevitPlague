using System.Windows.Input;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Input;
using Autodesk.Revit.UI;
using RevitPlague.Core.Services;

namespace RevitPlague.ViewModels;

public class HomeViewModel
{
    public HomeViewModel(RevitApiTaskHandler revitApiTaskHandler)
    {
        RevitApiTaskHandler = revitApiTaskHandler;
        
        HomeVMCommand = new RelayCommand(DeleteInstances);
    }
    
    public RevitApiTaskHandler RevitApiTaskHandler { get; }
    public ICommand HomeVMCommand { get; }

    private void DeleteInstances()
    {
        RevitApiTaskHandler.Run(application =>
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