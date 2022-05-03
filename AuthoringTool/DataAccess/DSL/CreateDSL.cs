using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.DataAccess.DSL;

public class CreateDSL
{
    
    
    public LearningWorldViewModel? SelectedLearningWorld { get; set; }
    
    public LearningWorldViewModel GetLearningworldProperties(LearningWorldViewModel selectedLearningWorld)
    {
        SelectedLearningWorld = selectedLearningWorld;
        return SelectedLearningWorld;
    }
}