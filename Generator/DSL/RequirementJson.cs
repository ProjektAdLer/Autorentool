namespace AuthoringTool.DataAccess.DSL;

/// <summary>
/// Requirements describe all the needed Spaces, Topics or elements that need to be completed, before another space,
/// topic or element can be viewed or started
/// </summary>
public class RequirementJson : IRequirementJson
{
    public RequirementJson(string type, List<int> value)
    {
        Type = type;
        Value = value;
    }

    public string Type { get; set; }
    public List<int> Value { get; set; }
}