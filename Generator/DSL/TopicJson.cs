namespace Generator.DSL;

/// <summary>
/// Topics are Sections in moodle. They include spaces and/or Elements.
/// </summary>
public class TopicJson : ITopicJson
{
    // incremented id for all topics
    public int? TopicId { get; set; }
    
    // the name of the topic
    public string? Name { get; set; }
    
    // the identifier has the name of the element, this information is needed for the API calls from the 2D3D Team.
    public IdentifierJson? Identifier {get; set; }
    
    // Which spaces are in a topic
    public List<int>? TopicContent { get; set; }

}