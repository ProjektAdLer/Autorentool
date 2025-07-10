using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using Shared.Theme;

namespace Presentation.Components.Forms.Models;

public class LearningWorldFormModel
{
    public LearningWorldFormModel()
    {
        Name = "";
        Shortname = "";
        Authors = "";
        Language = "";
        Description = "";
        LearningOutcomeCollection = new LearningOutcomeCollectionViewModel();
        WorldTheme = default;
        EvaluationLink = "";
        EnrolmentKey = "";
        StoryStart = "";
        StoryEnd = "";
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public LearningOutcomeCollectionViewModel LearningOutcomeCollection { get; set; }
    public WorldTheme WorldTheme { get; set; }
    public string EvaluationLink { get; set; }
    public string EnrolmentKey { get; set; }
    public string StoryStart { get; set; }
    public string StoryEnd { get; set; }
}