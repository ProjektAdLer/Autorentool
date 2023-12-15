namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent;

public class AdaptivityContentViewModel : IAdaptivityContentViewModel
{
    public AdaptivityContentViewModel(ICollection<IAdaptivityTaskViewModel>? tasks = null)
    {
        tasks ??= new List<IAdaptivityTaskViewModel>();
        Tasks = tasks;
        Name = "";
        UnsavedChanges = true;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityContentViewModel()
    {
        Tasks = null!;
        Name = "";
        UnsavedChanges = false;
    }

    public ICollection<IAdaptivityTaskViewModel> Tasks { get; set; }
    public string Name { get; init; }
    private bool InternalUnsavedChanges { get; set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges || Tasks.Any(task => task.UnsavedChanges);
        set => InternalUnsavedChanges = value;
    }

}