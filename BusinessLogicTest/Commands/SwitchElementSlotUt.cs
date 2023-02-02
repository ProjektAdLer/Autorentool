using BusinessLogic.Commands;
using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class SwitchElementSlotUt
{
    [Test]
    public void MoveElementToEmptySlot_Execute_MovesElement()
    {
        var parent = new Space("sn", "ssn", "sa", "sd", "sg", 5,
            new SpaceLayout(new IElement[4], FloorPlanEnum.Rectangle2X2));
        var content = new Content("cn", "ct", "cf");
        var element = new Element("en", "esn", content, "url",
            "ea", "ed", "eg", ElementDifficultyEnum.Medium, parent, 8, 9, 17f, 29f);
        parent.SpaceLayout.Elements[0] = element;


        var actionWasInvoked = false;
        Action<Space> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceElementInLayoutFromLayout(parent, element, 2, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(parent.SpaceLayout.Elements[0], Is.EqualTo(element));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(parent.SpaceLayout.Elements[2], Is.EqualTo(element));
        });
    }

    [Test]
    public void MoveElementToAssignedSlot_Execute_SwitchesElements()
    {
        var parent = new Space("sn", "ssn", "sa", "sd", "sg", 5,
            new SpaceLayout(new IElement[4], FloorPlanEnum.Rectangle2X2));
        var content = new Content("cn", "ct", "cf");
        var element = new Element("en", "esn", content, "url",
            "ea", "ed", "eg", ElementDifficultyEnum.Medium, parent, 8, 9, 17f, 29f);
        var element2 = new Element("en2", "esn2", content, "url2",
            "ea2", "ed2", "eg2", ElementDifficultyEnum.Medium, parent, 8, 9, 17f, 29f);
        parent.SpaceLayout.Elements[0] = element;
        parent.SpaceLayout.Elements[2] = element2;


        var actionWasInvoked = false;
        Action<Space> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceElementInLayoutFromLayout(parent, element, 2, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(2));
            Assert.That(parent.SpaceLayout.Elements[0], Is.EqualTo(element));
            Assert.That(parent.SpaceLayout.Elements[2], Is.EqualTo(element2));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(2));
            Assert.That(parent.SpaceLayout.Elements[2], Is.EqualTo(element));
            Assert.That(parent.SpaceLayout.Elements[0], Is.EqualTo(element2));
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var parent = new Space("sn", "ssn", "sa", "sd", "sg", 5,
            new SpaceLayout(new IElement[4], FloorPlanEnum.Rectangle2X2));
        var content = new Content("cn", "ct", "cf");
        var element = new Element("en", "esn", content, "url",
            "ea", "ed", "eg", ElementDifficultyEnum.Medium, parent, 8, 9, 17f, 29f);
        parent.SpaceLayout.Elements[0] = element;


        var actionWasInvoked = false;
        Action<Space> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceElementInLayoutFromLayout(parent, element, 2, mappingAction);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesMovingElement()
    {
        var parent = new Space("sn", "ssn", "sa", "sd", "sg", 5,
            new SpaceLayout(new IElement[4], FloorPlanEnum.Rectangle2X2));
        var content = new Content("cn", "ct", "cf");
        var element = new Element("en", "esn", content, "url",
            "ea", "ed", "eg", ElementDifficultyEnum.Medium, parent, 8, 9, 17f, 29f);
        parent.SpaceLayout.Elements[0] = element;


        var actionWasInvoked = false;
        Action<Space> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceElementInLayoutFromLayout(parent, element, 2, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(parent.SpaceLayout.Elements[0], Is.EqualTo(element));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(parent.SpaceLayout.Elements[2], Is.EqualTo(element));
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(parent.SpaceLayout.Elements[0], Is.EqualTo(element));
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(parent.SpaceLayout.Elements[2], Is.EqualTo(element));
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesSwitchingElements()
    {
        var parent = new Space("sn", "ssn", "sa", "sd", "sg", 5,
            new SpaceLayout(new IElement[4], FloorPlanEnum.Rectangle2X2));
        var content = new Content("cn", "ct", "cf");
        var element = new Element("en", "esn", content, "url",
            "ea", "ed", "eg", ElementDifficultyEnum.Medium, parent, 8, 9, 17f, 29f);
        var element2 = new Element("en2", "esn2", content, "url2",
            "ea2", "ed2", "eg2", ElementDifficultyEnum.Medium, parent, 8, 9, 17f, 29f);
        parent.SpaceLayout.Elements[0] = element;
        parent.SpaceLayout.Elements[2] = element2;


        var actionWasInvoked = false;
        Action<Space> mappingAction = _ => actionWasInvoked = true;

        var command = new PlaceElementInLayoutFromLayout(parent, element, 2, mappingAction);

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.False);
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(2));
            Assert.That(parent.SpaceLayout.Elements[0], Is.EqualTo(element));
            Assert.That(parent.SpaceLayout.Elements[2], Is.EqualTo(element2));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(2));
            Assert.That(parent.SpaceLayout.Elements[2], Is.EqualTo(element));
            Assert.That(parent.SpaceLayout.Elements[0], Is.EqualTo(element2));
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(2));
            Assert.That(parent.SpaceLayout.Elements[0], Is.EqualTo(element));
            Assert.That(parent.SpaceLayout.Elements[2], Is.EqualTo(element2));
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionWasInvoked, Is.True);
            Assert.That(parent.ContainedElements.Count(), Is.EqualTo(2));
            Assert.That(parent.SpaceLayout.Elements[2], Is.EqualTo(element));
            Assert.That(parent.SpaceLayout.Elements[0], Is.EqualTo(element2));
        });
    }
}