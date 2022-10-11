namespace Generator.DSL;

public interface ILearningSpaceJson
{
    int SpaceId { get; set; }

    // the identifier has the name of the element, this information is needed for the API calls from the 2D3D Team.
   IdentifierJson Identifier { get; set; }
   
   string? Description { get; set; }
    
    // A list that has all the id´s of the included elements of a space. 
   List<int> LearningSpaceContent { get; set; }
    
    // requirements are needed to describe the Path of the Topic, Space and element. 
    // it is a list of topics, spaces or elements that need to be completed, before a particular element can be startet
    List<int>? Requirements { get;}
}