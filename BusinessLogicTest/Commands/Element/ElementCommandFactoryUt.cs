using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Element;

[TestFixture]
public class ElementCommandFactoryUt
{
    [SetUp]
    public void Setup()
    {
        _factory = new ElementCommandFactory(new NullLoggerFactory());
    }

    private ElementCommandFactory _factory = null!;

    [Test]
    // ANF-ID: [AWA0002, AWA0003]
    public void GetCreateInSlotCommand_WithLearningSpaceAndParameters_ReturnsCreateLearningElementInSlotCommand()
    {
        // Arrange
        var parentSpace = EntityProvider.GetLearningSpace();
        var slotIndex = 1;
        var name = "Element";
        var learningContent = EntityProvider.GetLinkContent();
        var description = "Description";
        var goals = "Goals";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var elementModel = ElementModel.l_h5p_slotmachine_1;
        var workload = 10;
        var points = 100;
        var positionX = 0.5;
        var positionY = 0.5;
        Action<LearningSpace> mappingAction = _ => { };

        // Act
        var result = _factory.GetCreateInSlotCommand(parentSpace, slotIndex, name, learningContent, description, goals,
            difficulty, elementModel, workload, points, positionX, positionY, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<CreateLearningElementInSlot>());
        var resultCasted = result as CreateLearningElementInSlot;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.ParentSpace, Is.EqualTo(parentSpace));
            Assert.That(resultCasted.SlotIndex, Is.EqualTo(slotIndex));
            Assert.That(resultCasted.LearningElement.Name, Is.EqualTo(name));
            Assert.That(resultCasted.LearningElement.LearningContent, Is.EqualTo(learningContent));
            Assert.That(resultCasted.LearningElement.Description, Is.EqualTo(description));
            Assert.That(resultCasted.LearningElement.Goals, Is.EqualTo(goals));
            Assert.That(resultCasted.LearningElement.Difficulty, Is.EqualTo(difficulty));
            Assert.That(resultCasted.LearningElement.Workload, Is.EqualTo(workload));
            Assert.That(resultCasted.LearningElement.Points, Is.EqualTo(points));
            Assert.That(resultCasted.LearningElement.PositionX, Is.EqualTo(positionX));
            Assert.That(resultCasted.LearningElement.PositionY, Is.EqualTo(positionY));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    // ANF-ID: [AWA0002, AWA0003]
    public void GetCreateInSlotCommand_WithLearningSpaceAndLearningElement_ReturnsCreateLearningElementInSlotCommand()
    {
        // Arrange
        var parentSpace = EntityProvider.GetLearningSpace();
        var slotIndex = 1;
        var learningElement = EntityProvider.GetLearningElement();
        Action<LearningSpace> mappingAction = _ => { };

        // Act
        var result = _factory.GetCreateInSlotCommand(parentSpace, slotIndex, learningElement, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<CreateLearningElementInSlot>());
        var resultCasted = result as CreateLearningElementInSlot;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.ParentSpace, Is.EqualTo(parentSpace));
            Assert.That(resultCasted.SlotIndex, Is.EqualTo(slotIndex));
            Assert.That(resultCasted.LearningElement, Is.EqualTo(learningElement));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    // ANF-ID: [ASN0011]
    public void GetCreateStoryInSlotCommand_WithLearningSpaceAndParameters_ReturnsCreateStoryElementInSlotCommand()
    {
        // Arrange
        var parentSpace = EntityProvider.GetLearningSpace();
        var slotIndex = 1;
        var name = "Element";
        var learningContent = EntityProvider.GetLinkContent();
        var description = "Description";
        var goals = "Goals";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var elementModel = ElementModel.l_h5p_slotmachine_1;
        var workload = 10;
        var points = 100;
        var positionX = 0.5;
        var positionY = 0.5;
        Action<object> mappingAction = _ => { };

        // Act
        var result = _factory.GetCreateStoryInSlotCommand(parentSpace, slotIndex, name, learningContent, description,
            goals,
            difficulty, elementModel, workload, points, positionX, positionY, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<CreateStoryElementInSlot>());
        var resultCasted = result as CreateStoryElementInSlot;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.ParentSpace, Is.EqualTo(parentSpace));
            Assert.That(resultCasted.SlotIndex, Is.EqualTo(slotIndex));
            Assert.That(resultCasted.LearningElement.Name, Is.EqualTo(name));
            Assert.That(resultCasted.LearningElement.LearningContent, Is.EqualTo(learningContent));
            Assert.That(resultCasted.LearningElement.Description, Is.EqualTo(description));
            Assert.That(resultCasted.LearningElement.Goals, Is.EqualTo(goals));
            Assert.That(resultCasted.LearningElement.Difficulty, Is.EqualTo(difficulty));
            Assert.That(resultCasted.LearningElement.Workload, Is.EqualTo(workload));
            Assert.That(resultCasted.LearningElement.Points, Is.EqualTo(points));
            Assert.That(resultCasted.LearningElement.PositionX, Is.EqualTo(positionX));
            Assert.That(resultCasted.LearningElement.PositionY, Is.EqualTo(positionY));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    // ANF-ID: [AWA0002, AWA0003]
    public void GetCreateUnplacedCommand_WithLearningWorldAndParameters_ReturnsCreateUnplacedLearningElementCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var name = "Element";
        var learningContent = EntityProvider.GetLinkContent();
        var description = "Description";
        var goals = "Goals";
        var difficulty = LearningElementDifficultyEnum.Easy;
        var elementModel = ElementModel.l_h5p_slotmachine_1;
        var workload = 10;
        var points = 100;
        var positionX = 0.5;
        var positionY = 0.5;
        Action<LearningWorld> mappingAction = _ => { };

