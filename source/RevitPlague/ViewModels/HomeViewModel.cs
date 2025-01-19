using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Input;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using RevitPlague.Core.Services;

namespace RevitPlague.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    public HomeViewModel(RevitApiTaskExecutor revitApiTaskExecutor)
    {
        RevitApiTaskExecutor = revitApiTaskExecutor;
    }
    
    public RevitApiTaskExecutor RevitApiTaskExecutor { get; }

    
    [RelayCommand]
    private void DeleteInstances()
    {
        RevitApiTaskExecutor.ExecuteAsync(app =>
        {
            var selection = app.ActiveUIDocument.Selection;

            try
            {
                var document = app.ActiveUIDocument.Document;
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