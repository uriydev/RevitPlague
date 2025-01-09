using Autodesk.Revit.DB;
using System.Linq;
using RevitPlague.Contracts;

namespace RevitPlague.Models;

public class GetElementProperties : IGetEntityProperties
{
    private IGetPropertyValue _getPropertyValueService;
    private GetElementByIdService _getElementByIdService;

    public GetElementProperties(
        IGetPropertyValue getPropertyValueService,
        GetElementByIdService getElementByIdService)
    {
        _getPropertyValueService = getPropertyValueService;
        _getElementByIdService = getElementByIdService;
    }

    public PropertyDTO[] GetProperties(EntityDTO entity)
    {
        var element = _getElementByIdService
            .GetElement(entity.Id);

        var res = element
            .Parameters
            .Cast<Parameter>()
            .Select(p =>
                new PropertyDTO(
                    p.Definition.Name,
                    GetValue(entity.Id, p),
                    entity.Id))
            .ToArray();

        return res;
    }

    private string? GetValue(string entityId, Parameter p) =>
        _getPropertyValueService
            .GetPropertyValue(
                entityId,
                p.Definition.Name,
                null);
}