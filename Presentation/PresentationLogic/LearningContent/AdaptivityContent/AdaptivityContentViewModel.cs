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
    // ReSharper disable once MemberCanBePrivate.Global - disabled because we need a public property so automapper will map it
    public bool InternalUnsavedChanges { get; private set; }

    public bool UnsavedChanges
    {
        get => InternalUnsavedChanges || Tasks.Any(task => task.UnsavedChanges);
        set => InternalUnsavedChanges = value;
    }

}