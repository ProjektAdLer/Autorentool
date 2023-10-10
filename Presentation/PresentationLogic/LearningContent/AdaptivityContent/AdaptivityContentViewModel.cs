namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent;

public class AdaptivityContentViewModel : IAdaptivityContentViewModel
{
    public AdaptivityContentViewModel(ICollection<IAdaptivityTaskViewModel>? tasks = null)
    {
        tasks ??= new List<IAdaptivityTaskViewModel>();
        Tasks = tasks;
        Name = "";
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