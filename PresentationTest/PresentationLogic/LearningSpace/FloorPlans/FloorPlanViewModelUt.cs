using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout.FloorPlans;

namespace PresentationTest.PresentationLogic.LearningSpace.FloorPlans;

[TestFixture]
public class FloorPlanViewModelUt
{
    [Test]
    public void TestAllFloorPlanViewModels()
    {
        // Arrange
        var floorPlanViewModelTypes = Assembly.GetAssembly(typeof(IFloorPlanViewModel))?
            .GetTypes()
            .Where(type => typeof(IFloorPlanViewModel).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        if (floorPlanViewModelTypes == null)
        {
            Assert.Fail("No types implementing IFloorPlanViewModel found.");
        }

        var floorPlanViewModelTypesList = floorPlanViewModelTypes!.ToList();
        if (!floorPlanViewModelTypesList.Any())
        {
            Assert.Fail("No types implementing IFloorPlanViewModel found.");
        }

        // Act
        foreach (var viewModel in floorPlanViewModelTypesList.Select(viewModelType =>
                     Activator.CreateInstance(viewModelType) as IFloorPlanViewModel))
        {
            Assert.That(viewModel, Is.Not.Null);

            var capacity = viewModel!.Capacity;
            var cornerPoints = viewModel.CornerPoints;
            var elementSlotPositions = viewModel.ElementSlotPositions;
            var doorPositions = viewModel.DoorPositions;
            var icon = viewModel.GetIcon;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(capacity, Is.GreaterThan(0));
                Assert.That(cornerPoints, Has.Count.GreaterThan(3));
                Assert.That(elementSlotPositions, Is.Not.Empty);
                Assert.That(doorPositions, Is.Not.Empty);
                Assert.That(icon, Has.Length.GreaterThan(0));
            });
        }
    }
}