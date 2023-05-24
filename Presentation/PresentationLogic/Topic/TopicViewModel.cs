namespace Presentation.PresentationLogic.Topic;

public class TopicViewModel : ITopicViewModel
{
    public TopicViewModel()
    {
        Id = Guid.Empty;
        UnsavedChanges = false;
        Name = string.Empty;
    }

    public TopicViewModel(string name, bool unsavedChanges = false)
    {
        Id = Guid.NewGuid();
        UnsavedChanges = unsavedChanges;
        Name = name;
    }
    
    public string Name { get; set; }
    public bool UnsavedChanges { get; set; }
    public Guid Id { get; private set; }
}