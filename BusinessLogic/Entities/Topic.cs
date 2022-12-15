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
    }

    public Topic(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
    
    public string Name { get; set; }
    public Guid Id { get; set; }

    public IMemento GetMemento()
    {
        return new TopicMemento(Name);
    }

    public void RestoreMemento(IMemento memento)
    {
        if (memento is not TopicMemento topicMemento)
        {
            throw new ArgumentException("Incorrect IMemento implementation", nameof(memento));
        }
        Name = topicMemento.Name;
    }

    private record TopicMemento : IMemento
    {
        internal TopicMemento(string name)
        {
            Name = name;
        }
        
        internal string Name { get; }
    }
}