using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Layout;

[TestFixture]
public class PlaceLearningElementInLayoutFromUnplacedUt
{
    [Test]
    public void DragLearningElementFromUnplacedToFree_Execute_MovesLearningElementToSlot()
    {
        var world = EntityProvider.GetLearningWorld(unsavedChanges: false);
        var space = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X20_6L, unsavedChanges: false);
        world.LearningSpaces.Add(space);
        var element = EntityProvider.GetLearningElement(unsavedChanges: false);
        world.UnplacedLearningElements.Add(element);

        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromUnplaced(world, space, element, 2, mappingAction,
            new NullLogger<PlaceLearningElementInLayoutFromUnplaced>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(0));
            Assert.That(world.UnsavedChanges, Is.False);
            Assert.That(space.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(0));
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
            Assert.That(world.UnsavedChanges, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void DragLearningElementToAssignedSlot_Execute_SwitchesLearningElements()
    {
        var world = EntityProvider.GetLearningWorld(unsavedChanges: false);
        var space = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X20_6L, unsavedChanges: false);
        var element1 = EntityProvider.GetLearningElement(parent: null, unsavedChanges: false);
        var element2 = EntityProvider.GetLearningElement(parent: space, unsavedChanges: false);
        world.LearningSpaces.Add(space);
        world.UnplacedLearningElements.Add(element1);
        space.LearningSpaceLayout.LearningElements[2] = element2;

        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromUnplaced(world, space, element1, 2, mappingAction,
            new NullLogger<PlaceLearningElementInLayoutFromUnplaced>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element1), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element2));
            Assert.That(world.UnsavedChanges, Is.False);
            Assert.That(space.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element2), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element1));
            Assert.That(world.UnsavedChanges, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void Undo_MementoWorldIsNull_ThrowsException()
    {
        var world = EntityProvider.GetLearningWorld();
        var space = EntityProvider.GetLearningSpace();
        world.LearningSpaces.Add(space);
        var element = EntityProvider.GetLearningElement(parent: null);
        world.UnplacedLearningElements.Add(element);

        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromUnplaced(world, space, element, 2, mappingAction,
            new NullLogger<PlaceLearningElementInLayoutFromUnplaced>());

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("MementoWorld is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void Undo_MementoSpaceIsNull_ThrowsException()
    {
        var world = EntityProvider.GetLearningWorld();
        var space = EntityProvider.GetLearningSpace();
        world.LearningSpaces.Add(space);
        var element = EntityProvider.GetLearningElement(parent: null);
        world.UnplacedLearningElements.Add(element);

        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromUnplaced(world, space, element, 2, mappingAction,
            new NullLogger<PlaceLearningElementInLayoutFromUnplaced>());

        // Manually setting MementoWorld to bypass the first check
        var mementoWorld = world.GetMemento();
        command.MementoWorld = mementoWorld;

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("MementoSpace is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void Undo_MementoSpaceLayoutIsNull_ThrowsException()
    {
        var world = EntityProvider.GetLearningWorld();
        var space = EntityProvider.GetLearningSpace();
        world.LearningSpaces.Add(space);
        var element = EntityProvider.GetLearningElement(parent: null);
        world.UnplacedLearningElements.Add(element);

        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromUnplaced(world, space, element, 2, mappingAction,
            new NullLogger<PlaceLearningElementInLayoutFromUnplaced>());

        // Manually setting MementoWorld and MementoSpace to bypass the first two checks
        var mementoWorld = world.GetMemento();
        var mementoSpace = space.GetMemento();

        command.MementoWorld = mementoWorld;
        command.MementoSpace = mementoSpace;

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_mementoSpaceLayout is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesMovingLearningElement()
    {
        var world = EntityProvider.GetLearningWorld(unsavedChanges: false);
        var space = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X20_6L, unsavedChanges: false);
        var element = EntityProvider.GetLearningElement(parent: null, unsavedChanges: false);
        world.LearningSpaces.Add(space);
        world.UnplacedLearningElements.Add(element);

        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromUnplaced(world, space, element, 2, mappingAction,
            new NullLogger<PlaceLearningElementInLayoutFromUnplaced>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(0));
            Assert.That(world.UnsavedChanges, Is.False);
            Assert.That(space.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(0));
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
            Assert.That(world.UnsavedChanges, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(0));
            Assert.That(world.UnsavedChanges, Is.False);
            Assert.That(space.UnsavedChanges, Is.False);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(0));
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
            Assert.That(world.UnsavedChanges, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesSwitchingLearningElements()
    {
        var world = EntityProvider.GetLearningWorld(unsavedChanges: false);
        var space = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X20_6L, unsavedChanges: false);
        var element = EntityProvider.GetLearningElement(parent: null, unsavedChanges: false);
        var element2 = EntityProvider.GetLearningElement(parent: space, unsavedChanges: false, append: "2");
        world.LearningSpaces.Add(space);
        world.UnplacedLearningElements.Add(element);
        space.LearningSpaceLayout.LearningElements[2] = element2;

        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromUnplaced(world, space, element, 2, mappingAction,
            new NullLogger<PlaceLearningElementInLayoutFromUnplaced>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element2));
            Assert.That(world.UnsavedChanges, Is.False);
            Assert.That(space.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element2), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
            Assert.That(world.UnsavedChanges, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element2));
            Assert.That(world.UnsavedChanges, Is.False);
            Assert.That(space.UnsavedChanges, Is.False);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element2), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
            Assert.That(world.UnsavedChanges, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }
}