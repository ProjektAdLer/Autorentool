using BusinessLogic.Commands.Pathway;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Pathway;

[TestFixture]
public class CreateLearningPathWayUt
{
    [Test]
    // ANF-ID: [AHO11]
    public void Execute_CreateLearningPathWay()
    {
        var world = EntityProvider.GetLearningWorld(unsavedChanges: false);
        var space1 = EntityProvider.GetLearningSpace(unsavedChanges: false);
        var space2 = EntityProvider.GetLearningSpace(unsavedChanges: false);
        var space3 = EntityProvider.GetLearningSpace(unsavedChanges: false);
        var pathWayCondition = EntityProvider.GetPathWayCondition(unsavedChanges: false);
        world.LearningSpaces.Add(space1);
        world.PathWayConditions.Add(pathWayCondition);
        world.LearningSpaces.Add(space2);
        world.LearningSpaces.Add(space3);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var pathway1 = new LearningPathway(space1, pathWayCondition);
        space1.OutBoundObjects.Add(pathWayCondition);
        pathWayCondition.InBoundObjects.Add(space1);
        var pathway2 = new LearningPathway(space2, space3);
        space2.OutBoundObjects.Add(space3);
        space3.InBoundObjects.Add(space2);

        world.LearningPathways.Add(pathway1);
        world.LearningPathways.Add(pathway2);

        var command = new CreateLearningPathWay(world, pathWayCondition, space2, mappingAction,
            new NullLogger<CreateLearningPathWay>());

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningPathways, Has.Count.EqualTo(2));
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(command.HasError, Is.False);
            Assert.That(world.LearningPathways, Has.Count.EqualTo(3));
            Assert.That(world.LearningPathways.Last().SourceObject, Is.EqualTo(pathWayCondition));
            Assert.That(world.LearningPathways.Last().TargetObject, Is.EqualTo(space2));
            Assert.That(world.UnsavedChanges, Is.True);
        });
    }

    [Test]
    // ANF-ID: [AHO11]
    public void Execute_LearningPathWayAlreadyExists_HasErrorIsTrue()
    {
        var world = EntityProvider.GetLearningWorld();
        var space1 = EntityProvider.GetLearningSpace();
        var space2 = EntityProvider.GetLearningSpace();
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var pathway = new LearningPathway(space1, space2);
        world.LearningPathways.Add(pathway);

        var command = new CreateLearningPathWay(world, space1, space2, mappingAction,
            new NullLogger<CreateLearningPathWay>());

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(command.HasError, Is.True);
            Assert.That(world.LearningPathways, Has.Count.EqualTo(1));
        });
    }

    [Test]
    // ANF-ID: [AHO11]
    public void Execute_SourceSpaceIsTargetSpace_HasErrorIsTrue()
    {
        var world = EntityProvider.GetLearningWorld();
        var space = EntityProvider.GetLearningSpace();
        world.LearningSpaces.Add(space);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command =
            new CreateLearningPathWay(world, space, space, mappingAction, new NullLogger<CreateLearningPathWay>());

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(command.HasError, Is.True);
            Assert.That(world.LearningPathways, Has.Count.EqualTo(0));
        });
    }

    [Test]
    // ANF-ID: [AHO11]
    public void Execute_TargetSpaceAlreadyHasInboundSpace_HasErrorIsTrue()
    {
        var world = EntityProvider.GetLearningWorld();
        var space0 = EntityProvider.GetLearningSpace();
        var space1 = EntityProvider.GetLearningSpace();
        var space2 = EntityProvider.GetLearningSpace();
        world.LearningSpaces.Add(space0);
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        var pathway = new LearningPathway(space0, space1);
        world.LearningPathways.Add(pathway);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningPathWay(world, space2, space1, mappingAction,
            new NullLogger<CreateLearningPathWay>());

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(command.HasError, Is.True);
            Assert.That(world.LearningPathways, Has.Count.EqualTo(1));
        });
    }

    [Test]
    // ANF-ID: [AHO11]
    public void Execute_PathWayIsCircular_HasErrorIsTrue()
    {
        var world = EntityProvider.GetLearningWorld();
        var space1 = EntityProvider.GetLearningSpace();
        var space2 = EntityProvider.GetLearningSpace();
        var space3 = EntityProvider.GetLearningSpace();
        var space4 = EntityProvider.GetLearningSpace();
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        world.LearningSpaces.Add(space3);
        world.LearningSpaces.Add(space4);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var pathway1 = new LearningPathway(space1, space2);
        space1.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(space1);
        var pathway2 = new LearningPathway(space2, space3);
        space2.OutBoundObjects.Add(space3);
        space3.InBoundObjects.Add(space2);
        var pathway3 = new LearningPathway(space3, space4);
        space3.OutBoundObjects.Add(space4);
        space4.InBoundObjects.Add(space3);

        world.LearningPathways.Add(pathway1);
        world.LearningPathways.Add(pathway2);
        world.LearningPathways.Add(pathway3);

        var command = new CreateLearningPathWay(world, space4, space1, mappingAction,
            new NullLogger<CreateLearningPathWay>());

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(command.HasError, Is.True);
            Assert.That(world.LearningPathways, Has.Count.EqualTo(3));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = EntityProvider.GetLearningWorld();
        var space1 = EntityProvider.GetLearningSpace();
        var space2 = EntityProvider.GetLearningSpace();
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningPathWay(world, space1, space2, mappingAction,
            new NullLogger<CreateLearningPathWay>());

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());

        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoDoesCreateLearningPathWay()
    {
        var world = EntityProvider.GetLearningWorld(unsavedChanges: false);
        var space1 = EntityProvider.GetLearningSpace(unsavedChanges: false);
        var space2 = EntityProvider.GetLearningSpace(unsavedChanges: false);
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new CreateLearningPathWay(world, space1, space2, mappingAction,
            new NullLogger<CreateLearningPathWay>());

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningPathways, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.LearningPathways, Has.Count.EqualTo(1));
            Assert.That(world.LearningPathways.First().SourceObject, Is.EqualTo(space1));
            Assert.That(world.LearningPathways.First().TargetObject, Is.EqualTo(space2));
            Assert.That(world.UnsavedChanges, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(world.LearningPathways, Is.Empty);
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnsavedChanges, Is.False);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            actionWasInvoked = false;
            Assert.That(world.LearningPathways, Has.Count.EqualTo(1));
            Assert.That(world.LearningPathways.First().SourceObject, Is.EqualTo(space1));
            Assert.That(world.LearningPathways.First().TargetObject, Is.EqualTo(space2));
            Assert.That(world.UnsavedChanges, Is.True);
        });
    }
}