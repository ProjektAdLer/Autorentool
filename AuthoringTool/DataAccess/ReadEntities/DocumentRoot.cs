using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.DataAccess.ReadEntities;

public class DocumentRoot
{
    public LearningWorld learningWorld { get; set; }

    public LearningWorldViewModel? SelectedLearningWorld { get; set; }

    public LearningWorldViewModel GetLearningworldProperties(LearningWorldViewModel selectedLearningWorld)
    {
        SelectedLearningWorld = selectedLearningWorld;
        return SelectedLearningWorld;
    }
}