using System.ComponentModel;
using System.Runtime.CompilerServices;
using Presentation.Components;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpace;
using Presentation.PresentationLogic.SelectedViewModels;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpaceEditor;

public class AdvancedLearningSpaceEditorPresenter : IAdvancedLearningSpaceEditorPresenter, IAdvancedPositioningService
{
    private IAdvancedLearningSpaceViewModel? _advancedLearningSpaceVm;
    public AdvancedLearningSpaceEditorPresenter(ILogger<AdvancedLearningSpaceEditorPresenter> logger, ISelectedViewModelsProvider selectedViewModelsProvider)
    {
        Logger = logger;
    }
    private ILogger<AdvancedLearningSpaceEditorPresenter> Logger { get; }

    public IAdvancedLearningSpaceViewModel? AdvancedLearningSpaceVm
    {
        get => _advancedLearningSpaceVm;
        private set => SetField(ref _advancedLearningSpaceVm, value);
    }
    public IAdvancedComponentViewModel? SelectedAdvancedComponentViewModel { get; set; }

    public void DragSelectedAdvancedComponent(object sender, DraggedEventArgs<IAdvancedComponentViewModel> draggedEventArgs)
    {
        throw new NotImplementedException();
    }

    public void SetSelectedAdvancedComponentViewModel(IAdvancedComponentViewModel obj)
    {
        SelectedAdvancedComponentViewModel = obj;
    }

    public void SetAdvancedLearningSpace(AdvancedLearningSpaceViewModel advSpace)
    {
        AdvancedLearningSpaceVm = advSpace;
        Logger.LogDebug("LearningSpace set to {Name}", advSpace.Name);
    }

    public void DeleteSelectedAdvancedComponent()
    {

    }

    public void RightClickOnAdvancedComponent()
    {

    }

    public void CreateAdvancedComponent(IAdvancedComponentViewModel sourceObject, double x, double y)
    {
        throw new NotImplementedException();
    }

    public void DeleteAdvancedComponent(IAdvancedComponentViewModel targetObject)
    {
        throw new NotImplementedException();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
