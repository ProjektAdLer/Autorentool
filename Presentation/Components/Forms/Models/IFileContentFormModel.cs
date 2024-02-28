namespace Presentation.Components.Forms.Models;

public interface IFileContentFormModel : ILearningContentFormModel
{
    public string Type { get; set; }
    public string Filepath { get; set; }
}