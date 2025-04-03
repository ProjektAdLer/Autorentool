using System.Security.Cryptography;
using BusinessLogic.Entities.LearningContent.FileContent;

namespace BusinessLogic.Entities.LearningContent.H5P;

public class H5PContent : IFileContent
{
    public H5PContentState State { get; set; }
    public string Name { get; set; }
    public bool UnsavedChanges { get; set; }
    public string Type { get; set; }
    public string Filepath { get; set; }
    public bool PrimitiveH5P { get; set; }
    public H5PContent(H5PContentState state,string name, bool unsavedChanges, string type,
        string filepath, bool primitiveH5P)
    {
        State = state;
        Name = name;
        Type = type;
        Filepath = filepath;
        PrimitiveH5P = primitiveH5P;
    }

    public H5PContent(H5PContentState state,string name, string type, string filepath)
    {
        Name = "";
        Type = "";
        Filepath = "";
        UnsavedChanges = false;
        State = H5PContentState.UNKNOWN;
    }

    public bool Equals(ILearningContent? other)
    {
        throw new NotImplementedException();
    }

    
}