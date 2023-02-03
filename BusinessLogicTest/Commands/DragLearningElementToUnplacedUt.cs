using BusinessLogic.Commands;
using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class DragLearningElementToUnplacedUt
{
    [Test]
    public void DragLearningElementFromSlotToUnplaced_Execute_MovesLearningElementToUnplaced()
    {
        var world = new LearningWorld("wn", "wsn", "wa", "wl", "wd", "wg");
        var space = new LearningSpace("sn", "ssn", "sa", "sd", "sg", 5,
            new LearningSpaceLayout(new ILearningElement[4], FloorPlanEnum.Rectangle2X2));
        world.LearningSpaces.Add(space);
        var content = new LearningContent("cn", "ct", "cf");
        var element = new LearningElement("en", "esn", content, "url",
            "ea", "ed", "eg", LearningElementDifficultyEnum.Medium, null, 8, 9, 17f, 29f);
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
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(0));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new LearningWorld("wn", "wsn", "wa", "wl", "wd", "wg");
        var space = new LearningSpace("sn", "ssn", "sa", "sd", "sg", 5,
            new LearningSpaceLayout(new ILearningElement[4], FloorPlanEnum.Rectangle2X2));
        world.LearningSpaces.Add(space);
        var content = new LearningContent("cn", "ct", "cf");
        var element = new LearningElement("en", "esn", content, "url",
            "ea", "ed", "eg", LearningElementDifficultyEnum.Medium, null, 8, 9, 17f, 29f);
        space.LearningSpaceLayout.LearningElements[2] = element;


        var actionWasInvoked = false;
        Action<LearningWorld> mappingAction = _ => actionWasInvoked = true;

        var command = new RemoveLearningElementFromLayout(world, space, element, mappingAction);

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
            new LearningSpaceLayout(new ILearningElement[4], FloorPlanEnum.Rectangle2X2));
        world.LearningSpaces.Add(space);
        var content = new LearningContent("cn", "ct", "cf");
        var element = new LearningElement("en", "esn", content, "url",
            "ea", "ed", "eg", LearningElementDifficultyEnum.Medium, null, 8, 9, 17f, 29f);
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
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(0));
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(0));
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.LearningElements[2], Is.EqualTo(element));
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.Contains(element), Is.True);
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(0));
        });
    }
}