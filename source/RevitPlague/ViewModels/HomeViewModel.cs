using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Input;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using RevitPlague.Core.Services;

namespace RevitPlague.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    public HomeViewModel(RevitApiTaskHandler revitApiTaskHandler)
    {
        RevitApiTaskHandler = revitApiTaskHandler;
    }
    
    public RevitApiTaskHandler RevitApiTaskHandler { get; }

    [RelayCommand]
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