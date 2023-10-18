using BusinessLogic.Commands.Adaptivity.Action.ContentReference;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Adaptivity.Action.ContentReference;

[TestFixture]
public class EditContentReferenceActionUt
{
    [Test]
    public void Constructor_InitializesProperties()
    {
        var oldContent = EntityProvider.GetFileContent();
        var newContent = EntityProvider.GetLinkContent();
        var action = EntityProvider.GetContentReferenceAction(oldContent);
        var mappingAction = new Action<ContentReferenceAction>(_ => { });
        var logger = new NullLogger<EditContentReferenceAction>();
        var comment = "somecomment";

        var sut = CreateSystemUnderTest(action, newContent, comment, mappingAction, logger);

        Assert.Multiple(() =>
        {
            Assert.That(sut.Action, Is.EqualTo(action));
            Assert.That(sut.Content, Is.EqualTo(newContent));
            Assert.That(sut.Comment, Is.EqualTo(comment));
            Assert.That(sut.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void Execute_EditsActionWithContent_CallsMappingActionWithEditedAction()
    {
        var oldContent = EntityProvider.GetFileContent();
        var newContent = EntityProvider.GetLinkContent();
        var action = EntityProvider.GetContentReferenceAction(oldContent);
        var actionCalled = false;
        var comment = "somecomment";
        var mappingAction = new Action<ContentReferenceAction>(a =>
        {
            Assert.That(a, Is.EqualTo(action));
            Assert.That(a.Content, Is.EqualTo(newContent));
            actionCalled = true;
        });

        var sut = CreateSystemUnderTest(action, newContent, comment, mappingAction);

        sut.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(actionCalled, Is.True);
            Assert.That(action.Content, Is.EqualTo(newContent));
            Assert.That(action.Comment, Is.EqualTo(comment));
        });
    }

    [Test]
    public void Undo_UndoesEdit_CallsMappingActionWithRestoredAction()
    {
        var oldContent = EntityProvider.GetFileContent();
        var newContent = EntityProvider.GetLinkContent();
        var action = EntityProvider.GetContentReferenceAction(oldContent);
        var actionCallCount = 0;
        var comment = "somecomment";
        var mappingAction = new Action<ContentReferenceAction>(a =>
        {
            actionCallCount++;
            Assert.That(a, Is.EqualTo(action));
            if (actionCallCount == 2)
            {
                Assert.That(a.Content, Is.EqualTo(oldContent));
            }
        });

        var sut = CreateSystemUnderTest(action, newContent, comment, mappingAction);

        sut.Execute();
        action.Content = EntityProvider.GetLinkContent();
        sut.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(actionCallCount, Is.EqualTo(2));
            Assert.That(action.Content, Is.EqualTo(oldContent));
            Assert.That(action.Comment, Is.EqualTo(""));
        });
    }

    [Test]
    public void Redo_ExecutesCommandAgain()
    {
        var oldContent = EntityProvider.GetFileContent();
        var newContent = EntityProvider.GetLinkContent();
        var action = EntityProvider.GetContentReferenceAction(oldContent);
        var actionCallCount = 0;
        var mappingAction = new Action<ContentReferenceAction>(a =>
        {
            actionCallCount++;
            Assert.That(a, Is.EqualTo(action));
        });

        var sut = CreateSystemUnderTest(action, newContent, mappingAction: mappingAction);

        sut.Execute();
        sut.Undo();
        sut.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(actionCallCount, Is.EqualTo(3));
            Assert.That(action.Content, Is.EqualTo(newContent));
        });
    }

    private EditContentReferenceAction CreateSystemUnderTest(ContentReferenceAction? action = null,
        ILearningContent? content = null, string comment = "", Action<ContentReferenceAction>? mappingAction = null,
        ILogger<EditContentReferenceAction>? logger = null)
    {
        action ??= EntityProvider.GetContentReferenceAction();
        content ??= EntityProvider.GetLinkContent();
        mappingAction ??= _ => { };
        logger ??= new NullLogger<EditContentReferenceAction>();

        return new EditContentReferenceAction(action, content, comment, mappingAction, logger);
    }
}