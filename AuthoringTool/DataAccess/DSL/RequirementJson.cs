namespace AuthoringTool.DataAccess.DSL;

/// <summary>
/// Requirements describe all the needed Spaces, Topics or elements that need to be completed, before another space,
/// topic or element can be viewed or started
/// </summary>
public class RequirementJson : IRequirementJson
{
    public string? type { get; set; }
    public List<int>? value { get; set; }
}