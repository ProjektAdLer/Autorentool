using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.LearningElement.ActivationElement;

public class VideoActivationElementViewModel : ActivationElementViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    private VideoActivationElementViewModel() : base()
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="VideoActivationElementViewModel"/> class.
    /// </summary>
    public VideoActivationElementViewModel(string name, string shortname, ILearningElementViewModelParent? parent,
        LearningContentViewModel learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, double positionX, double positionY) : base(name,
        shortname, parent, learningContent, authors, description, goals, difficulty, workload, positionX, positionY)
    {
    }
}