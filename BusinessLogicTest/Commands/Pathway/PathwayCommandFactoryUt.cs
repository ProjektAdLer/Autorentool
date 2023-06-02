using BusinessLogic.Commands.Pathway;
using BusinessLogic.Entities;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Pathway;

[TestFixture]
public class PathwayCommandFactoryUt
{
    private PathwayCommandFactory _factory;

    [SetUp]
    public void Setup()
    {
        _factory = new PathwayCommandFactory();
    }

    [Test]
    public void GetCreateCommand_WithLearningWorldAndSourceObjectAndTargetObject_ReturnsCreateLearningPathWayCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var sourceObject = EntityProvider.GetLearningSpace();
        var targetObject = EntityProvider.GetLearningSpace();
        Action<LearningWorld> mappingAction = world => { };

        // Act
        var result = _factory.GetCreateCommand(learningWorld, sourceObject, targetObject, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<CreateLearningPathWay>());
        var resultCasted = result as CreateLearningPathWay;
        Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
        Assert.That(resultCasted.LearningPathway.SourceObject, Is.EqualTo(sourceObject));
        Assert.That(resultCasted.LearningPathway.TargetObject, Is.EqualTo(targetObject));
        Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
    }

    [Test]
    public void GetDeleteCommand_WithLearningWorldAndLearningPathway_ReturnsDeleteLearningPathWayCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var learningPathway = EntityProvider.GetLearningPathway();
        learningWorld.LearningPathways.Add(learningPathway);
        Action<LearningWorld> mappingAction = world => { };

        // Act
        var result = _factory.GetDeleteCommand(learningWorld, learningPathway, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<DeleteLearningPathWay>());
        var resultCasted = result as DeleteLearningPathWay;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.LearningPathway, Is.EqualTo(learningPathway));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetDragCommand_WithLearningObjectAndOldAndNewPositions_ReturnsDragObjectInPathWayCommand()
    {
        // Arrange
        var learningObject = EntityProvider.GetLearningSpace();
        var oldPositionX = 0.0;
        var oldPositionY = 0.0;
        var newPositionX = 10.0;
        var newPositionY = 20.0;
        Action<IObjectInPathWay> mappingAction = obj => { };

        // Act
        var result = _factory.GetDragCommand(learningObject, oldPositionX, oldPositionY, newPositionX, newPositionY,
            mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<DragObjectInPathWay>());
        var resultCasted = result as DragObjectInPathWay;
        Assert.That(resultCasted!.LearningObject, Is.EqualTo(learningObject));
        Assert.That(resultCasted.OldPositionX, Is.EqualTo(oldPositionX));
        Assert.That(resultCasted.OldPositionY, Is.EqualTo(oldPositionY));
        Assert.That(resultCasted.NewPositionX, Is.EqualTo(newPositionX));
        Assert.That(resultCasted.NewPositionY, Is.EqualTo(newPositionY));
        Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
    }
}