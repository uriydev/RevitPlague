namespace RevitPlague.Contracts;

public interface IGetEntities
{
    public EntityDTO[] GetEntities(string namePart);
}