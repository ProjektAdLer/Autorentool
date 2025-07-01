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
        Goals = "";
        WorldTheme = default;
        EvaluationLink = "";
        EvaluationLinkName = "";
        EvaluationLinkText = "";
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
    public string Goals { get; set; }
    public WorldTheme WorldTheme { get; set; }
    public string EvaluationLink { get; set; }
    public string EvaluationLinkName { get; set; }
    public string EvaluationLinkText { get; set; }
    public string EnrolmentKey { get; set; }
    public string StoryStart { get; set; }
    public string StoryEnd { get; set; }
}