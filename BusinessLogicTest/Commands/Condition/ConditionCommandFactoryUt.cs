using BusinessLogic.Commands.Condition;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Condition;

[TestFixture]
public class ConditionCommandFactoryUt
{
    private ConditionCommandFactory _factory = null!;

    [SetUp]
    public void SetUp()
    {
        _factory = new ConditionCommandFactory();
    }

    [Test]
    public void GetCreateCommand_WithCoordinates_ReturnsCreatePathWayCondition()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var condition = ConditionEnum.And;
        var positionX = 1.0;
        var positionY = 2.0;
        Action<LearningWorld> mappingAction = _ => { };

        // Act
        var result = _factory.GetCreateCommand(learningWorld, condition, positionX, positionY, mappingAction, null!);

        // Assert
        Assert.That(result, Is.InstanceOf<CreatePathWayCondition>());
        var resultCasted = result as CreatePathWayCondition;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.PathWayCondition.Condition, Is.EqualTo(condition));
            Assert.That(resultCasted.PathWayCondition.PositionX, Is.EqualTo(positionX));
            Assert.That(resultCasted.PathWayCondition.PositionY, Is.EqualTo(positionY));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
            Assert.That(resultCasted.SourceObject, Is.Null);
            Assert.That(resultCasted.TargetObject, Is.Null);
        });
    }

    [Test]
    public void GetCreateCommand_WithObjects_ReturnsCreatePathWayCondition()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var condition = ConditionEnum.And;
        var sourceObject = EntityProvider.GetLearningSpace();
        var targetObject = EntityProvider.GetLearningSpace();
        learningWorld.LearningSpaces.Add(sourceObject);
        learningWorld.LearningSpaces.Add(targetObject);
        Action<LearningWorld> mappingAction = _ => { };

        // Act
        var result = _factory.GetCreateCommand(learningWorld, condition, sourceObject, targetObject, mappingAction,
            null!);

        // Assert
        Assert.That(result, Is.InstanceOf<CreatePathWayCondition>());
        // Additional assertions specific to CreatePathWayCondition can be added here
        var resultCasted = result as CreatePathWayCondition;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.PathWayCondition.Condition, Is.EqualTo(condition));
            Assert.That(resultCasted.SourceObject, Is.EqualTo(sourceObject));
            Assert.That(resultCasted.TargetObject, Is.EqualTo(targetObject));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetDeleteCommand_ReturnsDeletePathWayCondition()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var pathWayCondition = EntityProvider.GetPathWayCondition();
        Action<LearningWorld> mappingAction = _ => { };

        // Act
        var result = _factory.GetDeleteCommand(learningWorld, pathWayCondition, mappingAction, null!);

        // Assert
        Assert.That(result, Is.InstanceOf<DeletePathWayCondition>());
        // Additional assertions specific to DeletePathWayCondition can be added here
        var resultCasted = result as DeletePathWayCondition;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.PathWayCondition, Is.EqualTo(pathWayCondition));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetEditCommand_ReturnsEditPathWayCondition()
    {
        // Arrange
        var pathWayCondition = EntityProvider.GetPathWayCondition();
        var condition = ConditionEnum.And;
        Action<PathWayCondition> mappingAction = _ => { };

        // Act
        var result = _factory.GetEditCommand(pathWayCondition, condition, mappingAction, null!);

        // Assert
        Assert.That(result, Is.InstanceOf<EditPathWayCondition>());
        // Additional assertions specific to EditPathWayCondition can be added here
        var resultCasted = result as EditPathWayCondition;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.PathWayCondition, Is.EqualTo(pathWayCondition));
            Assert.That(resultCasted.Condition, Is.EqualTo(condition));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }
}