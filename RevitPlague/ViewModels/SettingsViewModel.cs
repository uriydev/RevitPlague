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
    private ZoomElementService _zoomElementService;
    
    private EntityVM[] _entities;
    private string? _nameFilter;
    private EntityVM? _selectedEntity;
    
    public SettingsViewModel(
        RevitApiTaskHandler revitApiTaskHandler, 
        ElementToDTOConverter elementToDtoConverter)
    {
        RevitApiTaskHandler = revitApiTaskHandler;
        _elementToDtoConverter = elementToDtoConverter;
        
        PickAndZoomInstancesCommand = new RelayCommand(PickAndZoomInstances);
    }
    
    public RevitApiTaskHandler RevitApiTaskHandler { get; }
    public ICommand PickAndZoomInstancesCommand { get; }

    private void PickAndZoomInstances()
    {
        RevitApiTaskHandler.Run(application =>
        {
            _zoomElementService = new ZoomElementService(application.ActiveUIDocument);
            
            var selection = application.ActiveUIDocument.Selection;
            
            try
            {
                var pickedObject = selection.PickObject(ObjectType.Element);

                var elementId = pickedObject.ElementId;
                var document = application.ActiveUIDocument.Document;
                var element = document.GetElement(elementId);
                    
                EntityDTO res = _elementToDtoConverter.Convert(element);
                
                _zoomElementService.Zoom(res);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ошибка", ex.Message);
            }
        });
    }
}