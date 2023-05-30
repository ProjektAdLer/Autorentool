using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using NUnit.Framework;
using Shared;

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
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, parent,
            workload: 8, points: 9, positionX: 17f, positionY: 29f)
        {
            UnsavedChanges = false
        };
        parent.LearningSpaceLayout.LearningElements[0] = element;


        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromLayout(parent, element, 2, mappingAction);

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
        var parent = new LearningSpace("sn", "sd", "sg", 5, Theme.Campus,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.R_20X20_6L))
        {
            UnsavedChanges = false
        };
        var content = new FileContent("cn", "ct", "cf");
        var element1 = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, parent,
            workload: 8, points: 9, positionX: 17f, positionY: 29f)
        {
            UnsavedChanges = false
        };
        var element2 = new LearningElement("en2", content, "ed2", "eg2", LearningElementDifficultyEnum.Medium, parent,
            workload: 8, points: 9, positionX: 17f, positionY: 29f)
        {
            UnsavedChanges = false
        };
        parent.LearningSpaceLayout.LearningElements[0] = element1;
        parent.LearningSpaceLayout.LearningElements[2] = element2;


        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromLayout(parent, element1, 2, mappingAction);

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
        var parent = new LearningSpace("sn", "sd", "sg", 5, Theme.Campus,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.R_20X20_6L));
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, parent, workload: 8, points: 9, positionX: 17f, positionY: 29f);
        parent.LearningSpaceLayout.LearningElements[0] = element;


        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromLayout(parent, element, 2, mappingAction);

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
        var parent = new LearningSpace("sn", "sd", "sg", 5, Theme.Campus,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.R_20X20_6L))
        {
            UnsavedChanges = false
        };
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, parent,
            workload: 8, points: 9, positionX: 17f, positionY: 29f)
        {
            UnsavedChanges = false
        };
        parent.LearningSpaceLayout.LearningElements[0] = element;


        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromLayout(parent, element, 2, mappingAction);

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
        var parent = new LearningSpace("sn", "sd", "sg", 5, Theme.Campus,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.R_20X20_6L))
        {
            UnsavedChanges = false
        };
        var content = new FileContent("cn", "ct", "cf");
        var element1 = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, parent,
            workload: 8, points: 9, positionX: 17f, positionY: 29f)
        {
            UnsavedChanges = false
        };
        var element2 = new LearningElement("en2", content, "ed2", "eg2", LearningElementDifficultyEnum.Medium, parent,
            workload: 8, points: 9, positionX: 17f, positionY: 29f)
        {
            UnsavedChanges = false
        };
        parent.LearningSpaceLayout.LearningElements[0] = element1;
        parent.LearningSpaceLayout.LearningElements[2] = element2;


        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromLayout(parent, element1, 2, mappingAction);

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