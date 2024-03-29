using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.Layout;

[TestFixture]
public class RemoveStoryElementFromLayoutUt
{
    [Test]
    public void Constructor_AllPropertiesSet()
    {
        var (world, space, element) = GetPreparedWorldForTest();
        var mappingAction = Substitute.For<Action<LearningWorld>>();
        var logger = Substitute.For<ILogger<RemoveStoryElementFromLayout>>();

        var sut = GetSystemUnderTest(world, space, element, mappingAction, logger);

        Assert.Multiple(() =>
        {
            Assert.That(sut.LearningWorld, Is.EqualTo(world));
            Assert.That(sut.LearningSpace, Is.EqualTo(space));
            Assert.That(sut.LearningElement, Is.EqualTo(element));
            Assert.That(sut.MappingAction, Is.EqualTo(mappingAction));
            Assert.That(sut.Logger, Is.EqualTo(logger));
        });
    }
    
    [Test]
    public void Execute_StoryElementRemovedFromLayoutAndAddedToUnplacedLearningElements()
    {
        var (world, space, element) = GetPreparedWorldForTest();
        var mappingAction = Substitute.For<Action<LearningWorld>>();
        var logger = Substitute.For<ILogger<RemoveStoryElementFromLayout>>();
        var sut = GetSystemUnderTest(world, space, element, mappingAction, logger);

        sut.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.Zero);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.ElementAt(0), Is.EqualTo(element));
        });
    }
    
    [Test]
    public void UndoAfterExecute_RestoresState()
    {
        var (world, space, element) = GetPreparedWorldForTest();
        var mappingAction = Substitute.For<Action<LearningWorld>>();
        var logger = Substitute.For<ILogger<RemoveStoryElementFromLayout>>();
        var sut = GetSystemUnderTest(world, space, element, mappingAction, logger);
        sut.Execute();

        sut.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.StoryElements[1], Is.EqualTo(element));
            Assert.That(world.UnplacedLearningElements, Has.Count.Zero);
        });
    }

    [Test]
    public void RedoAfterUndoAfterExecute_SameStateAsAfterExecute()
    {
        var (world, space, element) = GetPreparedWorldForTest();
        var mappingAction = Substitute.For<Action<LearningWorld>>();
        var logger = Substitute.For<ILogger<RemoveStoryElementFromLayout>>();
        
        var sut = GetSystemUnderTest(world, space, element, mappingAction, logger);
        
        sut.Execute();
        sut.Undo();
        sut.Redo();
        
        Assert.Multiple(() =>
        {
            Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.Zero);
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements.ElementAt(0), Is.EqualTo(element));
        });
    }
    
    private static (LearningWorld world, LearningSpace space, LearningElement element) GetPreparedWorldForTest()
    {
        var world = EntityProvider.GetLearningWorld();
        var space = EntityProvider.GetLearningSpace();
        var element = EntityProvider.GetLearningElement();
        world.LearningSpaces.Add(space);
        space.LearningSpaceLayout.StoryElements.Add(1, element);
        return (world, space, element);
    }

    private RemoveStoryElementFromLayout GetSystemUnderTest(LearningWorld world, LearningSpace space,
        ILearningElement element, Action<LearningWorld> mappingAction, ILogger<RemoveStoryElementFromLayout> logger)
    {
        return new RemoveStoryElementFromLayout(world, space, element, mappingAction, logger);
    }
}