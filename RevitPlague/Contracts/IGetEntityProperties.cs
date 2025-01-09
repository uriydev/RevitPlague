namespace RevitPlague.Contracts;

public interface IGetEntityProperties
{
    public PropertyDTO[] GetProperties(EntityDTO entity);
}