using BusinessLogic.Commands.Pathway;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared.Theme;
using TestHelpers;

namespace BusinessLogicTest.Commands.Pathway;

[TestFixture]
public class DeleteLearningPathWayUt
{
    [Test]
    // ANF-ID: [AHO13]
    public void Execute_DeletesLearningPathWay()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f", WorldTheme.CampusAschaffenburg)
        {
            UnsavedChanges = false
        };
        var space1 = new LearningSpace("z", "w", 5, SpaceTheme.LearningArea,
            EntityProvider.GetLearningOutcomeCollection())
        {
            UnsavedChanges = false
        };
        var space2 = new LearningSpace("l", "o", 3, SpaceTheme.LearningArea,
            EntityProvider.GetLearningOutcomeCollection())
        {
            UnsavedChanges = false
        };
        var space3 = new LearningSpace("l", "o", 3, SpaceTheme.LearningArea,
            EntityProvider.GetLearningOutcomeCollection())
        {
            UnsavedChanges = false
        };
        var space4 = new LearningSpace("l", "o", 3, SpaceTheme.LearningArea,
            EntityProvider.GetLearningOutcomeCollection())
        {
            UnsavedChanges = false
        };
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        world.LearningSpaces.Add(space3);
        world.LearningSpaces.Add(space4);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var pathway1 = new LearningPathway(space1, space2);
        space1.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(space1);
        var pathway2 = new LearningPathway(space3, space4);
        space3.OutBoundObjects.Add(space4);
        space4.InBoundObjects.Add(space3);

        world.LearningPathways.Add(pathway1);
        world.LearningPathways.Add(pathway2);

        var command =
            new DeleteLearningPathWay(world, pathway2, mappingAction, new NullLogger<DeleteLearningPathWay>());

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningPathways, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.That(world.LearningPathways, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningPathways, Does.Not.Contain(pathway2));
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f", WorldTheme.CampusAschaffenburg);
        var space1 = new LearningSpace("z", "w", 5, SpaceTheme.LearningArea,
            EntityProvider.GetLearningOutcomeCollection());
        var space2 = new LearningSpace("l", "o", 3, SpaceTheme.LearningArea,
            EntityProvider.GetLearningOutcomeCollection());
        var pathWay = new LearningPathway(space1, space2);
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        world.LearningPathways.Add(pathWay);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new DeleteLearningPathWay(world, pathWay, mappingAction, new NullLogger<DeleteLearningPathWay>());

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesCreateLearningPathWay()
    {
        var world = new LearningWorld("a", "b", "c", "d", "e", "f",WorldTheme.CampusAschaffenburg)
        {
            UnsavedChanges = false
        };
        var space1 = new LearningSpace("z", "w", 5, SpaceTheme.LearningArea,
            EntityProvider.GetLearningOutcomeCollection())
        {
            UnsavedChanges = false
        };
        var space2 = new LearningSpace("l", "o", 3, SpaceTheme.LearningArea,
            EntityProvider.GetLearningOutcomeCollection())
        {
            UnsavedChanges = false
        };
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var pathway = new LearningPathway(space1, space2);

        world.LearningPathways.Add(pathway);

        var command = new DeleteLearningPathWay(world, pathway, mappingAction, new NullLogger<DeleteLearningPathWay>());

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningPathways, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            actionWasInvoked = false;
            Assert.That(world.LearningPathways, Has.Count.EqualTo(0));
            Assert.That(world.UnsavedChanges, Is.True);
        });

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningPathways, Has.Count.EqualTo(1));
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnsavedChanges, Is.False);
        });

        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.LearningPathways, Has.Count.EqualTo(0));
            Assert.That(world.UnsavedChanges, Is.True);
        });
    }
}