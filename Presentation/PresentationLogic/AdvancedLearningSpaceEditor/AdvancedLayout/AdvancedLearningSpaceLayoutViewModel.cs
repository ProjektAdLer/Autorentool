using JetBrains.Annotations;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;
using Presentation.PresentationLogic.LearningElement;

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
    }

    public IDictionary<int, ILearningElementViewModel> LearningElements
    {
        get => _learningElements;
        set => _learningElements = value;
    }

    public IDictionary<int, IAdvancedLearningElementSlotViewModel> AdvancedLearningElementSlots
    {
        get => _advancedLearningElementSlots;
        set =>_advancedLearningElementSlots = value;
    }

    public IDictionary<int, IAdvancedDecorationViewModel> AdvancedDecorations
    {
        get => _advancedDecorations;
        set => _advancedDecorations = value;
    }


    public int Capacity => 0;
    //TODO: Fix capacity
        // FloorPlanViewModel.Capacity;
    public int Count => ContainedLearningElements.Count();

    public IEnumerable<int> UsedIndices =>
        _learningElements.Keys;

    public IEnumerable<ILearningElementViewModel> ContainedLearningElements =>
        _learningElements.Values;

    public IEnumerable<IAdvancedLearningElementSlotViewModel> ContainedAdvancedLearningElementSlots =>
        _advancedLearningElementSlots.Values;

    private IDictionary<int, ILearningElementViewModel> _learningElements;
    private IDictionary<int, IAdvancedLearningElementSlotViewModel> _advancedLearningElementSlots;
    private IDictionary<int, IAdvancedDecorationViewModel> _advancedDecorations;
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
        AdvancedLearningElementSlots.Add(slotKey, new AdvancedLearningElementSlotViewModel(spaceId, slotKey, positionX, positionY));
    }
    public void AddAdvancedDecoration(Guid spaceId, int decorationKey, double positionX, double positionY)
    {
        AdvancedDecorations.Add(decorationKey, new AdvancedDecorationViewModel(spaceId, decorationKey, positionX, positionY));
    }

}