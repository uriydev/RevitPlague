using System.Windows.Input;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Input;
using Autodesk.Revit.UI;
using RevitPlague.Core.Services;

namespace RevitPlague.ViewModels;

public class SettingsViewModel
{
    public SettingsViewModel(
        RevitApiTaskHandler revitApiTaskHandler)
    {
        RevitApiTaskHandler = revitApiTaskHandler;

        PickAndZoomInstancesCommand = new RelayCommand(PickAndZoomInstance);
    }

    public RevitApiTaskHandler RevitApiTaskHandler { get; }
    public ICommand PickAndZoomInstancesCommand { get; }

    private void PickAndZoomInstance()
    {
        RevitApiTaskHandler.Run(application =>
        {
            var activeUIDocument = application.ActiveUIDocument;
            if (activeUIDocument == null)
            {
                TaskDialog.Show("Ошибка", "Нет активного документа Revit.");
                return;
            }
            
            var selection = activeUIDocument.Selection;
            
            try
            {
                var pickedObject = selection.PickObject(ObjectType.Element);
                var elementId = pickedObject.ElementId;

                var document = activeUIDocument.Document;
                var element = document.GetElement(elementId);
                
                activeUIDocument.ShowElements(element);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ошибка", ex.Message);
            }
        });
    }
}