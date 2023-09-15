namespace BusinessLogic.Entities.LearningContent.AdaptivityContent;

public class AdaptivityContent : IAdaptivityContent
{
    public AdaptivityContent(string name, ICollection<IAdaptivityTask> tasks)
    {
        Tasks = tasks;
        Name = name;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityContent()
    {
        Tasks = null!;
        Name = "";
    }

    public ICollection<IAdaptivityTask> Tasks { get; set; }
    public string Name { get; set; }

    public bool Equals(ILearningContent? other)
    {
        if (other is not AdaptivityContent adaptivityContent)
            return false;
        return Tasks.SequenceEqual(adaptivityContent.Tasks) && Name == adaptivityContent.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AdaptivityContent)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Tasks, Name);
    }

    public static bool operator ==(AdaptivityContent? left, AdaptivityContent? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(AdaptivityContent? left, AdaptivityContent? right)
    {
        return !Equals(left, right);
    }
}