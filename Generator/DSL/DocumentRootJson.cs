namespace Generator.DSL;

/// <summary>
/// This class represents DSL Root Informations. 
/// </summary>
public class DocumentRootJson : IDocumentRootJson
{
    public DocumentRootJson(string fileVersion, string amgVersion, string author, string language, LearningWorldJson world)
    {
        FileVersion = fileVersion;
        AmgVersion = amgVersion;
        Author = author;
        Language = language;
        World = world;
    }

    public string FileVersion { get; set; }
    
    public string AmgVersion { get; set; }
    
    public string Author { get; set; }
    
    public string Language { get; set; }
    public LearningWorldJson World { get; set; }
    
}