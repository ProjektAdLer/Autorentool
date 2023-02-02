using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Space;
using Shared;

namespace Presentation.PresentationLogic.Element;

public interface IElementViewModel : IDisplayableObject
{
    Guid Id { get; }
    new string Name { get; set; }
    string Description { get; set; }
    string Shortname { get; set; }
    ISpaceViewModel? Parent { get; set; }
    ContentViewModel Content { get; set; }
    string Url { get; set; }
    string Authors { get; set; }
    string Goals { get; set; }
    ElementDifficultyEnum Difficulty { get; set; }
    int Workload { get; set; }
    int Points { get; set; }
    double PositionX { get; set; }
    double PositionY { get; set; }
}