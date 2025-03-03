using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Commands.Element;
using BusinessLogic.Commands.Space;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands;

[TestFixture]
public class CommandStateManagerUt
{
    [Test]
    public void Execute_WithCommand_CallsExecuteOnCommand()
    {
        var commandMock = Substitute.For<ICommand>();

        var systemUnderTest = GetCommandStateManagerForTest();

        systemUnderTest.Execute(commandMock);

        commandMock.Received().Execute();
    }

    [Test]
    public void Execute_WithCommand_DoesNotPutCommandOnUndoStack()
    {
        var commandMock = Substitute.For<ICommand>();

        var systemUnderTest = GetCommandStateManagerForTest();

        systemUnderTest.Execute(commandMock);

        Assert.That(systemUnderTest.CanUndo, Is.False);
    }

    [Test]
    public void Execute_WithCommand_ClearsRedoStack()
    {
        var commandMock = Substitute.For<ICommand>();
        var undoCommandMock = Substitute.For<IUndoCommand>();

        var systemUnderTest = GetCommandStateManagerForTest();

        systemUnderTest.Execute(undoCommandMock);

        Assert.That(systemUnderTest.CanUndo);
        systemUnderTest.Undo();
        Assert.That(systemUnderTest.CanRedo);
        systemUnderTest.Execute(commandMock);
        Assert.That(systemUnderTest.CanRedo, Is.False);
    }

    [Test]
    public void Execute_WithUndoCommand_DoesPutCommandOnUndoStack()
    {
        var undoCommandMock = Substitute.For<IUndoCommand>();

        var systemUnderTest = GetCommandStateManagerForTest();

        systemUnderTest.Execute(undoCommandMock);

        Assert.That(systemUnderTest.CanUndo);
    }

    [Test]
    public void Undo_AfterExecuteUndoCommand_CallsUndoOnCommand()
    {
        var undoCommandMock = Substitute.For<IUndoCommand>();

        var systemUnderTest = GetCommandStateManagerForTest();

        systemUnderTest.Execute(undoCommandMock);
        systemUnderTest.Undo();

        undoCommandMock.Received().Undo();
    }

    [Test]
    public void Undo_AfterExecuteUndoCommand_PutsCommandOnRedoStack()
    {
        var undoCommandMock = Substitute.For<IUndoCommand>();

        var systemUnderTest = GetCommandStateManagerForTest();

        Assert.That(systemUnderTest.CanRedo, Is.False);

        systemUnderTest.Execute(undoCommandMock);
        systemUnderTest.Undo();

        Assert.That(systemUnderTest.CanRedo, Is.True);
    }