        // Act
        var result = _factory.GetCreateUnplacedCommand(learningWorld, name, learningContent, description, goals,
            difficulty, elementModel, workload, points, positionX, positionY, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<CreateUnplacedLearningElement>());
        var resultCasted = result as CreateUnplacedLearningElement;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.LearningElement.Name, Is.EqualTo(name));
            Assert.That(resultCasted.LearningElement.LearningContent, Is.EqualTo(learningContent));
            Assert.That(resultCasted.LearningElement.Description, Is.EqualTo(description));
            Assert.That(resultCasted.LearningElement.Goals, Is.EqualTo(goals));
            Assert.That(resultCasted.LearningElement.Difficulty, Is.EqualTo(difficulty));
            Assert.That(resultCasted.LearningElement.Workload, Is.EqualTo(workload));
            Assert.That(resultCasted.LearningElement.Points, Is.EqualTo(points));
            Assert.That(resultCasted.LearningElement.PositionX, Is.EqualTo(positionX));
            Assert.That(resultCasted.LearningElement.PositionY, Is.EqualTo(positionY));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    // ANF-ID: [AWA0016, AWA0011]
    public void GetDeleteInSpaceCommand_WithLearningElementAndLearningSpace_ReturnsDeleteLearningElementInSpaceCommand()
    {
        // Arrange
        var learningElement = EntityProvider.GetLearningElement();
        var parentSpace = EntityProvider.GetLearningSpace();
        Action<LearningSpace> mappingAction = _ => { };

        // Act
        var result = _factory.GetDeleteInSpaceCommand(learningElement, parentSpace, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<DeleteLearningElementInSpace>());
        var resultCasted = result as DeleteLearningElementInSpace;
        Assert.That(resultCasted!.LearningElement, Is.EqualTo(learningElement));
        Assert.That(resultCasted.ParentSpace, Is.EqualTo(parentSpace));
        Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
    }

    [Test]
    // ANF-ID: [ASN0015]
    public void
        GetDeleteStoryInSpaceCommand_WithLearningElementAndLearningSpace_ReturnsDeleteStoryElementInSpaceCommand()
    {
        // Arrange
        var learningElement = EntityProvider.GetLearningElement();
        var parentSpace = EntityProvider.GetLearningSpace();
        Action<LearningSpace> mappingAction = _ => { };

        // Act
        var result = _factory.GetDeleteStoryInSpaceCommand(learningElement, parentSpace, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<DeleteStoryElementInSpace>());
        var resultCasted = result as DeleteStoryElementInSpace;
        Assert.That(resultCasted!.LearningElement, Is.EqualTo(learningElement));
        Assert.That(resultCasted.ParentSpace, Is.EqualTo(parentSpace));
        Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
    }

    [Test]
    // ANF-ID: [AWA0016, AWA0011]
    public void GetDeleteInWorldCommand_WithLearningElementAndLearningWorld_ReturnsDeleteLearningElementInWorldCommand()
    {
        // Arrange
        var learningElement = EntityProvider.GetLearningElement();
        var parentWorld = EntityProvider.GetLearningWorld();
        Action<LearningWorld> mappingAction = _ => { };

        // Act
        var result = _factory.GetDeleteInWorldCommand(learningElement, parentWorld, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<DeleteLearningElementInWorld>());
        var resultCasted = result as DeleteLearningElementInWorld;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningElement, Is.EqualTo(learningElement));
            Assert.That(resultCasted.ParentWorld, Is.EqualTo(parentWorld));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    // ANF-ID: [AWA0015, AWA0010]
    public void GetEditCommand_WithLearningElementAndParameters_ReturnsEditLearningElementCommand()
    {
        // Arrange
        var learningElement = EntityProvider.GetLearningElement();
        var parentSpace = EntityProvider.GetLearningSpace();
        var name = "New Name";
        var description = "New Description";
        var goals = "New Goals";
        var difficulty = LearningElementDifficultyEnum.Medium;
        var elementModel = ElementModel.l_h5p_slotmachine_1;
        var workload = 20;
        var points = 200;
        var learningContent = EntityProvider.GetLinkContent();
        Action<LearningElement> mappingAction = _ => { };

        // Act
        var result = _factory.GetEditCommand(learningElement, parentSpace, name, description, goals, difficulty,
            elementModel, workload, points, learningContent, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<EditLearningElement>());
        var resultCasted = result as EditLearningElement;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningElement, Is.EqualTo(learningElement));
            Assert.That(resultCasted.ParentSpace, Is.EqualTo(parentSpace));
            Assert.That(resultCasted.ElementName, Is.EqualTo(name));
            Assert.That(resultCasted.Description, Is.EqualTo(description));
            Assert.That(resultCasted.Goals, Is.EqualTo(goals));
            Assert.That(resultCasted.Difficulty, Is.EqualTo(difficulty));
            Assert.That(resultCasted.Workload, Is.EqualTo(workload));
            Assert.That(resultCasted.Points, Is.EqualTo(points));
            Assert.That(resultCasted.LearningContent, Is.EqualTo(learningContent));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }
}