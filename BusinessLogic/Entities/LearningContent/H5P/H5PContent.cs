using System.Security.Cryptography;
using BusinessLogic.Entities.LearningContent.FileContent;
using Shared.H5P;

namespace BusinessLogic.Entities.LearningContent.H5P;


/// <summary>
///  TODO: wahrscheinlich löschen!!!!!!!!!!!!!!!
/// we take the -> FileContent.H5PContentState hack
/// </summary>
public class H5PContent : IFileContent
{
    public string Name { get; set; }
    public bool UnsavedChanges { get; set; }
    public string Type { get; set; }
    public string Filepath { get; set; }
    public bool IsH5P { get; set; }
    public H5PContentState H5PState { get; set; }

    public H5PContent(H5PContentState state,string name, bool unsavedChanges, string type,
        string filepath, bool primitiveH5P)
    {
        H5PState = state;
        Name = name;
        Type = type;
        Filepath = filepath;
        IsH5P = primitiveH5P;
    }

    public H5PContent(H5PContentState state,string name, string type, string filepath)
    {
        Name = "";
        Type = "";
        Filepath = "";
        UnsavedChanges = false;
        H5PState = H5PContentState.Unknown;
    }

    public bool Equals(ILearningContent? other)
    {
        throw new NotImplementedException();
    }

    
}