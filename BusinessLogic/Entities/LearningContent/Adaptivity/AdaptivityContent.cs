namespace BusinessLogic.Entities.LearningContent.Adaptivity;

public class AdaptivityContent : IAdaptivityContent
{
    public AdaptivityContent(ICollection<IAdaptivityTask> tasks)
    {
        Tasks = tasks;
        Name = "";
        UnsavedChanges = true;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityContent()
    {
        Tasks = null!;
        Name = "";
        UnsavedChanges = false;
    }

    // ReSharper disable once MemberCanBePrivate.Global - disabled because we need a public property so automapper will map it
    public bool InternalUnsavedChanges { get; private set; }

    public ICollection<IAdaptivityTask> Tasks { get; set; }
    public string Name { get; set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges || Tasks.Any(task => task.UnsavedChanges);
        set => InternalUnsavedChanges = value;
    }

    public bool Equals(ILearningContent? other)
    {
        if (other is not AdaptivityContent adaptivityContent)
            return false;
        return Tasks.SequenceEqual(adaptivityContent.Tasks) && Name == adaptivityContent.Name;
    }

    public IMemento GetMemento()
    {
        return new AdaptivityContentMemento(Name, Tasks, UnsavedChanges);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not AdaptivityContentMemento adaptivityContentMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }

        Name = adaptivityContentMemento.Name;
        Tasks = adaptivityContentMemento.Tasks;
        UnsavedChanges = adaptivityContentMemento.UnsavedChanges;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AdaptivityContent) obj);
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

    private record AdaptivityContentMemento : IMemento
    {
        internal AdaptivityContentMemento(string name, ICollection<IAdaptivityTask> tasks, bool unsavedChanges)
        {
            Name = name;
            Tasks = tasks.ToList();
            UnsavedChanges = unsavedChanges;
        }

        internal string Name { get; }
        internal ICollection<IAdaptivityTask> Tasks { get; }
        internal bool UnsavedChanges { get; }
    }
}