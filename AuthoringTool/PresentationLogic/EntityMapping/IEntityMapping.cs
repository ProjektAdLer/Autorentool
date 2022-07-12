using AuthoringTool.PresentationLogic.EntityMapping.LearningElementMapper;

namespace AuthoringTool.PresentationLogic.EntityMapping;

public interface IEntityMapping
{
    ILearningWorldMapper WorldMapper { get; }
    ILearningSpaceMapper SpaceMapper { get; }
    ILearningElementMapper ElementMapper { get; }
}