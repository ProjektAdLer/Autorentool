using BusinessLogic.Commands;
using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class DragElementFromUnplacedUt
{
    [Test]
    public void DragElementFromUnplacedToFree_Execute_MovesElementToSlot()
    {
        var world = new World("wn", "wsn", "wa", "wl", "wd", "wg");
        var space = new Space("sn", "ssn", "sa", "sd", "sg", 5,
            new SpaceLayout(new IElement[4], FloorPlanEnum.Rectangle2X2));
        world.Spaces.Add(space);
        var content = new Content("cn", "ct", "cf");
        var element = new Element("en", "esn", content, "url",
            "ea", "ed", "eg", ElementDifficultyEnum.Medium, null, 8, 9, 17f, 29f);
        world.UnplacedElements.Add(element);


        var actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceElementInLayoutFromUnplaced(world, space, element, 2, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnplacedElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedElements.Contains(element), Is.True);
            Assert.That(space.ContainedElements.Count(), Is.EqualTo(0));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedElements, Has.Count.EqualTo(0));
            Assert.That(space.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(space.SpaceLayout.Elements[2], Is.EqualTo(element));
        });
    }

    [Test]
    public void DragElementToAssignedSlot_Execute_SwitchesElements()
    {
        var world = new World("wn", "wsn", "wa", "wl", "wd", "wg");
        var space = new Space("sn", "ssn", "sa", "sd", "sg", 5,
            new SpaceLayout(new IElement[4], FloorPlanEnum.Rectangle2X2));
        world.Spaces.Add(space);
        var content = new Content("cn", "ct", "cf");
        var element = new Element("en", "esn", content, "url",
            "ea", "ed", "eg", ElementDifficultyEnum.Medium, null, 8, 9, 17f, 29f);
        world.UnplacedElements.Add(element);
        var element2 = new Element("en2", "esn2", content, "url2",
            "ea2", "ed2", "eg2", ElementDifficultyEnum.Medium, space, 8, 9, 17f, 29f);
        space.SpaceLayout.Elements[2] = element2;


        var actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceElementInLayoutFromUnplaced(world, space, element, 2, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnplacedElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedElements.Contains(element), Is.True);
            Assert.That(space.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(space.SpaceLayout.Elements[2], Is.EqualTo(element2));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedElements.Contains(element2), Is.True);
            Assert.That(space.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(space.SpaceLayout.Elements[2], Is.EqualTo(element));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var world = new World("wn", "wsn", "wa", "wl", "wd", "wg");
        var space = new Space("sn", "ssn", "sa", "sd", "sg", 5,
            new SpaceLayout(new IElement[4], FloorPlanEnum.Rectangle2X2));
        world.Spaces.Add(space);
        var content = new Content("cn", "ct", "cf");
        var element = new Element("en", "esn", content, "url",
            "ea", "ed", "eg", ElementDifficultyEnum.Medium, null, 8, 9, 17f, 29f);
        world.UnplacedElements.Add(element);


        var actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceElementInLayoutFromUnplaced(world, space, element, 2, mappingAction);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_mementoWorld or _mementoSpaceLayout is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesMovingElement()
    {
        var world = new World("wn", "wsn", "wa", "wl", "wd", "wg");
        var space = new Space("sn", "ssn", "sa", "sd", "sg", 5,
            new SpaceLayout(new IElement[4], FloorPlanEnum.Rectangle2X2));
        world.Spaces.Add(space);
        var content = new Content("cn", "ct", "cf");
        var element = new Element("en", "esn", content, "url",
            "ea", "ed", "eg", ElementDifficultyEnum.Medium, null, 8, 9, 17f, 29f);
        world.UnplacedElements.Add(element);


        var actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceElementInLayoutFromUnplaced(world, space, element, 2, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnplacedElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedElements.Contains(element), Is.True);
            Assert.That(space.ContainedElements.Count(), Is.EqualTo(0));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedElements, Has.Count.EqualTo(0));
            Assert.That(space.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(space.SpaceLayout.Elements[2], Is.EqualTo(element));
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedElements.Contains(element), Is.True);
            Assert.That(space.ContainedElements.Count(), Is.EqualTo(0));
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedElements, Has.Count.EqualTo(0));
            Assert.That(space.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(space.SpaceLayout.Elements[2], Is.EqualTo(element));
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesSwitchingElements()
    {
        var world = new World("wn", "wsn", "wa", "wl", "wd", "wg");
        var space = new Space("sn", "ssn", "sa", "sd", "sg", 5,
            new SpaceLayout(new IElement[4], FloorPlanEnum.Rectangle2X2));
        world.Spaces.Add(space);
        var content = new Content("cn", "ct", "cf");
        var element = new Element("en", "esn", content, "url",
            "ea", "ed", "eg", ElementDifficultyEnum.Medium, null, 8, 9, 17f, 29f);
        world.UnplacedElements.Add(element);
        var element2 = new Element("en2", "esn2", content, "url2",
            "ea2", "ed2", "eg2", ElementDifficultyEnum.Medium, space, 8, 9, 17f, 29f);
        space.SpaceLayout.Elements[2] = element2;


        var actionWasInvoked = false;
        Action<World> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceElementInLayoutFromUnplaced(world, space, element, 2, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(world.UnplacedElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedElements.Contains(element), Is.True);
            Assert.That(space.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(space.SpaceLayout.Elements[2], Is.EqualTo(element2));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedElements.Contains(element2), Is.True);
            Assert.That(space.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(space.SpaceLayout.Elements[2], Is.EqualTo(element));
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedElements.Contains(element), Is.True);
            Assert.That(space.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(space.SpaceLayout.Elements[2], Is.EqualTo(element2));
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(world.UnplacedElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedElements.Contains(element2), Is.True);
            Assert.That(space.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(space.SpaceLayout.Elements[2], Is.EqualTo(element));
        });
    }
}