using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class LoadLearningElementUt
{
    [Test]
    public void Execute_LoadsLearningElement()
    {
        var testParameter = new TestParameter();
        var space = testParameter.SpaceParent;
        var element =
            new LearningElement("a", null!, "b",
                "c", LearningElementDifficultyEnum.Easy, space, 1, 9, 2, 3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, 0, filepath, mockBusinessLogic, mappingAction);

        Assert.That(space.ContainedLearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(space.ContainedLearningElements.First(), Is.InstanceOf(typeof(LearningElement)));
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var space = new LearningSpace("a", "d", "e", 3);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, 0, "element", mockBusinessLogic, mappingAction);

        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));

        Assert.IsFalse(actionWasInvoked);
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesLoadLearningElement()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var testParameter = new TestParameter();
        var space = testParameter.SpaceParent;
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var element =
            new LearningElement("a", null!, "b",
                "c", LearningElementDifficultyEnum.Easy, space, workload: 1, points: 9, positionX: 2, positionY: 3);
        var element2 = new LearningElement("f", null!, "i", "j",
            LearningElementDifficultyEnum.Easy, space, workload: 5, points: 2, positionX: 1, positionY: 5);
        mockBusinessLogic.LoadLearningElement(Arg.Any<string>()).Returns(element);
        space.LearningSpaceLayout.LearningElements = new Dictionary<int, ILearningElement>
        {
            {
                0, element2
            }
        };
        var command = new LoadLearningElement(space, 1, "element", mockBusinessLogic, mappingAction);

        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));

        command.Execute();

        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(2));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
        Assert.That(space.ContainedLearningElements.Skip(1).First(), Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked);
        actionWasInvoked = false;

        command.Undo();

        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked);
        actionWasInvoked = false;

        command.Redo();

        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(2));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
        Assert.That(space.ContainedLearningElements.Skip(1).First(), Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked);
        actionWasInvoked = false;

        command.Undo();

        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked);
        actionWasInvoked = false;

        command.Redo();

        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(2));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
        Assert.That(space.ContainedLearningElements.Skip(1).First(), Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked);
        actionWasInvoked = false;
    }
}