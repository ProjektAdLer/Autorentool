using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Layout;

[TestFixture]
public class RemoveLearningElementFromLayoutUt
{
    [Test]
    public void DragLearningElementFromSlotToUnplaced_Execute_MovesLearningElementToUnplaced()
    {
        var world = EntityProvider.GetLearningWorld();
        var space = EntityProvider.GetLearningSpace();
        world.LearningSpaces.Add(space);
        var element = EntityProvider.GetLearningElement();
        space.LearningSpaceLayout.LearningElements[2] = element;

        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new RemoveLearningElementFromLayout(world, space, element, mappingAction,
            new NullLogger<RemoveLearningElementFromLayout>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(0));
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
            Assert.That(space.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(0));
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void Undo_MementoWorldIsNull_ThrowsException()
    {
        var world = EntityProvider.GetLearningWorld();
        var space = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X20_6L);
        world.LearningSpaces.Add(space);
        var element = EntityProvider.GetLearningElement();
        space.LearningSpaceLayout.LearningElements[2] = element;

        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new RemoveLearningElementFromLayout(world, space, element, mappingAction,
            new NullLogger<RemoveLearningElementFromLayout>());

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
        var space = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X20_6L);
        world.LearningSpaces.Add(space);
        var element = EntityProvider.GetLearningElement();
        space.LearningSpaceLayout.LearningElements[2] = element;

        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new RemoveLearningElementFromLayout(world, space, element, mappingAction,
            new NullLogger<RemoveLearningElementFromLayout>());

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
        var space = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X20_6L);
        world.LearningSpaces.Add(space);
        var element = EntityProvider.GetLearningElement();
        space.LearningSpaceLayout.LearningElements[2] = element;

        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new RemoveLearningElementFromLayout(world, space, element, mappingAction,
            new NullLogger<RemoveLearningElementFromLayout>());

        // Manually setting MementoWorld and MementoSpace to bypass the first check
        var mementoWorld = world.GetMemento();
        command.MementoWorld = mementoWorld;
        var mementoSpace = space.GetMemento();
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
        var world = EntityProvider.GetLearningWorld();
        var space = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X20_6L);
        world.LearningSpaces.Add(space);
        var element = EntityProvider.GetLearningElement();
        space.LearningSpaceLayout.LearningElements[2] = element;

        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new RemoveLearningElementFromLayout(world, space, element, mappingAction,
            new NullLogger<RemoveLearningElementFromLayout>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(0));
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
            Assert.That(space.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(0));
            Assert.That(space.UnsavedChanges, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(0));
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
            Assert.That(space.UnsavedChanges, Is.False);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(0));
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }
}