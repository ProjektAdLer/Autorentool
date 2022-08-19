using AuthoringToolLib.PresentationLogic.EntityMapping.LearningElementMapper;

namespace AuthoringToolLib.PresentationLogic.EntityMapping;

public class EntityMapping : IEntityMapping
{
    public EntityMapping(ILearningWorldMapper worldMapper, ILearningSpaceMapper spaceMapper,
        ILearningElementMapper elementMapper)
    {
        WorldMapper = worldMapper;
        SpaceMapper = spaceMapper;
        ElementMapper = elementMapper;
    }
    public ILearningWorldMapper WorldMapper { get; }
    public ILearningSpaceMapper SpaceMapper { get; }
    public ILearningElementMapper ElementMapper { get; }
}