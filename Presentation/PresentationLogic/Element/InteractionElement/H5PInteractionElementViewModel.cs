using JetBrains.Annotations;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Space;
using Shared;

namespace Presentation.PresentationLogic.Element.InteractionElement;

public class H5PInteractionElementViewModel : InteractionElementViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private H5PInteractionElementViewModel()
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="H5PInteractionElementViewModel"/> class.
    /// </summary>
    internal H5PInteractionElementViewModel(string name, string shortname, ISpaceViewModel? parent,
        ContentViewModel content, string url, string authors, string description, string goals,
        ElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY) : base(
        name, shortname, parent, content, url, authors, description, goals, difficulty, workload, points, positionX,
        positionY)
    {
    }
}