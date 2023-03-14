using JetBrains.Annotations;

namespace Presentation.PresentationLogic.LearningContent;

public abstract class LearningContentViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    protected LearningContentViewModel()
    {
        Name = "";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningContentViewModel"/> class.
    /// </summary>
    /// <param name="name">The name of the learning content</param>
    protected LearningContentViewModel(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    
}