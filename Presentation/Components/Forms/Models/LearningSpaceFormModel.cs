using Shared;

namespace Presentation.Components.Forms.Models;

public class LearningSpaceFormModel
{
    public LearningSpaceFormModel()
    {
        Name = "";
        Description = "";
        Goals = "";
        RequiredPoints = null;
        Theme = default;
    }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public int? RequiredPoints { get; set; }
    public Theme Theme { get; set; }
    
    public bool AdvancedMode { get; set; }
}