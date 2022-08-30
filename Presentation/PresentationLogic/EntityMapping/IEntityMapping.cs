using Presentation.PresentationLogic.EntityMapping.LearningElementMapper;

namespace Presentation.PresentationLogic.EntityMapping;

public interface IEntityMapping
{
    ILearningWorldMapper WorldMapper { get; }
    ILearningSpaceMapper SpaceMapper { get; }
    ILearningElementMapper ElementMapper { get; }
}