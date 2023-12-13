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
        EvaluationLink = "";
        EnrolmentKey = "";
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Language { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public string EvaluationLink { get; set; }
    public string EnrolmentKey { get; set; }
}