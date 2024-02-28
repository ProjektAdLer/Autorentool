using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Layout;

[TestFixture]
public class PlaceStoryElementInLayoutFromUnplacedUt
{
    [Test]
    public void Constructor_AllPropertiesSet()
    {
        var (world, space, element) = GetPreparedWorldForTest();
        var mappingAction = Substitute.For<Action<LearningWorld>>();
        var logger = Substitute.For<ILogger<PlaceStoryElementInLayoutFromUnplaced>>();

        var sut = GetSystemUnderTest(world, space, element, 0, mappingAction, logger);

        Assert.Multiple(() =>
        {
            Assert.That(sut.LearningWorld, Is.EqualTo(world));
            Assert.That(sut.LearningSpace, Is.EqualTo(space));
            Assert.That(sut.LearningElement, Is.EqualTo(element));
            Assert.That(sut.NewSlotIndex, Is.EqualTo(0));
            Assert.That(sut.MappingAction, Is.EqualTo(mappingAction));
            Assert.That(sut.Logger, Is.EqualTo(logger));
        });
    }
    
    [Test]
    public void Execute_StoryElementPlacedInLayoutAndRemovedFromUnplacedLearningElements()
    {
        var (world, space, element) = GetPreparedWorldForTest();
        var mappingAction = Substitute.For<Action<LearningWorld>>();
        var logger = Substitute.For<ILogger<PlaceStoryElementInLayoutFromUnplaced>>();
        var sut = GetSystemUnderTest(world, space, element, 0, mappingAction, logger);

        sut.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.StoryElements[0], Is.EqualTo(element));
            Assert.That(world.UnplacedLearningElements, Has.Count.Zero);
        });
    }

    [Test]
    public void Execute_ElementAtSlotAlready_ReplacesElement()
    {
        var (world, space, element) = GetPreparedWorldForTest();
        var existingElement = EntityProvider.GetLearningElement();
        space.LearningSpaceLayout.StoryElements.Add(0, existingElement);
        var mappingAction = Substitute.For<Action<LearningWorld>>();
        var logger = Substitute.For<ILogger<PlaceStoryElementInLayoutFromUnplaced>>();
        var sut = GetSystemUnderTest(world, space, element, 0, mappingAction, logger);
        
        sut.Execute();
        
        Assert.Multiple(() =>
        {
            Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.StoryElements[0], Is.EqualTo(element));
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.ElementAt(0), Is.EqualTo(existingElement));
        });
    }

    [Test]
    public void UndoAfterExecute_RestoresState()
    {
        var (world, space, element) = GetPreparedWorldForTest();
        var mappingAction = Substitute.For<Action<LearningWorld>>();
        var logger = Substitute.For<ILogger<PlaceStoryElementInLayoutFromUnplaced>>();
        var sut = GetSystemUnderTest(world, space, element, 0, mappingAction, logger);
        sut.Execute();

        sut.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.Zero);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.ElementAt(0), Is.EqualTo(element));
        });
    }
    
    [Test]
    public void RedoAfterUndoAfterExecute_SameStateAsAfterExecute()
    {
        var (world, space, element) = GetPreparedWorldForTest();
        var mappingAction = Substitute.For<Action<LearningWorld>>();
        var logger = Substitute.For<ILogger<PlaceStoryElementInLayoutFromUnplaced>>();
        var sut = GetSystemUnderTest(world, space, element, 0, mappingAction, logger);
        sut.Execute();
        sut.Undo();

        sut.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.StoryElements[0], Is.EqualTo(element));
            Assert.That(world.UnplacedLearningElements, Has.Count.Zero);
        });
    }

    private static (LearningWorld world, LearningSpace space, LearningElement element) GetPreparedWorldForTest()
    {
        var world = EntityProvider.GetLearningWorld();
        var space = EntityProvider.GetLearningSpace();
        var element = EntityProvider.GetLearningElement();
        world.LearningSpaces.Add(space);
        world.UnplacedLearningElements.Add(element);
        return (world, space, element);
    }
    private PlaceStoryElementInLayoutFromUnplaced GetSystemUnderTest(LearningWorld world, LearningSpace space,
        ILearningElement element, int slotIndex, Action<LearningWorld> mappingAction,
        ILogger<PlaceStoryElementInLayoutFromUnplaced> logger)
    {
        return new PlaceStoryElementInLayoutFromUnplaced(world, space, element, slotIndex, mappingAction, logger);
    }
}