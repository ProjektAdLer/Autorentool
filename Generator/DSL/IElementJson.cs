namespace Generator.DSL;

public interface IElementJson : IHasType
{
    int ElementId { get; set; }
    string ElementUUID { get; set; }
    string ElementName { get; set; }
    string? ElementDescription { get; set; }
    string[] ElementGoals { get; set; }

    string ElementCategory { get; set; }

    string ElementFileType { get; set; }
}