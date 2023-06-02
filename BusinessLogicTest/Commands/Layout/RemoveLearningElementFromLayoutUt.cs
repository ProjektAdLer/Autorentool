using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
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
        var space = EntityProvider.GetLearningSpace(false);
        world.LearningSpaces.Add(space);
        var content = EntityProvider.GetFileContent();
        var element = EntityProvider.GetLearningElement(false);
        space.LearningSpaceLayout.LearningElements[2] = element;


        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new RemoveLearningElementFromLayout(world, space, element, mappingAction);

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
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = EntityProvider.GetLearningWorld();
        var space = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X20_6L);
        world.LearningSpaces.Add(space);
        var content = EntityProvider.GetFileContent();
        var element = EntityProvider.GetLearningElement();
        space.LearningSpaceLayout.LearningElements[2] = element;


        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new RemoveLearningElementFromLayout(world, space, element, mappingAction);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_mementoWorld is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesMovingLearningElement()
    {
        var world = EntityProvider.GetLearningWorld();
        var space = EntityProvider.GetLearningSpace(false, floorPlan: FloorPlanEnum.R_20X20_6L);
        world.LearningSpaces.Add(space);
        var content = EntityProvider.GetFileContent();
        var element = EntityProvider.GetLearningElement(false);
        space.LearningSpaceLayout.LearningElements[2] = element;


        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new RemoveLearningElementFromLayout(world, space, element, mappingAction);

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