using BusinessLogic.Entities.LearningContent.H5P;
using Shared.H5P;

namespace BusinessLogic.Entities.LearningContent.FileContent;

public interface IFileContent : ILearningContent
{
    string Type { get; set; }
    string Filepath { get; set; }
    /// <summary>
    /// Indicates if this FileContent is a H5P
    /// Only if this IsH5P is true
    /// -> <see cref="H5PState"/> should used and displayed.
    /// This is a messy hack. Normally we should make a class H5PFileContent!
    /// </summary>
    bool IsH5P { get; set; }
    
    /// <summary>w
    /// Consider Documentation of <see cref="IsH5P"/>
    /// </summary>
    public H5PContentState H5PState { get; set; }
}