using BusinessLogic.API;
using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Space;
[TestFixture]
public class SpaceCommandFactoryUt
{
    private SpaceCommandFactory _factory = null!;

    [SetUp]
    public void Setup()
    {
        _factory = new SpaceCommandFactory();
    }

    [Test]
    public void GetCreateCommand_WithLearningWorldAndParameters_ReturnsCreateLearningSpaceCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var name = "Space 1";
        var description = "Space description";
        var goals = "Space goals";
        var requiredPoints = 10;
        var positionX = 1.5;
        var positionY = 2.5;
        var topic = EntityProvider.GetTopic();
        Action<LearningWorld> mappingAction = world => { };

        // Act
        var result = _factory.GetCreateCommand(learningWorld, name, description, goals, requiredPoints, positionX,
            positionY, topic, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<CreateLearningSpace>());
        var resultCasted = result as CreateLearningSpace;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.LearningSpace.Name, Is.EqualTo(name));
            Assert.That(resultCasted.LearningSpace.Description, Is.EqualTo(description));
            Assert.That(resultCasted.LearningSpace.Goals, Is.EqualTo(goals));
            Assert.That(resultCasted.LearningSpace.RequiredPoints, Is.EqualTo(requiredPoints));
            Assert.That(resultCasted.LearningSpace.PositionX, Is.EqualTo(positionX));
            Assert.That(resultCasted.LearningSpace.PositionY, Is.EqualTo(positionY));
            Assert.That(resultCasted.LearningSpace.AssignedTopic, Is.EqualTo(topic));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetCreateCommand_WithLearningWorldAndLearningSpace_ReturnsCreateLearningSpaceCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var learningSpace = EntityProvider.GetLearningSpace();
        Action<LearningWorld> mappingAction = world => { };

        // Act
        var result = _factory.GetCreateCommand(learningWorld, learningSpace, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<CreateLearningSpace>());
        var resultCasted = result as CreateLearningSpace;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.LearningSpace, Is.EqualTo(learningSpace));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetDeleteCommand_WithLearningWorldAndLearningSpace_ReturnsDeleteLearningSpaceCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var learningSpace = EntityProvider.GetLearningSpace();
        Action<LearningWorld> mappingAction = world => { };

        // Act
        var result = _factory.GetDeleteCommand(learningWorld, learningSpace, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<DeleteLearningSpace>());
        var resultCasted = result as DeleteLearningSpace;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.LearningSpace, Is.EqualTo(learningSpace));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetEditCommand_WithLearningSpaceAndParameters_ReturnsEditLearningSpaceCommand()
    {
        // Arrange
        var learningSpace = EntityProvider.GetLearningSpace();
        var name = "Updated Space";
        var description = "Updated description";
        var goals = "Updated goals";
        var requiredPoints = 5;
        var topic = EntityProvider.GetTopic();
        Action<LearningSpace> mappingAction = space => { };

        // Act
        var result = _factory.GetEditCommand(learningSpace, name, description, goals, requiredPoints, topic,
            mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<EditLearningSpace>());
        var resultCasted = result as EditLearningSpace;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningSpace, Is.EqualTo(learningSpace));
            Assert.That(resultCasted.SpaceName, Is.EqualTo(name));
            Assert.That(resultCasted.Description, Is.EqualTo(description));
            Assert.That(resultCasted.Goals, Is.EqualTo(goals));
            Assert.That(resultCasted.RequiredPoints, Is.EqualTo(requiredPoints));
            Assert.That(resultCasted.Topic, Is.EqualTo(topic));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetLoadCommand_WithLearningWorldAndFilePath_ReturnsLoadLearningSpaceCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var filepath = "path/to/file";
        var businessLogic = Substitute.For<IBusinessLogic>();
        Action<LearningWorld> mappingAction = world => { };

        // Act
        var result = _factory.GetLoadCommand(learningWorld, filepath, businessLogic, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<LoadLearningSpace>());
        var resultCasted = result as LoadLearningSpace;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.BusinessLogic, Is.EqualTo(businessLogic));
            Assert.That(resultCasted.Filepath, Is.EqualTo(filepath));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetLoadCommand_WithLearningWorldAndStream_ReturnsLoadLearningSpaceCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var stream = new MemoryStream();
        var businessLogic = Substitute.For<IBusinessLogic>();
        var learningSpace = EntityProvider.GetLearningSpace();
        businessLogic.LoadLearningSpace(Arg.Any<Stream>()).Returns(learningSpace);
        Action<LearningWorld> mappingAction = world => { };

        // Act
        var result = _factory.GetLoadCommand(learningWorld, stream, businessLogic, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<LoadLearningSpace>());
        var resultCasted = result as LoadLearningSpace;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.BusinessLogic, Is.EqualTo(businessLogic));
            Assert.That(resultCasted.Filepath, Is.EqualTo(""));
            Assert.That(resultCasted.LearningSpace, Is.EqualTo(learningSpace));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetSaveCommand_WithBusinessLogicAndLearningSpace_ReturnsSaveLearningSpaceCommand()
    {
        // Arrange
        var businessLogic = Substitute.For<IBusinessLogic>();
        var learningSpace = EntityProvider.GetLearningSpace();
        var filepath = "path/to/file";

        // Act
        var result = _factory.GetSaveCommand(businessLogic, learningSpace, filepath);

        // Assert
        Assert.That(result, Is.InstanceOf<SaveLearningSpace>());
        var resultCasted = result as SaveLearningSpace;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.BusinessLogic, Is.EqualTo(businessLogic));
            Assert.That(resultCasted.LearningSpace, Is.EqualTo(learningSpace));
            Assert.That(resultCasted.Filepath, Is.EqualTo(filepath));
        });
    }
}
