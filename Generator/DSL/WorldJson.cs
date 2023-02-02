
namespace Generator.DSL;

public class WorldJson : IWorldJson
{
    // the identifier has the name of the element, this information is needed for the API calls from the 2D3D Team.
    public WorldJson(string idNumber, IdentifierJson identifier, List<int> worldContent, List<TopicJson> topics, 
        List<SpaceJson> spaces, List<ElementJson> elements, string? description = null, 
        string? goals = null)
    {
        IdNumber = idNumber;
        Identifier = identifier;
        Description = description ?? "";
        Goals = goals ?? "";
        WorldContent = worldContent;
        Topics = topics;
        Spaces = spaces;
        Elements = elements;
    }
    
    public string IdNumber { get; set; }
    
    public IdentifierJson Identifier { get; set; }
    
    public string Description { get; set; }
    
    public string Goals { get; set; }
    
    // A list that has all the id´s of the included Topics of a world. 
    public List<int> WorldContent { get; set; }
    
    // for the correct structure the topics are added to the  World
    public List<TopicJson> Topics { get; set; }
    
    // for the correct structure the Spaces are added to the  World
    public List<SpaceJson> Spaces { get; set; }
    
    // for the correct structure the Elements are added to the  World
    public List<ElementJson> Elements { get; set; }
    

}