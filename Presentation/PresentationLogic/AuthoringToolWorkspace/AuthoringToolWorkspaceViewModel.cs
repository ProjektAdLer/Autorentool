using System.ComponentModel;
using System.Runtime.CompilerServices;
using BusinessLogic.Validation;
using JetBrains.Annotations;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

public class AuthoringToolWorkspaceViewModel : IAuthoringToolWorkspaceViewModel
{
    private IDictionary<string, string>? editDialogInitialValues;

    /// <summary>
    /// Constructor for both normal usage and Automapper
    /// </summary>
    public AuthoringToolWorkspaceViewModel()
    {
        _learningWorlds = new List<ILearningWorldViewModel>();
        EditDialogInitialValues = null;
    }

    internal List<ILearningWorldViewModel> _learningWorlds;
    
    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModel.LearningWorlds"/>
    public IList<ILearningWorldViewModel> LearningWorlds => _learningWorlds;

    /// <inheritdoc cref="ILearningWorldNamesProvider.WorldNames"/>
    public IEnumerable<(Guid, string)> WorldNames => _learningWorlds.Select(world => (world.Id, world.Name));
    
    /// <inheritdoc cref="ILearningWorldNamesProvider.WorldShortnames"/>
    public IEnumerable<(Guid, string)> WorldShortnames => _learningWorlds.Select(world => (world.Id, world.Shortname));

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModel.RemoveLearningWorld"/>
    public void RemoveLearningWorld(LearningWorldViewModel learningWorld)
    {
        _learningWorlds.Remove(learningWorld);
        OnPropertyChanged(nameof(LearningWorlds));
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