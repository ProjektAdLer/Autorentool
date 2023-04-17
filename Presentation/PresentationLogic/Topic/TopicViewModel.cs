namespace Presentation.PresentationLogic.Topic;

public class TopicViewModel : ITopicViewModel
{
    public TopicViewModel()
    {
        Id = Guid.Empty;
        Name = string.Empty;
    }

    public TopicViewModel(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
    
    public string Name { get; set; }
    public Guid Id { get; private set; }
}