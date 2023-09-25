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
    public void GetFloorPlan_ForEachFloorPlanEnum_ReturnsCorrectViewModel()
    {
        // Arrange
        var floorPlanNames = Enum.GetValues(typeof(FloorPlanEnum)).Cast<FloorPlanEnum>();

        foreach (var floorPlan in floorPlanNames)
        {
            var expectedViewModelName = floorPlan.ToString().Replace("_", "") + "ViewModel";

            // Act
            var viewModel = FloorPlanViewModelProvider.GetFloorPlan(floorPlan);

            // Assert
            var viewModelType = viewModel.GetType();
            var viewModelTypeName = viewModelType.Name;

            Assert.That(viewModelTypeName, Is.EqualTo(expectedViewModelName));
        }
    }

    [Test]
    public void GetFloorPlan_UnknownEnumValue_ThrowsException()
    {
        var unknownEnumValue = Enum.GetValues(typeof(FloorPlanEnum)).Length;
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            FloorPlanViewModelProvider.GetFloorPlan((FloorPlanEnum) unknownEnumValue));
    }
}