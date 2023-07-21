using BusinessLogic.Commands.Space;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands.Space;

[TestFixture]

public class DeleteLearningSpaceUt
{
    [Test]
    public void Execute_DeletesLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f")
        {
            UnsavedChanges = false
        };
        var space1 = new LearningSpace("a", "d", "e", 4, Theme.Campus)
        {
            UnsavedChanges = false
        };
        var space2 = new LearningSpace("g", "j", "k", 5, Theme.Campus)
        {
            UnsavedChanges = false
        };
        var space3 = new LearningSpace("g", "j", "k", 5, Theme.Campus)
        {
            UnsavedChanges = false
        };
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        world.LearningSpaces.Add(space3);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var logger = Substitute.For<ILogger<SpaceCommandFactory>>();
        
        world.LearningPathways.Add(new LearningPathway(space1, space2));
        space1.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(space1);
        world.LearningPathways.Add(new LearningPathway(space2, space3));
        space2.OutBoundObjects.Add(space3);
        space3.InBoundObjects.Add(space2);

        var command = new DeleteLearningSpace(world, space2, mappingAction, logger);
        
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Does.Contain(space2));
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnsavedChanges, Is.False);
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.LearningPathways, Has.Count.EqualTo(0));
            Assert.That(world.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new LearningSpace("g", "j", "k", 5, Theme.Campus);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var logger = Substitute.For<ILogger<SpaceCommandFactory>>();

        var command = new DeleteLearningSpace(world,space, mappingAction, logger);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesDeleteLearningSpace()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f")
        {
            UnsavedChanges = false
        };
        var space1 = new LearningSpace("g", "j", "k", 5, Theme.Campus)
        {
            UnsavedChanges = false
        };
        var space2 = new LearningSpace("l", "o", "p", 7, Theme.Campus)
        {
            UnsavedChanges = false
        };
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        bool actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;
        var logger = Substitute.For<ILogger<SpaceCommandFactory>>();
        
        var command = new DeleteLearningSpace(world, space1, mappingAction, logger);
        
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnsavedChanges, Is.False);
        });
        
        command.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
            
            Assert.That(actionWasInvoked);
            Assert.That(world.UnsavedChanges, Is.True);
        });
        
        actionWasInvoked = false;
        
        command.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnsavedChanges, Is.False);
        });
        
        actionWasInvoked = false;
        
        command.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
            
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnsavedChanges, Is.True);
        });
    }
}