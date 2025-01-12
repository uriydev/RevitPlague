using Autodesk.Revit.UI;

namespace RevitPlague.Models;

public class ZoomElementServiceFactory
{
    public ZoomElementService Create(UIDocument uiDocument)
    {
        return new ZoomElementService(uiDocument);
    }
}