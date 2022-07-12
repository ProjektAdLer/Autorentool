namespace AuthoringTool.DataAccess.DSL;

/// <summary>
/// The DocumentRoot only need the learningWorld
/// This class is needed, because the definition of the structure wants the LearningWorld as its own tag in the Json file. 
/// </summary>
public class DocumentRootJson : IDocumentRootJson
{
    public LearningWorldJson? learningWorld { get; set; }
    
}