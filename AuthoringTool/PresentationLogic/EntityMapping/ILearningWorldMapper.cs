using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.EntityMapping;

public interface ILearningWorldMapper
{
    public Entities.LearningWorld ToEntity(LearningWorldViewModel viewModel);
    public LearningWorldViewModel ToViewModel(Entities.ILearningWorld entity);
}