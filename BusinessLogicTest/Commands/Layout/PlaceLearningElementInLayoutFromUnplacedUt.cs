using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands.Layout;

[TestFixture]
public class PlaceLearningElementInLayoutFromUnplacedUt
{
    [Test]
    public void DragLearningElementFromUnplacedToFree_Execute_MovesLearningElementToSlot()
    {
        var world = new LearningWorld("wn", "wsn", "wa", "wl", "wd", "wg")
        {
            UnsavedChanges = false
        };
        var space = new LearningSpace("sn", "sd", "sg", 5, Theme.Campus,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.Rectangle2X2))
        {
            UnsavedChanges = false
        };
        world.LearningSpaces.Add(space);
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, null,
            workload: 8, points: 9, positionX: 17f, positionY: 29f)
        {
            UnsavedChanges = false
        };
        world.UnplacedLearningElements.Add(element);


        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromUnplaced(world, space, element, 2, mappingAction);

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
        var world = new LearningWorld("wn", "wsn", "wa", "wl", "wd", "wg")
        {
            UnsavedChanges = false
        };
        var space = new LearningSpace("sn", "sd", "sg", 5, Theme.Campus,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.Rectangle2X2))
        {
            UnsavedChanges = false
        };
        var content = new FileContent("cn", "ct", "cf");
        var element1 = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, null,
            workload: 8, points: 9, positionX: 17f, positionY: 29f)
        {
            UnsavedChanges = false
        };
        var element2 = new LearningElement("en2", content, "ed2", "eg2", LearningElementDifficultyEnum.Medium, space,
            workload: 8, points: 9, positionX: 17f, positionY: 29f)
        {
            UnsavedChanges = false
        };
        world.LearningSpaces.Add(space);
        world.UnplacedLearningElements.Add(element1);
        space.LearningSpaceLayout.LearningElements[2] = element2;


        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromUnplaced(world, space, element1, 2, mappingAction);

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
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("wn", "wsn", "wa", "wl", "wd", "wg");
        var space = new LearningSpace("sn", "sd", "sg", 5, Theme.Campus,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.Rectangle2X2));
        world.LearningSpaces.Add(space);
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, null, workload: 8, points: 9, positionX: 17f, positionY: 29f);
        world.UnplacedLearningElements.Add(element);


        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromUnplaced(world, space, element, 2, mappingAction);

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
        var world = new LearningWorld("wn", "wsn", "wa", "wl", "wd", "wg")
        {
            UnsavedChanges = false
        };
        var space = new LearningSpace("sn", "sd", "sg", 5, Theme.Campus,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.Rectangle2X2))
        {
            UnsavedChanges = false
        };
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, null,
            workload: 8, points: 9, positionX: 17f, positionY: 29f)
        {
            UnsavedChanges = false
        };
        world.LearningSpaces.Add(space);
        world.UnplacedLearningElements.Add(element);


        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromUnplaced(world, space, element, 2, mappingAction);

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
        var world = new LearningWorld("wn", "wsn", "wa", "wl", "wd", "wg")
        {
            UnsavedChanges = false
        };
        var space = new LearningSpace("sn", "sd", "sg", 5, Theme.Campus,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.Rectangle2X2))
        {
            UnsavedChanges = false
        };
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, null,
            workload: 8, points: 9, positionX: 17f, positionY: 29f)
        {
            UnsavedChanges = false
        };
        var element2 = new LearningElement("en2", content, "ed2", "eg2", LearningElementDifficultyEnum.Medium, space,
            workload: 8, points: 9, positionX: 17f, positionY: 29f)
        {
            UnsavedChanges = false
        };
        world.LearningSpaces.Add(space);
        world.UnplacedLearningElements.Add(element);
        space.LearningSpaceLayout.LearningElements[2] = element2;


        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceLearningElementInLayoutFromUnplaced(world, space, element, 2, mappingAction);

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