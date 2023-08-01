using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Layout;

[TestFixture]
public class PlaceLearningElementInLayoutFromLayoutUt
{
    [Test]
    public void MoveLearningElementToEmptySlot_Execute_MovesLearningElement()
    {
        var parent = new LearningSpace("sn", "sd", "sg", 5, Theme.Campus,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.R_20X20_6L))
        {
            UnsavedChanges = false
        };
        var element = EntityProvider.GetLearningElement(parent: parent, unsavedChanges: false);
        parent.LearningSpaceLayout.LearningElements[0] = element;

        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromLayout(parent, element, 2, mappingAction,
            new NullLogger<PlaceLearningElementInLayoutFromLayout>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element));
            Assert.That(parent.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
            Assert.That(parent.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void MoveLearningElementToAssignedSlot_Execute_SwitchesLearningElements()
    {
        var parent = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X20_6L, unsavedChanges: false);
        var element1 = EntityProvider.GetLearningElement(parent: parent, unsavedChanges: false);
        var element2 = EntityProvider.GetLearningElement(parent: parent, unsavedChanges: false, append: "2");
        parent.LearningSpaceLayout.LearningElements[0] = element1;
        parent.LearningSpaceLayout.LearningElements[2] = element2;

        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromLayout(parent, element1, 2, mappingAction,
            new NullLogger<PlaceLearningElementInLayoutFromLayout>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element2));
            Assert.That(parent.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element2));
            Assert.That(parent.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var parent = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X20_6L);
        var element = EntityProvider.GetLearningElement(parent: parent);
        parent.LearningSpaceLayout.LearningElements[0] = element;

        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromLayout(parent, element, 2, mappingAction,
            new NullLogger<PlaceLearningElementInLayoutFromLayout>());

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_mementoLayout is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesMovingLearningElement()
    {
        var parent = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X20_6L, unsavedChanges: false);
        var element = EntityProvider.GetLearningElement(parent: parent, unsavedChanges: false);
        parent.LearningSpaceLayout.LearningElements[0] = element;

        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromLayout(parent, element, 2, mappingAction,
            new NullLogger<PlaceLearningElementInLayoutFromLayout>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element));
            Assert.That(parent.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
            Assert.That(parent.UnsavedChanges, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element));
            Assert.That(parent.UnsavedChanges, Is.False);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
            Assert.That(parent.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesSwitchingLearningElements()
    {
        var parent = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X20_6L, unsavedChanges: false);
        var element1 = EntityProvider.GetLearningElement(parent: parent, unsavedChanges: false);
        var element2 = EntityProvider.GetLearningElement(parent: parent, unsavedChanges: false, append: "2");
        parent.LearningSpaceLayout.LearningElements[0] = element1;
        parent.LearningSpaceLayout.LearningElements[2] = element2;

        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromLayout(parent, element1, 2, mappingAction,
            new NullLogger<PlaceLearningElementInLayoutFromLayout>());

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element2));
            Assert.That(parent.UnsavedChanges, Is.False);
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element2));
            Assert.That(parent.UnsavedChanges, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element2));
            Assert.That(parent.UnsavedChanges, Is.False);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element2));
            Assert.That(parent.UnsavedChanges, Is.True);
        });
    }
}