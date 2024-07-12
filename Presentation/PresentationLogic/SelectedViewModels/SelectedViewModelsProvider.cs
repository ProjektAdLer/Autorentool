using System.ComponentModel;
using System.Runtime.CompilerServices;
using BusinessLogic.Commands;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.Story;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.SelectedViewModels;

public class SelectedViewModelsProvider : ISelectedViewModelsProvider
{
    private readonly IOnUndoRedo _onUndoRedo;
    private readonly Stack<ISelectedViewModelStackEntry> _redoStack = new();

    private readonly Stack<ISelectedViewModelStackEntry> _undoStack = new();
    private int _activeElementSlotInSpace = -1;
    private int _activeStorySlotInSpace;
    private ILearningContentViewModel? _learningContent;
    private ILearningElementViewModel? _learningElement;
    private ISelectableObjectInWorldViewModel? _learningObjectInPathWay;
    private ILearningWorldViewModel? _learningWorld;

    public SelectedViewModelsProvider(IOnUndoRedo onUndoRedo, ILogger<SelectedViewModelsProvider> logger)
    {
        _onUndoRedo = onUndoRedo;
        _onUndoRedo.OnUndo += OnUndo;
        _onUndoRedo.OnRedo += OnRedo;

        Logger = logger;
    }

    private ILogger<SelectedViewModelsProvider> Logger { get; }

    public ILearningWorldViewModel? LearningWorld
    {
        get => _learningWorld;
        private set => SetField(ref _learningWorld, value);
    }

    public ISelectableObjectInWorldViewModel? LearningObjectInPathWay
    {
        get => _learningObjectInPathWay;
        private set => SetField(ref _learningObjectInPathWay, value);
    }

    public ILearningElementViewModel? LearningElement
    {
        get => _learningElement;
        private set => SetField(ref _learningElement, value);
    }

    public ILearningContentViewModel? LearningContent
    {
        get => _learningContent;
        private set => SetField(ref _learningContent, value);
    }

    public int ActiveElementSlotInSpace
    {
        get => _activeElementSlotInSpace;
        private set => SetField(ref _activeElementSlotInSpace, value);
    }

    public int ActiveStorySlotInSpace
    {
        get => _activeStorySlotInSpace;
        private set => SetField(ref _activeStorySlotInSpace, value);
    }

    public void SetActiveElementSlotInSpace(int slot, ICommand? command)
    {
        if (command is not null)
            _undoStack.Push(new ActiveElementSlotInSpaceStackEntry(command, ActiveElementSlotInSpace,
                s => ActiveElementSlotInSpace = s));
        ActiveElementSlotInSpace = slot;
        ActiveStorySlotInSpace = -1;
        Logger.LogTrace("ActiveSlotInSpace set to {Slot}", slot);
        _redoStack.Clear();
    }

    public void SetActiveStorySlotInSpace(int slot, ICommand? command)
    {
        if (command is not null)
            _undoStack.Push(new ActiveStorySlotInSpaceStackEntry(command, ActiveStorySlotInSpace,
                s => ActiveStorySlotInSpace = s));
        ActiveStorySlotInSpace = slot;
        ActiveElementSlotInSpace = -1;
        Logger.LogTrace("ActiveStorySlotInSpace set to {Slot}", slot);
        _redoStack.Clear();
    }

    public void SetLearningWorld(ILearningWorldViewModel? learningWorld, ICommand? command)
    {
        if (command is not null)
            _undoStack.Push(
                new SelectedLearningWorldViewModelStackEntry(command, LearningWorld, lw => LearningWorld = lw));
        LearningWorld = learningWorld;
        Logger.LogTrace("LearningWorld set to {LearningWorld} with id {Id}", learningWorld?.Name, learningWorld?.Id);
        SetActiveElementSlotInSpace(-1, command);
        _redoStack.Clear();
    }

    public void SetLearningObjectInPathWay(ISelectableObjectInWorldViewModel? learningObjectInPathWay,
        ICommand? command)
    {
        if (command is not null)
            _undoStack.Push(new SelectedLearningObjectInPathWayViewModelStackEntry(command, LearningObjectInPathWay,
                obj => LearningObjectInPathWay = obj));
        LearningObjectInPathWay = learningObjectInPathWay;
        switch (learningObjectInPathWay)
        {
            case null:
                Logger.LogTrace("LearningObjectInPathWay set to null");
                break;
            case LearningPathwayViewModel learningPathwayViewModel:
                Logger.LogTrace("LearningObjectInPathWay set to {Id}", learningPathwayViewModel.Id);
                break;
            case PathWayConditionViewModel pathWayConditionViewModel:
                Logger.LogTrace("LearningObjectInPathWay set to {Id}", pathWayConditionViewModel.Id);
                break;
            case LearningSpaceViewModel learningSpaceViewModel:
                Logger.LogTrace("LearningObjectInPathWay set to space {Name} with id {Id}",
                    learningSpaceViewModel.Name, learningSpaceViewModel.Id);
                break;
            default:
                Logger.LogTrace("LearningObjectInPathWay set to an unrecognized object type");
                break;
        }

        SetActiveElementSlotInSpace(-1, command);
        _redoStack.Clear();
    }

