using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands.Space;


[TestFixture]

public class CreateLearningSpaceUt
{
    [Test]
    public void Execute_CreatesLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var name = "space1";
        var description = "space for learning";
        var goals = "learning";
        var requiredPoints = 10;
        var theme = Theme.Campus;
        var positionX = 1;
        var positionY = 2;
        var topic = new BusinessLogic.Entities.Topic("topic1");
        world.Topics.Add(topic);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var logger = Substitute.For<ILogger<SpaceCommandFactory>>();

        var command = new CreateLearningSpace(world, name, description, goals, requiredPoints, theme, positionX, positionY, topic, mappingAction,logger);
        
        Assert.IsEmpty(world.LearningSpaces);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var space = world.LearningSpaces.First();
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo("space1"));
            Assert.That(space.Description, Is.EqualTo("space for learning"));
            Assert.That(space.Goals, Is.EqualTo("learning"));
            Assert.That(space.RequiredPoints, Is.EqualTo(10));
            Assert.That(space.Theme, Is.EqualTo(Theme.Campus));
            Assert.That(space.PositionX, Is.EqualTo(1));
            Assert.That(space.PositionY, Is.EqualTo(2));
            Assert.That(space.AssignedTopic, Is.EqualTo(topic));
        });
    }
    
    [Test]
    public void Execute_AddsLearningSpaceAndSetAsSelectedLearningObject()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new LearningSpace("z","w","v", 5, Theme.Campus);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var logger = Substitute.For<ILogger<SpaceCommandFactory>>();

        var command = new CreateLearningSpace(world, space, mappingAction, logger);
        
        Assert.IsEmpty(world.LearningSpaces);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(world.LearningSpaces.First(), Is.EqualTo(space)); }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var name = "space1";
        var description = "space for learning";
        var goals = "learning";
        var topic = new BusinessLogic.Entities.Topic("abc");
        var requiredPoints = 10;
        var positionX = 1;
        var positionY = 2;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var logger = Substitute.For<ILogger<SpaceCommandFactory>>();

        var command = new CreateLearningSpace(world, name, description, goals, requiredPoints, Theme.Campus, positionX, positionY, topic, mappingAction, logger);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    

    [Test]
    public void UndoRedo_UndoesAndRedoesCreateLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new LearningSpace("g", "j", "k", 5, Theme.Campus);
        world.LearningSpaces.Add(space);
        var name = "space1";
        var description = "space for learning";
        var goals = "learning";
        var topic = new BusinessLogic.Entities.Topic("abc");
        var requiredPoints = 10;
        var positionX = 1;
        var positionY = 2;
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var logger = Substitute.For<ILogger<SpaceCommandFactory>>();
        
        var command = new CreateLearningSpace(world, name, description, goals, requiredPoints, Theme.Campus, positionX, positionY, topic, mappingAction, logger);
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
        Assert.IsTrue(actionWasInvoked);
    }
}