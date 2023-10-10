using System.Text.Json.Serialization;

namespace Generator.DSL;

/// <summary>
/// This class represents DSL Root Informations. 
/// </summary>
public class DocumentRootJson : IDocumentRootJson
{
    [JsonConstructor]
    public DocumentRootJson(string fileVersion, string amgVersion, string author, string language,
        ILearningWorldJson world)
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
    public ILearningWorldJson World { get; set; }
}