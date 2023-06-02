using JetBrains.Annotations;

namespace BusinessLogic.Entities;

public class Topic : ITopic
{
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    [UsedImplicitly]
    protected Topic()
    {
        Id = Guid.Empty;
        Name = string.Empty;
        UnsavedChanges = false;
    }

    public Topic(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        UnsavedChanges = true;
    }
    
    public string Name { get; set; }
    public bool UnsavedChanges { get; set; }
    public Guid Id { get; set; }

    public IMemento GetMemento()
    {
        return new TopicMemento(Name, UnsavedChanges);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not TopicMemento topicMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }
        Name = topicMemento.Name;
        UnsavedChanges = topicMemento.UnsavedChanges;
    }

    private record TopicMemento : IMemento
    {
        internal TopicMemento(string name, bool unsavedChanges)
        {
            Name = name;
            UnsavedChanges = unsavedChanges;
        }
        
        internal string Name { get; }
        public bool UnsavedChanges { get; }
    }
}