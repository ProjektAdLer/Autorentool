using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Element;

[TestFixture]
public class CreateStoryElementInSlotUt
{
    [Test]
    public void ElementConstructor_AllPropertiesSet()
    {
        var (space, _) = GetEntitiesForTest();
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<CreateStoryElementInSlot>>();
        var learningElement = EntityProvider.GetLearningElement();

        var systemUnderTest = GetSystemUnderTest(space, 2, learningElement, mappingAction, logger);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ParentSpace, Is.EqualTo(space));
            Assert.That(systemUnderTest.SlotIndex, Is.EqualTo(2));
            Assert.That(systemUnderTest.LearningElement, Is.EqualTo(learningElement));
            Assert.That(systemUnderTest.MappingAction, Is.EqualTo(mappingAction));
            Assert.That(systemUnderTest.Logger, Is.EqualTo(logger));
        });
    }

    [Test]
    public void PropertyConstructor_AllPropertiesSet()
    {
        var (space, content) = GetEntitiesForTest();
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<CreateStoryElementInSlot>>();

        var systemUnderTest = GetSystemUnderTest(space, 2, "name", content, "description", "goals",
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 1, 1, 0, 0, mappingAction, logger);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ParentSpace, Is.EqualTo(space));
            Assert.That(systemUnderTest.SlotIndex, Is.EqualTo(2));
            Assert.That(systemUnderTest.LearningElement.Name, Is.EqualTo("name"));
            Assert.That(systemUnderTest.LearningElement.LearningContent, Is.EqualTo(content));
            Assert.That(systemUnderTest.LearningElement.Description, Is.EqualTo("description"));
            Assert.That(systemUnderTest.LearningElement.Goals, Is.EqualTo("goals"));
            Assert.That(systemUnderTest.LearningElement.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Easy));
            Assert.That(systemUnderTest.LearningElement.ElementModel, Is.EqualTo(ElementModel.l_h5p_slotmachine_1));
            Assert.That(systemUnderTest.LearningElement.Workload, Is.EqualTo(1));
            Assert.That(systemUnderTest.LearningElement.Points, Is.EqualTo(1));
            Assert.That(systemUnderTest.LearningElement.PositionX, Is.EqualTo(0));
            Assert.That(systemUnderTest.LearningElement.PositionY, Is.EqualTo(0));
            Assert.That(systemUnderTest.MappingAction, Is.EqualTo(mappingAction));
            Assert.That(systemUnderTest.Logger, Is.EqualTo(logger));
        });
    }

    [Test]
    // ANF-ID: [ASN0011]
    public void Execute_StoryElementCreatedAndAddedToLayout()
    {
        var (space, content) = GetEntitiesForTest();
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<CreateStoryElementInSlot>>();
        var systemUnderTest = GetSystemUnderTest(space, 2, "name", content, "description", "goals",
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 1, 1, 0, 0, mappingAction, logger);

        systemUnderTest.Execute();

        Assert.Multiple(() =>
        {
            Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.StoryElements[2], Is.EqualTo(systemUnderTest.LearningElement));
        });
    }

    [Test]
    public void UndoAfterExecute_RestoresState()
    {
        var (space, content) = GetEntitiesForTest();
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<CreateStoryElementInSlot>>();
        var systemUnderTest = GetSystemUnderTest(space, 2, "name", content, "description", "goals",
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 1, 1, 0, 0, mappingAction, logger);

        systemUnderTest.Execute();
        systemUnderTest.Undo();

        Assert.Multiple(() => { Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.Zero); });
    }

    [Test]
    public void RedoAfterUndoAfterExecute_SameStateAsAfterExecute()
    {
        var (space, content) = GetEntitiesForTest();
        var mappingAction = Substitute.For<Action<LearningSpace>>();
        var logger = Substitute.For<ILogger<CreateStoryElementInSlot>>();
        var systemUnderTest = GetSystemUnderTest(space, 2, "name", content, "description", "goals",
            LearningElementDifficultyEnum.Easy, ElementModel.l_h5p_slotmachine_1, 1, 1, 0, 0, mappingAction, logger);

        systemUnderTest.Execute();
        systemUnderTest.Undo();
        systemUnderTest.Redo();

        Assert.Multiple(() =>
        {
            Assert.That(space.LearningSpaceLayout.StoryElements, Has.Count.EqualTo(1));
            Assert.That(space.LearningSpaceLayout.StoryElements[2], Is.EqualTo(systemUnderTest.LearningElement));
        });
    }

    private static (LearningSpace space, ILearningContent content) GetEntitiesForTest()
    {
        var space = EntityProvider.GetLearningSpace();
        var content = EntityProvider.GetStoryContent();
        return (space, content);
    }

    private static CreateStoryElementInSlot GetSystemUnderTest(LearningSpace parentSpace, int slotIndex, string name,
        ILearningContent learningContent, string description, string goals,
        LearningElementDifficultyEnum difficulty, ElementModel elementModel, int workload, int points, double positionX,
        double positionY,
        Action<LearningSpace> mappingAction, ILogger<CreateStoryElementInSlot> logger)
    {
        return new CreateStoryElementInSlot(parentSpace, slotIndex, name, learningContent, description, goals,
            difficulty, elementModel, workload, points, positionX, positionY, mappingAction, logger);
    }

    private static CreateStoryElementInSlot GetSystemUnderTest(LearningSpace parentSpace,
        int slotIndex, LearningElement learningElement, Action<LearningSpace> mappingAction,
        ILogger<CreateStoryElementInSlot> logger)
    {
        return new CreateStoryElementInSlot(parentSpace, slotIndex, learningElement, mappingAction, logger);
    }
}