namespace RevitPlague.Models;

public class LibraryFamilyData
{
    public string FUID { get; set; }
    public string FamilyName { get; set; }
    public List<LibraryFamilyTypeData> Types { get; set; }
}