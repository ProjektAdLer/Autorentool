using JetBrains.Annotations;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;
using Shared;

namespace Presentation.PresentationLogic.LearningSpace.SpaceLayout;

public class LearningSpaceLayoutViewModel : ILearningSpaceLayoutViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    internal LearningSpaceLayoutViewModel()
    {
        FloorPlanViewModel = FloorPlanViewModelProvider.GetFloorPlan(FloorPlanEnum.NoFloorPlan);
        _floorPlanName = FloorPlanEnum.NoFloorPlan;
        _learningElements = Array.Empty<ILearningElementViewModel?>();
    }
    
    internal LearningSpaceLayoutViewModel(FloorPlanEnum floorPlanName)
    {
        FloorPlanViewModel = FloorPlanViewModelProvider.GetFloorPlan(floorPlanName);
        _floorPlanName = floorPlanName;
        _learningElements = new ILearningElementViewModel?[FloorPlanViewModel.Capacity];
    }

    public IFloorPlanViewModel FloorPlanViewModel { get; private set; }
    public FloorPlanEnum FloorPlanName
    {
        get => _floorPlanName;
        set => _floorPlanName = value;
    }

    public ILearningElementViewModel?[] LearningElements
    {
        get => _learningElements;
        set => _learningElements = value;
    }

    //public int Capacity => FloorPlanViewModel.Capacity;
    public int Capacity => _learningElements.Length;
    public int Count => ContainedLearningElements.Count();

    public IEnumerable<int> UsedIndices =>
        _learningElements.Where(x => x != null).Select(x => Array.IndexOf(_learningElements, x));

    public IEnumerable<ILearningElementViewModel> ContainedLearningElements => _learningElements.Where(e => e != null)!;

    private ILearningElementViewModel?[] _learningElements;
    private FloorPlanEnum _floorPlanName;

    public void ChangeFloorPlan(FloorPlanEnum floorPlanName)
    {
        if (Count <= FloorPlanViewModelProvider.GetFloorPlan(floorPlanName).Capacity)
        {
            _learningElements = AdaptLearningElementsToNewFloorPlan(_learningElements, _floorPlanName, floorPlanName);
            FloorPlanViewModel = FloorPlanViewModelProvider.GetFloorPlan(floorPlanName);
            _floorPlanName = floorPlanName;
        }
        else
            throw new ArgumentException("The new floor plan is too small for the current learning elements.");
    }

    public ILearningElementViewModel? GetElement(int index)
    {
        return _learningElements[index];
    }

    public void PutElement(int index, ILearningElementViewModel element)
    {
        _learningElements[index] = element;
    }

    public void RemoveElement(int index)
    {
        _learningElements[index] = null;
    }
    
    public void ClearAllElements()
    {
        Array.Clear(_learningElements, 0, _learningElements.Length);
    }

    private ILearningElementViewModel?[] AdaptLearningElementsToNewFloorPlan(
        ILearningElementViewModel?[] learningElements, FloorPlanEnum oldFloorPlanName, FloorPlanEnum newFloorPlanName)
    {
        if (oldFloorPlanName == newFloorPlanName) return learningElements;
        var newFloorPlanCapacity = FloorPlanViewModelProvider.GetFloorPlan(newFloorPlanName).Capacity;

        ILearningElementViewModel?[] newLearningElements = new ILearningElementViewModel[newFloorPlanCapacity];

        var newIndex = 0;
        for (var i = 0; i < Capacity; i++)
        {
            if (learningElements[i] == null) continue;

            newLearningElements[newIndex] = learningElements[i];
            newIndex++;
        }

        return newLearningElements;
    }
}