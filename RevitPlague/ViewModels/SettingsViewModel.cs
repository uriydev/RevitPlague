using System.Windows.Input;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Input;
using Autodesk.Revit.UI;
using RevitPlague.Contracts;
using RevitPlague.Core.Services;
using RevitPlague.Models;

namespace RevitPlague.ViewModels;

public class SettingsViewModel
{
    private readonly ElementToDTOConverter _elementToDtoConverter;
    private readonly ZoomElementServiceFactory _zoomElementServiceFactory;

    public SettingsViewModel(
        RevitApiTaskHandler revitApiTaskHandler,
        ElementToDTOConverter elementToDtoConverter,
        ZoomElementServiceFactory zoomElementServiceFactory)
    {
        RevitApiTaskHandler = revitApiTaskHandler;
        _elementToDtoConverter = elementToDtoConverter;
        _zoomElementServiceFactory = zoomElementServiceFactory;

        PickAndZoomInstancesCommand = new RelayCommand(PickAndZoomInstances);
    }

    public RevitApiTaskHandler RevitApiTaskHandler { get; }
    public ICommand PickAndZoomInstancesCommand { get; }

    private void PickAndZoomInstances()
    {
        RevitApiTaskHandler.Run(application =>
        {
            var activeUIDocument = application.ActiveUIDocument;
            if (activeUIDocument == null)
            {
                TaskDialog.Show("Ошибка", "Нет активного документа Revit.");
                return;
            }

            // Создаем экземпляр ZoomElementService через фабрику
            var zoomElementService = _zoomElementServiceFactory.Create(activeUIDocument);

            var selection = activeUIDocument.Selection;
            try
            {
                var pickedObject = selection.PickObject(ObjectType.Element);
                var elementId = pickedObject.ElementId;

                var document = activeUIDocument.Document;
                var element = document.GetElement(elementId);

                EntityDTO entity = _elementToDtoConverter.Convert(element);
                zoomElementService.Zoom(entity);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ошибка", ex.Message);
            }
        });
    }
}