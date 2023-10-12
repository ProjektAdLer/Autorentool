using BusinessLogic.Commands.Adaptivity.Action.ElementReference;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MudBlazor;
using NUnit.Framework;

namespace BusinessLogicTest.Commands.Adaptivity.Action.ElementReference;

[TestFixture]
public class EditElementReferenceActionUt
{
    [Test]
    public void Constructor_InitializesProperties()
    {
        var action = new ElementReferenceAction(Guid.NewGuid());
        var id = Guid.NewGuid();
        var mappingAction = new Action<ElementReferenceAction>(_ => { });
        var logger = new NullLogger<EditElementReferenceAction>();

        var sut = CreateSystemUnderTest(action, id, mappingAction, logger);

        Assert.Multiple(() =>
        {
            Assert.That(sut.Action, Is.EqualTo(action));
            Assert.That(sut.NewElementId, Is.EqualTo(id));
            Assert.That(sut.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void Execute_EditsActionWithGuid_CallsMappingActionWithEditedAction()
    {
        var action = new ElementReferenceAction(Guid.NewGuid());
        var id = Guid.NewGuid();
        var actionCalled = false;
        var mappingAction = new Action<ElementReferenceAction>(a =>
        {
            Assert.That(a, Is.EqualTo(action));
            Assert.That(a.ElementId, Is.EqualTo(id));
            actionCalled = true;
        });

        var sut = CreateSystemUnderTest(action, id, mappingAction);
        sut.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionCalled, Is.True);
            Assert.That(action.ElementId, Is.EqualTo(id));
        });
    }

    [Test]
    public void Undo_UndoesEdit_CallsMappingActionWithRestoredAction()
    {
        var oldId = Guid.NewGuid();
        var action = new ElementReferenceAction(oldId);
        var newId = Guid.NewGuid();
        var actionCallCount = 0;
        var mappingAction = new Action<ElementReferenceAction>(a =>
        {
            actionCallCount++;
            Assert.That(a, Is.EqualTo(action));
            if (actionCallCount == 2)
                Assert.That(a.ElementId, Is.EqualTo(oldId));
        });

        var sut = CreateSystemUnderTest(action, newId, mappingAction);

        sut.Execute();
        action.ElementId = Guid.NewGuid();
        sut.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionCallCount, Is.EqualTo(2));
            Assert.That(action.ElementId, Is.EqualTo(oldId));
        });
    }

    [Test]
    public void Redo_ExecutesCommandAgain()
    {
        var action = new ElementReferenceAction(Guid.NewGuid());
        var id = Guid.NewGuid();
        var actionCallCount = 0;
        var mappingAction = new Action<ElementReferenceAction>(a =>
        {
            Assert.That(a, Is.EqualTo(action));
            actionCallCount++;
        });

        var sut = CreateSystemUnderTest(action, id, mappingAction);

        sut.Execute();
        sut.Undo();
        sut.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionCallCount, Is.EqualTo(3));
            Assert.That(action.ElementId, Is.EqualTo(id));
        });
    }

    private EditElementReferenceAction CreateSystemUnderTest(ElementReferenceAction? action = null, Guid? id = null,
        Action<ElementReferenceAction>? mappingAction = null, ILogger<EditElementReferenceAction>? logger = null)
    {
        action ??= new ElementReferenceAction(Guid.NewGuid());
        id ??= Guid.NewGuid();
        mappingAction ??= _ => { };
        logger ??= new NullLogger<EditElementReferenceAction>();
        return new EditElementReferenceAction(action, id.Value, mappingAction, logger);
    }
}