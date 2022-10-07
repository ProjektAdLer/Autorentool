using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningSpace;
using Shared;

namespace Presentation.PresentationLogic.LearningElement.TestElement;

public abstract class TestElementViewModel : LearningElementViewModel
{
    /// <summary>
    /// Protected Constructor for AutoMapper
    /// </summary>
    protected TestElementViewModel()
    {
    }

    protected TestElementViewModel(string name, string shortname, ILearningSpaceViewModel? parent,
        LearningContentViewModel content, string? url, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY) : base(
        name, shortname, content, url, authors, description, goals, difficulty, parent, workload, points, positionX,
        positionY)
    {
    }
}