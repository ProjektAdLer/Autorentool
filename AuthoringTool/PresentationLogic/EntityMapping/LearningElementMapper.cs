using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.PresentationLogic.EntityMapping;

public class LearningElementMapper : ILearningElementMapper
{
    private readonly ILearningContentMapper _contentMapper;
    private ILogger<LearningElementMapper> Logger { get; }

    public LearningElementMapper(ILogger<LearningElementMapper> logger, ILearningContentMapper contentMapper)
    {
        Logger = logger;
        _contentMapper = contentMapper;
    }
    public Entities.LearningElement ToEntity(LearningElementViewModel viewModel)
    {
        return new Entities.LearningElement(viewModel.Name, viewModel.Shortname,
            viewModel.Parent?.Name, _contentMapper.ToEntity(viewModel.LearningContent),
            viewModel.Authors, viewModel.Description, viewModel.Goals, viewModel.Difficulty,
            viewModel.Workload, viewModel.PositionX, viewModel.PositionY);
    }

    public LearningElementViewModel ToViewModel(Entities.ILearningElement entity,
        ILearningElementViewModelParent? caller = null)
    {
        //sanity check
        if (caller != null && entity.ParentName != caller.Name)
        {
            Logger.LogError(
                $"caller was not null but caller.Name != entity.ParentName: {caller.Name}!={entity.ParentName}");
        }
        return new LearningElementViewModel(entity.Name, entity.Shortname, caller,
            _contentMapper.ToViewModel(entity.Content), entity.Authors, entity.Description, entity.Goals,
            entity.Difficulty, entity.Workload, entity.PositionX, entity.PositionY);
    }
}