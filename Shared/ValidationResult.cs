namespace Shared;

public class ValidationResult
{
    public bool IsValid => !Errors.Any();
    public List<string> Errors { get; } = new();

    public string ToHtmlList()
    {
        return $"<ul>{string.Join(Environment.NewLine, Errors)}</ul>";
    }
}