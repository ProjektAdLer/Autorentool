using BusinessLogic.API;
using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Element;

[TestFixture]
public class ElementCommandFactoryUt
{
    private ElementCommandFactory _factory = null!;

    [SetUp]
    public void Setup()
    {
        _factory = new ElementCommandFactory();
    }

    [Test]
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
        Action<LearningSpace> mappingAction = space => { };

        // Act
        var result = _factory.GetCreateInSlotCommand(parentSpace, slotIndex, name, learningContent, description, goals,
            difficulty, elementModel, workload, points, positionX, positionY, mappingAction, null!);

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
    public void GetCreateInSlotCommand_WithLearningSpaceAndLearningElement_ReturnsCreateLearningElementInSlotCommand()
    {
        // Arrange
        var parentSpace = EntityProvider.GetLearningSpace();
        var slotIndex = 1;
        var learningElement = EntityProvider.GetLearningElement();
        Action<LearningSpace> mappingAction = space => { };

        // Act
        var result = _factory.GetCreateInSlotCommand(parentSpace, slotIndex, learningElement, mappingAction, null!);

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
        Action<LearningWorld> mappingAction = world => { };

        // Act
        var result = _factory.GetCreateUnplacedCommand(learningWorld, name, learningContent, description, goals,
            difficulty, elementModel, workload, points, positionX, positionY, mappingAction, null!);

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
    public void GetDeleteInSpaceCommand_WithLearningElementAndLearningSpace_ReturnsDeleteLearningElementInSpaceCommand()
    {
        // Arrange
        var learningElement = EntityProvider.GetLearningElement();
        var parentSpace = EntityProvider.GetLearningSpace();
        Action<LearningSpace> mappingAction = space => { };

        // Act
        var result = _factory.GetDeleteInSpaceCommand(learningElement, parentSpace, mappingAction, null!);

        // Assert
        Assert.That(result, Is.InstanceOf<DeleteLearningElementInSpace>());
        var resultCasted = result as DeleteLearningElementInSpace;
        Assert.That(resultCasted!.LearningElement, Is.EqualTo(learningElement));
        Assert.That(resultCasted.ParentSpace, Is.EqualTo(parentSpace));
        Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
    }

    [Test]
    public void GetDeleteInWorldCommand_WithLearningElementAndLearningWorld_ReturnsDeleteLearningElementInWorldCommand()
    {
        // Arrange
        var learningElement = EntityProvider.GetLearningElement();
        var parentWorld = EntityProvider.GetLearningWorld();
        Action<LearningWorld> mappingAction = world => { };

        // Act
        var result = _factory.GetDeleteInWorldCommand(learningElement, parentWorld, mappingAction, null!);

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
    public void GetDragCommand_WithLearningElementAndCoordinates_ReturnsDragLearningElementCommand()
    {
        // Arrange
        var learningElement = EntityProvider.GetLearningElement();
        var oldPositionX = 0.2;
        var oldPositionY = 0.3;
        var newPositionX = 0.4;
        var newPositionY = 0.5;
        Action<LearningElement> mappingAction = element => { };

        // Act
        var result = _factory.GetDragCommand(learningElement, oldPositionX, oldPositionY, newPositionX, newPositionY,
            mappingAction, null!);

        // Assert
        Assert.That(result, Is.InstanceOf<DragLearningElement>());
        var resultCasted = result as DragLearningElement;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningElement, Is.EqualTo(learningElement));
            Assert.That(resultCasted.OldPositionX, Is.EqualTo(oldPositionX));
            Assert.That(resultCasted.OldPositionY, Is.EqualTo(oldPositionY));
            Assert.That(resultCasted.NewPositionX, Is.EqualTo(newPositionX));
            Assert.That(resultCasted.NewPositionY, Is.EqualTo(newPositionY));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
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
        Action<LearningElement> mappingAction = element => { };

        // Act
        var result = _factory.GetEditCommand(learningElement, parentSpace, name, description, goals, difficulty,
            elementModel, workload, points, learningContent, mappingAction, null!);

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

    [Test]
    public void GetLoadCommand_WithLearningSpaceAndFilePath_ReturnsLoadLearningElementCommandUsingFilePath()
    {
        // Arrange
        var parentSpace = EntityProvider.GetLearningSpace();
        var slotIndex = 1;
        var filepath = "/path/to/file";
        var businessLogic = Substitute.For<IBusinessLogic>();
        Action<LearningSpace> mappingAction = space => { };

        // Act
        var result = _factory.GetLoadCommand(parentSpace, slotIndex, filepath, businessLogic, mappingAction, null!);

        // Assert
        Assert.That(result, Is.InstanceOf<LoadLearningElement>());
        var resultCasted = result as LoadLearningElement;
        Assert.That(resultCasted!.ParentSpace, Is.EqualTo(parentSpace));
        Assert.That(resultCasted.SlotIndex, Is.EqualTo(slotIndex));
        Assert.That(resultCasted.Filepath, Is.EqualTo(filepath));
        Assert.That(resultCasted.BusinessLogic, Is.EqualTo(businessLogic));
        Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
    }

    [Test]
    public void GetLoadCommand_WithLearningSpaceAndStream_ReturnsLoadLearningElementCommandUsingStream()
    {
        // Arrange
        var parentSpace = EntityProvider.GetLearningSpace();
        var slotIndex = 1;
        var stream = new MemoryStream();
        var businessLogic = Substitute.For<IBusinessLogic>();
        var learningElement = EntityProvider.GetLearningElement();
        businessLogic.LoadLearningElement(Arg.Any<Stream>()).Returns(learningElement);
        Action<LearningSpace> mappingAction = space => { };

        // Act
        var result = _factory.GetLoadCommand(parentSpace, slotIndex, stream, businessLogic, mappingAction, null!);

        // Assert
        Assert.That(result, Is.InstanceOf<LoadLearningElement>());
        var resultCasted = result as LoadLearningElement;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.ParentSpace, Is.EqualTo(parentSpace));
            Assert.That(resultCasted.SlotIndex, Is.EqualTo(slotIndex));
            Assert.That(resultCasted.LearningElement, Is.EqualTo(learningElement));
            Assert.That(resultCasted.BusinessLogic, Is.EqualTo(businessLogic));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetSaveCommand_WithBusinessLogicAndLearningElement_ReturnsSaveLearningElementCommand()
    {
        // Arrange
        var businessLogic = Substitute.For<IBusinessLogic>();
        var learningElement = EntityProvider.GetLearningElement();
        var filepath = "/path/to/file";

        // Act
        var result = _factory.GetSaveCommand(businessLogic, learningElement, filepath, null!);

        // Assert
        Assert.That(result, Is.InstanceOf<SaveLearningElement>());
        var resultCasted = result as SaveLearningElement;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.BusinessLogic, Is.EqualTo(businessLogic));
            Assert.That(resultCasted.LearningElement, Is.EqualTo(learningElement));
            Assert.That(resultCasted.Filepath, Is.EqualTo(filepath));
        });
    }
}