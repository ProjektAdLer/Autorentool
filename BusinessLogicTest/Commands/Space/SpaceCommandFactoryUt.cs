using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared;
using Shared.Theme;
using TestHelpers;

namespace BusinessLogicTest.Commands.Space;

[TestFixture]
public class SpaceCommandFactoryUt
{
    [SetUp]
    public void Setup()
    {
        _factory = new SpaceCommandFactory(new NullLoggerFactory());
    }

    private SpaceCommandFactory _factory = null!;

    [Test]
    // ANF-ID: [AWA0001]
    public void GetCreateCommand_WithLearningWorldAndParameters_ReturnsCreateLearningSpaceCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var name = "Space 1";
        var description = "Space description";
        var requiredPoints = 10;
        var positionX = 1.5;
        var positionY = 2.5;
        var topic = EntityProvider.GetTopic();
        var learningOutcomeCollection = EntityProvider.GetLearningOutcomeCollection();
        var theme = SpaceTheme.LearningArea;
        Action<LearningWorld> mappingAction = _ => { };

        // Act
        var result = _factory.GetCreateCommand(learningWorld, name, description, learningOutcomeCollection,
            requiredPoints,
            theme,
            positionX, positionY, topic, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<CreateLearningSpace>());
        var resultCasted = result as CreateLearningSpace;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.LearningSpace.Name, Is.EqualTo(name));
            Assert.That(resultCasted.LearningSpace.Description, Is.EqualTo(description));
            Assert.That(resultCasted.LearningSpace.LearningOutcomeCollection, Is.EqualTo(learningOutcomeCollection));
            Assert.That(resultCasted.LearningSpace.RequiredPoints, Is.EqualTo(requiredPoints));
            Assert.That(resultCasted.LearningSpace.PositionX, Is.EqualTo(positionX));
            Assert.That(resultCasted.LearningSpace.PositionY, Is.EqualTo(positionY));
            Assert.That(resultCasted.LearningSpace.AssignedTopic, Is.EqualTo(topic));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    // ANF-ID: [AWA0001]
    public void GetCreateCommand_WithLearningWorldAndLearningSpace_ReturnsCreateLearningSpaceCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var learningSpace = EntityProvider.GetLearningSpace();
        Action<LearningWorld> mappingAction = _ => { };

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
    // ANF-ID: [AWA0024]
    public void GetDeleteCommand_WithLearningWorldAndLearningSpace_ReturnsDeleteLearningSpaceCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var learningSpace = EntityProvider.GetLearningSpace();
        Action<LearningWorld> mappingAction = _ => { };

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
    // ANF-ID: [AWA0023]
    public void GetEditCommand_WithLearningSpaceAndParameters_ReturnsEditLearningSpaceCommand()
    {
        // Arrange
        var learningSpace = EntityProvider.GetLearningSpace();
        var name = "Updated Space";
        var description = "Updated description";
        var requiredPoints = 5;
        var topic = EntityProvider.GetTopic();
        var theme = SpaceTheme.LearningArea;
        Action<ILearningSpace> mappingAction = _ => { };

        // Act
        var result = _factory.GetEditCommand(learningSpace, name, description, requiredPoints, theme,
            topic, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<EditLearningSpace>());
        var resultCasted = result as EditLearningSpace;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningSpace, Is.EqualTo(learningSpace));
            Assert.That(resultCasted.SpaceName, Is.EqualTo(name));
            Assert.That(resultCasted.Description, Is.EqualTo(description));
            Assert.That(resultCasted.RequiredPoints, Is.EqualTo(requiredPoints));
            Assert.That(resultCasted.Topic, Is.EqualTo(topic));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }
}