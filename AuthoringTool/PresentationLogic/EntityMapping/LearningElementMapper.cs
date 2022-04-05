using AuthoringTool.PresentationLogic.LearningElement;

namespace AuthoringTool.PresentationLogic.EntityMapping;

public class LearningElementMapper : ILearningElementMapper
{
    public Entities.LearningElement ToEntity(LearningElementViewModel viewModel)
    {
        return new Entities.LearningElement(viewModel.Name, viewModel.Shortname, viewModel.Parent, viewModel.Assignment,
            viewModel.Type, viewModel.Content, viewModel.Authors, viewModel.Description, viewModel.Goals,
            viewModel.PositionX, viewModel.PositionY);
    }

    public LearningElementViewModel ToViewModel(Entities.ILearningElement entity)
    {
        return new LearningElementViewModel(entity.Name, entity.Shortname, entity.Parent, entity.Assignment,
            entity.Type, entity.Content, entity.Authors, entity.Description, entity.Goals,
            entity.PositionX, entity.PositionY);
    }
}