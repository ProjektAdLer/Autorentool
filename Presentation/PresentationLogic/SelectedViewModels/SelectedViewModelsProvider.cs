using System.ComponentModel;
using System.Runtime.CompilerServices;
using BusinessLogic.Commands;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;

namespace Presentation.PresentationLogic.SelectedViewModels;

public class SelectedViewModelsProvider : ISelectedViewModelsProvider
{
    private readonly IOnUndoRedo _onUndoRedo;
    private ILearningWorldViewModel? _learningWorld;
    private ISelectableObjectInWorldViewModel? _learningObjectInPathWay;
    private ILearningElementViewModel? _learningElement;
    private ILearningContentViewModel? _learningContent;
    private int _activeSlotInSpace = -1;

    private readonly Stack<ISelectedViewModelStackEntry> _undoStack = new();
    private readonly Stack<ISelectedViewModelStackEntry> _redoStack = new();
    
    private ILogger<SelectedViewModelsProvider> _logger { get; }

    public SelectedViewModelsProvider(IOnUndoRedo onUndoRedo, ILogger<SelectedViewModelsProvider> logger)
    {
        _onUndoRedo = onUndoRedo;
        _onUndoRedo.OnUndo += OnUndo;
        _onUndoRedo.OnRedo += OnRedo;
        
        _logger = logger;
    }

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
                ActiveSlotInSpaceStackEntry sl => new ActiveSlotInSpaceStackEntry(sl.Command, ActiveSlotInSpace, s => ActiveSlotInSpace = s),
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
                ActiveSlotInSpaceStackEntry sl => new ActiveSlotInSpaceStackEntry(sl.Command, ActiveSlotInSpace, s => ActiveSlotInSpace = s),
                _ => throw new InvalidEnumArgumentException()
            };
            stackEntry.Apply();
            _redoStack.Push(redoStackEntry);
        }
    }

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
    
    public int ActiveSlotInSpace
    {
        get => _activeSlotInSpace;
        private set => SetField(ref _activeSlotInSpace, value);
    }
    
    public void SetActiveSlotInSpace(int slot, ICommand? command)
    {
        if (command is not null)
            _undoStack.Push(new ActiveSlotInSpaceStackEntry(command, ActiveSlotInSpace, s => ActiveSlotInSpace = s));
        ActiveSlotInSpace = slot;
        _logger.LogTrace("ActiveSlotInSpace set to {slot}", slot);
        _redoStack.Clear();
    }

    public void SetLearningWorld(ILearningWorldViewModel? learningWorld, ICommand? command)
    {
        if (command is not null)
            _undoStack.Push(
                new SelectedLearningWorldViewModelStackEntry(command, LearningWorld, lw => LearningWorld = lw));
        LearningWorld = learningWorld;
        _logger.LogTrace("LearningWorld set to {learningWorld} with id {id}", learningWorld?.Name, learningWorld?.Id);
        SetActiveSlotInSpace(-1, command);
        _redoStack.Clear();
    }

    public void SetLearningObjectInPathWay(ISelectableObjectInWorldViewModel? learningObjectInPathWay,
        ICommand? command)
    {
        if (command is not null)
            _undoStack.Push(new SelectedLearningObjectInPathWayViewModelStackEntry(command, LearningObjectInPathWay,
                obj => LearningObjectInPathWay = obj));
        LearningObjectInPathWay = learningObjectInPathWay;

        _logger.LogTrace(learningObjectInPathWay switch
        {
            null => "LearningObjectInPathWay set to null",
            LearningPathwayViewModel learningPathwayViewModel => $"LearningObjectInPathWay set to {learningPathwayViewModel.Id}",
            PathWayConditionViewModel pathWayConditionViewModel => $"LearningObjectInPathWay set to {pathWayConditionViewModel.Id}",
            LearningSpaceViewModel learningSpaceViewModel => $"LearningObjectInPathWay set to space {learningSpaceViewModel.Name} with id {learningSpaceViewModel.Id}",
            _ => "LearningObjectInPathWay set to an unrecognized object type"
        });
        
        SetActiveSlotInSpace(-1, command);
        _redoStack.Clear();
    }

    public void SetLearningElement(ILearningElementViewModel? learningElement, ICommand? command)
    {
        if (command is not null)
            _undoStack.Push(
                new SelectedLearningElementViewModelStackEntry(command, LearningElement, le => LearningElement = le));
        LearningElement = learningElement;
        _logger.LogTrace("LearningElement set to {learningElement} with id {id}", learningElement?.Name, learningElement?.Id);
        SetActiveSlotInSpace(-1, command);
        _redoStack.Clear();
    }

    public void SetLearningContent(ILearningContentViewModel? content, ICommand? command)
    {
        if (command is not null)
            _undoStack.Push(
                new SelectedLearningContentViewModelStackEntry(command, LearningContent, lc => LearningContent = lc));
        LearningContent = content;
        _logger.LogTrace("LearningContent set to {learningContent}", content?.Name);
        _redoStack.Clear();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}