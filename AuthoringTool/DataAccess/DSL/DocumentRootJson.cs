namespace AuthoringTool.DataAccess.DSL;

/// <summary>
/// The DocumentRoot only needs the learningWorld
/// This class is needed, because the definition of the Dsl-Structure has the LearningWorld as its own tag. 
/// </summary>
public class DocumentRootJson : IDocumentRootJson
{
    public DocumentRootJson(LearningWorldJson learningWorld)
    {
        LearningWorld = learningWorld;
    }

    public LearningWorldJson LearningWorld { get; set; }
    
}