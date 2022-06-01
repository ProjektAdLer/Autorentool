namespace AuthoringTool.DataAccess.DSL;

/// <summary>
///  The Identifier tag has the name and type (needed for the Moodle API, so that the 2D3D Team knows what kind of Type they need to search)
///  of the learningWorld, Topic, Space and element. 
/// </summary>
public class IdentifierJson : IIdentifierJson
{
    public string? type { get; set; }
    public string? value { get; set; }
}