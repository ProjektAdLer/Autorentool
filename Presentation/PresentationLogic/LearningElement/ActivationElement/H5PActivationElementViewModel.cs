using Presentation.PresentationLogic.LearningContent;
using Shared;

namespace Presentation.PresentationLogic.LearningElement.ActivationElement;

public class H5PActivationElementViewModel : ActivationElementViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    private H5PActivationElementViewModel() : base()
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="H5PActivationElementViewModel"/> class.
    /// </summary>
    public H5PActivationElementViewModel(string name, string shortname, ILearningElementViewModelParent? parent,
        LearningContentViewModel learningContent, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, double positionX, double positionY) : base(name,
        shortname, parent, learningContent, authors, description, goals, difficulty, workload, positionX, positionY)
    {
    } 
} 