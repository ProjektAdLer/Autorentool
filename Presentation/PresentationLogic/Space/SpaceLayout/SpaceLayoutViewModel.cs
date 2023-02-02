using JetBrains.Annotations;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Space.SpaceLayout.FloorPlans;
using Shared;

namespace Presentation.PresentationLogic.Space.SpaceLayout;

public class SpaceLayoutViewModel : ISpaceLayoutViewModel
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    internal SpaceLayoutViewModel()
    {
        FloorPlanViewModel = FloorPlanViewModelProvider.GetFloorPlan(FloorPlanEnum.NoFloorPlan);
        _floorPlanName = FloorPlanEnum.NoFloorPlan;
        _elements = Array.Empty<IElementViewModel?>();
    }
    
    internal SpaceLayoutViewModel(FloorPlanEnum floorPlanName)
    {
        FloorPlanViewModel = FloorPlanViewModelProvider.GetFloorPlan(floorPlanName);
        _floorPlanName = floorPlanName;
        _elements = new IElementViewModel?[FloorPlanViewModel.Capacity];
    }

    public IFloorPlanViewModel FloorPlanViewModel { get; private set; }
    public FloorPlanEnum FloorPlanName
    {
        get => _floorPlanName;
        set
        {
            _floorPlanName = value;
            ChangeFloorPlan(_floorPlanName);
        }
    }

    public IElementViewModel?[] Elements
    {
        get => _elements;
        set => _elements = value;
    }

    //public int Capacity => FloorPlanViewModel.Capacity;
    public int Capacity => _elements.Length;
    public int Count => ContainedElements.Count();

    public IEnumerable<int> UsedIndices =>
        _elements.Where(x => x != null).Select(x => Array.IndexOf(_elements, x));

    public IEnumerable<IElementViewModel> ContainedElements => _elements.Where(e => e != null)!;

    private IElementViewModel?[] _elements;
    private FloorPlanEnum _floorPlanName;

    public void ChangeFloorPlan(FloorPlanEnum floorPlanName)
    {
        if (Count <= FloorPlanViewModelProvider.GetFloorPlan(floorPlanName).Capacity)
        {
            _elements = AdaptElementsToNewFloorPlan(_elements, _floorPlanName, floorPlanName);
            FloorPlanViewModel = FloorPlanViewModelProvider.GetFloorPlan(floorPlanName);
            _floorPlanName = floorPlanName;
        }
        else
            throw new ArgumentException("The new floor plan is too small for the current  elements.");
    }

    public IElementViewModel? GetElement(int index)
    {
        return _elements[index];
    }

    public void PutElement(int index, IElementViewModel element)
    {
        _elements[index] = element;
    }

    public void RemoveElement(int index)
    {
        _elements[index] = null;
    }
    
    public void ClearAllElements()
    {
        Array.Clear(_elements, 0, _elements.Length);
    }

    private IElementViewModel?[] AdaptElementsToNewFloorPlan(
        IElementViewModel?[] elements, FloorPlanEnum oldFloorPlanName, FloorPlanEnum newFloorPlanName)
    {
        if (oldFloorPlanName == newFloorPlanName) return elements;
        var newFloorPlanCapacity = FloorPlanViewModelProvider.GetFloorPlan(newFloorPlanName).Capacity;

        IElementViewModel?[] newElements = new IElementViewModel[newFloorPlanCapacity];

        var newIndex = 0;
        for (var i = 0; i < Capacity; i++)
        {
            if (elements[i] == null) continue;

            newElements[newIndex] = elements[i];
            newIndex++;
        }

        return newElements;
    }
}