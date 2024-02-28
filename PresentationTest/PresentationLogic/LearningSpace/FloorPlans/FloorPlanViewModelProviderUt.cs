using System;
using System.Linq;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;
using Shared;

namespace PresentationTest.PresentationLogic.LearningSpace.FloorPlans;

[TestFixture]
public class FloorPlanViewModelProviderUt
{
    [Test]
    public void GetFloorPlan_ForEachFloorPlanEnum_ReturnsCorrectViewModel([Values] FloorPlanEnum floorPlan)
    {
        // Arrange
        var expectedViewModelName = floorPlan.ToString().Replace("_", "") + "ViewModel";
        var expectedCapacity = int.Parse(floorPlan.ToString().Split("_").Last().Replace("L", ""));

        // Act
        var viewModel = FloorPlanViewModelProvider.GetFloorPlan(floorPlan);

        // Assert
        var viewModelType = viewModel.GetType();
        var viewModelTypeName = viewModelType.Name;

        Assert.Multiple(() =>
        {
            Assert.That(viewModelTypeName, Is.EqualTo(expectedViewModelName));
            Assert.That(viewModel.Capacity, Is.EqualTo(expectedCapacity));
            Assert.That(viewModel.ElementSlotPositions, Has.Count.EqualTo(expectedCapacity));
        });
    }

    [Test]
    public void GetFloorPlan_UnknownEnumValue_ThrowsException()
    {
        var unknownEnumValue = Enum.GetValues(typeof(FloorPlanEnum)).Length;
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            FloorPlanViewModelProvider.GetFloorPlan((FloorPlanEnum)unknownEnumValue));
    }
}