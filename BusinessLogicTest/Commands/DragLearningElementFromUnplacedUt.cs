using BusinessLogic.Commands;
using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class DragLearningElementFromUnplacedUt
{
    [Test]
    public void DragLearningElementFromUnplacedToFree_Execute_MovesLearningElementToSlot()
    {
        var world = new LearningWorld("wn", "wsn", "wa", "wl", "wd", "wg");
        var space = new LearningSpace("sn", "ssn", "sa", "sd", "sg", 5,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.Rectangle2X2));
        world.LearningSpaces.Add(space);
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, null, workload: 8, points: 9, positionX: 17f, positionY: 29f);
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
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(0));
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
        });
    }

    [Test]
    public void DragLearningElementToAssignedSlot_Execute_SwitchesLearningElements()
    {
        var world = new LearningWorld("wn", "wsn", "wa", "wl", "wd", "wg");
        var space = new LearningSpace("sn", "ssn", "sa", "sd", "sg", 5,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.Rectangle2X2));
        world.LearningSpaces.Add(space);
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, null, workload: 8, points: 9, positionX: 17f, positionY: 29f);
        world.UnplacedLearningElements.Add(element);
        var element2 = new LearningElement("en2", content, "ed2", "eg2", LearningElementDifficultyEnum.Medium, space, workload: 8, points: 9, positionX: 17f, positionY: 29f);
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
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element2), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("wn", "wsn", "wa", "wl", "wd", "wg");
        var space = new LearningSpace("sn", "ssn", "sa", "sd", "sg", 5,
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
            Assert.That(ex!.Message, Is.EqualTo("_mementoWorld or _mementoSpaceLayout is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesMovingLearningElement()
    {
        var world = new LearningWorld("wn", "wsn", "wa", "wl", "wd", "wg");
        var space = new LearningSpace("sn", "ssn", "sa", "sd", "sg", 5,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.Rectangle2X2));
        world.LearningSpaces.Add(space);
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, null, workload: 8, points: 9, positionX: 17f, positionY: 29f);
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
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(0));
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(0));
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(0));
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesSwitchingLearningElements()
    {
        var world = new LearningWorld("wn", "wsn", "wa", "wl", "wd", "wg");
        var space = new LearningSpace("sn", "ssn", "sa", "sd", "sg", 5,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.Rectangle2X2));
        world.LearningSpaces.Add(space);
        var content = new FileContent("cn", "ct", "cf");
        var element = new LearningElement("en", content, "ed", "eg", LearningElementDifficultyEnum.Medium, null, workload: 8, points: 9, positionX: 17f, positionY: 29f);
        world.UnplacedLearningElements.Add(element);
        var element2 = new LearningElement("en2", content, "ed2", "eg2", LearningElementDifficultyEnum.Medium, space, workload: 8, points: 9, positionX: 17f, positionY: 29f);
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
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element2), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
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
        });
    }
}