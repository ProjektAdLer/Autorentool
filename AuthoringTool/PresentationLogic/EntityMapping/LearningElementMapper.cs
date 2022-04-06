using System.Runtime.Serialization;
using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.PresentationLogic.EntityMapping;

public class LearningElementMapper : ILearningElementMapper
{
    private ILogger<LearningElementMapper> Logger { get; }

    public LearningElementMapper(ILogger<LearningElementMapper> logger)
    {
        Logger = logger;
    }
    public Entities.LearningElement ToEntity(LearningElementViewModel viewModel)
    {
        return new Entities.LearningElement(viewModel.Name, viewModel.Shortname,
            viewModel.Type, viewModel.Parent?.Name, viewModel.Content, viewModel.Authors, viewModel.Description, viewModel.Goals,
            viewModel.PositionX, viewModel.PositionY);
    }

    public LearningElementViewModel ToViewModel(Entities.ILearningElement entity, ILearningElementViewModelParent? caller = null)
    {
        //sanity check
        if (caller != null && entity.ParentName != caller.Name)
        {
            Logger.LogError(
                $"caller was not null but caller.Name != entity.ParentName: {caller.Name}!={entity.ParentName}");
        }
        return new LearningElementViewModel(entity.Name, entity.Shortname, caller,
            entity.Type, entity.Content, entity.Authors, entity.Description, entity.Goals,
            entity.PositionX, entity.PositionY);
    }
}