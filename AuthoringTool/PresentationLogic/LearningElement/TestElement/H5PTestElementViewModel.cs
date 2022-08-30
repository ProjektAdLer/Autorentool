using AuthoringTool.PresentationLogic.LearningContent;

namespace AuthoringTool.PresentationLogic.LearningElement.TestElement;

public class H5PTestElementViewModel : TestElementViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    private H5PTestElementViewModel() : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="H5PTestElementViewModel"/> class.
    /// </summary>
    public H5PTestElementViewModel(string name, string shortname, ILearningElementViewModelParent? parent,
        LearningContentViewModel learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, double positionX, double positionY) : base(name,
        shortname, parent, learningContent, authors, description, goals, difficulty, workload, positionX, positionY)
    {
    }
}