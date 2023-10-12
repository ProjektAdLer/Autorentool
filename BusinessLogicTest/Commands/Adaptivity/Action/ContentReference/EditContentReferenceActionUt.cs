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
        var action = new ContentReferenceAction(oldContent);
        var mappingAction = new Action<ContentReferenceAction>(_ => { });
        var logger = new NullLogger<EditContentReferenceAction>();

        var sut = CreateSystemUnderTest(action, newContent, mappingAction, logger);

        Assert.Multiple(() =>
        {
            Assert.That(sut.Action, Is.EqualTo(action));
            Assert.That(sut.Content, Is.EqualTo(newContent));
            Assert.That(sut.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void Execute_EditsActionWithContent_CallsMappingActionWithEditedAction()
    {
        var oldContent = EntityProvider.GetFileContent();
        var newContent = EntityProvider.GetLinkContent();
        var action = new ContentReferenceAction(oldContent);
        var actionCalled = false;
        var mappingAction = new Action<ContentReferenceAction>(a =>
        {
            Assert.That(a, Is.EqualTo(action));
            Assert.That(a.Content, Is.EqualTo(newContent));
            actionCalled = true;
        });
        
        var sut = CreateSystemUnderTest(action, newContent, mappingAction);
        
        sut.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(actionCalled, Is.True);
            Assert.That(action.Content, Is.EqualTo(newContent));
        });
    }

    [Test]
    public void Undo_UndoesEdit_CallsMappingActionWithRestoredAction()
    {
        var oldContent = EntityProvider.GetFileContent();
        var newContent = EntityProvider.GetLinkContent();
        var action = new ContentReferenceAction(oldContent);
        var actionCallCount = 0;
        var mappingAction = new Action<ContentReferenceAction>(a =>
        {
            actionCallCount++;
            Assert.That(a, Is.EqualTo(action));
            if (actionCallCount == 2)
            {
                Assert.That(a.Content, Is.EqualTo(oldContent));
            }
        });
        
        var sut = CreateSystemUnderTest(action, newContent, mappingAction);
        
        sut.Execute();
        action.Content = EntityProvider.GetLinkContent();
        sut.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(actionCallCount, Is.EqualTo(2));
            Assert.That(action.Content, Is.EqualTo(oldContent));
        });
    }

    [Test]
    public void Redo_ExecutesCommandAgain()
    {
        var oldContent = EntityProvider.GetFileContent();
        var newContent = EntityProvider.GetLinkContent();
        var action = new ContentReferenceAction(oldContent);
        var actionCallCount = 0;
        var mappingAction = new Action<ContentReferenceAction>(a =>
        {
            actionCallCount++;
            Assert.That(a, Is.EqualTo(action));
        });
        
        var sut = CreateSystemUnderTest(action, newContent, mappingAction);
        
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
        ILearningContent? content = null, Action<ContentReferenceAction>? mappingAction = null,
        ILogger<EditContentReferenceAction>? logger = null)
    {
        action ??= new ContentReferenceAction(EntityProvider.GetFileContent());
        content ??= EntityProvider.GetLinkContent();
        mappingAction ??= _ => { };
        logger ??= new NullLogger<EditContentReferenceAction>();

        return new EditContentReferenceAction(action, content, mappingAction, logger);
    }
}