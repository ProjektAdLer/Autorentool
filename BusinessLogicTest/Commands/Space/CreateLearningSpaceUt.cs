using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared.Theme;
using TestHelpers;

namespace BusinessLogicTest.Commands.Space;

[TestFixture]
public class CreateLearningSpaceUt
{
    [Test]
    // ANF-ID: [AWA0001]
    public void Execute_CreatesLearningSpace()
    {
        var world = EntityProvider.GetLearningWorld();
        var name = "space1";
        var description = "space for learning";
        var learningOutcomeCollection = EntityProvider.GetLearningOutcomeCollection();
        var requiredPoints = 10;
        var theme = SpaceTheme.LearningArea;
        var positionX = 1;
        var positionY = 2;
        var topic = new BusinessLogic.Entities.Topic("topic1");
        world.Topics.Add(topic);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningSpace(world, name, description, learningOutcomeCollection,
            requiredPoints, theme,
            positionX,
            positionY, topic, mappingAction, new NullLogger<CreateLearningSpace>());

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
        var space = world.LearningSpaces.First();
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Description, Is.EqualTo("space for learning"));
            Assert.That(space.LearningOutcomeCollection, Is.EqualTo(learningOutcomeCollection));
            Assert.That(space.RequiredPoints, Is.EqualTo(10));
            Assert.That(space.SpaceTheme, Is.EqualTo(SpaceTheme.LearningArea));
            Assert.That(space.PositionX, Is.EqualTo(1));
            Assert.That(space.PositionY, Is.EqualTo(2));
            Assert.That(space.AssignedTopic, Is.EqualTo(topic));
        });
    }

    [Test]
    // ANF-ID: [AWA0001]
    public void Execute_AddsLearningSpaceAndSetAsSelectedLearningObject()
    {
        var world = EntityProvider.GetLearningWorld();
        var space = new LearningSpace("z", "w", 5, SpaceTheme.LearningArea);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningSpace(world, space, mappingAction, new NullLogger<CreateLearningSpace>());

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
        Assert.That(world.LearningSpaces.First(), Is.EqualTo(space));
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = EntityProvider.GetLearningWorld();
        var name = "space1";
        var description = "space for learning";
        var topic = new BusinessLogic.Entities.Topic("abc");
        var requiredPoints = 10;
        var positionX = 1;
        var positionY = 2;
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningSpace(world, name, description, EntityProvider.GetLearningOutcomeCollection(),
            requiredPoints,
            SpaceTheme.LearningArea, positionX, positionY, topic, mappingAction,
            new NullLogger<CreateLearningSpace>());

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }


    [Test]
    public void UndoRedo_UndoesAndRedoesCreateLearningSpace()
    {
        var world = EntityProvider.GetLearningWorld();
        var space = new LearningSpace("g", "j", 5, SpaceTheme.LearningArea);
        world.LearningSpaces.Add(space);
        var name = "space1";
        var description = "space for learning";
        var topic = new BusinessLogic.Entities.Topic("abc");
        var requiredPoints = 10;
        var positionX = 1;
        var positionY = 2;
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningSpace(world, name, description, EntityProvider.GetLearningOutcomeCollection(),
            requiredPoints,
            SpaceTheme.LearningArea, positionX,
            positionY, topic, mappingAction, new NullLogger<CreateLearningSpace>());

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.True);
        });
    }
}