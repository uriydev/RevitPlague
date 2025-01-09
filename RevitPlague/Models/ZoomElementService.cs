using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPlague.Contracts;

namespace RevitPlague.Models;

public class ZoomElementService : IZoomEntity
{
    private UIDocument _uiDocument;

    public ZoomElementService(UIDocument uiDocument)
    {
        _uiDocument = uiDocument;
    }

    public void Zoom(EntityDTO element)
    {
        var ids = new ElementId[]
        {
            new ElementId(int.Parse(element.Id))
        };

        _uiDocument?.ShowElements(ids);
    }
}