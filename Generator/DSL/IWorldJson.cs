namespace Generator.DSL;

public interface IWorldJson
{
    string IdNumber { get; set; }

    IdentifierJson Identifier { get; set; }
    
    string Description { get; set; }
    
    string Goals { get; set; }
    
    // A list that has all the id´s of the included Topics of a world. 
    List<int> WorldContent { get; set; }
    
    // for the correct structure the topics are added to the  World
    List<TopicJson> Topics { get; set; }
    
    // for the correct structure the Spaces are added to the  World
    List<SpaceJson> Spaces { get; set; }
    
    // for the correct structure the Elements are added to the  World
    List<ElementJson> Elements { get; set; }
}