    [Test]
    public void Undo_WithCanUndoFalse_ThrowsInvalidOperationException()
    {
        var systemUnderTest = GetCommandStateManagerForTest();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.CanUndo, Is.False);
            Assert.That(() => systemUnderTest.Undo(),
                Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("no command to undo"));
        });
    }

    [Test]
    public void Redo_AfterUndoingUndoCommand_CallsRedoOnCommand()
    {
        var undoCommandMock = Substitute.For<IUndoCommand>();

        var systemUnderTest = GetCommandStateManagerForTest();

        systemUnderTest.Execute(undoCommandMock);
        systemUnderTest.Undo();
        systemUnderTest.Redo();
        undoCommandMock.Received().Redo();
    }

    [Test]
    public void Redo_AfterUndoingUndoCommand_PutsCommandOnUndoStack()
    {
        var undoCommandMock = Substitute.For<IUndoCommand>();

        var systemUnderTest = GetCommandStateManagerForTest();

        systemUnderTest.Execute(undoCommandMock);
        systemUnderTest.Undo();

        Assert.That(systemUnderTest.CanUndo, Is.False);

        systemUnderTest.Redo();

        Assert.That(systemUnderTest.CanUndo, Is.True);
    }

    [Test]
    public void Redo_WithCanRedoFalse_ThrowsInvalidOperationException()
    {
        var systemUnderTest = GetCommandStateManagerForTest();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.CanRedo, Is.False);
            Assert.That(() => systemUnderTest.Redo(),
                Throws.TypeOf<InvalidOperationException>().With.Message.EqualTo("no command to redo"));
        });
    }

    [Test]
    public void ExecuteCommandAfterRedoingAnCommand_TriggersRemovedCommandsFromStacksWithRemainingCommandsInStacks()
    {
        var undoCommandMock = Substitute.For<IUndoCommand>();
        var undoCommandMock2 = Substitute.For<IUndoCommand>();
        var undoCommandMock3 = Substitute.For<IUndoCommand>();

        var systemUnderTest = GetCommandStateManagerForTest();
        var wasCalled = false;
        var eventObjects = new List<object>();
        systemUnderTest.RemovedCommandsFromStacks += (_, args) =>
        {
            wasCalled = true;
            eventObjects = args.ObjectsInStacks.ToList();
        };

        systemUnderTest.Execute(undoCommandMock);
        systemUnderTest.Undo();
        systemUnderTest.Redo();
        systemUnderTest.Execute(undoCommandMock2);
        systemUnderTest.Undo();
        systemUnderTest.Execute(undoCommandMock3);

        Assert.That(systemUnderTest.CanUndo, Is.True);
        Assert.That(systemUnderTest.CanRedo, Is.False);
        Assert.That(wasCalled, Is.True);
        // eventObjects has count 0 because the mocked commands do not return an object in GetObjectFromCommand
        Assert.That(eventObjects, Has.Count.EqualTo(0));
    }

    [Test]
    public void
        ExecuteCommandAfterRedoingAnCommand_TriggersRemovedCommandsFromStacksWithRemainingCommandsInStacks_AllCommandTypes()
    {
        var learningElement = EntityProvider.GetLearningElement();
        learningElement.LearningContent = EntityProvider.GetFileContent();
        var learningSpace = EntityProvider.GetLearningSpace(floorPlan: FloorPlanEnum.R_20X30_8L);
        var learningWorld = EntityProvider.GetLearningWorld();
        var workspace = new AuthoringToolWorkspace(new List<ILearningWorld>());
        var createLearningElementCommand = new CreateLearningElementInSlot(learningSpace, 0, learningElement, _ => { },
            new NullLogger<CreateLearningElementInSlot>());
        var createLearningSpaceCommand = new CreateLearningSpace(learningWorld, learningSpace, _ => { },
            new NullLogger<CreateLearningSpace>());
        var createLearningWorldCommand =
            new CreateLearningWorld(workspace, learningWorld, _ => { }, new NullLogger<CreateLearningWorld>());
        var deleteLearningElementCommand = new DeleteLearningElementInSpace(learningElement, learningSpace, _ => { },
            new NullLogger<DeleteLearningElementInSpace>());
        var deleteLearningSpaceCommand = new DeleteLearningSpace(learningWorld, learningSpace, _ => { },
            new NullLogger<DeleteLearningSpace>());
        var deleteLearningWorldCommand =
            new DeleteLearningWorld(workspace, learningWorld, _ => { }, new NullLogger<DeleteLearningWorld>());
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        mockBusinessLogic.LoadLearningWorld(Arg.Any<string>()).Returns(learningWorld);
        var loadLearningWorldCommand = new LoadLearningWorld(workspace, "w", mockBusinessLogic, _ => { },
            new NullLogger<LoadLearningWorld>());

        var secondLearningWorld = EntityProvider.GetLearningWorld(append: "2");
        var thirdLearningWorld = EntityProvider.GetLearningWorld(append: "3");
        var createSecondLearningWorldCommand = new CreateLearningWorld(workspace, secondLearningWorld, _ => { },
            new NullLogger<CreateLearningWorld>());
        var createThirdLearningWorldCommand = new CreateLearningWorld(workspace, thirdLearningWorld, _ => { },
            new NullLogger<CreateLearningWorld>());

        var systemUnderTest = GetCommandStateManagerForTest();
        var wasCalled = false;
        var eventObjects = new List<object>();
        systemUnderTest.RemovedCommandsFromStacks += (_, args) =>
        {
            wasCalled = true;
            eventObjects = args.ObjectsInStacks.ToList();
        };

        systemUnderTest.Execute(createLearningWorldCommand);
        systemUnderTest.Execute(createLearningSpaceCommand);
        systemUnderTest.Execute(createLearningElementCommand);
        systemUnderTest.Execute(deleteLearningElementCommand);
        systemUnderTest.Execute(deleteLearningSpaceCommand);
        systemUnderTest.Execute(deleteLearningWorldCommand);
        systemUnderTest.Execute(loadLearningWorldCommand);
        // command to remove
        systemUnderTest.Execute(createSecondLearningWorldCommand);
        systemUnderTest.Undo();
        systemUnderTest.Execute(createThirdLearningWorldCommand);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.CanUndo, Is.True);
            Assert.That(systemUnderTest.CanRedo, Is.False);
        });
        Assert.Multiple(() =>
        {
            Assert.That(wasCalled, Is.True);
            Assert.That(eventObjects, Has.Count.EqualTo(4));
        });
        Assert.Multiple(() =>
        {
            Assert.That(eventObjects, Has.Member(learningElement));
            Assert.That(eventObjects, Has.Member(learningSpace));
            Assert.That(eventObjects, Has.Member(learningWorld));
            Assert.That(eventObjects, Has.Member(thirdLearningWorld));
        });
    }

    private CommandStateManager GetCommandStateManagerForTest() => new();
}