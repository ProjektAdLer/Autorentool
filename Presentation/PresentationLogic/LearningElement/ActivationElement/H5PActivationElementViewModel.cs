using JetBrains.Annotations;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningSpace;
using Shared;

namespace Presentation.PresentationLogic.LearningElement.ActivationElement;

public class H5PActivationElementViewModel : ActivationElementViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private H5PActivationElementViewModel()
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="H5PActivationElementViewModel"/> class.
    /// </summary>
    internal H5PActivationElementViewModel(string name, string shortname, ILearningSpaceViewModel? parent,
        ILearningContentViewModel learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY) : base(
        name, shortname, parent, learningContent, authors, description, goals, difficulty, workload, points, positionX,
        positionY)
    {
    } 
} 