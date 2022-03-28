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
            viewModel.Goals, viewModel.LearningElements.Select(element => _elementMapper.ToEntity(element)).ToList());
    }

    public LearningSpaceViewModel ToViewModel(Entities.ILearningSpace entity)
    {
        return new LearningSpaceViewModel(entity.Name, entity.Shortname, entity.Authors, entity.Description,
            entity.Goals, entity.LearningElements.Select(element => _elementMapper.ToViewModel(element)).ToList());
    }
}