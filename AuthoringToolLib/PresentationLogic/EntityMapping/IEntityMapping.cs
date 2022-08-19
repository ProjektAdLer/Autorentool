using AuthoringToolLib.PresentationLogic.EntityMapping.LearningElementMapper;

namespace AuthoringToolLib.PresentationLogic.EntityMapping;

public interface IEntityMapping
{
    ILearningWorldMapper WorldMapper { get; }
    ILearningSpaceMapper SpaceMapper { get; }
    ILearningElementMapper ElementMapper { get; }
}