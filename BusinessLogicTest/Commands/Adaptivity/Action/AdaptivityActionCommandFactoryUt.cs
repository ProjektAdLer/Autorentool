using BusinessLogic.Commands.Adaptivity.Action;
using BusinessLogic.Commands.Adaptivity.Action.Comment;
using BusinessLogic.Commands.Adaptivity.Action.ContentReference;
using BusinessLogic.Commands.Adaptivity.Action.ElementReference;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using NUnit.Framework;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TestHelpers;

[TestFixture]
class AdaptivityActionCommandFactoryUt
{
    [Test]
    public void GetEditCommentAction_WhenCalled_ReturnsEditCommentAction()
    {
        // Arrange
        var systemUnderTest = CreateSystemUnderTest();
        var action = EntityProvider.GetCommentAction();
        var comment = "Test Comment";
        Action<CommentAction> mappingAction = _ => { };

        // Act
        var result = systemUnderTest.GetEditCommentAction(action, comment, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<EditCommentAction>());
        var castedResult = (EditCommentAction)result;
        Assert.Multiple(() =>
        {
            Assert.That(castedResult.Action, Is.EqualTo(action));
            Assert.That(castedResult.Comment, Is.EqualTo(comment));
            Assert.That(castedResult.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetEditContentReferenceAction_WhenCalled_ReturnsEditContentReferenceAction()
    {
        // Arrange
        var systemUnderTest = CreateSystemUnderTest();
        var action = EntityProvider.GetContentReferenceAction();
        var content = Substitute.For<ILearningContent>();
        var comment = "somecomment";
        Action<ContentReferenceAction> mappingAction = _ => { };

        // Act
        var result = systemUnderTest.GetEditContentReferenceAction(action, content, comment, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<EditContentReferenceAction>());
        var castedResult = (EditContentReferenceAction)result;
        Assert.Multiple(() =>
        {
            Assert.That(castedResult.Action, Is.EqualTo(action));
            Assert.That(castedResult.Content, Is.EqualTo(content));
            Assert.That(castedResult.Comment, Is.EqualTo(comment));
            Assert.That(castedResult.MappingAction, Is.EqualTo(mappingAction));
        });

    }

    [Test]
    public void GetEditElementReferenceAction_WhenCalled_ReturnsEditElementReferenceAction()
    {
        // Arrange
        var factory = CreateSystemUnderTest();
        var action = EntityProvider.GetElementReferenceAction();
        var elementId = Guid.NewGuid();
        var comment = "somecomment";
        Action<ElementReferenceAction> mappingAction = _ => { };

        // Act
        var result = factory.GetEditElementReferenceAction(action, elementId, comment, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<EditElementReferenceAction>());
        var castedResult = (EditElementReferenceAction)result;
        Assert.Multiple(() =>
        {
            Assert.That(castedResult.Action, Is.EqualTo(action));
            Assert.That(castedResult.ElementId, Is.EqualTo(elementId));
            Assert.That(castedResult.Comment, Is.EqualTo(comment));
            Assert.That(castedResult.MappingAction, Is.EqualTo(mappingAction));
        });
    }
    
    private AdaptivityActionCommandFactory CreateSystemUnderTest(ILoggerFactory? loggerFactory = null)
    {
        loggerFactory ??= new NullLoggerFactory();
        return new AdaptivityActionCommandFactory(loggerFactory);
    }
}
