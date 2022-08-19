using AuthoringToolLib.PresentationLogic.LearningContent;

namespace AuthoringToolLib.PresentationLogic.LearningElement.ActivationElement;

public class VideoActivationElementViewModel : ActivationElementViewModel
{
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