namespace PersistEntities.LearningContent;

public class AdaptivityContentPe : IAdaptivityContentPe
{
    public AdaptivityContentPe(string name, ICollection<IAdaptivityTaskPe> tasks, IEnumerable<IAdaptivityRulePe> rules)
    {
        Tasks = tasks;
        Name = name;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityContentPe()
    {
        Tasks = null!;
        Name = "";
    }

    public ICollection<IAdaptivityTaskPe> Tasks { get; set; }
    public string Name { get; set; }
}