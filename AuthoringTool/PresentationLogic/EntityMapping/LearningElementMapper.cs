using System.Runtime.Serialization;
using AuthoringTool.PresentationLogic.LearningContent;
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
            viewModel.ElementType, viewModel.Parent?.Name, viewModel.ContentType, viewModel.Authors, viewModel.Description, viewModel.Goals,
            viewModel.PositionX, viewModel.PositionY);
    }

    public LearningElementViewModel ToViewModel(Entities.ILearningElement entity,
        ILearningElementViewModelParent? caller = null, LearningContentViewModel? learningContent = null)
    {
        //sanity check
        if (caller != null && entity.ParentName != caller.Name)
        {
            Logger.LogError(
                $"caller was not null but caller.Name != entity.ParentName: {caller.Name}!={entity.ParentName}");
        }
        return new LearningElementViewModel(entity.Name, entity.Shortname, caller,
            entity.ElementType, entity.ContentType, learningContent, entity.Authors, entity.Description, entity.Goals,
            entity.PositionX, entity.PositionY);
    }
}