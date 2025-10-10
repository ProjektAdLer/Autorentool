using Shared.H5P;

namespace Presentation.Components.Forms.Models;

public interface IFileContentFormModel : ILearningContentFormModel
{
    public string Type { get; set; }
    public string Filepath { get; set; }
    bool IsH5P { get; set; }
    H5PContentState H5PState { get; set; }
}