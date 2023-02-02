using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Presentation.PresentationLogic.World;

namespace Presentation.PresentationLogic.AuthoringToolWorkspace;

public class AuthoringToolWorkspaceViewModel : IAuthoringToolWorkspaceViewModel
{
    private WorldViewModel? _selectedWorld;
    private IDictionary<string, string>? editDialogInitialValues;

    /// <summary>
    /// Constructor for both normal usage and Automapper
    /// </summary>
    public AuthoringToolWorkspaceViewModel()
    {
        _worlds = new List<WorldViewModel>();
        SelectedWorld = null;
        EditDialogInitialValues = null;
    }

    internal List<WorldViewModel> _worlds;
    
    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModel.Worlds"/>
    public IList<WorldViewModel> Worlds => _worlds;

    /// <inheritdoc cref="IWorldNamesProvider.WorldNames"/>
    public IEnumerable<string> WorldNames => _worlds.Select(world => world.Name);
    
    /// <inheritdoc cref="IWorldNamesProvider.WorldShortNames"/>
    public IEnumerable<string> WorldShortNames => _worlds.Select(world => world.Shortname);

    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModel.RemoveWorld"/>
    public void RemoveWorld(WorldViewModel world)
    {
        _worlds.Remove(world);
        OnPropertyChanged(nameof(Worlds));
    }
    
    /// <inheritdoc cref="IAuthoringToolWorkspaceViewModel.SelectedWorld"/>
    public WorldViewModel? SelectedWorld
    {
        get => _selectedWorld;
        set
        {
            if (value != null && !Worlds.Contains(value))
                throw new ArgumentException("value isn't contained in collection.");
            _selectedWorld = value;
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