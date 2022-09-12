using JetBrains.Annotations;
using Presentation.PresentationLogic.LearningContent;
using Shared;

namespace Presentation.PresentationLogic.LearningElement.TestElement;

public class H5PTestElementViewModel : TestElementViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private H5PTestElementViewModel()
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