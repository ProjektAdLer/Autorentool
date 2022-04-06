using AuthoringTool.PresentationLogic.LearningSpace;

namespace AuthoringTool.PresentationLogic.EntityMapping;

public class LearningSpaceMapper : ILearningSpaceMapper
{
    private readonly ILearningElementMapper _elementMapper;

    public LearningSpaceMapper(ILearningElementMapper elementMapper)
    {
        _elementMapper = elementMapper;
    }

    public Entities.LearningSpace ToEntity(LearningSpaceViewModel viewModel)
    {
        return new Entities.LearningSpace(viewModel.Name, viewModel.Shortname, viewModel.Authors, viewModel.Description,
            viewModel.Goals, viewModel.LearningElements.Select(element => _elementMapper.ToEntity(element)).ToList(),
            viewModel.PositionX, viewModel.PositionY);
    }

    public LearningSpaceViewModel ToViewModel(Entities.ILearningSpace entity)
    {
        var retval = new LearningSpaceViewModel(entity.Name, entity.Shortname, entity.Authors, entity.Description,
            entity.Goals, null, entity.PositionX, entity.PositionY);
        retval.LearningElements =
            entity.LearningElements.Select(element => _elementMapper.ToViewModel(element, retval)).ToList();
        return retval;
    }
}