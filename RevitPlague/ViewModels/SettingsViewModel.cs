using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.Input;
using Autodesk.Revit.UI;
using RevitPlague.Commands;
using RevitPlague.Contracts;
using RevitPlague.Core.Services;

namespace RevitPlague.ViewModels;

public class SettingsViewModel
{
    // private readonly ActionEventHandler _actionEventHandler;
    private readonly RevitApiTaskHandler _actionEventHandler;
    
    private EntityVM[] _entities;
    private string? _nameFilter;
    private EntityVM? _selectedEntity;

    public SettingsViewModel(RevitApiTaskHandler actionEventHandler)
    {
        _actionEventHandler = actionEventHandler;
        
        RunLongRevit = new RelayCommand(PlaceInstances);
    }
    
    public event PropertyChangedEventHandler PropertyChanged;

    public ICommand RunLongRevit { get; }

    private void PlaceInstances()
    {
        // _actionEventHandler.Raise(application =>
        _actionEventHandler.Run(application =>
        {
            var selection = application.ActiveUIDocument.Selection;

            try
            {
                var pickedObjects = selection.PickObjects(ObjectType.Element);

                foreach (var pickedObject in pickedObjects)
                {
                    var elementId = pickedObject.ElementId;
                    var document = application.ActiveUIDocument.Document;
                    var element = document.GetElement(elementId);
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ошибка", ex.Message);
            }
        });
    }
}