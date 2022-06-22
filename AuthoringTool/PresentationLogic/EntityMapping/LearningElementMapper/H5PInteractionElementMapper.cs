using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningElement.InteractionElement;

namespace AuthoringTool.PresentationLogic.EntityMapping.LearningElementMapper;

public class H5PInteractionElementMapper : IH5PInteractionElementMapper
{
    private readonly ILearningContentMapper _contentMapper;
    private ILogger<H5PInteractionElementMapper> Logger { get; }

    public H5PInteractionElementMapper(ILogger<H5PInteractionElementMapper> logger, ILearningContentMapper contentMapper)
    {
        Logger = logger;
        _contentMapper = contentMapper;
    }
    public Entities.LearningElement ToEntity(LearningElementViewModel viewModel)
    {
        return new Entities.H5PInteractionElement(viewModel.Name, viewModel.Shortname,
            viewModel.Parent?.Name, _contentMapper.ToEntity(viewModel.LearningContent),
            viewModel.Authors, viewModel.Description, viewModel.Goals, viewModel.Difficulty,
            viewModel.Workload, viewModel.PositionX, viewModel.PositionY);
    }

    public LearningElementViewModel ToViewModel(Entities.LearningElement entity,
        ILearningElementViewModelParent? caller = null)
    {
        return new H5PInteractionElementViewModel(entity.Name, entity.Shortname, caller,
            _contentMapper.ToViewModel(entity.Content), entity.Authors, entity.Description, entity.Goals,
            entity.Difficulty, entity.Workload, entity.PositionX, entity.PositionY);
    }
}