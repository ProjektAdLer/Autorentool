using AuthoringTool.API.Configuration;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.PresentationLogic.API;

public interface IPresentationLogic
{
    IAuthoringToolConfiguration Configuration { get; }
    IBusinessLogic BusinessLogic { get;  }
    void ConstructBackup();
    void SaveLearningWorld(LearningWorldViewModel learningWorldViewModel);
    Task<LearningWorldViewModel> LoadLearningWorld();
}