    public void SetLearningElement(ILearningElementViewModel? learningElement, ICommand? command)
    {
        if (command is not null)
            _undoStack.Push(
                new SelectedLearningElementViewModelStackEntry(command, LearningElement, le => LearningElement = le));
        LearningElement = learningElement;
        Logger.LogTrace("LearningElement set to {LearningElement} with id {Id}", learningElement?.Name,
            learningElement?.Id);
        if (learningElement?.LearningContent is StoryContentViewModel)
            SetActiveStorySlotInSpace(-1, command);
        else
            SetActiveElementSlotInSpace(-1, command);
        _redoStack.Clear();
    }

    public void SetLearningContent(ILearningContentViewModel? content, ICommand? command)
    {
        if (command is not null)
            _undoStack.Push(
                new SelectedLearningContentViewModelStackEntry(command, LearningContent, lc => LearningContent = lc));
        LearningContent = content;
        Logger.LogTrace("LearningContent set to {LearningContent}", content?.Name);
        _redoStack.Clear();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnRedo(ICommand obj)
    {
        while (_redoStack.Any() && _redoStack.Peek().Command.Equals(obj))
        {
            var stackEntry = _redoStack.Pop();
            ISelectedViewModelStackEntry undoStackEntry = stackEntry switch
            {
                SelectedLearningWorldViewModelStackEntry we => new SelectedLearningWorldViewModelStackEntry(we.Command,
                    LearningWorld, lw => LearningWorld = lw),
                SelectedLearningObjectInPathWayViewModelStackEntry op => new
                    SelectedLearningObjectInPathWayViewModelStackEntry(op.Command, LearningObjectInPathWay,
                        lo => LearningObjectInPathWay = lo),
                SelectedLearningElementViewModelStackEntry el => new SelectedLearningElementViewModelStackEntry(
                    el.Command, LearningElement, le => LearningElement = le),
                SelectedLearningContentViewModelStackEntry ce => new SelectedLearningContentViewModelStackEntry(
                    ce.Command, LearningContent, lc => LearningContent = lc),
                ActiveElementSlotInSpaceStackEntry sl => new ActiveElementSlotInSpaceStackEntry(sl.Command,
                    ActiveElementSlotInSpace,
                    s => ActiveElementSlotInSpace = s),
                ActiveStorySlotInSpaceStackEntry stl => new ActiveStorySlotInSpaceStackEntry(stl.Command,
                    ActiveStorySlotInSpace,
                    s => ActiveStorySlotInSpace = s),
                _ => throw new InvalidEnumArgumentException()
            };
            stackEntry.Apply();
            _undoStack.Push(undoStackEntry);
        }
    }

    private void OnUndo(ICommand obj)
    {
        while (_undoStack.Any() && _undoStack.Peek().Command.Equals(obj))
        {
            var stackEntry = _undoStack.Pop();
            ISelectedViewModelStackEntry redoStackEntry = stackEntry switch
            {
                SelectedLearningWorldViewModelStackEntry we => new SelectedLearningWorldViewModelStackEntry(we.Command,
                    LearningWorld, lw => LearningWorld = lw),
                SelectedLearningObjectInPathWayViewModelStackEntry op => new
                    SelectedLearningObjectInPathWayViewModelStackEntry(op.Command, LearningObjectInPathWay,
                        lo => LearningObjectInPathWay = lo),
                SelectedLearningElementViewModelStackEntry el => new SelectedLearningElementViewModelStackEntry(
                    el.Command, LearningElement, le => LearningElement = le),
                SelectedLearningContentViewModelStackEntry ce => new SelectedLearningContentViewModelStackEntry(
                    ce.Command, LearningContent, lc => LearningContent = lc),
                ActiveElementSlotInSpaceStackEntry sl => new ActiveElementSlotInSpaceStackEntry(sl.Command,
                    ActiveElementSlotInSpace,
                    s => ActiveElementSlotInSpace = s),
                ActiveStorySlotInSpaceStackEntry stl => new ActiveStorySlotInSpaceStackEntry(stl.Command,
                    ActiveStorySlotInSpace,
                    s => ActiveStorySlotInSpace = s),
                _ => throw new InvalidEnumArgumentException()
            };
            stackEntry.Apply();
            _redoStack.Push(redoStackEntry);
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }
}