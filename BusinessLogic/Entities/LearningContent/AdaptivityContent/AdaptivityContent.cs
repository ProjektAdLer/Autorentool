namespace BusinessLogic.Entities.LearningContent.AdaptivityContent;

class AdaptivityContent : IAdaptivityContent
{
    public AdaptivityContent(string name, IEnumerable<IAdaptivityTask> tasks, IEnumerable<IAdaptivityRule> rules)
    {
        Tasks = tasks;
        Rules = rules;
        Name = name;
    }

    public IEnumerable<IAdaptivityTask> Tasks { get; set; }
    public IEnumerable<IAdaptivityRule> Rules { get; set; }
    public string Name { get; set; }
}