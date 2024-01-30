﻿using JetBrains.Annotations;
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
    private LearningSpaceLayoutViewModel()
    {
        FloorPlanViewModel = FloorPlanViewModelProvider.GetFloorPlan(FloorPlanEnum.R_20X30_8L);
        _floorPlanName = FloorPlanEnum.R_20X30_8L;
        LearningElements = new Dictionary<int, ILearningElementViewModel>();
        StoryElements = new Dictionary<int, ILearningElementViewModel>();
    }
    
    internal LearningSpaceLayoutViewModel(FloorPlanEnum floorPlanName)
    {
        FloorPlanViewModel = FloorPlanViewModelProvider.GetFloorPlan(floorPlanName);
        _floorPlanName = floorPlanName;
        LearningElements = new Dictionary<int, ILearningElementViewModel>();
        StoryElements = new Dictionary<int, ILearningElementViewModel>();
    }

    public IFloorPlanViewModel FloorPlanViewModel { get; private set; }
    public FloorPlanEnum FloorPlanName
    {
        get => _floorPlanName;
        set
        {
            if (value == _floorPlanName) return;
            _floorPlanName = value;
            ChangeFloorPlan(_floorPlanName);
        }
    }

    public IDictionary<int, ILearningElementViewModel> LearningElements { get; set; }
    public IDictionary<int, ILearningElementViewModel> StoryElements { get; set; }

    public int Capacity => FloorPlanViewModel.Capacity;
    public int Count => ContainedLearningElements.Count();

    public IEnumerable<int> UsedIndices =>
        LearningElements.Keys;

    public IEnumerable<ILearningElementViewModel> ContainedLearningElements =>
        LearningElements.Values;

    private FloorPlanEnum _floorPlanName;

    public void ChangeFloorPlan(FloorPlanEnum floorPlanName)
    {
        LearningElements = AdaptLearningElementsToNewFloorPlan(LearningElements, _floorPlanName, floorPlanName);
        FloorPlanViewModel = FloorPlanViewModelProvider.GetFloorPlan(floorPlanName);
        _floorPlanName = floorPlanName;
    }

    public ILearningElementViewModel GetElement(int index)
    {
        return LearningElements[index];
    }

    public void PutElement(int index, ILearningElementViewModel element)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index), index,
                $"Index is out of range for the current floor plan with max capacity of {FloorPlanViewModel.Capacity}");
        LearningElements[index] = element;
    }

    public void RemoveElement(int index)
    {
        if (!LearningElements.ContainsKey(index))
            throw new ArgumentException("There was no element at the given index", nameof(index));
        LearningElements.Remove(index);
    }
    
    public void ClearAllElements() => LearningElements.Clear();

    private IDictionary<int, ILearningElementViewModel> AdaptLearningElementsToNewFloorPlan(
        IDictionary<int, ILearningElementViewModel> learningElements, FloorPlanEnum oldFloorPlanName, FloorPlanEnum newFloorPlanName)
    {
        if (oldFloorPlanName == newFloorPlanName) return learningElements;
        var newFloorPlanCapacity = FloorPlanViewModelProvider.GetFloorPlan(newFloorPlanName).Capacity;

        var newLearningElementsUncompressed = learningElements
            .OrderBy(kvP => kvP.Key) //default comparer is ascending
            .Take(newFloorPlanCapacity)
            .ToList();
        var newLearningElements =
            newLearningElementsUncompressed
                .Any(kvP => kvP.Key > newFloorPlanCapacity - 1)
                //we only have to compress the indices if any of the indices of the elements is higher than the new capacity
                ? newLearningElementsUncompressed.Select((kvP, i) =>
                    new KeyValuePair<int, ILearningElementViewModel>(i, kvP.Value))
                : newLearningElementsUncompressed;
        return newLearningElements.ToDictionary(kvP => kvP.Key, kvP => kvP.Value);
    }
}