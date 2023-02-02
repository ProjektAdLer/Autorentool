
namespace Generator.DSL;

/// <summary>
///  The Identifier tag has the name and type (needed for the Moodle API, so that the 2D3D Team knows what kind of Type they need to search)
///  of the world, Topic, Space and element. 
/// </summary>
public class IdentifierJson : IIdentifierJson
{
    public IdentifierJson(string type, string value)
    {
        Type = type;
        Value = value;
    }

    public string Type { get; set; }
    public string Value { get; set; }
}