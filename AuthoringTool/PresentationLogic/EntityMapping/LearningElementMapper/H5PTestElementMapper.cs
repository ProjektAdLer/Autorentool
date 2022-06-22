using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningElement.TestElement;

namespace AuthoringTool.PresentationLogic.EntityMapping.LearningElementMapper;

public class H5PTestElementMapper : IH5PTestElementMapper
{
    private readonly ILearningContentMapper _contentMapper;
    private ILogger<H5PTestElementMapper> Logger { get; }

    public H5PTestElementMapper(ILogger<H5PTestElementMapper> logger, ILearningContentMapper contentMapper)
    {
        Logger = logger;
        _contentMapper = contentMapper;
    }
    public Entities.LearningElement ToEntity(LearningElementViewModel viewModel)
    {
        return new Entities.H5PTestElement(viewModel.Name, viewModel.Shortname,
            viewModel.Parent?.Name, _contentMapper.ToEntity(viewModel.LearningContent),
            viewModel.Authors, viewModel.Description, viewModel.Goals, viewModel.Difficulty,
            viewModel.Workload, viewModel.PositionX, viewModel.PositionY);
    }

    public LearningElementViewModel ToViewModel(Entities.LearningElement entity,
        ILearningElementViewModelParent? caller = null)
    {
        return new H5PTestElementViewModel(entity.Name, entity.Shortname, caller,
            _contentMapper.ToViewModel(entity.Content), entity.Authors, entity.Description, entity.Goals,
            entity.Difficulty, entity.Workload, entity.PositionX, entity.PositionY);
    }
}