namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent;

public class AdaptivityContentViewModel : IAdaptivityContentViewModel
{
    public AdaptivityContentViewModel(string name, ICollection<IAdaptivityTaskViewModel> tasks)
    {
        Tasks = tasks;
        Name = name;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityContentViewModel()
    {
        Tasks = null!;
        Name = "";
    }

    public ICollection<IAdaptivityTaskViewModel> Tasks { get; set; }
    public string Name { get; init; }
}