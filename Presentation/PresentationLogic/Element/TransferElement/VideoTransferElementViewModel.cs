using JetBrains.Annotations;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Space;
using Shared;

namespace Presentation.PresentationLogic.Element.TransferElement;

public class VideoTransferElementViewModel : TransferElementViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private VideoTransferElementViewModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VideoTransferElementViewModel"/> class.
    /// </summary>
    internal VideoTransferElementViewModel(string name, string shortname, ISpaceViewModel? parent,
        ContentViewModel content, string url, string authors, string description, string goals,
        ElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY) : base(
        name, shortname, parent, content, url, authors, description, goals, difficulty, workload, points, positionX,
        positionY)
    {
    }
}