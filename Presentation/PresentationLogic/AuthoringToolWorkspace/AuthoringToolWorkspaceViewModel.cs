using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

public class AuthoringToolWorkspaceViewModel : IAuthoringToolWorkspaceViewModel
{
    private LearningWorldViewModel? selectedLearningWorld;
    private IDictionary<string, string>? editDialogInitialValues;

    /// <summary>
    /// Constructor for both normal usage and Automapper
    /// </summary>
    public AuthoringToolWorkspaceViewModel()
    {
        _learningWorlds = new List<LearningWorldViewModel>();
        SelectedLearningWorld = null;
        EditDialogInitialValues = null;
    }

    private List<LearningWorldViewModel> _learningWorlds;
    
    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModel.LearningWorlds"/>
    public IEnumerable<LearningWorldViewModel> LearningWorlds => _learningWorlds;

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModel.AddLearningWorld"/>
    public void AddLearningWorld(LearningWorldViewModel learningWorld)
    {
        _learningWorlds.Add(learningWorld);
        OnPropertyChanged(nameof(LearningWorlds));
    }

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModel.RemoveLearningWorld"/>
    public void RemoveLearningWorld(LearningWorldViewModel learningWorld)
    {
        _learningWorlds.Remove(learningWorld);
        OnPropertyChanged(nameof(LearningWorlds));
    }
    
    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModel.SelectedLearningWorld"/>
    public LearningWorldViewModel? SelectedLearningWorld
    {
        get => selectedLearningWorld;
        set
        {
            if (value != null && !LearningWorlds.Contains(value))
                throw new ArgumentException("value isn't contained in collection.");
            selectedLearningWorld = value;
            OnPropertyChanged();
        }
    }
    
    
    public IDictionary<string, string>? EditDialogInitialValues
    {
        get => editDialogInitialValues;
        set
        {
            editDialogInitialValues = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}