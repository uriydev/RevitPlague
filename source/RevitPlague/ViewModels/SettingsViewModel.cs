using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Input;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using RevitPlague.Core.Services;

namespace RevitPlague.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    public SettingsViewModel(RevitApiTaskHandler revitApiTaskHandler)
    {
        RevitApiTaskHandler = revitApiTaskHandler;
    }

    public RevitApiTaskHandler RevitApiTaskHandler { get; }

    [RelayCommand]
    private void PickAndZoomInstance()
    {
        RevitApiTaskHandler.Run(application =>
        {
            var activeUiDocument = application.ActiveUIDocument;
            if (activeUiDocument == null)
            {
                TaskDialog.Show("Ошибка", "Нет активного документа Revit.");
                return;
            }
            
            var selection = activeUiDocument.Selection;
            
            try
            {
                var pickedObject = selection.PickObject(ObjectType.Element);
                var elementId = pickedObject.ElementId;
                var document = activeUiDocument.Document;
                var element = document.GetElement(elementId);
                activeUiDocument.ShowElements(element);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ошибка", ex.Message);
            }
        });
    }
}