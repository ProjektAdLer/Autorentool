namespace BusinessLogic.Entities.LearningContent.AdaptivityContent;

public class AdaptivityContent : IAdaptivityContent
{
    public AdaptivityContent(string name, IEnumerable<IAdaptivityTask> tasks, IEnumerable<IAdaptivityRule> rules)
    {
        Tasks = tasks;
        Rules = rules;
        Name = name;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityContent()
    {
        Tasks = null!;
        Rules = null!;
        Name = "";
    }

    public IEnumerable<IAdaptivityTask> Tasks { get; set; }
    public IEnumerable<IAdaptivityRule> Rules { get; set; }
    public string Name { get; set; }
}