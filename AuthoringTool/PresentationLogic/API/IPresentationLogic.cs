using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.API;

public interface IPresentationLogic
{
    IAuthoringToolConfiguration Configuration { get; }
    IBusinessLogic BusinessLogic { get;  }
    void ConstructBackup();
    Task SaveLearningWorldAsync(LearningWorldViewModel learningWorldViewModel);
    Task<LearningWorldViewModel> LoadLearningWorld();
    void SaveLearningSpace(LearningSpaceViewModel learningSpaceViewModel);
    Task<LearningSpaceViewModel> LoadLearningSpace();
    void SaveLearningElement(LearningElementViewModel learningElementViewModel);
    Task<LearningElementViewModel> LoadLearningElement();
}