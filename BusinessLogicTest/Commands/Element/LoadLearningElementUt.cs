using BusinessLogic.API;
using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Element;

[TestFixture]
public class LoadLearningElementUt
{
    [Test]
    public void Execute_LoadsLearningElement()
    {
        var testParameter = new TestParameter();
        var space = testParameter.SpaceParent;
        var element = EntityProvider.GetLearningElement(parent: space);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, 0, filepath, mockBusinessLogic, mappingAction,
            new NullLogger<LoadLearningElement>());

        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements, Is.Empty);
            Assert.That(actionWasInvoked, Is.False);
        });

        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element));
        });
        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.First(), Is.InstanceOf(typeof(LearningElement)));
            Assert.That(actionWasInvoked, Is.True);
        });
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var space = new LearningSpace("a", "d", "e", 3, Theme.CampusAschaffenburg);
        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, 0, "element", mockBusinessLogic, mappingAction, null!);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
            Assert.That(actionWasInvoked, Is.False);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesLoadLearningElement()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var testParameter = new TestParameter();
        var space = testParameter.SpaceParent;
        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var element = EntityProvider.GetLearningElement(parent: space);
        var element2 = EntityProvider.GetLearningElement(parent: space, append: "2");
        mockBusinessLogic.LoadLearningElement(Arg.Any<string>()).Returns(element);
        space.LearningSpaceLayout.LearningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, element2
            }
        };
        var command = new LoadLearningElement(space, 1, "element", mockBusinessLogic, mappingAction,
            new NullLogger<LoadLearningElement>());

        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
        });

        command.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
            Assert.That(space.ContainedLearningElements.Skip(1).First(), Is.EqualTo(element));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
            Assert.That(space.ContainedLearningElements.Skip(1).First(), Is.EqualTo(element));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;

        command.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(2));
            Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
            Assert.That(space.ContainedLearningElements.Skip(1).First(), Is.EqualTo(element));
            Assert.That(actionWasInvoked, Is.True);
        });
        actionWasInvoked = false;
    }
}