using AuthoringToolLib.PresentationLogic.LearningElement;
using AuthoringToolLib.PresentationLogic.LearningElement.InteractionElement;

namespace AuthoringToolLib.PresentationLogic.EntityMapping.LearningElementMapper;

public class H5PInteractionElementMapper : IH5PInteractionElementMapper
{
    private readonly ILearningContentMapper _contentMapper;

    public H5PInteractionElementMapper(ILearningContentMapper contentMapper)
    {
        _contentMapper = contentMapper;
    }
    public Entities.LearningElement ToEntity(ILearningElementViewModel viewModel)
    {
        return new Entities.H5PInteractionElement(viewModel.Name, viewModel.Shortname,
            viewModel.Parent?.Name, _contentMapper.ToEntity(viewModel.LearningContent),
            viewModel.Authors, viewModel.Description, viewModel.Goals, viewModel.Difficulty,
            viewModel.Workload, viewModel.PositionX, viewModel.PositionY);
    }

    public ILearningElementViewModel ToViewModel(Entities.LearningElement entity,
        ILearningElementViewModelParent? caller = null)
    {
        return new H5PInteractionElementViewModel(entity.Name, entity.Shortname, caller,
            _contentMapper.ToViewModel(entity.Content), entity.Authors, entity.Description, entity.Goals,
            entity.Difficulty, entity.Workload, entity.PositionX, entity.PositionY);
    }
}