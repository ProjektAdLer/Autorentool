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
}