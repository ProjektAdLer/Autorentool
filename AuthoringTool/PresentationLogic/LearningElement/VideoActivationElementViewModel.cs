using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.LearningElement;

public class VideoActivationElementViewModel : ActivationElementViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VideoActivationElementViewModel"/> class.
    /// </summary>
    public VideoActivationElementViewModel(string name, string shortname, ILearningElementViewModelParent? parent,
        LearningContentViewModel learningContent, string authors, string description, string goals, int workload,
        double positionX, double positionY) : base(name, shortname, parent, learningContent, authors, description,
        goals, workload, positionX, positionY)
    {
    }
}