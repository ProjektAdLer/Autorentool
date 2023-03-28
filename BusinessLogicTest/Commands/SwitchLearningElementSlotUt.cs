using BusinessLogic.Commands;
using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class SwitchLearningElementSlotUt
{
    [Test]
    public void MoveLearningElementToEmptySlot_Execute_MovesLearningElement()
    {
        var parent = new LearningSpace("sn", "ssn", "sa", "sd", "sg", 5,
            new LearningSpaceLayout(new ILearningElement[4], FloorPlanEnum.Rectangle2X2));
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, parent, workload: 8, points: 9, positionX: 17f, positionY: 29f);
        parent.LearningSpaceLayout.LearningElements[0] = element;


        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromLayout(parent, element, 2, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
        });
    }

    [Test]
    public void MoveLearningElementToAssignedSlot_Execute_SwitchesLearningElements()
    {
        var parent = new LearningSpace("sn", "ssn", "sa", "sd", "sg", 5,
            new LearningSpaceLayout(new ILearningElement[4], FloorPlanEnum.Rectangle2X2));
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, parent, workload: 8, points: 9, positionX: 17f, positionY: 29f);
        var element2 = new LearningElement("en2", content, "ed2", "eg2", LearningElementDifficultyEnum.Medium, parent, workload: 8, points: 9, positionX: 17f, positionY: 29f);
        parent.LearningSpaceLayout.LearningElements[0] = element;
        parent.LearningSpaceLayout.LearningElements[2] = element2;


        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromLayout(parent, element, 2, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element2));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element2));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var parent = new LearningSpace("sn", "ssn", "sa", "sd", "sg", 5,
            new LearningSpaceLayout(new ILearningElement[4], FloorPlanEnum.Rectangle2X2));
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, parent, workload: 8, points: 9, positionX: 17f, positionY: 29f);
        parent.LearningSpaceLayout.LearningElements[0] = element;


        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromLayout(parent, element, 2, mappingAction);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesMovingLearningElement()
    {
        var parent = new LearningSpace("sn", "ssn", "sa", "sd", "sg", 5,
            new LearningSpaceLayout(new ILearningElement[4], FloorPlanEnum.Rectangle2X2));
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, parent, workload: 8, points: 9, positionX: 17f, positionY: 29f);
        parent.LearningSpaceLayout.LearningElements[0] = element;


        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromLayout(parent, element, 2, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element));
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesSwitchingLearningElements()
    {
        var parent = new LearningSpace("sn", "ssn", "sa", "sd", "sg", 5,
            new LearningSpaceLayout(new ILearningElement[4], FloorPlanEnum.Rectangle2X2));
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, parent, workload: 8, points: 9, positionX: 17f, positionY: 29f);
        var element2 = new LearningElement("en2", content, "ed2", "eg2", LearningElementDifficultyEnum.Medium, parent, workload: 8, points: 9, positionX: 17f, positionY: 29f);
        parent.LearningSpaceLayout.LearningElements[0] = element;
        parent.LearningSpaceLayout.LearningElements[2] = element2;


        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromLayout(parent, element, 2, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element2));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element2));
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element2));
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(parent.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
            Assert.That(parent.LearningSpaceLayout.LearningElements[0], Is.EqualTo(element2));
        });
    }
}