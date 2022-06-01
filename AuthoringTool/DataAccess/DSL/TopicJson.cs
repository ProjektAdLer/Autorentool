namespace AuthoringTool.DataAccess.DSL;

/// <summary>
/// Topics are Sections in moodle. They include spaces and/or elements.
/// </summary>
public class TopicJson : ITopicJson
{
    // incremented id for all topics
    public int? topicId { get; set; }
    
    // the name of the topic
    public string? name { get; set; }
    
    // the identifier has the name of the element, this information is needed for the API calls from the 2D3D Team.
    public IdentifierJson? identifier {get; set; }
    
    // Which spaces are in a topic
    public List<int>? topicContent { get; set; }
    
    // requirements are needed to describe the Path of the Topic, Space and element. 
    // it is a list of topics, spaces or elements that need to be completed, before a particular element can be startet
    public List<RequirementJson>? requirements { get; set; }
}