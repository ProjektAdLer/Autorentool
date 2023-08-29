namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent;

class AdaptivityContentViewModel : IAdaptivityContentViewModel
{
    public AdaptivityContentViewModel(string name, IEnumerable<IAdaptivityTaskViewModel> tasks, IEnumerable<IAdaptivityRuleViewModel> rules)
    {
        Tasks = tasks;
        Rules = rules;
        Name = name;
    }

    public IEnumerable<IAdaptivityTaskViewModel> Tasks { get; set; }
    public IEnumerable<IAdaptivityRuleViewModel> Rules { get; set; }
    public string Name { get; init; }
}