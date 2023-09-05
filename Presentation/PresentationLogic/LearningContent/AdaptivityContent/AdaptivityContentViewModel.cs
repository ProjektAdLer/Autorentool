namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent;

public class AdaptivityContentViewModel : IAdaptivityContentViewModel
{
    public AdaptivityContentViewModel(string name, IEnumerable<IAdaptivityTaskViewModel> tasks, IEnumerable<IAdaptivityRuleViewModel> rules)
    {
        Tasks = tasks;
        Rules = rules;
        Name = name;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityContentViewModel()
    {
        Tasks = null!;
        Rules = null!;
        Name = "";
    }

    public IEnumerable<IAdaptivityTaskViewModel> Tasks { get; set; }
    public IEnumerable<IAdaptivityRuleViewModel> Rules { get; set; }
    public string Name { get; init; }
}