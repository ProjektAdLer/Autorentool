namespace Presentation.Components.Forms.Models;

public interface ILinkContentFormModel : ILearningContentFormModel
{
    public string Link { get; set; }
    public bool IsDeleted { get; set; }
}