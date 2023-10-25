using JetBrains.Annotations;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;
using Presentation.PresentationLogic.LearningElement;
using Shared;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLayout;

public class AdvancedLearningSpaceLayoutViewModel : IAdvancedLearningSpaceLayoutViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    internal AdvancedLearningSpaceLayoutViewModel()
    {
        _learningElements = new Dictionary<int, ILearningElementViewModel>();
        _advancedLearningElementSlots = new Dictionary<int, IAdvancedLearningElementSlotViewModel>();
        _advancedDecorations = new Dictionary<int, IAdvancedDecorationViewModel>();
        _advancedCornerPoints = new Dictionary<int, DoublePoint>
        {
            { 0, new DoublePoint { X = 50, Y = 50 } },
            { 1, new DoublePoint { X = 500, Y = 50 } },
            { 2, new DoublePoint { X = 500, Y = 200 } },
            { 3, new DoublePoint { X = 50, Y = 200 } }
        };
        _entryDoorPosition = new DoublePoint { X = 50, Y = 125 };
        _exitDoorPosition = new DoublePoint { X = 500, Y = 125 };
    }

    public IDictionary<int, ILearningElementViewModel> LearningElements
    {
        get => _learningElements;
        set => _learningElements = value;
    }

    public IDictionary<int, IAdvancedLearningElementSlotViewModel> AdvancedLearningElementSlots
    {
        get => _advancedLearningElementSlots;
        set => _advancedLearningElementSlots = value;
    }

    public IDictionary<int, IAdvancedDecorationViewModel> AdvancedDecorations
    {
        get => _advancedDecorations;
        set => _advancedDecorations = value;
    }

    public IDictionary<int, DoublePoint> AdvancedCornerPoints
    {
        get => _advancedCornerPoints;
        set => _advancedCornerPoints = value;
    }

    public DoublePoint EntryDoorPosition
    {
        get => _entryDoorPosition;
        set => _entryDoorPosition = value;
    }

    public DoublePoint ExitDoorPosition
    {
        get => _exitDoorPosition;
        set => _exitDoorPosition = value;
    }

    public int Count => ContainedLearningElements.Count();

    public IEnumerable<int> UsedIndices =>
        _learningElements.Keys;

    public IEnumerable<ILearningElementViewModel> ContainedLearningElements =>
        _learningElements.Values;

    public IEnumerable<IAdvancedLearningElementSlotViewModel> ContainedAdvancedLearningElementSlots =>
        _advancedLearningElementSlots.Values;

    public IEnumerable<IAdvancedDecorationViewModel> ContainedAdvancedDecorations =>
        _advancedDecorations.Values;

    public IEnumerable<DoublePoint> ContainedAdvancedCornerPoints =>
        _advancedCornerPoints.Values;

    private IDictionary<int, ILearningElementViewModel> _learningElements;
    private IDictionary<int, IAdvancedLearningElementSlotViewModel> _advancedLearningElementSlots;
    private IDictionary<int, IAdvancedDecorationViewModel> _advancedDecorations;
    private IDictionary<int, DoublePoint> _advancedCornerPoints;
    private DoublePoint _entryDoorPosition;
    private DoublePoint _exitDoorPosition;

    public ILearningElementViewModel GetElement(int index)
    {
        return _learningElements[index];
    }

    public void PutElement(int index, ILearningElementViewModel element)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index), index,
                $"Index is out of range for the current floor plan with max capacity of "
                // + $"{FloorPlanViewModel.Capacity}"
            );
        _learningElements[index] = element;
    }

    public void RemoveElement(int index)
    {
        if (!LearningElements.ContainsKey(index))
            throw new ArgumentException("There was no element at the given index", nameof(index));
        _learningElements.Remove(index);
    }

    public void ClearAllElements() => _learningElements.Clear();

    public void AddAdvancedLearningElementSlot(Guid spaceId, int slotKey, double positionX, double positionY)
    {
        AdvancedLearningElementSlots.Add(slotKey,
            new AdvancedLearningElementSlotViewModel(spaceId, slotKey, positionX, positionY));
    }

    public void AddAdvancedDecoration(Guid spaceId, int decorationKey, double positionX, double positionY)
    {
        AdvancedDecorations.Add(decorationKey,
            new AdvancedDecorationViewModel(spaceId, decorationKey, positionX, positionY));
    }
}