using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Layout;
[TestFixture]
public class LayoutCommandFactoryTests
{
    private LayoutCommandFactory _factory;

    [SetUp]
    public void Setup()
    {
        _factory = new LayoutCommandFactory();
    }

    [Test]
    public void GetChangeCommand_WithLearningSpaceAndLearningWorld_ReturnsChangeLearningSpaceLayoutCommand()
    {
        // Arrange
        var learningSpace = EntityProvider.GetLearningSpace();
        var learningWorld = EntityProvider.GetLearningWorld();
        var floorPlanName = FloorPlanEnum.R20X206L;
        Action<LearningWorld> mappingAction = world => { };

        // Act
        var result = _factory.GetChangeCommand(learningSpace, learningWorld, floorPlanName, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<ChangeLearningSpaceLayout>());
        var resultCasted = result as ChangeLearningSpaceLayout;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningSpace, Is.EqualTo(learningSpace));
            Assert.That(resultCasted.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.FloorPlanName, Is.EqualTo(floorPlanName));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetPlaceFromLayoutCommand_WithLearningSpaceAndLearningElement_ReturnsPlaceLearningElementInLayoutFromLayoutCommand()
    {
        // Arrange
        var parentSpace = EntityProvider.GetLearningSpace();
        var learningElement = EntityProvider.GetLearningElement();
        parentSpace.LearningSpaceLayout.LearningElements.Add(0, learningElement);
        var newSlotIndex = 1;
        Action<LearningSpace> mappingAction = space => { };

        // Act
        var result = _factory.GetPlaceFromLayoutCommand(parentSpace, learningElement, newSlotIndex, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<PlaceLearningElementInLayoutFromLayout>());
        var resultCasted = result as PlaceLearningElementInLayoutFromLayout;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.ParentSpace, Is.EqualTo(parentSpace));
            Assert.That(resultCasted.LearningElement, Is.EqualTo(learningElement));
            Assert.That(resultCasted.NewSlotIndex, Is.EqualTo(newSlotIndex));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetPlaceFromUnplacedCommand_WithLearningWorldAndLearningSpaceAndLearningElement_ReturnsPlaceLearningElementInLayoutFromUnplacedCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var learningSpace = EntityProvider.GetLearningSpace();
        learningWorld.LearningSpaces.Add(learningSpace);
        var learningElement = EntityProvider.GetLearningElement();
        learningWorld.UnplacedLearningElements.Add(learningElement);
        var newSlotIndex = 1;
        Action<LearningWorld> mappingAction = world => { };

        // Act
        var result = _factory.GetPlaceFromUnplacedCommand(learningWorld, learningSpace, learningElement, newSlotIndex, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<PlaceLearningElementInLayoutFromUnplaced>());
        var resultCasted = result as PlaceLearningElementInLayoutFromUnplaced;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.LearningSpace, Is.EqualTo(learningSpace));
            Assert.That(resultCasted.LearningElement, Is.EqualTo(learningElement));
            Assert.That(resultCasted.NewSlotIndex, Is.EqualTo(newSlotIndex));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetRemoveCommand_WithLearningWorldAndLearningSpaceAndLearningElement_ReturnsRemoveLearningElementFromLayoutCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var learningSpace = EntityProvider.GetLearningSpace();
        learningWorld.LearningSpaces.Add(learningSpace);
        var learningElement = EntityProvider.GetLearningElement();
        learningSpace.LearningSpaceLayout.LearningElements.Add(0, learningElement);
        Action<LearningWorld> mappingAction = world => { };

        // Act
        var result = _factory.GetRemoveCommand(learningWorld, learningSpace, learningElement, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<RemoveLearningElementFromLayout>());
        var resultCasted = result as RemoveLearningElementFromLayout;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.LearningSpace, Is.EqualTo(learningSpace));
            Assert.That(resultCasted.LearningElement, Is.EqualTo(learningElement));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }
}
