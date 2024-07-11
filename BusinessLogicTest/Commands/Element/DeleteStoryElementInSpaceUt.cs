using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Element;

[TestFixture]
public class DeleteStoryElementInSpaceUt
{
    [Test]
    public void Constructor_AllPropertiesSet()
    {
        var (space, element) = GetEntitiesForTest();
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<DeleteStoryElementInSpace>>();

        var sut = GetSystemUnderTest(element, space, mappingAction, logger);

        Assert.Multiple(() =>
        {
            Assert.That(sut.LearningElement, Is.EqualTo(element));
            Assert.That(sut.ParentSpace, Is.EqualTo(space));
            Assert.That(sut.MappingAction, Is.EqualTo(mappingAction));
            Assert.That(sut.Logger, Is.EqualTo(logger));
        });
    }

    [Test]
    // ANF-ID: [ASN0015]
    public void Execute_StoryElementRemovedFromLayout()
    {
        var (space, element) = GetEntitiesForTest();
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<DeleteStoryElementInSpace>>();
        var sut = GetSystemUnderTest(element, space, mappingAction, logger);

        sut.Execute();

        Assert.Multiple(() => { Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.Zero); });
    }

    [Test]
    public void UndoAfterExecute_RestoresState()
    {
        var (space, element) = GetEntitiesForTest();
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<DeleteStoryElementInSpace>>();
        var sut = GetSystemUnderTest(element, space, mappingAction, logger);

        sut.Execute();
        sut.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.StoryElements[0], Is.EqualTo(element));
        });
    }

    [Test]
    public void RedoAfterUndoAfterExecute_SameStateAsAfterExecute()
    {
        var (space, element) = GetEntitiesForTest();
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<DeleteStoryElementInSpace>>();
        var sut = GetSystemUnderTest(element, space, mappingAction, logger);

        sut.Execute();
        sut.Undo();
        sut.Redo();

        Assert.Multiple(() => { Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.Zero); });
    }

    private static (LearningSpace space, LearningElement element) GetEntitiesForTest()
    {
        var space = EntityProvider.GetLearningSpace();
        var element = EntityProvider.GetLearningElement();
        space.LearningSpaceLayout.StoryElements.Add(0, element);
        return (space, element);
    }

    private DeleteStoryElementInSpace GetSystemUnderTest(LearningElement learningElement, LearningSpace parentSpace,
        Action<LearningSpace> mappingAction, ILogger<DeleteStoryElementInSpace> logger)
    {
        return new DeleteStoryElementInSpace(learningElement, parentSpace, mappingAction, logger);
    }
}