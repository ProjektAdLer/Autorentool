using JetBrains.Annotations;
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
    }

    public IDictionary<int, ILearningElementViewModel> LearningElements
    {
        get => _learningElements;
        set => _learningElements = value;
    }

    public int Capacity => 0;
    //TODO: Fix capacity
        // FloorPlanViewModel.Capacity;
    public int Count => ContainedLearningElements.Count();

    public IEnumerable<int> UsedIndices =>
        _learningElements.Keys;

    public IEnumerable<ILearningElementViewModel> ContainedLearningElements =>
        _learningElements.Values;

    private IDictionary<int, ILearningElementViewModel> _learningElements;

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

}