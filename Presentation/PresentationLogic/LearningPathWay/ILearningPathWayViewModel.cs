using Presentation.PresentationLogic.LearningSpace;

namespace Presentation.PresentationLogic.LearningPathway;

public interface ILearningPathWayViewModel : ISelectableObjectInWorldViewModel
{
    IObjectInPathWayViewModel SourceObject { get; set; }
    IObjectInPathWayViewModel TargetObject { get; set; }
}