using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Layout;

[TestFixture]
public class PlaceStoryElementInLayoutFromLayoutUt
{
    [Test]
    public void Constructor_AllPropertiesSet()
    {
        var (space, element) = GetPreparedSpaceForTest();
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<PlaceStoryElementInLayoutFromLayout>>();

        var sut = GetSystemUnderTest(space, element, 1, mappingAction, logger);

        Assert.Multiple(() =>
        {
            Assert.That(sut.ParentSpace, Is.EqualTo(space));
            Assert.That(sut.LearningElement, Is.EqualTo(element));
            Assert.That(sut.NewSlotIndex, Is.EqualTo(1));
            Assert.That(sut.MappingAction, Is.EqualTo(mappingAction));
            Assert.That(sut.Logger, Is.EqualTo(logger));
        });
    }

    [Test]
    // ANF-ID: [AWA0039, AWA0023]
    public void Execute_StoryElementPlacedInLayoutAndReplacedElementMovedToUnplacedLearningElements()
    {
        var (space, element) = GetPreparedSpaceForTest();
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<PlaceStoryElementInLayoutFromLayout>>();
        var sut = GetSystemUnderTest(space, element, 1, mappingAction, logger);

        sut.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.StoryElements[1], Is.EqualTo(element));
        });
    }

    [Test]
    // ANF-ID: [AWA0039, AWA0023]
    public void Execute_ElementAtSlotAlready_SwapsElements()
    {
        var (space, element) = GetPreparedSpaceForTest();
        var existingElement = EntityProvider.GetLearningElement();
        space.LearningSpaceLayout.StoryElements.Add(1, existingElement);
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<PlaceStoryElementInLayoutFromLayout>>();
        var sut = GetSystemUnderTest(space, element, 1, mappingAction, logger);

        sut.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.EqualTo(2));
            Assert.That(space.LearningSpaceLayout.StoryElements[0], Is.EqualTo(existingElement));
            Assert.That(space.LearningSpaceLayout.StoryElements[1], Is.EqualTo(element));
        });
    }

    [Test]
    public void UndoAfterExecute_RestoresState()
    {
        var (space, element) = GetPreparedSpaceForTest();
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<PlaceStoryElementInLayoutFromLayout>>();
        var sut = GetSystemUnderTest(space, element, 1, mappingAction, logger);

        sut.Execute();
        sut.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(space.LearningSpaceLayout.StoryElements[0], Is.EqualTo(element));
            Assert.That(space.LearningSpaceLayout.StoryElements, Has.No.ContainKey(1));
        });
    }

    [Test]
    public void Undo_MementoLayoutIsNull_ThrowsException()
    {
        var (space, element) = GetPreparedSpaceForTest();
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<PlaceStoryElementInLayoutFromLayout>>();
        var systemUnderTest = GetSystemUnderTest(space, element, 1, mappingAction, logger);

        var ex = Assert.Throws<InvalidOperationException>(() => systemUnderTest.Undo());
        Assert.That(ex!.Message, Is.EqualTo("MementoLayout is null"));
    }

    [Test]
    public void Undo_MementoSpaceIsNull_ThrowsException()
    {
        var (space, element) = GetPreparedSpaceForTest();
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<PlaceStoryElementInLayoutFromLayout>>();
        var systemUnderTest = GetSystemUnderTest(space, element, 1, mappingAction, logger);

        // Manually set MementoLayout to avoid InvalidOperationException
        var memento = space.LearningSpaceLayout.GetMemento();
        systemUnderTest.MementoLayout = memento;

        var ex = Assert.Throws<InvalidOperationException>(() => systemUnderTest.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_mementoSpace is null"));
    }

    [Test]
    public void RedoAfterUndoAfterExecute_SameStateAsAfterExecute()
    {
        var (space, element) = GetPreparedSpaceForTest();
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<PlaceStoryElementInLayoutFromLayout>>();
        var sut = GetSystemUnderTest(space, element, 1, mappingAction, logger);
        sut.Execute();
        sut.Undo();

        sut.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.StoryElements[1], Is.EqualTo(element));
        });
    }

    private static (LearningSpace, ILearningElement) GetPreparedSpaceForTest()
    {
        var space = EntityProvider.GetLearningSpace();
        var element = EntityProvider.GetLearningElement();
        space.LearningSpaceLayout.StoryElements.Add(0, element);
        return (space, element);
    }

    private PlaceStoryElementInLayoutFromLayout GetSystemUnderTest(LearningSpace parentSpace,
        ILearningElement learningElement, int newSlotIndex, Action<LearningSpace> mappingAction,
        ILogger<PlaceStoryElementInLayoutFromLayout> logger)
    {
        return new PlaceStoryElementInLayoutFromLayout(parentSpace, learningElement, newSlotIndex, mappingAction,
            logger);
    }
}