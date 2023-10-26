using BusinessLogic.Commands.Adaptivity.Action.Comment;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace BusinessLogicTest.Commands.Adaptivity.Action.Comment;

[TestFixture]
public class EditCommentActionUt
{
    [Test]
    public void Constructor_InitializesProperties()
    {
        var action = new CommentAction("an action");
        var comment = "a comment";
        var mappingAction = new Action<CommentAction>(_ => { });
        var logger = new NullLogger<EditCommentAction>();

        var sut = CreateSystemUnderTest(action, comment, mappingAction, logger);

        Assert.Multiple(() =>
        {
            Assert.That(sut.Action, Is.EqualTo(action));
            Assert.That(sut.Comment, Is.EqualTo(comment));
            Assert.That(sut.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void Execute_EditsActionWithComment_CallsMappingActionWithEditedAction()
    {
        var action = new CommentAction("an action");
        var comment = "a comment";
        var actionCalled = false;
        var mappingAction = new Action<CommentAction>(a =>
        {
            Assert.That(a, Is.EqualTo(action));
            Assert.That(a.Comment, Is.EqualTo(comment));
            actionCalled = true;
        });

        var sut = CreateSystemUnderTest(action, comment, mappingAction);

        sut.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionCalled, Is.True);
            Assert.That(action.Comment, Is.EqualTo(comment));
        });
    }

    [Test]
    public void Undo_UndoesEdit_CallsMappingActionWithRestoredAction()
    {
        var action = new CommentAction("an action");
        var comment = "a comment";
        var actionCallCount = 0;
        var mappingAction = new Action<CommentAction>(a =>
        {
            actionCallCount++;
            Assert.That(a, Is.EqualTo(action));
            if (actionCallCount == 2)
                Assert.That(a.Comment, Is.EqualTo("an action"));
        });

        var sut = CreateSystemUnderTest(action, comment, mappingAction);

        sut.Execute();
        action.Comment = "foobar";
        sut.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionCallCount, Is.EqualTo(2));
            Assert.That(action.Comment, Is.EqualTo("an action"));
        });
    }

    [Test]
    public void Redo_ExecutesCommandAgain()
    {
        var action = new CommentAction("an action");
        var comment = "a comment";
        var actionCallCount = 0;
        var mappingAction = new Action<CommentAction>(a =>
        {
            actionCallCount++;
            Assert.That(a, Is.EqualTo(action));
        });
        
        var sut = CreateSystemUnderTest(action, comment, mappingAction);
        
        sut.Execute();
        sut.Undo();
        sut.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.That(actionCallCount, Is.EqualTo(3));
            Assert.That(action.Comment, Is.EqualTo(comment));
        });
    }

    private EditCommentAction CreateSystemUnderTest(CommentAction? action = null, string comment = "",
        Action<CommentAction>? mappingAction = null, ILogger<EditCommentAction>? logger = null)
    {
        action ??= new CommentAction("");
        mappingAction ??= _ => { };
        logger ??= new NullLogger<EditCommentAction>();
        return new EditCommentAction(action, comment, mappingAction, logger);
    }
}