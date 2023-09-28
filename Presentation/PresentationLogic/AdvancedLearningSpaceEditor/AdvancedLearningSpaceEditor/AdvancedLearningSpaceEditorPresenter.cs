using System.ComponentModel;
using System.Runtime.CompilerServices;
using Presentation.Components;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpace;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.SelectedViewModels;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpaceEditor;

public class AdvancedLearningSpaceEditorPresenter : IAdvancedLearningSpaceEditorPresenter
{
    private readonly IErrorService _errorService;
    private IAdvancedLearningSpaceViewModel? _advancedLearningSpaceVm;
    private readonly ISelectedViewModelsProvider _selectedViewModelsProvider;

    public AdvancedLearningSpaceEditorPresenter(ILogger<AdvancedLearningSpaceEditorPresenter> logger,
        ISelectedViewModelsProvider selectedViewModelsProvider, IErrorService errorService)
    {
        Logger = logger;
        _selectedViewModelsProvider = selectedViewModelsProvider;
        _selectedViewModelsProvider.PropertyChanged += SelectedViewModelsProviderOnPropertyChanged;
        _errorService = errorService;
    }

    private ILogger<AdvancedLearningSpaceEditorPresenter> Logger { get; }

    public IAdvancedLearningSpaceViewModel? AdvancedLearningSpaceViewModel
    {
        get => _advancedLearningSpaceVm;
        private set => SetField(ref _advancedLearningSpaceVm, value);
    }

    public IAdvancedComponentViewModel? SelectedAdvancedComponentViewModel { get; set; }

    public void SetSelectedAdvancedComponentViewModel(IAdvancedComponentViewModel obj)
    {
        SelectedAdvancedComponentViewModel = obj;
    }

    public void DragSelectedAdvancedComponent(object sender,
        DraggedEventArgs<IAdvancedComponentViewModel> draggedEventArgs)
    {
        throw new NotImplementedException();
    }

    public void DeleteSelectedAdvancedComponent()
    {
    }

    public void RightClickOnAdvancedComponent()
    {
    }

    public void HideRightClickMenu()
    {
    }


    public void SetAdvancedLearningSpace(AdvancedLearningSpaceViewModel advSpace)
    {
        AdvancedLearningSpaceViewModel = advSpace;
        Logger.LogDebug("LearningSpace set to {Name}", advSpace.Name);
    }

    public void CreateAdvancedLearningElementSlot(double positionX = 50D, double positionY = 50D)
    {
        if (AdvancedLearningSpaceViewModel == null)
            throw new ApplicationException("AdvancedLearningSpaceViewModel is null!");
        var spaceId = AdvancedLearningSpaceViewModel.Id;
        var slotKey = AdvancedLearningSpaceViewModel.AdvancedLearningSpaceLayout.AdvancedLearningElementSlots.Count;

        AdvancedLearningSpaceViewModel.AdvancedLearningSpaceLayout.AddAdvancedLearningElementSlot(spaceId, slotKey,
            positionX, positionY);
    }
    public void CreateAdvancedDecoration(double positionX = 50D, double positionY = 50D)
    {
        if (AdvancedLearningSpaceViewModel == null)
            throw new ApplicationException("AdvancedLearningSpaceViewModel is null!");
        var spaceId = AdvancedLearningSpaceViewModel.Id;
        var decorationKey = AdvancedLearningSpaceViewModel.AdvancedLearningSpaceLayout.AdvancedDecorations.Count;

        AdvancedLearningSpaceViewModel.AdvancedLearningSpaceLayout.AddAdvancedDecoration(spaceId, decorationKey,
            positionX, positionY);
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

    private void SelectedViewModelsProviderOnPropertyChanged(object? caller, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_selectedViewModelsProvider.LearningObjectInPathWay))
        {
            if (caller is not ISelectedViewModelsProvider)
            {
                LogAndSetError("OnSelectedViewModelsProviderOnPropertyChanged",
                    $"Caller must be of type ISelectedViewModelsProvider, got {caller?.GetType()}",
                    "Caller must be of type ISelectedViewModelsProvider");
                return;
            }

            if (_selectedViewModelsProvider.LearningObjectInPathWay is LearningSpaceViewModel)
                AdvancedLearningSpaceViewModel = null;
            else if (_selectedViewModelsProvider.LearningObjectInPathWay is AdvancedLearningSpaceViewModel advSpace)
                AdvancedLearningSpaceViewModel = advSpace;
            else if (_selectedViewModelsProvider.LearningObjectInPathWay is null)
                AdvancedLearningSpaceViewModel = null;
        }
    }

    private void LogAndSetError(string operation, string errorDetail, string userMessage)
    {
        Logger.LogError("Error in {Operation}: {ErrorDetail}", operation, errorDetail);
        _errorService.SetError("Operation failed", userMessage);
    }
}