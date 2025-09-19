using Shared.H5P;

namespace Presentation.Components.Forms.Models;

public class FileContentFormModel : IFileContentFormModel, IEquatable<FileContentFormModel>
{
    /// <summary>
    /// Parameterless constructor required for <see cref="BaseForm{TForm,TEntity}"/> TForm constraint.
    /// </summary>
    public FileContentFormModel()
    {
        Name = "";
        Type = "";
        Filepath = "";
        IsH5P = false;
        H5PState = H5PContentState.NotValidated;
    }

    public FileContentFormModel(string name, string type, string filepath)
    {
        Name = name;
        Type = type;
        Filepath = filepath;
        IsH5P = false;
        H5PState = H5PContentState.NotValidated;
    }

    public string Name { get; set; }
    public string Type { get; set; }
    public string Filepath { get; set; }
    public bool IsH5P { get; set; }
    public H5PContentState H5PState { get; set; }

    public bool Equals(FileContentFormModel? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Type == other.Type && Filepath == other.Filepath;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((FileContentFormModel)obj);
    }

    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyMemberInGetHashCode
        return HashCode.Combine(Name, Type, Filepath);
        // ReSharper restore NonReadonlyMemberInGetHashCode
    }
}