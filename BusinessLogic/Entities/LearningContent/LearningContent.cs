using JetBrains.Annotations;

namespace BusinessLogic.Entities.LearningContent;

public abstract class LearningContent : ILearningContent
{
    protected LearningContent(string name)
    {
        Name = name;
    }
    
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    protected LearningContent()
    {
        Name = "";
    }
    
    public string Name { get; set; }
}