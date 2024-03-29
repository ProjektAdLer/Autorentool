namespace BusinessLogic.Entities.LearningContent;

public static class AllowedFileEndings
{
    public static IEnumerable<string> Endings => new[]
    {
        ".txt", ".c", ".h", ".cpp", ".cc", ".c++", ".py", ".cs", ".js", ".php", ".html", ".css",
        ".jpg", ".jpeg", ".png", ".webp", ".bmp",
        ".h5p",
        ".pdf"
    };
    
    public static string EndingsCommaSeparated => string.Join(",", Endings);
    public static string EndingsCommaSeparatedNicely => string.Join(", ", Endings);
}